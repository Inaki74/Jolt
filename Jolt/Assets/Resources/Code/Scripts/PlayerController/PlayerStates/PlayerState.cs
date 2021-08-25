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
                protected virtual string AnimString => "";
                public virtual bool Flippable { get; set; } = true;

                protected float _enterTime;

                public PlayerState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData)
                {
                    this._stateMachine = stateMachine;
                    this._player = player;
                    this._playerData = playerData;
                }

                public virtual void Enter()
                {
                    //Debug.Log(this);
                    //_player.Sr.color = AssociatedColor;
                    _enterTime = Time.time;

                    if (!string.IsNullOrEmpty(AnimString) && !_player.GetAnimationBool(AnimString))
                    {
                        _player.SetAnimationBool(AnimString, true);
                    }
                }

                public virtual void Exit()
                {
                    if (!string.IsNullOrEmpty(AnimString))
                    {
                        if(string.IsNullOrEmpty(_stateMachine.NextState.AnimString) || _stateMachine.NextState.AnimString != AnimString)
                        {
                            _player.SetAnimationBool(AnimString, false);
                        }
                    }
                }

                protected virtual bool StateChangeCheck()
                {
                    return true;
                }

                protected virtual void PlayerControlAction()
                {

                }

                public void LogicUpdate()
                {
                    _stateMachine.CurrentState.StateChangeCheck();
                    _stateMachine.CurrentState.PlayerControlAction();
                    _player.Move(_player.Velocity);

                    _stateMachine.ChangeState();
                }

                public virtual void PhysicsUpdate()
                {
                    //_player.Move(_player.Velocity);
                }
            }
        }
    }
}