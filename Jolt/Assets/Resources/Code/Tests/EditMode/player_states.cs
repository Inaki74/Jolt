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
                [Test]
                public void dead_state_goes_to_idle_state_if_player_revives()
                {
                    
                }

                [Test]
                public void entering_dead_state_triggers_death_event()
                {
                    // TODO
                }

                [Test]
                public void leaving_dead_state_respawns_player_in_right_position()
                {

                }

                [Test]
                public void entering_dead_state_instantiates_particles()
                {

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
                    _statemachineMock.DeadState.Returns(new DeadState(_statemachineMock, _playerMock, _playerDataMock));

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testMoveState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testMoveState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ChangeState(_statemachineMock.AirborneState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {
                    //TODO
                }

                [Test]
                public void other()
                {

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
                    _statemachineMock.DeadState.Returns(new DeadState(_statemachineMock, _playerMock, _playerDataMock));

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testIdleState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testIdleState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ChangeState(_statemachineMock.AirborneState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {

                }

                [Test]
                public void other()
                {

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
                    _statemachineMock.DeadState.Returns(new DeadState(_statemachineMock, _playerMock, _playerDataMock));

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.DeadState);
                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(false);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.Received().ChangeState(_statemachineMock.AirborneState);
                }

                [Test]
                public void being_grounded_does_not_lead_to_airborne_state()
                {
                    _playerMock.IsDead.Returns(false);
                    _playerMock.CheckIsGrounded().Returns(true);
                    _playerMock.InputManager.Returns(_playerInputManagerMock);
                    _playerInputManagerMock.MovementVector.Returns(Vector2.zero);
                    _playerInputManagerMock.DashBegin.Returns(false);
                    _statemachineMock.AirborneState.Returns(new AirborneState(_statemachineMock, _playerMock, _playerDataMock));

                    _testRecoilState.LogicUpdate();

                    _statemachineMock.DidNotReceive().ChangeState(_statemachineMock.AirborneState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
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

                    _statemachineMock.Received().ChangeState(_statemachineMock.PreDashState);
                }

                [Test]
                public void jumping_input_leads_to_jumping_state()
                {

                }

                [Test]
                public void other()
                {

                }
            }

            public class airborne_state
            {
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    IPlayer player = Substitute.For<IPlayer>();
                    player.IsDead.Returns(true);
                    IPlayerData playerData = Substitute.For<IPlayerData>();
                    IPlayerStateMachine stateMachine = Substitute.For<IPlayerStateMachine>();
                    stateMachine.DeadState.Returns(new DeadState(stateMachine, player, playerData));
                    AirborneState airborneState = new AirborneState(stateMachine, player, playerData);

                    airborneState.LogicUpdate();

                    stateMachine.Received().ChangeState(stateMachine.DeadState);
                }

                [Test]
                public void other()
                {

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
                    stateMachine.DeadState.Returns(new DeadState(stateMachine, player, playerData));
                    DashingState dashingState = new DashingState(stateMachine, player, playerData);

                    dashingState.LogicUpdate();

                    stateMachine.Received().ChangeState(stateMachine.DeadState);
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
                    stateMachine.DeadState.Returns(new DeadState(stateMachine, player, playerData));
                    PreDashState preDashState = new PreDashState(stateMachine, player, playerData);

                    preDashState.LogicUpdate();

                    stateMachine.Received().ChangeState(stateMachine.DeadState);
                }

                [Test]
                public void other()
                {

                }
            }
        }
    }
}
