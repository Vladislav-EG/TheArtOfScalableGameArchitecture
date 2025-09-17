using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour, IService
{
	public PlayerInputReader Player { get; private set; }
	public UIInputReader UI { get; private set; }
	public PlayerInputActions PlayerInputActions { get; private set; }

	public async Task InitializeAsync()
	{
		PlayerInputActions = new PlayerInputActions();

		Player = new PlayerInputReader();
		UI = new UIInputReader();

		PlayerInputActions.Player.SetCallbacks(Player);
		PlayerInputActions.UI.SetCallbacks(UI);

		EnableGameplay();

		await Task.CompletedTask; // Фиктивный await, чтобы убрать warning
	}

	public void EnableGameplay()
	{
		PlayerInputActions.UI.Disable();
		PlayerInputActions.Player.Enable();
	}

	public void EnableUI()
	{
		PlayerInputActions.Player.Disable();
		PlayerInputActions.UI.Enable();
	}

	public void EnableAll()
	{
		PlayerInputActions.Player.Enable();
		PlayerInputActions.UI.Enable();
	}

	public void DisableAll()
	{
		PlayerInputActions.Player.Disable();
		PlayerInputActions.UI.Disable();
	}

	private void OnEnable() => PlayerInputActions.Enable();
	private void OnDisable() => PlayerInputActions.Disable();

	public void ResetFrameStates()
	{
		Player.ResetFrameStates();
		UI.ResetFrameStates();
	}
}

public class PlayerInputReader : PlayerInputActions.IPlayerActions
{
	private readonly VectorInputHandler _moveHandler = new();
	private readonly ButtonInputHandler _jumpHandler = new();
	private readonly ButtonInputHandler _dashHandler = new();
	private readonly ButtonInputHandler _crouchHandler = new();
	private readonly ButtonInputHandler _pauseHandler = new();

	public event Action<Vector2> OnMoveChanged;
	public event Action<InputButtonState> OnJumpChanged;
	public event Action<InputButtonState> OnDashChanged;
	public event Action<InputButtonState> OnCrouchChanged;
	public event Action<InputButtonState> OnPauseChanged;

	public void OnMove(InputAction.CallbackContext context)
	{
		_moveHandler.Update(context);
		OnMoveChanged?.Invoke(_moveHandler.Value);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		_jumpHandler.Update(context);
		OnJumpChanged?.Invoke(_jumpHandler.State);
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		_dashHandler.Update(context);
		OnDashChanged?.Invoke(_dashHandler.State);
	}

	public void OnCrouch(InputAction.CallbackContext context)
	{
		_crouchHandler.Update(context);
		OnCrouchChanged?.Invoke(_crouchHandler.State);
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		_pauseHandler.Update(context);
		OnPauseChanged?.Invoke(_pauseHandler.State);
	}

	public Vector2 GetMoveDirection() => _moveHandler.Value;
	public float GetVerticalDirection() => _moveHandler.Value.y;
	public float GetHorizontalDirection() => _moveHandler.Value.x;

	public InputButtonState GetJumpState() => _jumpHandler.State;
	public InputButtonState GetDashState() => _dashHandler.State;
	public InputButtonState GetCrouchState() => _crouchHandler.State;
	public InputButtonState GetPauseState() => _pauseHandler.State;

	public void ResetFrameStates()
	{
		_jumpHandler.State.ResetFrameState();
		_dashHandler.State.ResetFrameState();
		_crouchHandler.State.ResetFrameState();
		_pauseHandler.State.ResetFrameState();
	}
}

public class UIInputReader : PlayerInputActions.IUIActions
{
	private readonly ButtonInputHandler _pauseHandler = new();

	public event Action OnNavigateChanged;
	public event Action OnSubmitChanged;
	public event Action OnCancelEventChanged;
	public event Action OnPauseEventChanged;

	public void OnNavigate(InputAction.CallbackContext context)
	{
		Debug.Log("Navigate");
	}

	public void OnSubmit(InputAction.CallbackContext context)
	{
		Debug.Log("Submit");
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		_pauseHandler.Update(context);


		Debug.Log("PauseUI");
	}

	public InputButtonState GetPauseState() => _pauseHandler.State;

	public void ResetFrameStates()
	{
		_pauseHandler.State.ResetFrameState();
	}
}
