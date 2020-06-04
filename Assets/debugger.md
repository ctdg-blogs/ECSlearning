#### ECS Debuggers
Window -> Analysis
+ EntityDebugger
+ FrameDebugger
+ PhysicsDebugger
+ 大饼
   + Integration: Profiler <--> FrameDebugger
   + Logic Flow Map (蓝图？)
##### 左侧 System列表
+ in PlayerLoop -- Frame Debugger 
+ running time
+ 右上 World
##### 中间 选中的System/Chunk
+ (可以自定义)ComponentData as EntityName
+ QueryList
   + ComponentGroups <--> Query
   + ReadOnly -- Blue
   + RW -- Green
   + Subtractive -- Read
+ Entity
##### 右侧可选 ArchetypeChunkInfo
+ QueryList
+ 竖线 == 一个Chunk
+ 最下数字 == Chunk中Entity数量
+ 右上数字 == 涉及cpu数量
