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
        runOnce = false;
        found = false;
    }

    private void Update()
    {
        if (currentDashCD > 0)
        {
            currentDashCD -= Time.deltaTime;
        }

    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if(context.ReadValue<Vector2>().x > 0)
        {
            MovementVector = Vector2.right;
            moving = true;
        }else if (context.ReadValue<Vector2>().x < 0)
        {
            MovementVector = Vector2.left;
            moving = true;
        }
        else
        {
            MovementVector = Vector2.zero;
            moving = false;
        }
    }

    //Context -> value, phase(when it was started, performed and cancelled), 
    private bool runOnce;
    private bool found;
    private bool moving;
    private int foundTouchId;

    public void OnBeginDashInput(InputAction.CallbackContext context)
    {
        //if (context.started)
        //{
        //    Debug.Log("Started: " + context.started);
        //}
        //if (context.performed) {
        //    Debug.Log("Performed: " + context.performed);
        //}
        //if (context.canceled) {
        //    Debug.Log("Ended: " + context.canceled);
        //}

        Debug.Log("pene");

        if (moving)
        {
            StartCoroutine("CoAux");
        }
        else
        {
            if (!runOnce)
            {
                StartCoroutine("CoManageTouches");
            }
        }
        
    }

    private IEnumerator CoAux()
    {
        while (moving)
        {
            if (!runOnce)
            {
                StartCoroutine("CoManageTouches");
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    private IEnumerator CoManageTouches()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count && !found; i++)
        {
            //Find the first touch that is not using the joystick and is not null
            if (ValidateInitialDashPoint(Touchscreen.current.touches[i].startPosition.ReadValue(), 80, 340, 360, 110) &&
                Touchscreen.current.touches[i].startPosition.ReadValue() != Vector2.zero)
            {
                runOnce = true;
                found = true;

                if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD < 0)
                {
                    DashBegin = true;
                    InitialDashPoint = Touchscreen.current.touches[i].startPosition.ReadValue();
                }

                while (DashBegin)
                {
                    FinalDashPoint = Touchscreen.current.touches[i].position.ReadValue();

                    if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        DashBegin = false;
                        currentDashCD = dashCD;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        found = false;
        runOnce = false;
    }

    //Returns false if its inside the selected bounds (not valid)
    private bool ValidateInitialDashPoint(Vector2 point, float xLowerBound, float yHigherBound, float xHigherBound, float yLowerBound)
    {
        return !(point.x > xLowerBound && point.x < xHigherBound && point.y > yLowerBound && point.y < yHigherBound);
    }

    //TRY 1:
    //This is a working touch, just in case ill leave it here
    //bool isValid = ValidateInitialDashPoint(Touchscreen.current.primaryTouch.position.ReadValue(), 50, 290, 300, 40);

    //            if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && isValid) && currentDashCD< 0)
    //            {
    //                DashBegin = true;
    //                InitialDashPoint = Touchscreen.current.primaryTouch.position.ReadValue();
    //            }

    //            bool wasValid = ValidateInitialDashPoint(InitialDashPoint, 50, 290, 300, 40);

    //            if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && wasValid))
    //            {
    //                DashBegin = false;
    //                currentDashCD = dashCD;
    //            }

    //TRY 2:
    //if (Touchscreen.current.touches[foundTouch].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD< 0)
    //        {
    //            DashBegin = true;
    //            InitialDashPoint = Touchscreen.current.touches[foundTouch].startPosition.ReadValue();
    //        }

    //        if (Touchscreen.current.touches[foundTouch].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
    //        {
    //            DashBegin = false;
    //            currentDashCD = dashCD;
    //        }

    //TRY 3(best competitor):
    //    Debug.Log("Run1");
    //        if (!runOnce)
    //        {
    //            Debug.Log("Run2");
    //            StartCoroutine("CoManageTouches");
    //}

    

    //TRY 4:
    //foundTouchId = DecideTouch();

    //UnityEngine.InputSystem.Controls.TouchControl foundTouch = FindTouch(Touchscreen.current.touches.ToArray(), foundTouchId);
    ////runOnce = true;

    //Debug.Log(foundTouch);

    //        if(foundTouch != null)
    //        {
    //            Debug.Log(foundTouch.phase.ReadValue());
    //            if (foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD< 0)
    //            {
    //                DashBegin = true;
    //                InitialDashPoint = foundTouch.startPosition.ReadValue();
    //            }

    //            if(foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
    //            {
    //                FinalDashPoint = foundTouch.position.ReadValue();
    //            }

    //            if (foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
    //            {
    //                DashBegin = false;
    //                //runOnce = false;
    //                currentDashCD = dashCD;
    //            }
    //        }
    //        else
    //        {
    //            runOnce = false;
    //            DashBegin = false;
    //        }

    private int DecideTouch()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //Find all the touches that are not using the joystick, use the first one
            if (ValidateInitialDashPoint(Touchscreen.current.touches[i].startPosition.ReadValue(), 50, 290, 300, 40) &&
                Touchscreen.current.touches[i].startPosition.ReadValue() != Vector2.zero)
                return Touchscreen.current.touches[i].touchId.ReadValue();
        }

        return -1;
    }

    private UnityEngine.InputSystem.Controls.TouchControl FindTouch(UnityEngine.InputSystem.Controls.TouchControl[] touches, int id)
    {
        for (int i = 0; i < touches.Length; i++)
        {
            //Find all the touches that are not using the joystick, use the first one
            if (touches[i].touchId.ReadValue() == id)
                return touches[i];
        }

        return null;
    }
}









