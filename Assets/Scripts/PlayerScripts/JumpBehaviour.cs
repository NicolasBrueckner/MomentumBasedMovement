#region

using UnityEngine;

#endregion

public class JumpBehaviour : MonoBehaviour, IFixedUpdateObserver
{
	[ DebugGUIGraph( autoScale: true, r: 1f ) ]
	private float Height => transform.position.y;

	[ Range( 0f, 50f ) ]
	public float jumpForce;

	public float fallMultiplier = 1f;
	public float lowJumpMultiplier = 1f;
	public GroundCheck groundCheck;

	private bool IsGrounded => groundCheck.IsGrounded;
	private Collider _cashedCollider;
	private bool _isJumpButtonDown;
	private Vector3 _jumpDirection = Vector3.up;
	private Vector3 _gravity;
	private Rigidbody _rb;

	private static InputEventManager InputManager => InputEventManager.Instance;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_gravity = Physics.gravity;
	}

	private void Start()
	{
		FixedUpdateManager.RegisterObserver( this );

		InputManager.JumpPerformed += OnJumpPerformedReceived;
		InputManager.JumpCanceled += OnJumpCanceledReceived;
	}

	public void ObservedFixedUpdate()
	{
		HandleJumpVelocity();
	}

	private void OnJumpPerformedReceived()
	{
		_isJumpButtonDown = true;
		if( IsGrounded )
			_rb.AddForce( _jumpDirection * jumpForce, ForceMode.Impulse );
	}

	private void HandleJumpVelocity()
	{
		switch( _rb.linearVelocity.y )
		{
			case < 0:
				_rb.AddForce( _gravity * ( fallMultiplier - 1 ), ForceMode.Force );
				break;
			case > 0 when !_isJumpButtonDown:
				_rb.AddForce( _gravity * ( lowJumpMultiplier - 1 ), ForceMode.Force );
				break;
		}
	}

	private void SetJumpDirection( Collider other )
	{
		if( _cashedCollider == other )
			return;

		_cashedCollider = other;

		if( !other )
		{
			_jumpDirection = Vector3.zero;
			return;
		}

		Vector3 direction = other.transform.position - transform.position;

		_jumpDirection = Physics.Raycast( transform.position, direction, out RaycastHit hit )
			                 ? hit.normal
			                 : Vector3.zero;
	}

	private void OnJumpCanceledReceived() => _isJumpButtonDown = false;
}