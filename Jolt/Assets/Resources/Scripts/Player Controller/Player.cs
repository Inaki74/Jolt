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
    public DashingState DashingState { get; private set; }
    public In_NodeState InNodeState { get; private set; }
    public ExitNodeState ExitNodeState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public PlayerInputManager InputManager { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    public LineRenderer Lr { get; private set; }
    public CircleCollider2D Cc{ get; private set; }
    [SerializeField] private Camera mainCamera;
    #endregion

    #region Auxiliary Variables
    private Vector2 _auxVector;
    public Vector2 CurrentVelocity { get; private set; }
    public Transform groundCheckOne;
    public Transform groundCheckTwo;

    private Vector3 DashStart;
    private Vector3 DashFinish;

    private Vector3 cachedTransform;

    [SerializeField] private bool isTouchingNode;
    private bool isTouchingRail;

    private Collider2D nodeInfo;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        MoveState = new MoveState(StateMachine, this, playerData, Color.white);
        IdleState = new IdleState(StateMachine, this, playerData, Color.yellow);
        AirborneState = new AirborneState(StateMachine, this, playerData, Color.red);
        RecoilState = new RecoilState(StateMachine, this, playerData, Color.magenta);
        PreDashState = new PreDashState(StateMachine, this, playerData, Color.gray);
        DashingState = new DashingState(StateMachine, this, playerData, Color.cyan);
        InNodeState = new In_NodeState(StateMachine, this, playerData, Color.clear);
        ExitNodeState = new ExitNodeState(StateMachine, this, playerData, Color.magenta);
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        InputManager = GetComponent<PlayerInputManager>();
        Lr = GetComponent<LineRenderer>();
        Cc = GetComponent<CircleCollider2D>();


        Lr.enabled = false;
        Lr.startWidth = 0.3f; Lr.endWidth = 0.001f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Node" && StateMachine.GetState() == "DashingState")
        {
            isTouchingNode = true;
            nodeInfo = collision;
            collision.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        if(collision.tag == "Rail" && StateMachine.GetState() == "DashingState")
        {
            isTouchingRail = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Node")
        {
            isTouchingNode = false;
        }

        if (collision.tag == "Rail" && StateMachine.GetState() == "DashingState")
        {
            //isTouchingRail = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Node" && StateMachine.GetState() == "DashingState")
        {
            isTouchingNode = false;
            collision.GetComponent<SpriteRenderer>().color = Color.grey;
        }

        if (collision.tag == "Rail" && StateMachine.GetState() == "DashingState")
        {
            isTouchingRail = false;
        }
    }
    #endregion

    #region Set Functions
    public void SetMovementX(float velocity)
    {
        _auxVector.Set(velocity, CurrentVelocity.y);
        Rb.velocity = _auxVector;
        CurrentVelocity = _auxVector;
    }

    public void SetMovementY(float velocity)
    {
        _auxVector.Set(CurrentVelocity.x, velocity);
        Rb.velocity = _auxVector;
        CurrentVelocity = _auxVector;
    }

    public void SetDashMovement(float velocity)
    {
        //cachedTransform = transform.position
        _auxVector = DashFinish - cachedTransform;
        DashFinish = _auxVector;
        _auxVector.Set(DashFinish.normalized.x, DashFinish.normalized.y);
        Rb.velocity = _auxVector * velocity;
        CurrentVelocity = _auxVector * velocity;
    }

    public void SetArrowRendering()
    {
        Lr.enabled = true;

        //violeta
        Lr.SetPosition(0, DashStart);
        Lr.SetPosition(1, DashFinish);
    }

    public void SetDashVectors(Vector3 mouseStartPos, Vector3 mouseFinalPos)
    {
        //To later move the camera towards the mouse point, we may need to capture Camera.main at the moment of touch
        Vector2 aux1 = mainCamera.ScreenToWorldPoint(mouseStartPos);
        Vector2 aux2 = mainCamera.ScreenToWorldPoint(mouseFinalPos);

        Vector2 translationVector = new Vector2(transform.position.x - aux1.x, transform.position.y - aux1.y);

        DashStart.Set(aux1.x + translationVector.x, aux1.y + translationVector.y, 0);
        DashFinish.Set(aux2.x + translationVector.x, aux2.y + translationVector.y, 0);

        cachedTransform = transform.position;
    }

    public void SetGravityScale(float gravity) { Rb.gravityScale = gravity; }

    public void SetPosition(Vector2 pos) { transform.position = pos; }

    public void SetActivePhysicsCollider(bool set) { Cc.enabled = set; }

    public void SetActiveSpriteRenderer(bool set) { Sr.enabled = set; }
    #endregion

    #region Check Functions
    public bool CheckIsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckOne.position, playerData.checkGroundRadius, playerData.whatIsGround)
            || Physics2D.OverlapCircle(groundCheckTwo.position, playerData.checkGroundRadius, playerData.whatIsGround);
    }

    public bool CheckIsTouchingNode() { return isTouchingNode; }

    public bool CheckIsTouchingRail() { return isTouchingRail; }
    #endregion

    public Collider2D GetNodeInfo() { return nodeInfo; }

    public void DeactivateArrowRendering()
    {
        Lr.SetPosition(0, Vector2.zero);
        Lr.SetPosition(1, Vector2.zero);

        Lr.enabled = false;
    }

}
