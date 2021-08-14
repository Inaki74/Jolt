using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

namespace Jolt
{
    namespace Tests
    {
        using PlayerController;
        using PlayerController.PlayerStates;

        public class player_states
        {
            public class dead_state
            {
                private IPlayer _playerMock;
                private IPlayerStateMachine _statemachineMock;
                private IPlayerInputManager _playerInputManagerMock;
                private IPlayerData _playerDataMock;
                private DeadState _testDeadState;

                [SetUp]
                public void Setup()
                {
                    _playerMock = Substitute.For<IPlayer>();
                    _playerDataMock = Substitute.For<IPlayerData>();
                    _statemachineMock = Substitute.For<IPlayerStateMachine>();
                    _playerInputManagerMock = Substitute.For<IPlayerInputManager>();
                    _testDeadState = new DeadState(_statemachineMock, _playerMock, _playerDataMock);
                }

                [Test]
                public void dead_state_goes_to_idle_state_if_player_revives()
                {
                    _playerDataMock.DeadTimer.Returns(0f);
                    var idleStateReturns = new IdleState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.IdleState.Returns(idleStateReturns);

                    _testDeadState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.IdleState);
                }

                [Test]
                public void entering_dead_state_triggers_death_event()
                {
                    // TODO
                }

                [Test]
                public void leaving_dead_state_respawns_player_in_right_position()
                {
                    _testDeadState.Exit();

                    _playerMock.Received().ResetPosition();
                }
            }

            public class move_state
            {
                private IPlayer _playerMock;
                private IPlayerStateMachine _statemachineMock;
                private IPlayerInputManager _playerInputManagerMock;
                private IPlayerData _playerDataMock;
                private MoveState _testMoveState;

                [SetUp]
                public void Setup()
                {
                    _playerMock = Substitute.For<IPlayer>();
                    _playerDataMock = Substitute.For<IPlayerData>();
                    _statemachineMock = Substitute.For<IPlayerStateMachine>();
                    _playerInputManagerMock = Substitute.For<IPlayerInputManager>();
                    _testMoveState = new MoveState(_statemachineMock, _playerMock, _playerDataMock);
                }

                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    _playerMock.IsDead.Returns(true);
                    var deadStateReturns = new DeadState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.DeadState.Returns(deadStateReturns);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void dashing_input_while_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void dashing_input_while_not_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {
                    //TODO
                }

