using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]

//find entity with our tag and set it to false
public partial struct ResetInputSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach(var (tag, entity) in SystemAPI.Query<FireProjectileTag>().WithEntityAccess())
        {
            ecb.SetComponentEnabled<FireProjectileTag>(entity, false);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
