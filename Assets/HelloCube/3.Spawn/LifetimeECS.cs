
using Unity.Entities;

namespace ctdg
{
    public struct LifetimeData : IComponentData
    {
        public float TimeSpan;
    }

    public class LifetimeSystem : SystemBase
    {
        private EntityCommandBufferSystem m_GCBarrier;

        protected override void OnCreate()
        {
            m_GCBarrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmdBuffer = m_GCBarrier.CreateCommandBuffer().ToConcurrent(); //这个操作必须在主cpu
            //以下可IJobChunk
            var deltaTime = Time.DeltaTime; //生存期延长
            Entities.WithName("LifetimeData") //这个函数目前是个饼
           .ForEach((Entity entity, int nativeThreadIndex, ref LifetimeData lifetimeData) =>
            {
                lifetimeData.TimeSpan -= deltaTime;
                if (lifetimeData.TimeSpan < 0.0f)
                {
                    cmdBuffer.DestroyEntity(nativeThreadIndex, entity); //仅addToBuffer
                }
            }).ScheduleParallel();
            m_GCBarrier.AddJobHandleForProducer(Dependency);
        }
    }
}

