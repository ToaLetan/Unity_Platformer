using UnityEngine;
using System.Collections;

public class InputManager
{
	public delegate void InputEvent();
	//Held key events
	public event InputEvent OnKeyLeft;
	public event InputEvent OnKeyRight;
	public event InputEvent OnKeyJump;

	//Released key events
	public event InputEvent ReleaseKeyLeftRight;
	public event InputEvent ReleaseKeyJump;

	public KeyCode leftKey;
	public KeyCode rightKey;
	public KeyCode jumpKey;

	private static InputManager instance;

	public InputManager()
	{
		leftKey = KeyCode.A;
		rightKey =  KeyCode.D;
		jumpKey = KeyCode.Space;
	}

	public static InputManager Instance
	{
		get
		{
			if(instance == null)
				instance = new InputManager();
			return instance;
		}
	}

	public void Update()
	{
		CheckInput();
	}

	public void CheckInput()
	{
		if(Input.GetKey(leftKey))
		{
			if(OnKeyLeft != null)
				OnKeyLeft();
		}

		if(Input.GetKey(rightKey))
		{
			if(OnKeyRight != null)
				OnKeyRight();
		}

		if(!Input.GetKey(leftKey) && !Input.GetKey(rightKey))
		{
			if(ReleaseKeyLeftRight != null)
				ReleaseKeyLeftRight();
		}

		if(Input.GetKey(jumpKey))
		{
			if(OnKeyJump != null)
				OnKeyJump();
		}
		else
		{
			if(ReleaseKeyJump != null)
				ReleaseKeyJump();
		}
	}
	

}