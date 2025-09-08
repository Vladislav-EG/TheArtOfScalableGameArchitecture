using System;
using PlayerStates;
using UnityEngine;
using UnityHFSM;
using UnityHFSM.Visualization;  // Import the animator graph feature.


public class PlayerController : MonoBehaviour
{
	// TODO _inputReader.GetHorizontalDirection() != 0 в InputReader добавить отдельные методы по типу: 
	// public bool IsMovingHorizontally() => Mathf.Abs(GetHorizontalDirection()) > Mathf.Epsilon; 
	// public bool IsStandingStill() => Mathf.Abs(GetHorizontalDirection()) <= Mathf.Epsilon;

	
	[SerializeField] private Rigidbody2D _rigidbody;
	[SerializeField] private BoxCollider2D _boxCollider;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private Animator _fsmAnimator;
	// [SerializeField] private InputReader _inputReader;
	[SerializeField] private InputService _inputService;
	

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
		_inputService = ServiceLocator.Get<InputService>(); // FIXME
		
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
		
		_stateMachine.AddState("GroundedRoot", _groundedStateMachine);
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
		_groundedStateMachine.AddTwoWayTransition("Grounded", "Crouching", t => _inputService.Player.GetCrouchState().IsHeld);
		
		_crouchingStateMachine.AddTransition("Crouching", "Crouch", t => _inputService.Player.GetHorizontalDirection() != 0);
		_crouchingStateMachine.AddTransition("Crouching", "IdleCrouch", t => _inputService.Player.GetHorizontalDirection() == 0);

		_crouchingStateMachine.AddTransition("IdleCrouch", "Crouch", t => _inputService.Player.GetHorizontalDirection() != 0);
		_crouchingStateMachine.AddTransition("Crouch", "IdleCrouch",    t => _inputService.Player.GetHorizontalDirection() == 0);
		
		
		// Grounded to Standing
		_groundedStateMachine.AddTwoWayTransition("Grounded", "Standing", t => !_inputService.Player.GetCrouchState().IsHeld); // В общем переход сразу если никакие другие не проходят

		_standingStateMachine.AddTransition("Standing", "Idle", t => _inputService.Player.GetHorizontalDirection() == 0);
		_standingStateMachine.AddTransition("Standing", "Walk", t => _inputService.Player.GetHorizontalDirection() != 0);
		
		// _standingStateMachine.AddTwoWayTransition("Idle", "Walk" t => Input.GetAxisRaw("Horizontal") != 0);
		_standingStateMachine.AddTransition("Idle", "Walk", t => _inputService.Player.GetHorizontalDirection() != 0);
		_standingStateMachine.AddTransition("Walk", "Idle",    t => _inputService.Player.GetHorizontalDirection() == 0);
		
		// Grounded to Airborne
		_stateMachine.AddTransition("GroundedRoot", "Airborne",    t => _inputService.Player.GetJumpState().WasPressedThisFrame || !_boxCollider.IsTouchingLayers(layerMask));
		
		_airborneStateMachine.AddTransition("Airborne", "Jump",    t => _inputService.Player.GetJumpState().WasPressedThisFrame);
		_airborneStateMachine.AddTransition("Airborne", "Fall",    t => _rigidbody.linearVelocity.y < 0f);

		_airborneStateMachine.AddTransition("Jump", "Fall",    t => _rigidbody.linearVelocity.y < 0f);
		// _airborneStateMachine.AddTransition("Fall", "Jump",    t => Input.GetKeyDown("space"));

		// Airborne to Grounded
		_stateMachine.AddTransition("Airborne", "GroundedRoot", t => _rigidbody.linearVelocity.y <= 0f && _boxCollider.IsTouchingLayers(layerMask));
		 
		// To DASH
		_stateMachine.AddTransition("Airborne", "Dash", t => _inputService.Player.GetDashState().WasPressedThisFrame);
		_stateMachine.AddTransition("GroundedRoot", "Dash", t => _inputService.Player.GetDashState().WasPressedThisFrame && _groundedStateMachine.ActiveState != _crouchingStateMachine);
		
		// Form Dash
		_stateMachine.AddTransition("Dash", "Airborne", t => !_boxCollider.IsTouchingLayers(layerMask));
		_stateMachine.AddTransition("Dash", "GroundedRoot", t => _boxCollider.IsTouchingLayers(layerMask));

		
		_groundedStateMachine.SetStartState("Grounded");
		_airborneStateMachine.SetStartState("Airborne");
		_crouchingStateMachine.SetStartState("Crouching");
		_standingStateMachine.SetStartState("Standing");
		_stateMachine.SetStartState("GroundedRoot");
		
		_stateMachine.Init();
		
#if UNITY_EDITOR // TODO Вынести в статический класс
		HfsmAnimatorGraph.CreateAnimatorFromStateMachine(
			_stateMachine,
			outputFolderPath: "Assets/DebugAnimators",
			animatorName: "StateMachineAnimatorGraph.controller"
		);
#endif
	}

	private void Update()
	{
		_stateMachine.OnLogic();
		
#if UNITY_EDITOR // TODO Вынести в статический класс
		HfsmAnimatorGraph.PreviewStateMachineInAnimator(_stateMachine, _fsmAnimator);
#endif
	}

	private void FixedUpdate()
	{
		// _stateMachine.OnFixedUpdate();
		
		_stateMachine.OnAction("OnFixedLogic");
	}

	private void LateUpdate()
	{
		_inputService.ResetFrameStates();
		
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
