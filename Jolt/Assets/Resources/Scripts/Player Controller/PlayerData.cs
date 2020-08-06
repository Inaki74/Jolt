using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State Variables")]
    public float movementSpeed = 10.0f;

    [Header("Checks Variables")]
    public float checkGroundRadius = 0.05f;
    public LayerMask whatIsGround;
}
