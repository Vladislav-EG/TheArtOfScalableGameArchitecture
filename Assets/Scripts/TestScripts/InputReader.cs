using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions
{
	// TODO 2 интерфейса один для кнопок с UI другой для кнопок с Gameplay
	// TODO InputReader занимается двумя обязанностями он дает считывать нажатие клавиш и включает отличает карты

	public PlayerInputActions InputActions;

	private readonly VectorInputHandler _moveHandler = new VectorInputHandler();
	private readonly ButtonInputHandler _dashHandler = new ButtonInputHandler();
	private readonly ButtonInputHandler _jumpHandler = new ButtonInputHandler();
	private readonly ButtonInputHandler _crouchHandler = new ButtonInputHandler();

	private readonly ButtonInputHandler _pauseHandler = new ButtonInputHandler();
	private readonly ButtonInputHandler _cancelHandler = new ButtonInputHandler();

	public event Action<Vector2> OnMoveChanged;
	public event Action<InputButtonState> OnDashChanged;
	public event Action<InputButtonState> OnJumpChanged;
	public event Action<InputButtonState> OnCrouchChanged;

	private enum MyEnum
	{
		Dash,
		Jump
	}

	private readonly Dictionary<MyEnum, ButtonInputHandler> _buttonHandlers = new();
	private readonly Dictionary<string, VectorInputHandler> _inputHandlers = new(); // inputService.Player.GetDashState.IsPressed

	private void Awake()
	{
		_buttonHandlers[MyEnum.Dash] = _dashHandler;
		_buttonHandlers[MyEnum.Jump] = _jumpHandler;
	}

	private void OnEnable()
	{
		if (InputActions == null)
		{
			InputActions = new PlayerInputActions();
			InputActions.Player.SetCallbacks(this);
			InputActions.UI.SetCallbacks(this);
		}
		InputActions.Enable();
		InputActions.UI.Enable();
	}

	private void OnDisable()
	{
		InputActions.Disable();
		InputActions.UI.Disable();
	}

	public void EnableGameplay() // TODO Создать интерфейс
	{
		InputActions.UI.Disable();
		InputActions.Player.Enable();
	}

	public void EnableUI()
	{
		InputActions.Player.Disable();
		InputActions.UI.Enable();
	}

	public void EnableAll()
	{
		// если нужно чтобы и UI и Player работали одновременно
		InputActions.Player.Enable();
		InputActions.UI.Enable();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		_moveHandler.Update(context);
		OnMoveChanged?.Invoke(_moveHandler.Value);
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		_dashHandler.Update(context);
		OnDashChanged?.Invoke(_dashHandler.State);
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		_pauseHandler.Update(context);

	}

	public void OnJump(InputAction.CallbackContext context)
	{
		_jumpHandler.Update(context);
		OnJumpChanged?.Invoke(_jumpHandler.State);
	}


	public void OnCrouch(InputAction.CallbackContext context)
	{
		_crouchHandler.Update(context);
		OnCrouchChanged?.Invoke(_crouchHandler.State);
	}

	// public InputButtonState GetDashState() => _dashHandler != null ? _dashHandler.State : new InputButtonState();
	// public InputButtonState GetJumpState() => _jumpHandler != null ? _jumpHandler.State : new InputButtonState();
	// public InputButtonState GetCrouchState() => _crouchHandler != null ? _crouchHandler.State : new InputButtonState();
	// public Vector2 GetMoveDirection() => _moveHandler != null ? _moveHandler.Value : Vector2.zero;

	public Vector2 GetMoveDirection() => _moveHandler.Value;
	public float GetVerticalDirection() => _moveHandler.Value.y;

	public float GetHorizontalDirection() => _moveHandler.Value.x;
	public InputButtonState GetJumpState() => _jumpHandler.State;
	public InputButtonState GetCrouchState() => _crouchHandler.State;
	public InputButtonState GetDashState() => _dashHandler.State;

	public InputButtonState GetPauseState() => _pauseHandler.State;

	public InputButtonState GetCancelState() => _cancelHandler.State;



	// public InputAction GetTest() => InputActions.Player.Jump;
	// public InputAction GetTest2() => InputActions.Player.Move;



	public void ResetFrameStates()
	{
		_dashHandler.State.ResetFrameState();
		_jumpHandler.State.ResetFrameState();
		_crouchHandler.State.ResetFrameState();
		_pauseHandler.State.ResetFrameState();

		_cancelHandler.State.ResetFrameState();

	}

	// public float GetRawHorizontalDirection() 
	// {
	//     if (_moveHandler == null) return 0f;
	//
	//     float x = _moveHandler.Value.x;
	//     return Mathf.Approximately(x, 0f) ? 0f : Mathf.Sign(x);
	// }
	//
	//
	// public Vector2 GetNormalizedHorizontalDirection() 
	// {
	//     if (_moveHandler == null) return Vector2.zero;
	//     
	//     float x = _moveHandler.Value.x;
	//     float normalizedX = Mathf.Approximately(x, 0f) ? 0f : Mathf.Sign(x);
	//     return new Vector2(normalizedX, 0f);
	// }

	// UI
	public void OnNavigate(InputAction.CallbackContext context)
	{
		Debug.Log("Navigate");
	}

	public void OnSubmit(InputAction.CallbackContext context)
	{
		Debug.Log("OnSubmit");
	}

	public void OnCancel(InputAction.CallbackContext context)
	{
		_pauseHandler.Update(context);
		_cancelHandler.Update(context);

		Debug.Log("OnCancel");
	}
}



