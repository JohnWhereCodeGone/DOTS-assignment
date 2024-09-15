using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float enemySpeed;
    public float lifeTimeEnemy = 10f;
    
    class EnemyAuthoringBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(enemyEntity, new LifeTime { Value = authoring.lifeTimeEnemy} );
            AddComponent(enemyEntity, new Target { });
            AddComponent(enemyEntity, new EnemyTag { });

        }
    }

}

public struct Target : IComponentData
{
    LocalTransform Value;
}

public struct EnemySpeed : IComponentData
{
    float Value;
}

public struct EnemyTag : IComponentData { }


