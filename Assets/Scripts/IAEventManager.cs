using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class IAEventManager : MonoBehaviour
{
	private PlayerIA _input;
	private InputAction _lookAction;

	private InputAction _moveAction;
	public static IAEventManager Instance{ get; private set; }

	private void Awake()
	{
		Instance = Utility.CreateSingleton( Instance, gameObject );

		_input = new();

		SetActions();
		BindActions();
		EnableAllActions();
	}

	private void OnDestroy()
	{
		DisableAllActions();
		UnbindActions();
	}

	public event Action<InputAction.CallbackContext> MovePerformed;
	public event Action<InputAction.CallbackContext> MoveCanceled;
	public event Action<InputAction.CallbackContext> LookPerformed;
	public event Action<InputAction.CallbackContext> LookCanceled;

	#region Action Setup

	private void SetActions()
	{
		_moveAction = _input.Player.Move;
		_lookAction = _input.Player.Look;
	}

	private void BindActions()
	{
		_moveAction.performed += OnMovePerformed;
		_moveAction.canceled += OnMoveCanceled;
		_lookAction.performed += OnLookPerformed;
		_lookAction.canceled += OnLookCanceled;
	}

	private void UnbindActions()
	{
		_moveAction.performed -= OnMovePerformed;
		_moveAction.canceled -= OnMoveCanceled;
		_lookAction.performed -= OnLookPerformed;
		_lookAction.canceled -= OnLookCanceled;
	}

	private void EnableAllActions()
	{
		_moveAction.Enable();
		_lookAction.Enable();
	}

	private void DisableAllActions()
	{
		_moveAction.Disable();
		_lookAction.Disable();
	}

	#endregion

	#region Event Methods

	private void OnMovePerformed( InputAction.CallbackContext ctx )
	{
		MovePerformed?.Invoke( ctx );
	}

	private void OnMoveCanceled( InputAction.CallbackContext ctx )
	{
		MoveCanceled?.Invoke( ctx );
	}

	private void OnLookPerformed( InputAction.CallbackContext ctx )
	{
		LookPerformed?.Invoke( ctx );
	}

	private void OnLookCanceled( InputAction.CallbackContext ctx )
	{
		LookCanceled?.Invoke( ctx );
	}

	#endregion

	/*private void Update()
	{
		Vector2 m = _moveAction.ReadValue<Vector2>();
		Vector2 l = _lookAction.ReadValue<Vector2>();
		Debug.Log($"Move: {m}, Look: {l}");
	}*/
}