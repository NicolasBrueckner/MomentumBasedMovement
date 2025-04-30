#region

using System;
using Unity.Mathematics;
using UnityEngine;
using static GrapplingStateMachine;

#endregion


public class GrapplingBehaviour : MonoBehaviour, IFixedUpdateObserver
{
	public float rayMaxLength;
	public LayerMask rayLayerMask;
	public Transform camTransform;
	public Rigidbody rb;
	public LineRenderer rope;

	private RaycastHit _currentHit;
	private RayChecker _rayCheck;
	private static InputEventManager InputManager => InputEventManager.Instance;

	private void Start()
	{
		InputManager.GrapplePerformed += OnGrapplePerformedReceived;
		InputManager.GrappleCanceled += OnGrappleCanceledReceived;
		InputManager.PullPerformed += OnPullPerformedReceived;
		InputManager.PullCanceled += OnPullCanceledReceived;

		FixedUpdateManager.RegisterObserver( this );

		_rayCheck = new( camTransform, rayLayerMask, rayMaxLength, true );
	}

	public void ObservedFixedUpdate()
	{
		switch( CurrentGrapplingState )
		{
			case GrapplingState.None:
				break;
			case GrapplingState.Grapple:
				HandleGrappling();
				break;
			case GrapplingState.Pull:
				HandlePulling();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		DrawGrapplingLine();
	}

	private void OnGrapplePerformedReceived()
	{
		if( !_rayCheck.IsHitting )
			return;

		_currentHit = _rayCheck.Hit;
		ChangeGrapplingState( GrapplingState.Grapple );
	}

	private static void OnGrappleCanceledReceived() => ChangeGrapplingState( GrapplingState.None );

	private void OnPullPerformedReceived()
	{
		if( !_rayCheck.IsHitting )
			return;

		_currentHit = _rayCheck.Hit;
		ChangeGrapplingState( GrapplingState.Pull );
	}

	private static void OnPullCanceledReceived() => ChangeGrapplingState( GrapplingState.None );

	private void HandleGrappling()
	{
		Vector3 currentVelocity = rb.linearVelocity;
		Vector3 ropeDirection = ( _currentHit.point - transform.position ).normalized;

		rb.linearVelocity = Vector3.ProjectOnPlane( currentVelocity, ropeDirection ) * 0.9995f;
	}

	private void HandlePulling()
	{
		float distance = math.distance( transform.position, _currentHit.point );

		if( distance <= 1 )
			return;
		
		const float pullMaxSpd = 100;
		const float pullMinSpd = 20;
		
		float t = math.saturate( distance / 10 );
		float speed=math.lerp(pullMinSpd, pullMaxSpd, t  );
		float3 direction = math.normalize( _currentHit.point - transform.position );
		
		rb.linearVelocity = direction * speed;
	}

	private void DrawGrapplingLine()
	{
		if( _rayCheck.IsHitting )
		{
			rope.enabled = true;
			rope.SetPosition( 0, transform.position );
			rope.SetPosition( 1, _currentHit.point );
		}
		else
		{
			rope.enabled = false;
		}
	}
}