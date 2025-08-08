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
    private StateMachine _jumpingStateMachine;
    
    private IdleState _idleState;
    private WalkState _walkState;
    private JumpState _jumpState;
    private FallState _fallState;
    private DashState _dashState;

    private void Awake()
    {
        _idleState = new IdleState();
        _walkState = new WalkState(_rigidbody);
        _jumpState = new JumpState(_rigidbody, transform);
        _fallState = new FallState();
        _dashState = new DashState(_rigidbody, transform);

        _stateMachine = new StateMachine(); 
        _groundedStateMachine = new StateMachine();
        _jumpingStateMachine = new StateMachine();
        
        _groundedStateMachine.AddState("Grounded", onEnter: state => Debug.Log("Grounded"));
        _groundedStateMachine.AddState("Idle", _idleState);
        _groundedStateMachine.AddState("Walk", _walkState);

        _jumpingStateMachine.AddState("Jumping", onEnter: state => Debug.Log("Jumping"));
        _jumpingStateMachine.AddState("Jump", _jumpState);
        _jumpingStateMachine.AddState("Fall", _fallState);
        
        _stateMachine.AddState("Grounded", _groundedStateMachine);
        _stateMachine.AddState("Jumping", _jumpingStateMachine);
        _stateMachine.AddState("Dash", _dashState);


        _groundedStateMachine.AddTransition("Grounded", "Idle", t => Input.GetAxisRaw("Horizontal") == 0);
        _groundedStateMachine.AddTransition("Grounded", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);

        // _groundedStateMachine.AddTwoWayTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);

        _groundedStateMachine.AddTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        _groundedStateMachine.AddTransition("Walk", "Idle",    t => Input.GetAxisRaw("Horizontal") == 0);

        _stateMachine.AddTransition("Grounded", "Jumping",    t => Input.GetKeyDown("space"));
        
        _jumpingStateMachine.AddExitTransition("Fall", t => _boxCollider.IsTouchingLayers(layerMask));
        
        _jumpingStateMachine.AddTransition("Jumping", "Jump",    t => Input.GetKey("space"));
        _jumpingStateMachine.AddTransition("Jumping", "Fall",    t => _rigidbody.linearVelocity.y < 0f);

        _jumpingStateMachine.AddTransition("Jump", "Fall",    t => _rigidbody.linearVelocity.y < 0f);

        // _stateMachine.AddTransition(new TransitionAfter("Fall", "Grounded", 0.5f));
        _stateMachine.AddTransition("Jumping", "Grounded", t => _rigidbody.linearVelocity.y <= 0f && _boxCollider.IsTouchingLayers(layerMask));
        
        _stateMachine.AddTransition("Jumping", "Dash", t => Input.GetKeyDown("o"));
        _stateMachine.AddTransition("Grounded", "Dash", t => Input.GetKeyDown("o"));
        _stateMachine.AddTransition("Dash", "Jumping", t => !_boxCollider.IsTouchingLayers(layerMask));
        _stateMachine.AddTransition("Dash", "Grounded", t => _boxCollider.IsTouchingLayers(layerMask));



        _groundedStateMachine.SetStartState("Grounded");
        _jumpingStateMachine.SetStartState("Jumping");

        _stateMachine.SetStartState("Grounded");
        _stateMachine.Init();
    }

    private void Update()
    {
        _stateMachine.OnLogic();
    }
}
