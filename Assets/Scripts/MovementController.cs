#region

using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

#endregion

public class MovementController : MonoBehaviour
{
	public float baseMoveSpeed;
	public float sprintFactor;
	public float jumpForce;
	public Rigidbody rb;
	public ProximityChecker proximityChecker;

	private Vector2 _cachedMoveInput;
	private float _currentMoveSpeed;
	private bool _isGrounded;
	private Vector3 _surfaceNormal;
	private IAEventManager IA_EM => IAEventManager.Instance;

	private void Start()
	{
		BindAllEvents();

		UpdateMoveSpeed( 1f );
	}

	private void FixedUpdate()
	{
		if( _cachedMoveInput != Vector2.zero )
		{
			MoveInDirection( _cachedMoveInput );
		}
		else
		{
			float yVelocity = rb.velocity.y;
			Vector3 hVelocity = new( rb.velocity.x, 0f, rb.velocity.z );
			Vector3 newVelocity = Vector3.Lerp( hVelocity, Vector3.zero, 0.3f );

			newVelocity.y = yVelocity;
			rb.velocity = newVelocity;
		}
	}

	private void BindAllEvents()
	{
		proximityChecker.ProximityEntered += OnProximityEnteredReceived;
		proximityChecker.ProximityExited += OnProximityExitedReceived;

		IA_EM.MovePerformed += OnMovePerformedReceived;
		IA_EM.MoveCanceled += OnMoveCanceledReceived;
		IA_EM.JumpPerformed += OnJumpPerformedReceived;
		IA_EM.SprintPerformed += OnSprintPerformedReceived;
		IA_EM.SprintCanceled += OnSprintCanceledReceived;
	}

	private void OnMovePerformedReceived( InputAction.CallbackContext ctx )
	{
		_cachedMoveInput = ctx.ReadValue<Vector2>();
	}

	private void OnMoveCanceledReceived( InputAction.CallbackContext ctx )
	{
		_cachedMoveInput = ctx.ReadValue<Vector2>();
	}

	private void OnJumpPerformedReceived()
	{
		if( _isGrounded )
			AddJumpForce();
	}

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
		Vector3 movement = ( transform.right * dir.x + transform.forward * dir.y ) * _currentMoveSpeed;

		movement.y = rb.velocity.y;
		rb.velocity = movement;
	}

	private void AddJumpForce()
	{
		rb.AddForce( _surfaceNormal * jumpForce, ForceMode.Impulse );
	}

	private void UpdateMoveSpeed( float factor )
	{
		_currentMoveSpeed = baseMoveSpeed * factor;
	}

	private void OnProximityEnteredReceived( Collider other )
	{
		_isGrounded = true;
		_surfaceNormal = GetSurfaceNormal( other );
	}

	private void OnProximityExitedReceived()
	{
		_isGrounded = false;
		_surfaceNormal = Vector3.zero;
	}

	private Vector3 GetSurfaceNormal( Collider other )
	{
		Vector3 direction = other.transform.position - transform.position;

		return Physics.Raycast( transform.position, direction, out RaycastHit hit )
			       ? hit.normal
			       : Vector3.zero;
	}
}