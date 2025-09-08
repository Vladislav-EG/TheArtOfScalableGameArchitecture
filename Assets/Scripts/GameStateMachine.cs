using System.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

public class BootstrapState : StateBase<string>
{
	public BootstrapState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }

	public override void OnEnter()
	{
		DebugColorLog.LogEnter<BootstrapState>();
	}
}

public class GameplayState : StateBase<string>
{
	public GameplayState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }

	private PauseService _pauseService;

	public override void Init()
	{
		Debug.Log("ADASDSADSA");
		_pauseService = ServiceLocator.Get<PauseService>();
	}

	public override void OnEnter()
	{
		DebugColorLog.LogEnter<GameplayState>();
		_pauseService.ResumeGame();
		// PauseService.Instance.ResumeGame();
	}
}

public class PauseState : StateBase<string>
{
	public PauseState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }

	private PauseService _pauseService;

	public override void Init()
	{
		Debug.Log("ADASDSADSA");
		_pauseService = ServiceLocator.Get<PauseService>();
	}

	public override void OnEnter()
	{
		DebugColorLog.LogEnter<PauseState>();
		_pauseService.PauseGame();
		// PauseService.Instance.PauseGame();
	}
}

public static class ServiceCreatePauseUI
{
	public static void CreatePauseUI()
	{

	}
}

public class GameStateMachine : MonoBehaviour, IService
{
	private StateMachine _gameStateMachine;

	private BootstrapState _bootstrapState;
	private GameplayState _gameplayState;
	private PauseState _pauseState;


	// [SerializeField] private InputService _inputService; // FIXME

	private InputService _inputService;


	private bool _bootstrapDone;


	public async Task InitializeAsync()
	{
		BootstrapperMono.OnBootstrapCompleted += OnBootstrapCompleted;
		// BootstrapperMono.OnBootstrapCompleted += () => _bootstrapDone = true; 

		_inputService = ServiceLocator.Get<InputService>();

		_gameStateMachine = new StateMachine();

		CreateStates();
		ConfigureStates();
		ConfigureTransitions();

		_gameStateMachine.Init();

		await Task.CompletedTask; // Фиктивный await, чтобы убрать warning
	}

	private void OnBootstrapCompleted()
	{
		_bootstrapDone = true;
		_gameStateMachine.OnLogic(); // Принудительно обнови FSM прямо здесь, чтобы переход сработал мгновенно
									 // Или: _gameStateMachine.RequestStateChange("Gameplay"); если библиотека поддерживает
	}

	private void Awake()
	{
		// BootstrapperMono.OnBootstrapCompleted += () => _bootstrapDone = true; // подписка


		// _gameStateMachine = new StateMachine();

		// CreateStates();
		// ConfigureStates();
		// ConfigureTransitions();

		// _gameStateMachine.Init();
	}

	private void Update()
	{
		_gameStateMachine.OnLogic();
	}

	private void CreateStates()
	{
		_gameplayState = new GameplayState();
		_pauseState = new PauseState();
		_bootstrapState = new BootstrapState();
	}

	private void ConfigureStates()
	{
		_gameStateMachine.AddState("Bootstrap", _bootstrapState);
		_gameStateMachine.AddState("Gameplay", _gameplayState);
		_gameStateMachine.AddState("Pause", _pauseState);
	}

	private void ConfigureTransitions()
	{
		_gameStateMachine.AddTransition("Bootstrap", "Gameplay", t => _bootstrapDone);
		_gameStateMachine.AddTransition("Gameplay", "Pause", t => _inputService.Player.GetPauseState().WasPressedThisFrame);
		_gameStateMachine.AddTransition("Pause", "Gameplay", t => _inputService.UI.GetPauseState().WasPressedThisFrame);

		_gameStateMachine.SetStartState("Bootstrap");
	}
}