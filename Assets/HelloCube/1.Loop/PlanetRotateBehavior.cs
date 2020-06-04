using Unity.Entities;
using UnityEngine;


namespace ctdg
{
    //这个垃圾玩意有bug，拖拽上GO之后不可以改文件名字（内部Serialize）
    [GenerateAuthoringComponent] //[System.Serializable] 重复特性
    public struct PlanetRotateBehavior : IComponentData
    {
        public float AngularVelocity; //deg per second
    }

    //已声明成MonoBehaviour的class的便捷转换
    //i.e.: GO <--> Entity & 字段 <--> Data
    /*
    [RequiresEntityConversion]
    public class MonoPlanetRotateBehavior : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float AngularVelocity;  //注意同一个GO/Entity的同名字段问题

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSys)
        {
            var data = new PlanetRotateBehavior(); //可以用类型初始化器修改值，eg:math.radians
            dstManager.AddComponentData(entity, data); 
        }
    }
    */
}

