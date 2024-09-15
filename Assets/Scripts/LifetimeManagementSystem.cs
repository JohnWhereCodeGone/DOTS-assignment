using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using System.Linq.Expressions;

[BurstCompile]

[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
public partial struct IsDestroyingManagementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsDestroying>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((IsDestroying tag, Entity entity) in SystemAPI.Query<IsDestroying>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);

            if (SystemAPI.HasComponent<EnemyTag>(entity))
            {
                Entity SpawnerEntity = SystemAPI.GetSingletonEntity<Spawner>();
                Spawner spawnComponent = SystemAPI.GetComponent<Spawner>(SpawnerEntity);
                spawnComponent.enemyCount--;
                ecb.SetComponent(SpawnerEntity, spawnComponent);
            }
            
        }

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public partial struct LifetimeManagementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<LifeTime>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        float tempDeltaTime = SystemAPI.Time.DeltaTime;
        new LifeJob
        {
            ecb = ecb,
            DeltaTime = tempDeltaTime,
        }.Schedule();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}



[BurstCompile]
public partial struct LifeJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public float DeltaTime;
    [BurstCompile]
    public void Execute(Entity entity, ref LifeTime lifeTime) //lifetime component lets it find the projectile
    {
        lifeTime.Value -= DeltaTime;
        
        if (lifeTime.Value <= 0)
        {
            ecb.AddComponent<IsDestroying>(entity);
        }
        
    }
}
