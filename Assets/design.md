#### How To Design
manual/gp_overview
文档Filter: Create Gameplay
see also: 3.spawn

##### IComponentData
C#Job System Trouble Shooting: 不要用static！
IComponentData
IBUfferElementData -- DynamicBuffer
ISharedComponentData -- same chunk, i.e. static

ISystemStateComponentData
ISystemStateSharedComponentData
ISystemStateBufferElementData



##### Entity挂载Convert To Entity脚本
[DisallowMultipleComponent]
[AddComponentMenu("DOTS/Convert To Entity")]

##### 每一个EntityQuery可以设置
+ WithBurst(精确度，是否同步更新)
+ 任意两种编译模式
   + .ForEach().ScheduleParallel()
   + new job & 设置dependency


##### subScene加载单位(blobAsset)
+ NewSubScene创建
+ 一个GO -- author整个blobAsset
+ componentData
   + spawner{config + Entity prefab}
   + prefab{rootEntity + componentBehavior}
+ 测试关卡使用单独文件MonoAuthoringData
+ 打包使用BlobAsset序列化

##### Version per GOConverter
+ Prefab 挂载的MonoTest脚本
   + 指定版本号 [ConverterVersion]
+ CommandEntityBuffer -- ScriptableObject as Factory 挂载的MonoTest脚本
+ EntityManager.GlobalSystemVersion
   
##### System
+ Queries -- Behavior基本单位，在TypeName中表达
+ OnCreate 
   + 获取锁
   + 获取syncPointSystem单例的引用，4s后回收
+ OnUpdate 
   + 继承 & 设置Dependency
   + 构造cmdBuffer，传递Job，删除自己，设置 Producer单例的 Dependency
+ sync point System
   + 构造System
   + 析构System

##### Unity GC
每frame GC, 
跨frame注意cached ref的Allocator设置，
回收ref导致的nullReferenceException -> 警告