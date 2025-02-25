using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed;
	public Rigidbody rb;

	private Vector2 _cachedMoveInput;

	private IAEventManager IA_EM => IAEventManager.Instance;

	private void Start()
	{
		IA_EM.MovePerformed += OnMovePerformedReceived;
		IA_EM.MoveCanceled += OnMoveCanceledReceived;
	}

	private void FixedUpdate()
	{
		if( _cachedMoveInput != Vector2.zero )
			MoveInDirection( _cachedMoveInput );
		else
			rb.velocity = Vector3.Lerp( rb.velocity, Vector3.zero, 0.3f );
	}

	private void OnMovePerformedReceived( InputAction.CallbackContext ctx )
	{
		_cachedMoveInput = ctx.ReadValue<Vector2>();
	}

	private void OnMoveCanceledReceived( InputAction.CallbackContext ctx )
	{
		_cachedMoveInput = ctx.ReadValue<Vector2>();
	}

	private void MoveInDirection( Vector2 dir )
	{
		Vector3 movement = ( transform.right * dir.x + transform.forward * dir.y ) * moveSpeed;
		rb.velocity = movement;
	}
}