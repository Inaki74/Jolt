using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components

    [SerializeField]
    private PlayerData playerData;
    private PlayerCollisions playerCollisions;
    private PlayerArrowRendering playerArrowRendering;
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerInputManager InputManager { get; private set; }
    public CircleHelperFunctions circleDrawer;

    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    public CircleCollider2D Cc{ get; private set; }
    
    [SerializeField] private Camera mainCamera;

    public GameObject deathParticles;
    #endregion

    #region Auxiliary Variables
    public Vector2 checkpoint;

    public Vector2 CurrentVelocity { get; private set; }
    
    public Transform groundCheckOne;
    public Transform groundCheckTwo;

    private Vector3 DashStart;
    private Vector3 DashFinish;

    private Vector2 _auxVector2;
    private Vector3 _auxVector3;

    [SerializeField] public bool IsDead { private get; set; } = false;

    
    #endregion

    #region Unity Callback Functions

    private void Start()
    {
        //Application.targetFrameRate = 60; // This shouldnt be here, its for testing purposes.
        GetComponents();
        StateMachine.Initialize();
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
        playerCollisions.TriggerEnter(collision, StateMachine.CurrentState);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        playerCollisions.TriggerStay(collision, StateMachine.CurrentState);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCollisions.TriggerExit(collision, StateMachine.CurrentState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerCollisions.CollisionEnter(collision, StateMachine.CurrentState);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerCollisions.CollisionExit(collision, StateMachine.CurrentState);
    }

    private void GetComponents()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        InputManager = GetComponent<PlayerInputManager>();
        Cc = GetComponent<CircleCollider2D>();

        StateMachine = new PlayerStateMachine(this, playerData);
        playerCollisions = new PlayerCollisions(this);
        playerArrowRendering = new PlayerArrowRendering(GetComponent<LineRenderer>());
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
        _auxVector2.Set(velocity, CurrentVelocity.y);
        Rb.velocity = _auxVector2;
        CurrentVelocity = _auxVector2;
    }

    public void SetMovementY(float velocity)
    {
        _auxVector2.Set(CurrentVelocity.x, velocity);
        Rb.velocity = _auxVector2;
        CurrentVelocity = _auxVector2;
    }

    public void SetMovementXByForce(Vector2 direction, float speed)
    {
        _auxVector2.Set(direction.x * speed, 0);
        Rb.AddForce(_auxVector2, ForceMode2D.Force);
    }

    public void SetDashMovement(float velocity)
    {
        _auxVector2 = DashFinish - _auxVector3;
        DashFinish = _auxVector2;
        _auxVector2.Set(DashFinish.normalized.x, DashFinish.normalized.y);
        Rb.velocity = _auxVector2 * velocity;
        Rb.velocity = Vector2.ClampMagnitude(Rb.velocity, velocity);
        CurrentVelocity = _auxVector2 * velocity;
    }

    public void SetArrowRendering()
    {
        Vector2 aux1 = DashStart;
        Vector2 aux2 = DashFinish;

        Vector2 direction = aux2 - aux1;
        
        playerArrowRendering.RenderArrow(aux1, aux2 + direction*2);
    }

    public void SetDashVectors(Vector3 mouseStartPos, Vector3 mouseFinalPos)
    {
        Vector3 direction = (mouseFinalPos - mouseStartPos).normalized;

        DashStart = transform.position;
        DashFinish = transform.position + direction;

        Debug.Log(transform.position);
        Debug.Log(direction);
        Debug.Log(DashFinish);

        //TODO: To later move the camera towards the mouse point, we may need to capture Camera.main at the moment of touch
        //TODO: Make the arrow length constant
        //Debug.Log(mouseStartPos);
        //Vector2 aux1 = mouseStartPos;
        //Vector2 aux2 = mouseFinalPos;

        //Vector2 dashTranslationVector = new Vector2(transform.position.x - aux1.x, transform.position.y - aux1.y);
        ////Vector2 arrowTranslationVector = new Vector2(transform.position.normalized.x - aux1.normalized.x, transform.position.normalized.y - aux1.normalized.y);

        //DashStart.Set(aux1.x + dashTranslationVector.x, aux1.y + dashTranslationVector.y, 0);
        //DashFinish.Set(aux2.x + dashTranslationVector.x, aux2.y + dashTranslationVector.y, 0);

        //arrowStart.Set(aux1.x + arrowTranslationVector.x, aux1.y + arrowTranslationVector.y, 0);
        //arrowFinish.Set(aux2.x + arrowTranslationVector.x, aux2.y + arrowTranslationVector.y, 0);

        _auxVector3 = transform.position;
    }

    public void SetVelocityToGivenVector(Vector2 v, float speed)
    {
        _auxVector2.Set(v.x * speed, v.y * speed);
        Rb.velocity = _auxVector2;
        CurrentVelocity = _auxVector2;
    }

    public void SetForceToGivenVector(Vector2 v, float speed)
    {
        _auxVector2.Set(v.x * speed, v.y * speed);
        Rb.AddForce(_auxVector2, ForceMode2D.Impulse);
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void SetGravityScale(float gravity)
    {
        Rb.gravityScale = gravity;
    }

    public void SetActivePhysicsCollider(bool set)
    {
        Cc.enabled = set;
    }

    public void SetActiveSpriteRenderer(bool set)
    {
        Sr.enabled = set;
    }
    #endregion

    #region Check Functions
    public bool CheckIsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckOne.position, playerData.checkGroundRadius, playerData.whatIsGround)
            || Physics2D.OverlapCircle(groundCheckTwo.position, playerData.checkGroundRadius, playerData.whatIsGround);
    }

    public bool CheckIsTouchingNode()
    {
        return playerCollisions.IsTouchingNode;
    }

    public bool CheckIsTouchingRail()
    {
        return playerCollisions.IsTouchingRail;
    }

    public bool CheckHasReachedPoint(Vector2 point)
    {
        //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + point.x + " , " + point.y + ")");
        return (Vector2)transform.position == point;
    }

    public bool CheckIfDead()
    {
        return IsDead;
    }

    #endregion

    #region Get Functions
    public Collider2D GetNodeInfo()
    {
        return playerCollisions.NodeInfo;
    }

    public RailController GetRailInfo()
    {
        return playerCollisions.FirstRail;
    }

    public Vector2 GetCurrentVelocity()
    {
        return Rb.velocity;
    }
    #endregion

    #region Other Functions
    public void DeactivateArrowRendering()
    {
        playerArrowRendering.DerenderArrow();
    }

    public void DeactivateCircleRendering()
    {
        circleDrawer.DerenderCircle();
    }

    public void MoveTowardsVector(Vector2 v, float velocity)
    {
        //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + v.x + " , " + v.y + ")");
        _auxVector2.Set(v.x, v.y);
        transform.position = Vector2.MoveTowards(transform.position, _auxVector2, velocity * Time.deltaTime);
    }

    public void ResetPosition()
    {
        // Instantiate particles
        // Move player to last checkpoint (but here we will have only one checkpoint, so skip)
        transform.position = checkpoint;
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
