using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class In_RailState : ConductorState
            {
                protected override Color AssociatedColor => Color.cyan;

                private RailController _currentRail;

                private Vector2 _nextPath;

                private float t;

                private float _speed;

                private bool _exiting;
                private bool _reachedPath;

                public In_RailState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _playerData.AllPaths.Clear();
                    _exiting = false;
                    //_nextPath = _player.transform.position; // TODO: Breaks rail mechanic.
                    _currentRail = _player.GetRailInfo();

                    t = 0f;
                    _speed = _currentRail.railSpeed;
                }

                public override void Exit()
                {
                    base.Exit();

                    Vector2[] aux = _playerData.AllPaths.ToArray();

                    //Debug.Log("PreLast: (" + aux[aux.Length - 2].x + " , " + aux[aux.Length - 2].y + ") , Last: (" + _nextPath.x + " , " + _nextPath.y + ")");
                    _stateMachine.ExitRailState.ExitVector = _nextPath - aux[aux.Length - 2];
                    _stateMachine.ExitRailState.ExitSpeed = _speed;
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _reachedPath = _player.CheckHasReachedPoint(_nextPath);

                    if (t < 1 && _reachedPath)
                    {
                        t += Time.deltaTime * _speed / 10;

                        _nextPath = SplineHelperFunctions.SplineCurve(_currentRail.ControlPoints.Length - 1, 0, t, _currentRail.ControlPoints);
                        if (t > 0.8)
                            _playerData.AllPaths.Add(_nextPath);
                    }
                    else if (t >= 1)
                    {
                        t = 0;
                        _exiting = true;
                    }

                    _player.MoveTowardsVector(_nextPath, _speed);

                    if (_exiting)
                    {
                        _stateMachine.ChangeState(_stateMachine.ExitRailState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    //Follows _speed according to points
                    //if (t < 1)
                    //{
                    //    t += Time.fixedDeltaTime * _speed;

                    //    Vector3 aux = SplineHelperFunctions.SplineCurve(_currentRail.ControlPoints.Length - 1, 0, t, _currentRail.ControlPoints);

                    //    _nextPath.Set(aux.x, aux.y);

                    //    player.SetPosition(_nextPath);
                    //    player.CheckHasReachedPoint(_nextPath);
                    //}
                    //else
                    //{
                    //    t = 0f;

                    //    _exiting = true;
                    //}
                }

                public override string ToString()
                {
                    return "InRailState";
                }
            }
        }
    }
}


