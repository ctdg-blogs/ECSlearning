using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ctdg
{
    public struct SpawnData : IComponentData
    {
        public Entity PrefabRoot;
        public float3 SpawnSize;
        public float TimeSpan;
    }

    //syncPoint Job Arrangment,i.e.ECB的构造&析构 必须在主线程完成 -> 避免竟态
    //Job通过ForEach分发给Burst子cpu完成(Transform & Render)，下一帧允许普通System.Update
    //syncPoint Job = add/remove componentData & change shared componentData
    //EntityCommandBuffer -> 存储syncPoint Job （costly）
    //BeginSimulationEntityCommandBufferSystem -> 一渲染帧后 分发 syncPoint job -> 普通system 需要 depend on 自己，以防传参成为syncPoint Data
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SpawnSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem m_syncPointCEManager; //CommandEntity
        
        protected override void OnCreate()
        {
            m_syncPointCEManager = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {   //线程安全的IBufferElementData，World管理的单例锁，Allocator.TempJob 4帧后回收
            EntityCommandBuffer.Concurrent cmdBuffer = m_syncPointCEManager.CreateCommandBuffer().ToConcurrent();
            //sync point job/Task.Run() 
            Entities.WithName("SpawnData") //找到自己GOToEntity，特殊编译设置
                .WithBurst(FloatMode.Default, FloatPrecision.Standard, true) //sync compile: compile before Update
                .ForEach((Entity entity, int entityInQueryIndex, in SpawnData spawnData, in LocalToWorld location) =>
                {                        //entityQueryIndex as CommandEntityIndex
                    for (var x = 0; x < spawnData.SpawnSize.x; x++)
                    {
                        for (var z = 0; z < spawnData.SpawnSize.z; z++)
                        {
                            Entity instance = cmdBuffer.Instantiate(entityInQueryIndex, spawnData.PrefabRoot);
                                                  //center transform(parent.transform), offset
                            float3 pos = math.transform(location.Value,new float3(x, 0, z));
                            //原生Component，new赋值
                            cmdBuffer.SetComponent(entityInQueryIndex, instance, new Translation {Value = pos});
                            //增加Component，也是new赋值
                            cmdBuffer.AddComponent(entityInQueryIndex, instance,
                                new PlanetRotateBehavior {AngularVelocity = 2});
                            cmdBuffer.AddComponent(entityInQueryIndex, instance, new LifetimeData {TimeSpan = spawnData.TimeSpan});
                        }
                    }
                                           //this CommandEntityIndex
                    cmdBuffer.DestroyEntity(entityInQueryIndex, entity);
                }).ScheduleParallel();
             //World Task scheduler got this ProducerTask
             m_syncPointCEManager.AddJobHandleForProducer(Dependency);
        }
    }
}
