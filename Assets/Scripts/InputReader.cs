using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class TestClass
{
    private InputAction _playerActions;
    
    public TestClass(InputAction playerActions)
    {
        _playerActions = playerActions;
    }
    
    public bool KeyWasPressed => _playerActions.WasPressedThisFrame();
    public bool KeyWasReleased => _playerActions.WasReleasedThisFrame();
    public bool KeyIsPressed => _playerActions.IsPressed();
}

public class InputReader : MonoBehaviour, IPlayerActions
{
    public InputSystem_Actions inputSystemActions;
    public JumpInputState jumpInputState;
    
    private TestClass _testClass;

    private void Awake()
    {
        OnEnablePlayerActions();
        
        jumpInputState = new JumpInputState();

        _testClass = new TestClass(inputSystemActions.Player.Jump);
    }
    
    public bool JumpKeyWasPressed => inputSystemActions.Player.Jump.WasPressedThisFrame();
    public bool JumpKeyWasReleased => inputSystemActions.Player.Jump.WasReleasedThisFrame();
    public bool JumpKeyPressed => inputSystemActions.Player.Jump.IsPressed();

    public bool test1;
    public bool test2;
    public bool test3;

    



    private void Update()
    {
        // Debug.Log(JumpKeyWasPressed);
        // Debug.Log(JumpKeyWasReleased);
        // Debug.Log(JumpKeyPressed);

        Debug.Log(_testClass.KeyWasPressed);
        Debug.Log(_testClass.KeyWasReleased);
        Debug.Log(_testClass.KeyIsPressed);
    }

    public void OnEnablePlayerActions()
    {
        if (inputSystemActions == null)
        {
            inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.SetCallbacks(this);
        }
        inputSystemActions.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        // Debug.Log(context.action.IsPressed());
        // Debug.Log(context.action.WasPressedThisFrame());
        // Debug.Log(context.action.WasReleasedThisFrame());

        test1 = context.action.IsPressed();
        test2 = context.action.WasPressedThisFrame();
        test3 = context.action.WasReleasedThisFrame();
        
        // Debug.Log(test1);
        // Debug.Log(test2);
        // Debug.Log(test3);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        // nope
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // nope
    }
}

// Вариант 3: Создайте отдельный класс для состояния ввода
public class JumpInputState
{
    public bool IsPressed { get; private set; }
    public bool WasPressedThisFrame { get; private set; }
    public bool WasReleasedThisFrame { get; private set; }
    
    public void UpdateState(InputAction jumpAction)
    {
        IsPressed = jumpAction.IsPressed();
        WasPressedThisFrame = jumpAction.WasPressedThisFrame();
        WasReleasedThisFrame = jumpAction.WasReleasedThisFrame();
    }
}
