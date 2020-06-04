using Unity.Entities;
using UnityEngine;
//Prefab 或 测试GO时挂载
namespace ctdg
{
    [RequiresEntityConversion]
    public class MonoPlanetRotateBehavior : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float AngularVelocity;  //注意同一个GO/Entity的同名字段问题

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSys)
        {
            var data = new PlanetRotateBehavior { AngularVelocity = this.AngularVelocity }; //类型初始化器修改值，eg:math.radians
            dstManager.AddComponentData(entity, data);
        }
    }
}

