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
		_pauseService = ServiceLocator.Get<PauseService>();
	}

	public override void OnEnter()
	{
		DebugColorLog.LogEnter<PauseState>();
		_pauseService.PauseGame();

		// PauseService.Instance.PauseGame();
	}
}


public class LoadLevelState : StateBase<string>
{
	public LoadLevelState(bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState) { }

	private SceneLoaderService _sceneLoaderService;

	public override void Init()
	{
		_sceneLoaderService = ServiceLocator.Get<SceneLoaderService>();
	}

	public override async void OnEnter()
	{
		DebugColorLog.LogEnter<LoadLevelState>();

		await _sceneLoaderService.LoadNextLevel();
	}
}

public class GameStateMachine : MonoBehaviour, IService
{
	private StateMachine _gameStateMachine;

	private BootstrapState _bootstrapState;
	private GameplayState _gameplayState;
	private PauseState _pauseState;
	private LoadLevelState _loadLevelState;

	private InputService _inputService;

	private bool _bootstrapDone;
	private bool _sceneLoadDone;
	private bool _levelEnded;

	public bool test;

	public async Task InitializeAsync()
	{
		_gameStateMachine = new StateMachine();

		Bootstrapper.OnBootstrapCompleted += OnBootstrapCompleted; // TODO Можно сделать ивентовую систему
																   // BootstrapperMono.OnBootstrapCompleted += () => _bootstrapDone = true; 
		// SceneLoaderService.OnLevelLoadCompleted += OnSceneLoadCompleted; // TODO Можно сделать ивентовую систему
																		 // SceneLoaderService.OnLevelEnded += OnLevelEnded; // TODO Можно сделать ивентовую систему

		SceneLoaderService.OnLevelLoadCompleted += () => _gameStateMachine.Trigger("LevelLoadDone");

		SceneLoaderService.OnLevelEnded += () => _gameStateMachine.Trigger("LevelFinished");
		

		_inputService = ServiceLocator.Get<InputService>();


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

	private void OnSceneLoadCompleted()
	{
		_sceneLoadDone = true;
		_gameStateMachine.OnLogic(); // Принудительно обнови FSM прямо здесь, чтобы переход сработал мгновенно
									 // Или: _gameStateMachine.RequestStateChange("Gameplay"); если библиотека поддерживает
	}

	private void OnLevelEnded()
	{
		_levelEnded = true;
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
		_loadLevelState = new LoadLevelState();
	}

	private void ConfigureStates()
	{
		_gameStateMachine.AddState("Bootstrap", _bootstrapState);
		_gameStateMachine.AddState("Gameplay", _gameplayState);
		_gameStateMachine.AddState("Pause", _pauseState);
		_gameStateMachine.AddState("LoadLevelState", _loadLevelState);
	}

	private void ConfigureTransitions()
	{
		// _gameStateMachine.AddTransition("Bootstrap", "Gameplay", t => _bootstrapDone);

		_gameStateMachine.AddTransition("Bootstrap", "LoadLevelState", t => _bootstrapDone);

		// _gameStateMachine.AddTransition("LoadLevelState", "Gameplay", t => _sceneLoadDone);
		_gameStateMachine.AddTriggerTransition("LevelLoadDone", new Transition("LoadLevelState", "Gameplay"));
		
		_gameStateMachine.AddTriggerTransition("LevelFinished", new Transition("Gameplay", "LoadLevelState"));
		// _gameStateMachine.AddTransition("Gameplay", "LoadLevelState", t => _levelEnded);

		// TODO Вынести в отдельную FSM
		_gameStateMachine.AddTransition("Gameplay", "Pause", t => _inputService.Player.GetPauseState().WasPressedThisFrame);
		_gameStateMachine.AddTransition("Pause", "Gameplay", t => _inputService.UI.GetPauseState().WasPressedThisFrame);

		_gameStateMachine.SetStartState("Bootstrap");
	}
}

// Создать отдельный класс где будут разщные триггеры для окончаения лвла и и они будут триггериить событие и передавать
// Данные в SceneLoader, который в свою очередь будет загружать нужную сцену 
// Есть сситема событий где и находится мое событие уровня прохошелся и к нему привзываются Sceneloader и GameStateMachine