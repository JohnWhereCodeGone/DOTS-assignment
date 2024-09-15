using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


[UpdateBefore(typeof(TransformSystemGroup))]
//jobs create code that delegates on another thread to then run and return with the completed task.
public partial struct PlayerMoveSystem : ISystem
{

   [BurstCompile] //it marks the code, into raw machine code, more performant. used for jobs.
   public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime; //adheres to delta time per Entity instead of globally.
        new PlayerMoveJob //jobs are delegated on different cpu threads to run and return
        {
            DeltaTime = deltaTime,
        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity
{
    public float DeltaTime;
    
    [BurstCompile]
    private void Execute(ref LocalTransform transform, in PlayerMoveInput input, PlayerMoveSpeed speed)
    {
        transform.Position.xy += input.Value * speed.Value * DeltaTime;
    }
}