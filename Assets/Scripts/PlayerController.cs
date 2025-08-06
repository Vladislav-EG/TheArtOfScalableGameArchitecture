using System;
using PlayerStates;
using UnityEngine;
using UnityHFSM;

public class PlayerController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private StateMachine _jumpStateMachine;
    
    private IdleState _idleState;
    private WalkState _walkState;
    private JumpState _jumpState;
    private JumpWalk _jumpWalkState;


    private void Awake()
    {
        _idleState = new IdleState();
        _walkState = new WalkState();
        _jumpState = new JumpState();
        _jumpWalkState = new JumpWalk();
        
        _stateMachine = new StateMachine();

        _jumpStateMachine = new StateMachine();
        _stateMachine.AddState("JumpState", _jumpStateMachine);
        
        _jumpStateMachine.AddState("Jump", _jumpState);
        _jumpStateMachine.AddState("JumpWalk", _jumpWalkState);
        
        _jumpStateMachine.SetStartState("Jump"); // FIXME

        _stateMachine.AddState("Idle", _idleState);
        _stateMachine.AddState("Walk", _walkState);
        
        _stateMachine.AddTransition("Idle", "Walk", t => Input.GetAxisRaw("Horizontal") != 0);
        _stateMachine.AddTransition("Walk", "Idle",    t => Input.GetAxisRaw("Horizontal") == 0);
        _stateMachine.AddTransition("Idle", "JumpState",    t => Input.GetKeyDown("space"));
        _jumpStateMachine.AddTransition("Jump", "JumpWalk",    t => Input.GetAxisRaw("Horizontal") != 0);
        _stateMachine.AddTransition("JumpState", "Idle",    t => Input.GetKeyUp("space"));
        _stateMachine.AddTransition("JumpState", "Idle",    t => Input.GetKeyUp("space"));
        


        
        // _stateMachine.AddTransition("Idle", "Jump",    t => Input.GetKeyDown("Space"));

        
        _stateMachine.SetStartState("Idle");

        _stateMachine.Init();
    }

    private void Update()
    {
        _stateMachine.OnLogic();
    }
}
