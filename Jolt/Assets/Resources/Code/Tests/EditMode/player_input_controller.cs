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
        using PlayerController.PlayerInput;

        public class player_input_controller
        {
            private PlayerInputController _inputManager;
            private IInputController _inputController;

            [SetUp]
            public void Setup()
            {
                _inputManager = new PlayerInputController();
                _inputController = Substitute.For<IInputController>();
            }

            [Test]
            public void moving_right_makes_movement_vector_a_right_vector()
            {
                _inputController.Horizontal.Returns(1f);
                _inputManager.SetInputController(_inputController);
                Vector2 movementVector = Vector2.zero;

                _inputManager.ManageMovement(ref movementVector);

                Assert.AreEqual(Vector2.right, movementVector);
            }

            [Test]
            public void moving_upwards_doesnt_affect_movement_vector()
            {
                _inputController.Vertical.Returns(1f);
                _inputManager.SetInputController(_inputController);
                Vector2 movementVector = Vector2.zero;

                _inputManager.ManageMovement(ref movementVector);

                Assert.AreEqual(Vector2.zero, movementVector);
            }

            [Test]
            public void starting_dash_input_begins_dash()
            {
                _inputController.DashDown.Returns(true);
                _inputController.Pointer.Returns(false);
                _inputManager.SetInputController(_inputController);
                bool dashBegin = false;
                Vector3 finalDashPoint = Vector3.zero;

                _inputManager.ManageDash(ref dashBegin, ref finalDashPoint);

                Assert.IsTrue(dashBegin);
            }

            [Test]
            public void starting_dash_input_sets_initial_dashpoint_but_not_final()
            {
                _inputController.DashDown.Returns(true);
                _inputController.Pointer.Returns(false);
                _inputManager.SetInputController(_inputController);
                bool dashBegin = false;
                Vector3 finalDashPoint = Vector3.zero;

                _inputManager.ManageDash(ref dashBegin, ref finalDashPoint);

                Assert.AreEqual(Vector3.zero, finalDashPoint);
            }

            [Test]
            public void holding_dash_input_without_pointer_sets_final_dashpoint_to_vertical_and_horizontal()
            {
                _inputController.DashDown.Returns(true);
                _inputController.DashHold.Returns(true);
                _inputController.Pointer.Returns(false);
                _inputController.Vertical.Returns(-1f);
                _inputManager.SetInputController(_inputController);
                bool dashBegin = false;
                Vector3 finalDashPoint = Vector3.zero;

                _inputManager.ManageDash(ref dashBegin, ref finalDashPoint);

                Assert.AreEqual(Vector3.down, finalDashPoint);
            }

            [Test]
            public void releasing_dash_stops_dash()
            {
                _inputController.DashUp.Returns(true);
                _inputController.Pointer.Returns(false);
                _inputController.Vertical.Returns(-1f);
                _inputManager.SetInputController(_inputController);
                bool dashBegin = false;
                Vector3 finalDashPoint = Vector3.zero;

                _inputManager.ManageDash(ref dashBegin, ref finalDashPoint);

                Assert.IsFalse(dashBegin);
            }
        }
    }
}
