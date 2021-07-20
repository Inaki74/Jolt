using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class PreDashState : AliveState
            {
                private bool _isDashStarted;
                private float _currentTime;
                private int _amountOfDashes;

                public PreDashState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                    ResetAmountOfDashes();
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();

                    _isDashStarted = true;
                    Time.timeScale = _playerData.timeSlow;
                    _player.DrawCircle(_player.InputManager.InitialDashPoint, _playerData.circleRadius);
                    DecreaseAmountOfDashes();
                    //Time.fixedDeltaTime = 0.1f * 0.02f; Works but doubles the CPU usage. Use RigidBodies with interpolate instead
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.DeactivateArrowRendering();
                    _player.DeactivateCircleRendering();
                    Time.timeScale = 1f;
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _currentTime = Time.time;
                    _isDashStarted = _player.InputManager.DashBegin && (_currentTime - _enterTime < _playerData.preDashTimeOut);

                    _player.SetDashVectors(_player.InputManager.InitialDashPoint, _player.InputManager.FinalDashPoint);
                    _player.SetArrowRendering();

                    //Cant be Cancelled, go to dashing when stopped pressing or after timeout
                    if (!_isDashStarted)
                    {
                        // transition to dashing
                        _stateMachine.ChangeState(_stateMachine.DashingState);
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "PreDashState";
                }

                public bool CanDash()
                {
                    return _amountOfDashes > 0;
                }

                public void ResetAmountOfDashes()
                {
                    _amountOfDashes = _playerData.amountOfDashes;
                }

                public void DecreaseAmountOfDashes()
                {
                    _amountOfDashes--;
                }
            }
        }
    }
}

