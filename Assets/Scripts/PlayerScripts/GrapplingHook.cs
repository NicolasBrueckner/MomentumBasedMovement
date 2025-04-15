#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class GrapplingHook : MonoBehaviour, IFixedUpdateObserver
{
	public float rayMaxLength;
	public LayerMask rayLayerMask;
	public Transform cameraTransform;
	public Rigidbody rb;
	public float grapplingForce;

	private RaycastHit _hit;
	private bool _isHitting;
	private bool _isAiming;

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
		_isHitting = _isAiming && CheckForTarget();

		Debug.Log( $"velocity: {rb.linearVelocity.magnitude}" );
	}

	private void OnAimPerformedReceived( InputAction.CallbackContext ctx ) => _isAiming = true;
	private void OnAimCanceledReceived( InputAction.CallbackContext ctx )  => _isAiming = false;

	private void OnShootPerformedReceived()
	{
		if( !_isHitting )
			return;

		Vector3 direction = ( _hit.point - transform.position ).normalized;
		rb.AddForce( direction * grapplingForce, ForceMode.Impulse );
	}

	private void OnShootCanceledReceived()
	{
	}

	private bool CheckForTarget() =>
		Physics.Raycast(
			cameraTransform.position,
			cameraTransform.forward,
			out _hit,
			rayMaxLength,
			rayLayerMask );
}