using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//logic for spawner, always running
public partial struct SpawnerSystem : ISystem
{
    public float LocalEnemyCount;
    public RefRW<Spawner> localSpawner;
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
    public void OnUpdate(ref SystemState state) { 
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach(RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            localSpawner = spawner; //setting in each loop is not performant.
            if (spawner.ValueRO.isWave)
            {
                
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                float3 pos = new float3(spawner.ValueRO.SpawnPosition.x, spawner.ValueRO.SpawnPosition.y, 0);
                //float lifeVal = 6f;
                //ecb.AddComponent(newEntity, new LifeTime { Value = lifeVal });
                
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(pos));
                spawner.ValueRW.enemyCount++;

                spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;

                
            }
            if (spawner.ValueRO.enemyCount <= 1 && spawner.ValueRO.WaveDelay < SystemAPI.Time.ElapsedTime ) //includes spawnrate duration lmao
            {
                spawner.ValueRW.WaveDelay = (float)SystemAPI.Time.ElapsedTime + 10f;
                spawner.ValueRW.isWave = true;
            }
            if (spawner.ValueRW.enemyCount >= 10)
            {
                spawner.ValueRW.isWave = false;
                spawner.ValueRW.enemyCount = 10; //for some reason 1 would be left over every couple of waves, so I resorted to resetting this as a quick fix. would love to know why this happened though.
            }


        }
        foreach ((IsDestroying tag, Entity entity) in SystemAPI.Query<IsDestroying>().WithEntityAccess())
        {
            if (state.EntityManager.HasComponent<EnemyTag>(entity))
            {
                localSpawner.ValueRW.enemyCount--;
            }
        }

        ecb.Playback(state.EntityManager); //fuck these two functions
        ecb.Dispose();




        
    }
    private void AdjustEnemyCount(Spawner spawner, float count)
    {
        spawner.enemyCount = count;
    }

    //spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime








}