                [Test]
                public void not_moving_leads_to_idle_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var returnsIdleState = new IdleState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.IdleState.Returns(returnsIdleState);

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.IdleState);
                }
            }

            public class idle_state
            {
                private IPlayer _playerMock;
                private IPlayerStateMachine _statemachineMock;
                private IPlayerInputManager _playerInputManagerMock;
                private IPlayerData _playerDataMock;
                private IdleState _testIdleState;

                [SetUp]
                public void Setup()
                {
                    _playerMock = Substitute.For<IPlayer>();
                    _playerDataMock = Substitute.For<IPlayerData>();
                    _statemachineMock = Substitute.For<IPlayerStateMachine>();
                    _playerInputManagerMock = Substitute.For<IPlayerInputManager>();
                    _testIdleState = new IdleState(_statemachineMock, _playerMock, _playerDataMock);
                }

                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    _playerMock.IsDead.Returns(true);
                    var deadStateReturns = new DeadState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.DeadState.Returns(deadStateReturns);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void dashing_input_while_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void dashing_input_while_not_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {
                    // TODO
                }

                [Test]
                public void moving_leads_to_move_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.right);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var returnsMoveState = new MoveState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.MoveState.Returns(returnsMoveState);

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.MoveState);
                }
            }

            public class recoil_state
            {
                private IPlayer _playerMock;
                private IPlayerStateMachine _statemachineMock;
                private IPlayerInputManager _playerInputManagerMock;
                private IPlayerData _playerDataMock;
                private RecoilState _testRecoilState;

                [SetUp]
                public void Setup()
                {
                    _playerMock = Substitute.For<IPlayer>();
                    _playerDataMock = Substitute.For<IPlayerData>();
                    _statemachineMock = Substitute.For<IPlayerStateMachine>();
                    _playerInputManagerMock = Substitute.For<IPlayerInputManager>();
                    _testRecoilState = new RecoilState(_statemachineMock, _playerMock, _playerDataMock);
                }

                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    _playerMock.IsDead.Returns(true);
                    var deadStateReturns = new DeadState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.DeadState.Returns(deadStateReturns);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    var airborneStateReturns = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.AirborneState.Returns(airborneStateReturns);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ScheduleStateChange(_statemachineMock.AirborneState);
                }

                [Test]
                public void dashing_input_while_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void dashing_input_while_not_grounded_leads_to_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    var returnsPreDash = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(returnsPreDash);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {

                }

                [Test]
                public void moving_leads_to_move_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.right);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _playerDataMock.RecoilTimer.Returns(0f);
                    var returnsMoveState = new MoveState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.MoveState.Returns(returnsMoveState);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.MoveState);
                }

                [Test]
                public void not_moving_leads_to_idle_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _playerDataMock.RecoilTimer.Returns(0f);
                    var returnsIdleState = new IdleState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.IdleState.Returns(returnsIdleState);

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.IdleState);
                }
            }

            public class airborne_state
            {
                private IPlayer _playerMock;
                private IPlayerStateMachine _statemachineMock;
                private IPlayerInputManager _playerInputManagerMock;
                private IPlayerData _playerDataMock;
                private AirborneState _testAirborneState;

                [SetUp]
                public void Setup()
                {
                    _playerMock = Substitute.For<IPlayer>();
                    _playerDataMock = Substitute.For<IPlayerData>();
                    _statemachineMock = Substitute.For<IPlayerStateMachine>();
                    _playerInputManagerMock = Substitute.For<IPlayerInputManager>();
                    _testAirborneState = new AirborneState(_statemachineMock, _playerMock, _playerDataMock);
                }

                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    _playerMock.IsDead.Returns(true);
                    var deadStateReturns = new DeadState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.DeadState.Returns(deadStateReturns);

                    _testAirborneState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.DeadState);
                }

                [Test]
                public void hitting_ground_results_in_recoil_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    ICanDash preStateCanDash = Substitute.For<PreDashState>(_statemachineMock, _playerMock, _playerDataMock);
                    preStateCanDash.CanDash().Returns(true);
                    var recoilStateReturns = new RecoilState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(preStateCanDash);
                    _statemachineMock.RecoilState.Returns(recoilStateReturns);

                    _testAirborneState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.RecoilState);
                }

                [Test]
                public void activating_dash_while_grounded_results_in_pre_dash_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerDataMock.AmountOfDashes.Returns(2);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(true);
                    var preDashStateReturns = new PreDashState(_statemachineMock, _playerMock, _playerDataMock);
                    _statemachineMock.PreDashState.Returns(preDashStateReturns);

                    _testAirborneState.LogicUpdate();

                    _statemachineMock.Received().ScheduleStateChange(_statemachineMock.PreDashState);
                }
            }

            public class dashing_state
            {
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    IPlayer player = Substitute.For<IPlayer>();
                    player.IsDead.Returns(true);
                    IPlayerData playerData = Substitute.For<IPlayerData>();
                    IPlayerStateMachine stateMachine = Substitute.For<IPlayerStateMachine>();
                    var deadStateReturns = new DeadState(stateMachine, player, playerData);
                    stateMachine.DeadState.Returns(deadStateReturns);
                    DashingState dashingState = new DashingState(stateMachine, player, playerData);

                    dashingState.LogicUpdate();

                    stateMachine.Received().ScheduleStateChange(stateMachine.DeadState);
                }

                [Test]
                public void other()
                {

                }
            }

            public class pre_dash_state
            {
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    IPlayer player = Substitute.For<IPlayer>();
                    player.IsDead.Returns(true);
                    IPlayerData playerData = Substitute.For<IPlayerData>();
                    IPlayerStateMachine stateMachine = Substitute.For<IPlayerStateMachine>();
                    var deadStateReturns = new DeadState(stateMachine, player, playerData);
                    stateMachine.DeadState.Returns(deadStateReturns);
                    PreDashState preDashState = new PreDashState(stateMachine, player, playerData);

                    preDashState.LogicUpdate();

                    stateMachine.Received().ScheduleStateChange(stateMachine.DeadState);
                }

                [Test]
                public void other()
                {

                }
            }
        }
    }
}
