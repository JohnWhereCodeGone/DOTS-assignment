using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


//Create the entity

//when you are compiling this scene, we want this code to run
// we want the baker that's atttached to be used for this specific script on this specific entity
// so it's going to run this code when it's creating the entity.
public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;
    public float inspectorWaveDuration = 15f;

    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); //dynamic / none = move / don't move

            AddComponent(entity, new Spawner
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnPosition = float2.zero,
                NextSpawnTime = 0,
                SpawnRate = authoring.SpawnRate,
                enemyCount = 0,
                WaveDelay = authoring.inspectorWaveDuration,
                
                
            });
           
        }
    }


}
