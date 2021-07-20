using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class PlayerState
            {
                protected PlayerStateMachine _stateMachine;
                protected Player _player;
                protected PlayerData _playerData;
                protected Color _associatedColor; // string animBool

                protected float _enterTime;

                public PlayerState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor)
                {
                    this._stateMachine = stateMachine;
                    this._player = player;
                    this._playerData = playerData;
                    this._associatedColor = associatedColor;
                }

                public virtual void Enter()
                {
                    DoChecks();

                    _player.Sr.color = _associatedColor;
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