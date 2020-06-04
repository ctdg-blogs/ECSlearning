using Unity.Burst;
using Unity.Collections; // 和集合相关的特性，eg: ReadOnly
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ctdg
{

    //内部struct job: 它是一个方法！！！
    [BurstCompile] //子cpu上的Job/Task支持爆发编译，可通过IJobChunk调用方法Execute处理Batch
    struct PlanetRotateJob : IJobChunk //子cpu：一个Execute本机代码，循环处理自己被分配锁的chunk
    {
        //局部变量，RW返参，ReadOnly传参
        public float deltaTime;
        public ArchetypeChunkComponentType<Rotation> RotationType;
        [ReadOnly] public ArchetypeChunkComponentType<PlanetRotateBehavior> PRBType;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkRotations = chunk.GetNativeArray(RotationType);
            var chunkPRBs = chunk.GetNativeArray(PRBType);
            //GNA(ArchetypeChunkType，用dataStruct特化)
            for (var i = 0; i < chunk.Count; i++)
            {
                var rot = chunkRotations[i];
                var prb = chunkPRBs[i];
                chunkRotations[i] = new Rotation
                {
                    Value = math.mul(math.normalize(rot.Value),
                        quaternion.AxisAngle(math.up(), prb.AngularVelocity * deltaTime))
                };
            }
        }
    }


    //SystemBase是最小Dependency单位,i.e. Task{JobHandle/cpu句柄 + Dependency执行上下文}
    //Unity中JobHandle与Dependency都可以指代Task 
    //Unity中Transform与Script都可以指代GameObject
    //TaskScheduler：OnUpdate{depend上文，通过锁获得数据，操作数据，向下文传depend}
    //protected SystemBase.Dependency 继承之后，应该合理设置所有base的Dependency
    public class RotationSystemByChunk :SystemBase
    {   //Unity维护struct数据在子cpu的分布 & 主cpu的锁更新 & 提供EntityQuery索引
        //CLR维护数据在堆上的分布 & 同步块的更新 & 提供同步块索引
        private EntityQuery m_group; //无序参数锁Query[filteredEntity in chunk]，
                                     //Container锁@主cpu，每次Update，按chunk分发锁给子cpu

        
        
        protected override void OnCreate()
        {   //构造锁
            m_group = GetEntityQuery(typeof(Rotation),  //RW锁
                ComponentType.ReadOnly<PlanetRotateBehavior>());  //只读锁
            
        }
        //主cpu爆发编译，向子cpu扔struct Job & Query[filteredEntity in chunk]锁 
        protected override void OnUpdate()
        {
            //Dependency.Complete();
            //提取job需要的参数，i.e.chunk lock & 局部变量
            var rotType = GetArchetypeChunkComponentType<Rotation>(); 
            var prbType = GetArchetypeChunkComponentType<PlanetRotateBehavior>(true); 
            var job = new PlanetRotateJob
            {
                RotationType = rotType,
                PRBType = prbType,
                deltaTime = Time.DeltaTime
            };
            //C#JobSystem维护 
            //维持System对job控制的原子性，Dependency == SystemJob/Task的执行上下文          
            //Dependency = job.Schedule(m_group, Dependency); //SystemBase的JobHandle属性，
                                                              //m_group在自己Complete后再次Schedule
            //JobForEach 与 JobChunk 指代不明确，导致编译器标红
            //未赋值给自己报错：job Type not assigned to Dependency
            Dependency = JobChunkExtensions.Schedule<PlanetRotateJob>(job, m_group, Dependency);
            //多依赖Job
            //NativeArray<JobHandle> handles = new NativeArray<JobHandle>(size, Allocator.TempJob);
            //JobHandle combinedHandle = JobHandle.CombineDependencies(handles);
        }
    }  
}

