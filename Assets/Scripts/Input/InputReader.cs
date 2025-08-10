using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    private PlayerInputActions _inputActions;

    private readonly VectorInputHandler _moveHandler = new VectorInputHandler();
    private readonly ButtonInputHandler _dashHandler = new ButtonInputHandler();
    private readonly ButtonInputHandler _jumpHandler = new ButtonInputHandler();
    private readonly ButtonInputHandler _crouchHandler = new ButtonInputHandler();

    public event Action<Vector2> OnMoveChanged;
    public event Action<InputButtonState> OnDashChanged;
    public event Action<InputButtonState> OnJumpChanged;
    public event Action<InputButtonState> OnCrouchChanged;
    
    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.SetCallbacks(this);
        }
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
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
    public InputButtonState GetJumpState() => _jumpHandler.State;
    public InputButtonState GetCrouchState() => _crouchHandler.State;
    public InputButtonState GetDashState() => _dashHandler.State;
    
    public void ResetFrameStates()
    {
        _dashHandler.State.ResetFrameState();
        _jumpHandler.State.ResetFrameState();
        _crouchHandler.State.ResetFrameState();
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

}

