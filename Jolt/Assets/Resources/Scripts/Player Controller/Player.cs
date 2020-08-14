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
    public In_RailState InRailState { get; private set; }
    public ExitRailState ExitRailState { get; private set; }
    public DeadState DeadState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public PlayerInputManager InputManager { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    public LineRenderer ArrowLr { get; private set; }
    public CircleCollider2D Cc{ get; private set; }
    [SerializeField] private Camera mainCamera;

    public GameObject deathParticles;
    #endregion

    #region Auxiliary Variables
    
    public Vector2 CurrentVelocity { get; private set; }
    public CircleHelperFunctions circleDrawer;
    public Transform groundCheckOne;
    public Transform groundCheckTwo;

    private Vector2 _auxVector;

    private Vector3 DashStart;
    private Vector3 DashFinish;

    private Vector3 cachedTransform;

    private bool isTouchingNode;
    private bool isTouchingRail;
    [SerializeField] private bool isDead;

    private Collider2D nodeInfo;
    private RailController firstRail;
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
        InRailState = new In_RailState(StateMachine, this, playerData, Color.cyan);
        ExitRailState = new ExitRailState(StateMachine, this, playerData, Color.magenta);
        DeadState = new DeadState(StateMachine, this, playerData, Color.clear);
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        InputManager = GetComponent<PlayerInputManager>();
        Cc = GetComponent<CircleCollider2D>();

        isDead = false;
        ArrowLr = GetComponent<LineRenderer>();
        ArrowLr.enabled = false;
        ArrowLr.startWidth = 0.3f; ArrowLr.endWidth = 0.001f;

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

        if((collision.tag == "Rail Entrance" || collision.tag == "Rail Exit") && StateMachine.GetState() == "DashingState")
        {
            isTouchingRail = true;
            firstRail = collision.GetComponentInParent<RailController>();

            SetPosition(collision.transform.position);

            if (collision.tag == "Rail Entrance" && firstRail.Inverted)
            {
                firstRail.InvertControlPoints();
            }

            if (collision.tag == "Rail Exit" && !firstRail.Inverted)
            {
                firstRail.InvertControlPoints();
            }
                
            collision.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        if(collision.tag == "Checkpoint")
        {
            playerData.lastCheckpoint = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Node")
        {
            isTouchingNode = false;
        }

        if (collision.tag == "Rail Entrance" || collision.tag == "Rail Exit")
        {
            isTouchingRail = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Node" && StateMachine.GetState() == "DashingState")
        {
            isTouchingNode = false;
            collision.GetComponent<SpriteRenderer>().color = Color.grey;
        }

        if (collision.tag == "Rail Entrance" || collision.tag == "Rail Exit")
        {
            collision.GetComponent<SpriteRenderer>().color = Color.grey;

            if (StateMachine.GetState() == "DashingState")
            {
                isTouchingRail = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Rubber")
        {
            isDead = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rubber")
        {
            isDead = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Vector2 center = new Vector2(1f, 0);
        //float radius = 4;

        //float x, y;
        //for (int i = 0; i <= 50; i += 1)
        //{
        //    x = radius * Mathf.Cos(Mathf.Rad2Deg * i) + center.x;
        //    y = radius * Mathf.Sin(Mathf.Rad2Deg * i) + center.y;

        //    Gizmos.DrawSphere(new Vector2(x, y), 0.1f);
        //}
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

    public void SetMovementXByForce(Vector2 direction, float speed)
    {
        _auxVector.Set(direction.x * speed, 0);
        Rb.AddForce(_auxVector, ForceMode2D.Force);
    }

    public void SetDashMovement(float velocity)
    {
        _auxVector = DashFinish - cachedTransform;
        DashFinish = _auxVector;
        _auxVector.Set(DashFinish.normalized.x, DashFinish.normalized.y);
        Rb.velocity = _auxVector * velocity;
        CurrentVelocity = _auxVector * velocity;
    }

    public void SetArrowRendering()
    {
        ArrowLr.enabled = true;

        Vector2 aux1 = DashStart;
        Vector2 aux2 = DashFinish;

        ArrowLr.SetPosition(0, aux1);
        ArrowLr.SetPosition(1, aux2);
    }

    public void SetDashVectors(Vector3 mouseStartPos, Vector3 mouseFinalPos)
    {
        //TODO: To later move the camera towards the mouse point, we may need to capture Camera.main at the moment of touch
        //TODO: Make the arrow length constant
        Debug.Log(mouseStartPos);
        Vector2 aux1 = mainCamera.ScreenToWorldPoint(mouseStartPos);
        Vector2 aux2 = mainCamera.ScreenToWorldPoint(mouseFinalPos);

        Vector2 dashTranslationVector = new Vector2(transform.position.x - aux1.x, transform.position.y - aux1.y);
        //Vector2 arrowTranslationVector = new Vector2(transform.position.normalized.x - aux1.normalized.x, transform.position.normalized.y - aux1.normalized.y);

        DashStart.Set(aux1.x + dashTranslationVector.x, aux1.y + dashTranslationVector.y, 0);
        DashFinish.Set(aux2.x + dashTranslationVector.x, aux2.y + dashTranslationVector.y, 0);

        //arrowStart.Set(aux1.x + arrowTranslationVector.x, aux1.y + arrowTranslationVector.y, 0);
        //arrowFinish.Set(aux2.x + arrowTranslationVector.x, aux2.y + arrowTranslationVector.y, 0);

        cachedTransform = transform.position;
    }

    public void SetVelocityToGivenVector(Vector2 v, float speed)
    {
        _auxVector.Set(v.x * speed, v.y * speed);
        Rb.velocity = _auxVector;
        CurrentVelocity = _auxVector;
    }

    public void SetForceToGivenVector(Vector2 v, float speed)
    {
        _auxVector.Set(v.x * speed, v.y * speed);
        Rb.AddForce(_auxVector, ForceMode2D.Impulse);
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

    public bool CheckHasReachedPoint(Vector2 point)
    {
        //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + point.x + " , " + point.y + ")");
        return (Vector2)transform.position == point;
    }

    public bool CheckIfDead()
    {
        return isDead;
    }
    #endregion

    #region Get Functions
    public Collider2D GetNodeInfo() { return nodeInfo; }

    public RailController GetRailInfo() { return firstRail; }

    public Vector2 GetCurrentVelocity() { return Rb.velocity; }
    #endregion

    #region Other Functions
    public void DeactivateArrowRendering()
    {
        ArrowLr.SetPosition(0, Vector2.zero);
        ArrowLr.SetPosition(1, Vector2.zero);

        ArrowLr.enabled = false;
    }

    public void DeactivateCircleRendering()
    {
        circleDrawer.DerenderCircle();
    }

    public void MoveTowardsVector(Vector2 v, float velocity)
    {
        //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + v.x + " , " + v.y + ")");
        _auxVector.Set(v.x, v.y);
        transform.position = Vector2.MoveTowards(transform.position, _auxVector, velocity * Time.deltaTime);
    }

    public void ResetPosition()
    {
        // Instantiate particles
        // Move player to last checkpoint (but here we will have only one checkpoint, so skip)
        transform.position = playerData.lastCheckpoint;
        // Reset objects (but here they are immutable so skip)
    }

    public void InstantiateDeathParticles()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

    public void DrawCircle(Vector2 mouseStartPos, float radius)
    {
        Vector2 center = mainCamera.ScreenToWorldPoint(mouseStartPos);

        circleDrawer.DrawCircle(center, radius);
    }
    #endregion
}
