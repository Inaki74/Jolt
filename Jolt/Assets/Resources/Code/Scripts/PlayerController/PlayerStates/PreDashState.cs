using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class PreDashState : AliveState, ICanDash
            {
                protected override Color AssociatedColor => Color.gray;
                protected override string AnimString => PlayerAnimations.Constants.PREDASH_BOOL;

                private bool _isDashStarted;
                private bool _isTouchingNode;
                private float _currentTime;
                private int _amountOfDashes;

                public PreDashState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                    ResetAmountOfDashes();
                }

                public override void Enter()
                {
                    base.Enter();

                    _isDashStarted = true;
                    Time.timeScale = _playerData.TimeSlow;
                    DecreaseAmountOfDashes();
                    //Time.fixedDeltaTime = 0.1f * 0.02f; Works but doubles the CPU usage. Use RigidBodies with interpolate instead
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.DeactivateArrowRendering();
                    Time.timeScale = 1f;
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;
                    _isDashStarted = _player.InputManager.DashBegin && (_currentTime - _enterTime < _playerData.PreDashTimeOut);
                    _isTouchingNode = _player.CheckIsTouchingNode();

                    _player.SetDashVectors(_player.InputManager.InitialDashPoint, _player.InputManager.FinalDashPoint);
                    _player.SetArrowRendering();


                    if (_isTouchingNode)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.InNodeState);
                        return false;
                    }

                    //Cant be Cancelled, go to dashing when stopped pressing or after timeout
                    if (!_isDashStarted)
                    {
                        // transition to dashing
                        _stateMachine.ScheduleStateChange(_stateMachine.DashingState);
                        return false;
                    }

                    return true;
                }

                public bool CanDash()
                {
                    return _amountOfDashes > 0;
                }

                public void ResetAmountOfDashes()
                {
                    _amountOfDashes = _playerData.AmountOfDashes;
                }

                public void DecreaseAmountOfDashes()
                {
                    _amountOfDashes--;
                }

                public override string ToString()
                {
                    return "PreDashString";
                }
            }
        }
    }
}

