using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State Variables")]
    public float movementSpeed = 10.0f;

    [Header("Pre-Dash State Variables")]
    public float timeSlow = 0.1f;
    public float preDashTimeOut;
    public int amountOfDashes;

    [Header("Dashing State Variables")]
    public float dashTimeOut;
    public float dashSpeed;

    [Header("Checks Variables")]
    public float checkGroundRadius = 0.05f;
    public LayerMask whatIsGround;
}
