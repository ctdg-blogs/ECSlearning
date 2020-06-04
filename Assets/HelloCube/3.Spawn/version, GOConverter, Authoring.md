#### Version, GOConverter, and Authoring

##### 挂载Authoring的GameObject as RevisingEntity
+ 在Hierarchy面板出现 ~~（废话）~~
+ 必须有Convert To Entity脚本
+ 修改文件名报错，修改文件名警告

##### PrefabMonoTest Authoring
+ MonoTest Attributes
   + RequiresEntityConversion 必需
   + ConverterVersion("gameProducer",versionNum) 可选

##### CommandEntityTest Authoring
+ CommandEntity -- 保存spawn & destroy配置信息的Entity, i.e. ScriptableObject as Factory
+ EntityCommandSystem{EntityCommandBuffer}
+ World 管理特殊的（同步点）EMBSystem
   + 同步点sync point更新
   ```CSharp
   私有引用构造System = World.GetOrCreateSystem<TEntityCommandBufferSystem>();
   ```
+ IDeclareReferencedPrefab 接口
   + 传入 List<GO> conversionSystem维护的当前版本prefab
   + 接口内部List.Add(authoredPrefab)
+ IConvertGameObjectToEntity 接口
   + World管理
+ ==Convert To Entity : Convert And Inject GO==
   + 在转换成GO的过程中，调用convertSys设置spawnData{Prefab}
   + 向dstManager添加spawnData