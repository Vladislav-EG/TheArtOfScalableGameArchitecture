using System;
using PlayerStates;
using UnityEngine;
using UnityHFSM;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask layerMask;

    private StateMachine _stateMachine;
    private StateMachine _groundedStateMachine;
    private StateMachine _airborneStateMachine;
    private StateMachine _crouchingStateMachine;
    private StateMachine _standingStateMachine;
    
    private IdleState _idleState;
    private WalkState _walkState;
    private JumpState _jumpState;
    private FallState _fallState;
    private DashState _dashState;
    private CrouchState _crouchState;
    private IdleCrouchState _idleCrouchState;
    
    private void Awake()
    {
        _idleState = new IdleState();
        _walkState = new WalkState(_rigidbody);
        _jumpState = new JumpState(_rigidbody, transform);
        _fallState = new FallState();
        _dashState = new DashState(_rigidbody, transform);
        _crouchState = new CrouchState();
        _idleCrouchState = new IdleCrouchState();

        _stateMachine = new StateMachine(); 
        _groundedStateMachine = new StateMachine();
        _airborneStateMachine = new StateMachine();
        _standingStateMachine = new StateMachine();
        _crouchingStateMachine = new StateMachine();
        
        _stateMachine.AddState("Grounded", _groundedStateMachine);
        _stateMachine.AddState("Airborne", _airborneStateMachine);
        _stateMachine.AddState("Dash", _dashState);
        
        _groundedStateMachine.AddState("Grounded", onEnter: state => Debug.Log("Grounded"), isGhostState: true);
        _groundedStateMachine.AddState("Standing", _standingStateMachine);
        _groundedStateMachine.AddState("Crouching", _crouchingStateMachine);

        _standingStateMachine.AddState("Standing", onEnter: state => Debug.Log("Standing"), isGhostState: true);
        _standingStateMachine.AddState("Idle", _idleState);
        _standingStateMachine.AddState("Walk", _walkState);
        
        _crouchingStateMachine.AddState("Crouching", onEnter: state => Debug.Log("Crouching"), isGhostState: true);
        _crouchingStateMachine.AddState("Crouch", _crouchState);
        _crouchingStateMachine.AddState("IdleCrouch", _idleCrouchState);
        
        _airborneStateMachine.AddState("Airborne", onEnter: state => Debug.Log("Airborne"), isGhostState: true);
        _airborneStateMachine.AddState("Jump", _jumpState);
        _airborneStateMachine.AddState("Fall", _fallState);
        
        
        // Grounded to Crouching
        _groundedStateMachine.AddTwoWayTransition("Grounded", "Crouching", t => Input.GetAxisRaw("Vertical") != 0);
        
        _crouchingStateMachine.AddTransition("Crouching", "Crouch", t => Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0);
        _crouchingStateMachine.AddTransition("Crouching", "IdleCrouch", t => Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0);

        _crouchingStateMachine.AddTransition("IdleCrouch", "Crouch", t => Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0);
        _crouchingStateMachine.AddTransition("Crouch", "IdleCrouch",    t => Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0);
        
        
        // Grounded to Standing
        _groundedStateMachine.AddTwoWayTransition("Grounded", "Standing", t => Input.GetAxisRaw("Vertical") == 0);

        _standingStateMachine.AddTransition("Standing", "Idle", t => Input.GetAxisRaw("Horizontal") == 0);
        _standingStateMachine.AddTransition("Standing", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        
        // _standingStateMachine.AddTwoWayTransition("Idle", "Walk" t => Input.GetAxisRaw("Horizontal") != 0);
        _standingStateMachine.AddTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        _standingStateMachine.AddTransition("Walk", "Idle",    t => Input.GetAxisRaw("Horizontal") == 0);
        

        // Grounded to Airborne
        _stateMachine.AddTransition("Grounded", "Airborne",    t => Input.GetKeyDown("space") || !_boxCollider.IsTouchingLayers(layerMask));
        
        _airborneStateMachine.AddTransition("Airborne", "Jump",    t => Input.GetKeyDown("space"));
        _airborneStateMachine.AddTransition("Airborne", "Fall",    t => _rigidbody.linearVelocity.y < 0f);

        _airborneStateMachine.AddTransition("Jump", "Fall",    t => _rigidbody.linearVelocity.y < 0f);
        // _airborneStateMachine.AddTransition("Fall", "Jump",    t => Input.GetKeyDown("space"));

        // Airborne to Grounded
        _stateMachine.AddTransition("Airborne", "Grounded", t => _rigidbody.linearVelocity.y <= 0f && _boxCollider.IsTouchingLayers(layerMask));
         
        // To DASH
        _stateMachine.AddTransition("Airborne", "Dash", t => Input.GetKeyDown("o"));
        _stateMachine.AddTransition("Grounded", "Dash", t => Input.GetKeyDown("o") && _groundedStateMachine.ActiveState != _crouchingStateMachine);
        
        // Form Dash
        _stateMachine.AddTransition("Dash", "Airborne", t => !_boxCollider.IsTouchingLayers(layerMask));
        _stateMachine.AddTransition("Dash", "Grounded", t => _boxCollider.IsTouchingLayers(layerMask));

        
        _groundedStateMachine.SetStartState("Grounded");
        _airborneStateMachine.SetStartState("Airborne");
        _crouchingStateMachine.SetStartState("Crouching");
        _standingStateMachine.SetStartState("Standing");
        _stateMachine.SetStartState("Grounded");
        
        _stateMachine.Init();
    }

    private void Update()
    {
        _stateMachine.OnLogic();
    }

    private void FixedUpdate()
    {
        // _stateMachine.OnFixedUpdate();
        
        _stateMachine.OnAction("OnFixedLogic");
    }
}

public class ActionStateWithFixed : ActionState
{
    protected ActionStateWithFixed(bool needsExitTime, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
        AddAction("OnFixedLogic", OnFixedLogic);
    }

    public virtual void OnFixedLogic() { }
}
