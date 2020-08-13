using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 MovementVector { get; private set; }

    public bool DashBegin { get; private set; }

    public Vector3 InitialDashPoint { get; private set; }

    public Vector3 FinalDashPoint { get; private set; }

    private float dashCD = 0.2f;
    private float currentDashCD;


    private void Start()
    {
        currentDashCD = -1;
    }

    private void Update()
    {
        if (DashBegin && currentDashCD < 0)
        {
            FinalDashPoint = Touchscreen.current.position.ReadValue();
        }

        currentDashCD -= Time.deltaTime;
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if(context.ReadValue<Vector2>().x > 0)
        {
            MovementVector = Vector2.right;
        }else if (context.ReadValue<Vector2>().x < 0)
        {
            MovementVector = Vector2.left;
        }
        else
        {
            MovementVector = Vector2.zero;
        }
    }

    //Context -> value, phase(when it was started, performed and cancelled), 

    public void OnBeginDashInput(InputAction.CallbackContext context)
    {
        bool isValid = ValidateInitialDashPoint(Touchscreen.current.primaryTouch.position.ReadValue(), 50, 290, 300, 40);

        if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && isValid) && currentDashCD < 0)
        {
            DashBegin = true;
            InitialDashPoint = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        bool wasValid = ValidateInitialDashPoint(InitialDashPoint, 50, 290, 300, 40);

        if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && wasValid))
        {
            DashBegin = false;
            currentDashCD = dashCD;
        }
    }

    private bool ValidateInitialDashPoint(Vector2 point, float xLowerBound, float yHigherBound, float xHigherBound, float yLowerBound)
    {
        return point.x < xLowerBound || point.x > xHigherBound || point.y < yLowerBound || point.y > yHigherBound;
    }
}
