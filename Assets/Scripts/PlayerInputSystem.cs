using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;


//System groups in order:
// Initialization System Group
// Simulation System Group
// Presentation System Group


[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)] //make system run in group
public partial class PlayerInputSystem : SystemBase
{

    private GameInput InputActions;
    private Entity Player;

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>(); //
        RequireForUpdate<PlayerMoveInput>(); //only updates when these components exist.
        InputActions = new GameInput();
    }

    protected override void OnStartRunning()
    {
        InputActions.Enable();
        InputActions.Gameplay.Shoot.performed += OnShoot;
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    private void OnShoot(InputAction.CallbackContext context) // if player exists, enable tag.
    {
        if (!SystemAPI.Exists(Player)) return;

        SystemAPI.SetComponentEnabled<FireProjectileTag>(Player, true);
    }

    protected override void OnUpdate()
    {
        Vector2 moveInput = InputActions.Gameplay.Move.ReadValue<Vector2>();
        SystemAPI.SetSingleton(new PlayerMoveInput { Value = moveInput }); //from PLayerAuthoring
    }

    protected override void OnStopRunning() //cleanup
    {
        InputActions.Disable();
        Player = Entity.Null;
    }
}
