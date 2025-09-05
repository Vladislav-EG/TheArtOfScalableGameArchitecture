using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputHandler
{
	void Update(InputAction.CallbackContext context);
}

public class VectorInputHandler : IInputHandler
{
	public Vector2 Value { get; private set; }

	public void Update(InputAction.CallbackContext context)
	{
		Value = context.ReadValue<Vector2>();
	}
}

public class ButtonInputHandler : IInputHandler
{
	public InputButtonState State { get; private set; } = new InputButtonState();

	public void Update(InputAction.CallbackContext context)
	{
		State.Update(context);
	}
}