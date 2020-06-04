#### Loop in System
~~component~~ --> data
~~gameobject~~ --> entity

```CSharp
:SystemBase{
    OnUpdate(){ //主线程调用
        foreach(entity)，
        if(HasData)，
        ExecuteAction，
        ScheduleJob/Task
    }
}
```
##### C#7.2 特性
两种锁修改的方式
ref readonly struct -- 读写Data -- 不兼容
in struct -- 读Data -- 源兼容

##### 指定EntityId选择 ==同一Entity==

##### 指定Archetype选择Entity
+ Entity.Foreach，void async
```CSharp
:SystemBase{
    OnUpdate(){ //主线程调用
        Entity.
        WithName("查找依据，必要的ComponentData"). 只能有一个ComponentData
        ForEach((ref struct Transforms, in struct IComponentData) => { 
            //Jobs to schedule <--> Tasks to batch on cpu
        }).
        ScheduleParallel();
    }
}
```


+ IJobChunk, Task async
```CSharp
struct Job:IJobChunk{
    //声明变量
    //（chunk全局）局部变量不变
    //入参，返参 转换成ChunkType，按chunk处理
    public ArchetypeChunkComponentType<T> TChunkType;
    public void Execute(Unity需求传参){
        var 变量 = chunk.GetNativeArray(ChunkType);
    }
}
System:SystemBase{
    private 锁索引NativeQuery
    OnCreate{初始化锁，RW&ReadOnly}
    OnUpdate{
        var 变量 = GetNativeArray
        var job = new Job{
            为所有变量赋值
            deltaTime = Time.DeltaTime
            }
        //工作依赖性
        Dependency = job.Schedule(NativeQuery, Dependencies起码包括自己);
    }

}
```
##### 指定BlobAssetStore选择Entity
根据紧凑的chunkedmemory选择
默认BlobAssetStore存储SubScene中的Entity

##### 指定EntityManager选择Entity
同一World的Entity

