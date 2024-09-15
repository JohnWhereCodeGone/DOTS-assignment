using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]

//queuing up the creating and setting of the projectile
public partial struct FireProjectileSystem : ISystem
{
    //var is for nerds
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob); //array that stores commands to be run on entities
        foreach((ProjectilePrefab projectilePrefab, LocalTransform transform, ProjectileLifetime life) in SystemAPI.Query<ProjectilePrefab, LocalTransform, ProjectileLifetime>().WithAll<FireProjectileTag>()) 
        {
            Entity newProjectile = ecb.Instantiate(projectilePrefab.Value);
            LocalTransform ProjectileTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
            ProjectileTransform.Scale = 0.3f;
            ecb.SetComponent(newProjectile, ProjectileTransform);
            ecb.AddComponent(newProjectile, new LifeTime { Value = life.Value });
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
