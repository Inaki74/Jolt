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
            [SetUp]
            public void Setup()
            {
            }

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
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {
                    IPlayer player = Substitute.For<IPlayer>();
                    PlayerData playerData = Substitute.For<PlayerData>();
                    PlayerStateMachine stateMachine = Substitute.For<PlayerStateMachine>(player, playerData);
                    MoveState movingState = new MoveState(stateMachine, player, playerData);


                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {

                }

                [Test]
                public void dashing_input_leads_to_pre_dash_state()
                {

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

            public class idle_state
            {
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {

                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {

                }

                [Test]
                public void dashing_input_leads_to_pre_dash_state()
                {

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
                [Test]
                public void alive_state_goes_to_dead_state_if_player_dies()
                {

                }

                [Test]
                public void not_being_grounded_leads_to_airborne_state()
                {

                }

                [Test]
                public void dashing_input_leads_to_pre_dash_state()
                {

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

                }

                [Test]
                public void other()
                {

                }
            }
        }
    }
}
