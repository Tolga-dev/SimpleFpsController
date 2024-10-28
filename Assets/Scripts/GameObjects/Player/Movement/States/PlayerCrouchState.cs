using System;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerCrouchState : MovementStateBase
    {
        [SerializeField] private float crouchMultiplier = 0.5f;

        public override void Update()
        {
            movementController.move *= crouchMultiplier;
        }

    }
}