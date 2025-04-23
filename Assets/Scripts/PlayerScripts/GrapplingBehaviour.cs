#region

using UnityEngine;
using UnityEngine.InputSystem;
using static GrapplingStateMachine;

#endregion


public class GrapplingBehaviour : MonoBehaviour, IFixedUpdateObserver
{
	public float rayMaxLength;
	public LayerMask rayLayerMask;
	public Transform camTransform;
	public Rigidbody rb;
	public LineRenderer rope;

	private RaycastHit _hit;

	private static InputEventManager InputManager => InputEventManager.Instance;

	private void Start()
	{
		InputManager.AimPerformed += OnAimPerformedReceived;
		InputManager.AimCanceled += OnAimCanceledReceived;
		InputManager.ShootPerformed += OnShootPerformedReceived;
		InputManager.ShootCanceled += OnShootCanceledReceived;

		FixedUpdateManager.RegisterObserver( this );
	}

	public void ObservedFixedUpdate()
	{
		CheckIfHitting();
		HandleGrappling();
		DrawGrapplingLine();
	}

	private static void OnAimPerformedReceived( InputAction.CallbackContext ctx )
	{
		if( CurrentGrapplingState != GrapplingState.None )
			return;

		ChangeGrapplingState( GrapplingState.Aiming );
	}

	private static void OnAimCanceledReceived( InputAction.CallbackContext ctx ) =>
		ChangeGrapplingState( GrapplingState.None );

	private static void OnShootPerformedReceived()
	{
		if( CurrentGrapplingState != GrapplingState.Hitting )
			return;

		ChangeGrapplingState( GrapplingState.Grappling );
	}

	private static void OnShootCanceledReceived()
	{
		if( CurrentGrapplingState != GrapplingState.Grappling )
			return;

		ChangeGrapplingState( GrapplingState.Aiming );
	}

	private bool CheckIfHitting()
	{
		if( CurrentGrapplingState != GrapplingState.Aiming )
			return false;

		if( !Physics.Raycast( camTransform.position, camTransform.forward, out _hit, rayMaxLength, rayLayerMask ) )
			return false;

		ChangeGrapplingState( GrapplingState.Hitting );
		return true;
	}

	private void HandleGrappling()
	{
		if( CurrentGrapplingState != GrapplingState.Grappling )
			return;

		Vector3 currentVelocity = rb.linearVelocity;
		Vector3 ropeDirection = ( _hit.point - transform.position ).normalized;

		rb.linearVelocity = Vector3.ProjectOnPlane( currentVelocity, ropeDirection ) * 0.9995f;
	}


	private void DrawGrapplingLine()
	{
		if( CurrentGrapplingState == GrapplingState.Grappling )
		{
			rope.enabled = true;
			rope.SetPosition( 0, transform.position );
			rope.SetPosition( 1, _hit.point );
		}
		else
		{
			rope.enabled = false;
		}
	}
}