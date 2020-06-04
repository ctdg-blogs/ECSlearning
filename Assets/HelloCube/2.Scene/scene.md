#### scene as packedEntityManagerSystem
~~gameObject as subScene~~ --> subscene

Hierarchy -> New Sub Scene -> 文件夹中选择位置

##### sceneSystem
+ 主场景默认创建GO，可以convert to entity
+ RAM最快虚拟页切换
+ ==在subScene内编辑== 
   + subScene==只创建Entity==
      + subScene脚本自动挂载convert to entity脚本
      + 根据 load/unload 选择 toGameObject/destroy
   + 暂时不支持（可以保存，EditorCollapse） 主场景编辑后save/ctrl+s
+ 默认ComponentData 
   + Transform
   + SubScene(Script)
+ 可选ComponentData
   + RequestSceneLoaded
+ hierarchy面板功能
   + LiveLink tick <--> 是否显示
   + hierarchy color <--> 面板上连线颜色
+ 分部显示功能
   + AutoLoad tick <--> gameView autoLoad
   + RequestSceneLoaded <--> gameView deferLoad
   + Load/Unload <--> editView
