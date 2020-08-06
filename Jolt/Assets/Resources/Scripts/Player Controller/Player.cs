using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region States
    public PlayerStateMachine StateMachine { get; private set; }

    public MoveState MoveState { get; private set; }
    public IdleState IdleState { get; private set; }
    public AirborneState AirborneState { get; private set; }
    public RecoilState RecoilState { get; private set; }
    public PreDashState PreDashState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public PlayerInputManager InputManager { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    #endregion

    private Vector2 _auxVector;
    public Vector2 CurrentVelocity { get; private set; }

    public Transform groundCheckOne;
    public Transform groundCheckTwo;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        MoveState = new MoveState(StateMachine, this, playerData, Color.white);
        IdleState = new IdleState(StateMachine, this, playerData, Color.yellow);
        AirborneState = new AirborneState(StateMachine, this, playerData, Color.red);
        RecoilState = new RecoilState(StateMachine, this, playerData, Color.magenta);
        PreDashState = new PreDashState(StateMachine, this, playerData, Color.gray);
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        InputManager = GetComponent<PlayerInputManager>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetMovementX(float velocity)
    {
        _auxVector.Set(velocity, CurrentVelocity.y);
        Rb.velocity = _auxVector;
        CurrentVelocity = _auxVector;
    }

    public bool CheckIsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckOne.position, playerData.checkGroundRadius, playerData.whatIsGround)
            || Physics2D.OverlapCircle(groundCheckTwo.position, playerData.checkGroundRadius, playerData.whatIsGround);
    }

}
