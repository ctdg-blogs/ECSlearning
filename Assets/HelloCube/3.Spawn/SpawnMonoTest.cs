using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace ctdg
{
    [RequiresEntityConversion]
    public class SpawnMonoTest : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public GameObject PrefabRoot;
        public float3 SpawnSize;
        public float TimeSpan;
        //IDeclareReferencedPrefabs
        public void DeclareReferencedPrefabs(List<GameObject> PrefabRoots) //整个blobAsset.convertSys的List
        {
            PrefabRoots.Add(PrefabRoot);
        }
        //IConvertGameObjectToEntity
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem convertSys)
        {
            //new 后未AddComponentData导致Unity报错
            //A component with type:LifetimeData has not been added to the entity.
            //这里的entity是SpawnSystem
            var spawnData = new SpawnData
            {
                PrefabRoot = convertSys.GetPrimaryEntity(PrefabRoot), //UninitializedObject
                SpawnSize = SpawnSize,
                TimeSpan = TimeSpan
            };                       //spawnerEntity, spawnData
            dstManager.AddComponentData(entity, spawnData); //整个World的EntityManager:IBufferElementData
        }
    }
}

