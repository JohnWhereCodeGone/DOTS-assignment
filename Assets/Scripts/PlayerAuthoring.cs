using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour //holding data for the entity to be converted in the baker
{
    public float MoveSpeed;

    public GameObject ProjectilePrefab;
    public float lifeTimeFloat = 1f;

    class PlayerAuthoringBaker : Baker<PlayerAuthoring> //authors are used to define what happens when you convert a gameobject to an entity
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerTag>(playerEntity);
            AddComponent<PlayerMoveInput>(playerEntity);

            AddComponent(playerEntity, new PlayerMoveSpeed
            {
                Value = authoring.MoveSpeed,
            });

            AddComponent<FireProjectileTag>(playerEntity);
            SetComponentEnabled<FireProjectileTag>(playerEntity,false);

            AddComponent(playerEntity, new ProjectilePrefab
            {
                Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
            });
            AddComponent(playerEntity, new ProjectileLifetime
            {
                Value = authoring.lifeTimeFloat
            });
        }
    }
}

public struct PlayerMoveInput : IComponentData //holds input value as a float 2
{
    public float2 Value;

}

public struct PlayerMoveSpeed : IComponentData //for movespeed
{
    public float Value;
}

public struct PlayerTag : IComponentData { } //find it easily

public struct ProjectilePrefab : IComponentData //holder for bullet projectile
{
    public Entity Value;
}

public struct ProjectileMoveSpeed : IComponentData //just speed
{
    public float Value;
}

public struct FireProjectileTag : IComponentData, IEnableableComponent { } //makes it so we can toggle the component onoff

public struct ProjectileLifetime : IComponentData
{
    public float Value;
}

public struct LifeTime : IComponentData
{
    public float Value;
}

public struct IsDestroying : IComponentData { }