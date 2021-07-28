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
                protected IPlayerStateMachine _stateMachine;
                protected IPlayer _player;
                protected IPlayerData _playerData;
                protected virtual Color AssociatedColor => Color.black;

                protected float _enterTime;

                public PlayerState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData)
                {
                    this._stateMachine = stateMachine;
                    this._player = player;
                    this._playerData = playerData;
                }

                public virtual void Enter()
                {
                    _player.Sr.color = AssociatedColor;
                    _enterTime = Time.time;
                }

                public virtual void Exit()
                {

                }

                public virtual bool LogicUpdate()
                {
                    return true;
                }

                public virtual void PhysicsUpdate()
                {
                }
            }
        }
    }
}