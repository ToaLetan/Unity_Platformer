﻿using UnityEngine;
using System.Collections;

public class InputManager
{
	public delegate void InputEvent();

	//Held key events
	public event InputEvent OnKeyLeft;
	public event InputEvent OnKeyRight;
	public event InputEvent OnKeyJump;
	public event InputEvent OnKeyReset;

	//Released key events
	public event InputEvent ReleaseKeyLeftRight;
	public event InputEvent ReleaseKeyJump;

	public KeyCode leftKey;
	public KeyCode rightKey;
	public KeyCode jumpKey;
	public KeyCode resetKey;

	private static InputManager instance;

	public InputManager()
	{
		leftKey = KeyCode.A;
		rightKey =  KeyCode.D;
		jumpKey = KeyCode.Space;
		resetKey = KeyCode.R;
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
		if(Input.GetKeyUp(jumpKey))
		{
			if(ReleaseKeyJump != null)
				ReleaseKeyJump();
		}

		if(Input.GetKey(resetKey))
		{
			if(OnKeyReset != null)
				OnKeyReset();
		}
	}
	

}