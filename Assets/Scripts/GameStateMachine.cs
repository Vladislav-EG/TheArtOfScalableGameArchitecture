using UnityEngine;
using UnityHFSM;

public class GameplayState : StateBase<string>
{
	public GameplayState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }

	public override void OnEnter()
	{
		DebugColorLog.LogEnter<GameplayState>();
		PauseService.Instance.ResumeGame();
	}
}

public class PauseState : StateBase<string>
{
	public PauseState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }
	
	public override void OnEnter()
	{
		DebugColorLog.LogEnter<PauseState>();
		PauseService.Instance.PauseGame();
	}
}

public static class ServiceCreatePauseUI
{
	public static void CreatePauseUI()
	{
		
	}
}

public class GameStateMachine : MonoBehaviour
{
	private StateMachine _gameStateMachine;

	private GameplayState _gameplayState;
	private PauseState _pauseState;
	
	[SerializeField] private InputReader _inputReader;

	private void Awake()
	{
		_gameStateMachine = new StateMachine();
		
		CreateStates();
		ConfigureStates();
		ConfigureTransitions();
		
		_gameStateMachine.Init();
	}

	private void Update()
	{
		_gameStateMachine.OnLogic();
	}

	private void CreateStates()
	{
		_gameplayState = new GameplayState();
		_pauseState = new PauseState();
	}

	private void ConfigureStates()
	{
		_gameStateMachine.AddState("Gameplay", _gameplayState);
		_gameStateMachine.AddState("Pause", _pauseState);
	}

	private void ConfigureTransitions()
	{
		// _gameStateMachine.AddTransition("Gameplay", "Pause", t => Input.GetKeyDown("escape"));
		_gameStateMachine.AddTransition("Gameplay", "Pause", t => _inputReader.GetPauseState().WasPressedThisFrame);
		
		_gameStateMachine.AddTransition("Pause", "Gameplay", t => _inputReader.GetCancelState().WasPressedThisFrame);

		_gameStateMachine.SetStartState("Gameplay");
	}
}