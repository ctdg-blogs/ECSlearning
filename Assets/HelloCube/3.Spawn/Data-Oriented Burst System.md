#### Data-Oriented Tech System
Entity=类，Data=成员字段，job=成员方法=Task + 执行上下文
System=TaskScheduler=BatchedTask/

##### 变长/定长Entity数组
+ IComponentData --> EntityQuery
+ IBufferElementData --> EntityQueryBuilder & ToEntityQuery() --> Archetype不友好，尽量通过保存parent索引替代