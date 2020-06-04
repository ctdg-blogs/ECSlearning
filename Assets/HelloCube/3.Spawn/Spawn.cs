using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms; //Translation,Rotation,Scale,CompositeXxx,NonUniformScale与parent无关
using UnityEngine;
using World = Unity.Entities.World;

namespace ctdg
{
    public class Spawn : MonoBehaviour
    {
        public GameObject PrefabRoot;
        public int CountX;
        public int CountZ;
        private float3 m_size = new float3(1.3F, 2F, 1.3F);

        // Start is called before the first frame update
        void Start()
        {
            //                                          FromEntityGUID,FromBlobAssetHashRef
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(PrefabRoot, settings);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            for (var x = 0; x < CountX; x++)
            {
                for (var z = 0; z < CountZ; z++)
                {
                    Entity instance = entityManager.Instantiate(prefab);//Instantiate Entity From EntityPrefab
                    float3 pos = transform.TransformPoint(new float3(x * m_size.x, 0, z * m_size.z));
                    entityManager.SetComponentData(instance, new Translation { Value = pos });
                }
            }
        }
    }
}

