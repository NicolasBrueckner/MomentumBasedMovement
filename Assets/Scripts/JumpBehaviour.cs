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

	private bool _isJumpButtonDown;
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

	private void OnJumpPerformedReceived()
	{
		_isJumpButtonDown = true;
		_rb.AddForce( Vector3.up * jumpForce, ForceMode.Impulse );
	}

	private void OnJumpCanceledReceived() => _isJumpButtonDown = false;
}