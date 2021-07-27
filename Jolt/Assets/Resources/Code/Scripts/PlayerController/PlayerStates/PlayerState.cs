using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class PlayerState
            {
                protected PlayerStateMachine _stateMachine;
                protected Player _player;
                protected PlayerData _playerData;
                protected virtual Color AssociatedColor => Color.black;

                protected float _enterTime;

                public PlayerState(PlayerStateMachine stateMachine, Player player, PlayerData playerData)
                {
                    this._stateMachine = stateMachine;
                    this._player = player;
                    this._playerData = playerData;
                }

                public virtual void Enter()
                {
                    DoChecks();

                    _player.Sr.color = AssociatedColor;
                    _enterTime = Time.time;
                }

                public virtual void Exit()
                {

                }

                public virtual void LogicUpdate()
                {
                }

                public virtual void PhysicsUpdate()
                {
                    DoChecks();
                }

                public virtual void DoChecks()
                {

                }
            }
        }
    }
}