using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting; //better performant vectors ie float2 instead of vector2
//structs are better for performance as they're allocated on the stack and not the heap
public struct Spawner : IComponentData
{

    public Entity Prefab; //representation of an object
    public float2 SpawnPosition; //vector2 from math
    public float NextSpawnTime; 
    public float SpawnRate;
    public float enemyCount;
    public float deathTime;
    public float WaveDelay;
    public bool isWave;


}


