#region

using System;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class IAEventManager : MonoBehaviour
{
	private PlayerIA _input;

	private InputAction _moveAction;
	private InputAction _lookAction;
	private InputAction _aimAction;
	private InputAction _shootAction;

	public event Action<InputAction.CallbackContext> MovePerformed;
	public event Action<InputAction.CallbackContext> MoveCanceled;
	public event Action<InputAction.CallbackContext> LookPerformed;
	public event Action<InputAction.CallbackContext> LookCanceled;
	public event Action<InputAction.CallbackContext> AimPerformed;
	public event Action<InputAction.CallbackContext> AimCanceled;
	public event Action<InputAction.CallbackContext> ShootPerformed;
	public event Action<InputAction.CallbackContext> ShootCanceled;

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

	#region Action Setup

	private void SetActions()
	{
		_moveAction = _input.Player.Move;
		_lookAction = _input.Player.Look;
		_aimAction = _input.Player.Aim;
		_shootAction = _input.Player.Shoot;
	}

	private void BindActions()
	{
		_moveAction.performed += OnMovePerformed;
		_moveAction.canceled += OnMoveCanceled;
		_lookAction.performed += OnLookPerformed;
		_lookAction.canceled += OnLookCanceled;
		_aimAction.performed += OnAimPerformed;
		_aimAction.canceled += OnAimCanceled;
		_shootAction.performed += OnShootPerformed;
		_shootAction.canceled += OnShootCanceled;
	}

	private void UnbindActions()
	{
		_moveAction.performed -= OnMovePerformed;
		_moveAction.canceled -= OnMoveCanceled;
		_lookAction.performed -= OnLookPerformed;
		_lookAction.canceled -= OnLookCanceled;
		_aimAction.performed -= OnAimPerformed;
		_aimAction.canceled -= OnAimCanceled;
		_shootAction.performed -= OnShootPerformed;
		_shootAction.canceled -= OnShootCanceled;
	}

	private void EnableAllActions()
	{
		_moveAction.Enable();
		_lookAction.Enable();
		_aimAction.Enable();
		_shootAction.Enable();
	}

	private void DisableAllActions()
	{
		_moveAction.Disable();
		_lookAction.Disable();
		_aimAction.Disable();
		_shootAction.Disable();
	}

	#endregion

	#region Event Methods

	private void OnMovePerformed( InputAction.CallbackContext ctx )  => MovePerformed?.Invoke( ctx );
	private void OnMoveCanceled( InputAction.CallbackContext ctx )   => MoveCanceled?.Invoke( ctx );
	private void OnLookPerformed( InputAction.CallbackContext ctx )  => LookPerformed?.Invoke( ctx );
	private void OnLookCanceled( InputAction.CallbackContext ctx )   => LookCanceled?.Invoke( ctx );
	private void OnAimPerformed( InputAction.CallbackContext ctx )   => AimPerformed?.Invoke( ctx );
	private void OnAimCanceled( InputAction.CallbackContext ctx )    => AimCanceled?.Invoke( ctx );
	private void OnShootPerformed( InputAction.CallbackContext ctx ) => ShootPerformed?.Invoke( ctx );
	private void OnShootCanceled( InputAction.CallbackContext ctx )  => ShootCanceled?.Invoke( ctx );

	#endregion

	/*private void Update()
	{
		Vector2 m = _moveAction.ReadValue<Vector2>();
		Vector2 l = _lookAction.ReadValue<Vector2>();
		Debug.Log($"Move: {m}, Look: {l}");
	}*/
}