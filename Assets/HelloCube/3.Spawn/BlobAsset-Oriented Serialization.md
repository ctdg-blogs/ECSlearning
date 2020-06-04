#### Asset-Oriented System Serialization
+ spawn prefabWithData
   + 需要UnityEngine.Start() --> :MonoBehavior
   + Entity配置 + JobSystem --> also DOTS

##### World -> struct BlobAsset(AnimationClip,Audio,SubScene...)
+ World ~ BlobAssetStore 
+ EntityManager
+ BlobAssetComputationContext<TString,TBlobAssetRootDataType>
   + ResetCache() clearWholeContext
   + Contains<TBlob>
   + Remove<TBlob>
   + TryAdd<TBlob>
   + TryGet<TBlob>
+ GameObjectConversionSettings
   + FromWorld
   + FromHash -- BlobAsset的Hash128 key
   + FromGUID -- Entity的AssetGUID
+ conversionFlag
   + gameView/sceneView LiveLink
   + assignName
+ TryCreateAssetExportWriter(rootEntity)
+ WithExtraSystem<TSystem>()

##### BlobAsset<TRootEntityArchedType>
+ 静态拓展方法 -- 序列化
   + blob Read<TBlob>(this BinaryReader)
   + void Write<TBlob>(this BinaryWriter,blob)
+ 两种序列化流
   + stream(filename,buffersize) 防异常接收流
   + memory(*data,bytesize) 底层流
+ BlobAsset(Reference)Change 修改blobAsset

##### 更高级的System读写控制
+ [WriteGroup()] IComponentData，可重复特性
   + 非显示继承，但有依赖关系的IComponentData
   + 同一个写入锁，预限制Dependency



##### 更高级的System.Update控制
+ Transform.Static:IComponentData
   + 只update LocalToWorld一次
   + update后 Unity添加Frozen:IComponentData
+ Transform.Parent/Child.Entity 获取父/子实体
   + ChildBuffer初始为8个
+ IBufferElementData
   + 入门 Scripting API: IBufferElementData & Dynamic Buffers & pragma disable:n去警告
   + 重复Data的List，避免因增删ComponentData改变ArchedType
   ```CSharp
   [InternalBufferCapacity(num)]
   public struct SomeBufferedData:IBufferElementData{
       public Element Value;
       对数组协变性提供支持的类型转换操作符
   }
   ```
   + 为Archetype的紧凑而耦合牺牲开发效率
      + 实用教程@Dynamic Buffers
      + Job字段
      ```CSharp
      跨虚拟页marshal传送entity
      EntityCommandBuffer.Concurrent cmdBuffer;
      System.Update&Create 需要的underlying type = buffer.Reinterpret<TElementData>() 
      //内部调用ToEntityQuery
      ```
      + Job.Execute
      文档api暂时Obsolete，恐怕要彻底封装，Entity按blobAsset/subScene卸载，后者是九宫格加载的最小逻辑单位

      + EntityManager/World层面增删
      ```CSharp
      EntityManager.AddBuffer<TElementData>(entity) 
      buffer.RemoveAt(Int32)
      ComponentSystem.GetBuffer<TElementData>(Entity) //System Creation BaseClass
      JobComponentSystem.GetBuffer<TElementData>(Entity) //Job Arrangement BaseClass
      ```

##### Create & Destroy Entity
```CSharp
//以BlobAsset（subScene）为基础加卸载（增删）Entity单位
//CLR 以AppDomain为基础加卸载（增删）程序集单位
Entity e = EntityManager.CreateEntity(typeof(SomeBufferedData),blob); 
//没有卸载程序集 <--> 没有卸载Entity
```
+ 特殊System获取Allocator.TempJob锁

   + 构造System 
      + 需要特性[UpdateInGroup(typeof(SimulationSystemGroup))]
      + Singleton Producer 双检锁
      + SetComponent(entity,==已有Component==)
         + 报错：newData has not been added to entity
      + AddComponent(entity, new SomeIComponentData)
   + 析构System
      + Barrier 并发GC锁

