#region

using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

#endregion

public class MovementBehaviour : MonoBehaviour, IFixedUpdateObserver
{
	[ Range( 0f, 50f ) ]
	public float baseMoveSpeed;

	[ Range( 1f, 5f ) ]
	public float sprintFactor;

	public Transform cameraTransform;

	private Rigidbody _rb;
	private Vector2 _cachedMoveInput;
	private float _currentMoveSpeed;
	private bool _wasMoving;

	private static InputEventManager InputManager => InputEventManager.Instance;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		BindAllEvents();
		FixedUpdateManager.RegisterObserver( this );

		UpdateMoveSpeed( 1f );
	}

	public void ObservedFixedUpdate()
	{
		if( _cachedMoveInput != Vector2.zero )
		{
			MoveInDirection( _cachedMoveInput );
			_wasMoving = true;
		}
		else if( _wasMoving )
		{
			_wasMoving = false;
		}
	}

	private void BindAllEvents()
	{
		InputManager.MovePerformed += OnMovePerformedReceived;
		InputManager.MoveCanceled += OnMoveCanceledReceived;
		InputManager.SprintPerformed += OnSprintPerformedReceived;
		InputManager.SprintCanceled += OnSprintCanceledReceived;
	}

	private void OnMovePerformedReceived( InputAction.CallbackContext ctx ) =>
		_cachedMoveInput = ctx.ReadValue<Vector2>();

	private void OnMoveCanceledReceived( InputAction.CallbackContext ctx ) =>
		_cachedMoveInput = ctx.ReadValue<Vector2>();

	private void OnSprintPerformedReceived()
	{
		UpdateMoveSpeed( sprintFactor );
	}

	private void OnSprintCanceledReceived()
	{
		UpdateMoveSpeed( 1f );
	}

	private void MoveInDirection( Vector2 dir )
	{
		Vector3 direction =
			Vector3.ProjectOnPlane( cameraTransform.right * dir.x + cameraTransform.forward * dir.y,
				Vector3.up ).normalized;

		Vector3 movement = direction * _currentMoveSpeed;
		movement.y = _rb.linearVelocity.y;
		_rb.linearVelocity = movement;
	}

	private void UpdateMoveSpeed( float factor )
	{
		_currentMoveSpeed = baseMoveSpeed * factor;
	}
}