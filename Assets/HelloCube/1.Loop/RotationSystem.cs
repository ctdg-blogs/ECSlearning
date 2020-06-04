using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
//安装Unity2019.3.14.f1 -> Resolve Package
//安装HybridRender preview.24 0.3.4
//物体挂载Convert To Entity 或 选中+DOTS 或 自己写代码
//常关注官方package/ConvertToXxx
namespace ctdg
{   
    public class RotationSystem : SystemBase
    {
        protected override void OnUpdate() //主CPU Parallel.Foreach按Entity分配Job -> 
        {                                  //匿名job 或 LambdaJobDescription，
                                           //无法Schedule(job,Dependency)，
                                           //其他System调用相同Data时依赖不明确
                                           //i.e.:不可以System继承后设置Dependency
            float deltaTime = Time.DeltaTime;
            Entities.WithName("Rotation").ForEach((ref Rotation rotation, in PlanetRotateBehavior prb) =>
            {
                rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(),
                    prb.AngularVelocity * deltaTime)); //旋转左乘
            }).ScheduleParallel(); //Unity提供的方法，这时已经ByChunk
        }
    }
}
