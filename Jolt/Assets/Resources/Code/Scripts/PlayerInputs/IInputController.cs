using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController
{
    void ManageMovement(Vector2 movementVector);

    void ManageJump(bool jumpingBool);

    void ManageDash(Vector2 initialDashPoint, Vector2 finalDashPoint);
}
