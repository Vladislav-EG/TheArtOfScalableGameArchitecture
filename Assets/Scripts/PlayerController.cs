using System;
using PlayerStates;
using UnityEngine;
using UnityHFSM;

public class PlayerController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private StateMachine _groundedStateMachine;
    private StateMachine _jumpingStateMachine;
    
    private IdleState _idleState;
    private WalkState _walkState;
    private JumpState _jumpState;
    private FallState _fallState;



    private void Awake()
    {
        _idleState = new IdleState();
        _walkState = new WalkState();
        _jumpState = new JumpState();
        _fallState = new FallState();

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


        _groundedStateMachine.AddTransition("Grounded", "Idle", t => Input.GetAxisRaw("Horizontal") == 0);
        _groundedStateMachine.AddTransition("Grounded", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);

        // _groundedStateMachine.AddTwoWayTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);

        _groundedStateMachine.AddTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        _groundedStateMachine.AddTransition("Walk", "Idle",    t => Input.GetAxisRaw("Horizontal") == 0);

        _stateMachine.AddTransition("Grounded", "Jumping",    t => Input.GetKeyDown("space"));
        _jumpingStateMachine.AddTransition("Jump", "Fall",    t => Input.GetKeyUp("space"));

        // _stateMachine.AddTransition(new TransitionAfter("Fall", "Grounded", 0.5f));
        _stateMachine.AddTransition(new TransitionAfter("Jumping", "Grounded", 0.5f));

            
        _groundedStateMachine.SetStartState("Grounded");
        _jumpingStateMachine.SetStartState("Jumping");

        _stateMachine.SetStartState("Grounded");
        _stateMachine.Init();

        
        // var fightFsm = new HybridStateMachine(
        //     beforeOnLogic: state => Debug.Log("ASD")
        //     );

        // _jumpStateMachine = new StateMachine();
        // _stateMachine.AddState("JumpState", _jumpStateMachine);
        //
        // _jumpStateMachine.AddState("Jump", _jumpState);
        // _jumpStateMachine.AddState("JumpWalk", _jumpWalkState);
        //
        // _jumpStateMachine.SetStartState("Jump"); // FIXME
        //
        // _stateMachine.AddState("Idle", _idleState);
        // _stateMachine.AddState("Walk", _walkState);
        //
        // _stateMachine.AddTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        // _stateMachine.AddTransition("Walk", "Idle",    t => Input.GetAxisRaw("Horizontal") == 0);
        // _stateMachine.AddTransition("Idle", "JumpState",    t => Input.GetKeyDown("space"));
        // _jumpStateMachine.AddTransition("Jump", "JumpWalk",    t => Input.GetAxisRaw("Horizontal") != 0);
        // _stateMachine.AddTransition("JumpState", "Idle",    t => Input.GetKeyUp("space"));
        // _stateMachine.AddTransition("JumpState", "Idle",    t => Input.GetKeyUp("space"));
        
        // _stateMachine.AddTransition("Idle", "Jump",    t => Input.GetKeyDown("Space"));

        //
        // _stateMachine.SetStartState("Idle");
        //
        // _stateMachine.Init();
    }

    private void Update()
    {
        _stateMachine.OnLogic();
    }
}
