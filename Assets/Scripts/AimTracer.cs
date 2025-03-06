#region

using System;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class AimTracer : MonoBehaviour
{
	public Transform rayOrigin;
	public float rayMaxLength;
	public LayerMask rayLayerMask;
	public event Action<RaycastHit> TargetHit;

	private Transform _rayTarget;
	private Vector3 _hitPoint;
	private bool _isAiming;

	private IAEventManager IA_EM => IAEventManager.Instance;

	private void Start()
	{
		IA_EM.AimPerformed += OnAimPerformedReceived;
		IA_EM.AimCanceled += OnAimCanceledReceived;
	}

	private void Update()
	{
		if( _isAiming )
			CheckForTarget();
	}

	private void OnAimPerformedReceived( InputAction.CallbackContext ctx ) => _isAiming = true;
	private void OnAimCanceledReceived( InputAction.CallbackContext ctx )  => _isAiming = false;

	private void CheckForTarget()
	{
		Vector3 rayDirection = rayOrigin.forward;

		if( Physics.Raycast( rayOrigin.position, rayDirection, out RaycastHit hitInfo, rayMaxLength, rayLayerMask ) )
		{
			_hitPoint = hitInfo.point; //only for debugging
			TargetHit?.Invoke( hitInfo );
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine( _hitPoint, rayOrigin.position );
		Gizmos.DrawSphere( _hitPoint, .5f );
	}
}