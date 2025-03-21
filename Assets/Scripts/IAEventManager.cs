#region

using System;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class IAEventManager : MonoBehaviour
{
	public static IAEventManager Instance{ get; private set; }

	private PlayerIA _input;
	private InputAction _moveAction;
	private InputAction _lookAction;
	private InputAction _aimAction;
	private InputAction _shootAction;
	private InputAction _jumpAction;
	private InputAction _sprintAction;
	private InputAction _velocityTestAction;

	public event Action<InputAction.CallbackContext> MovePerformed;
	public event Action<InputAction.CallbackContext> MoveCanceled;
	public event Action<InputAction.CallbackContext> LookPerformed;
	public event Action<InputAction.CallbackContext> LookCanceled;
	public event Action<InputAction.CallbackContext> AimPerformed;
	public event Action<InputAction.CallbackContext> AimCanceled;
	public event Action ShootPerformed;
	public event Action ShootCanceled;
	public event Action JumpPerformed;
	public event Action JumpCanceled;
	public event Action SprintPerformed;
	public event Action SprintCanceled;
	public event Action VelocityTestPerformed;

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
		_jumpAction = _input.Player.Jump;
		_sprintAction = _input.Player.Sprint;
		_velocityTestAction = _input.Debug.AddVelocity;
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
		_jumpAction.performed += OnJumpPerformed;
		_jumpAction.canceled += OnJumpCanceled;
		_sprintAction.performed += OnSprintPerformed;
		_sprintAction.canceled += OnSprintCanceled;
		_velocityTestAction.performed += OnVelocityTestPerformed;
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
		_jumpAction.performed -= OnJumpPerformed;
		_jumpAction.canceled -= OnJumpCanceled;
		_sprintAction.performed -= OnSprintPerformed;
		_sprintAction.canceled -= OnSprintCanceled;
		_velocityTestAction.performed -= OnVelocityTestPerformed;
	}

	private void EnableAllActions()
	{
		_moveAction.Enable();
		_lookAction.Enable();
		_aimAction.Enable();
		_shootAction.Enable();
		_jumpAction.Enable();
		_sprintAction.Enable();
		_velocityTestAction.Enable();
	}

	private void DisableAllActions()
	{
		_moveAction.Disable();
		_lookAction.Disable();
		_aimAction.Disable();
		_shootAction.Disable();
		_jumpAction.Disable();
		_sprintAction.Disable();
		_velocityTestAction.Disable();
	}

	#endregion

	#region Event Methods

	private void OnMovePerformed( InputAction.CallbackContext ctx )         => MovePerformed?.Invoke( ctx );
	private void OnMoveCanceled( InputAction.CallbackContext ctx )          => MoveCanceled?.Invoke( ctx );
	private void OnLookPerformed( InputAction.CallbackContext ctx )         => LookPerformed?.Invoke( ctx );
	private void OnLookCanceled( InputAction.CallbackContext ctx )          => LookCanceled?.Invoke( ctx );
	private void OnAimPerformed( InputAction.CallbackContext ctx )          => AimPerformed?.Invoke( ctx );
	private void OnAimCanceled( InputAction.CallbackContext ctx )           => AimCanceled?.Invoke( ctx );
	private void OnShootPerformed( InputAction.CallbackContext ctx )        => ShootPerformed?.Invoke();
	private void OnShootCanceled( InputAction.CallbackContext ctx )         => ShootCanceled?.Invoke();
	private void OnJumpPerformed( InputAction.CallbackContext ctx )         => JumpPerformed?.Invoke();
	private void OnJumpCanceled( InputAction.CallbackContext ctx )          => JumpCanceled?.Invoke();
	private void OnSprintPerformed( InputAction.CallbackContext ctx )       => SprintPerformed?.Invoke();
	private void OnSprintCanceled( InputAction.CallbackContext ctx )        => SprintCanceled?.Invoke();
	private void OnVelocityTestPerformed( InputAction.CallbackContext ctx ) => VelocityTestPerformed?.Invoke();

	#endregion
}