using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController
{
    void ManageMovement(ref Vector2 movementVector);

    void ManageJump(bool jumpingBool);

    void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint);
}
