using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ctdg
{
    [RequiresEntityConversion]
    public class LifetimeMonoTest : MonoBehaviour
    {
        public float TimeSpan;
        //IConvertGameObjectToEntity
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem convertSys)
        {
            var lifetimeData = new LifetimeData
            {
                TimeSpan = TimeSpan
            };                       //this convertedEntity, lifetimeData
            dstManager.AddComponentData(entity, lifetimeData); 
        }
    }
}

