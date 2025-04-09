#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class Slingshot : MonoBehaviour, IFixedUpdateObserver
{
	public ConfigurableJoint joint;
	public float rayMaxLength;
	public LayerMask rayLayerMask;
	public Transform cameraTransform;

	private static InputEventManager InputEm => InputEventManager.Instance;

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
		if( _isAiming )
			CheckForTarget();
		else
			_isHitting = false;
	}

	private void OnAimPerformedReceived( InputAction.CallbackContext ctx ) => _isAiming = true;
	private void OnAimCanceledReceived( InputAction.CallbackContext ctx )  => _isAiming = false;

	private void OnShootPerformedReceived()
	{
		if( _isHitting )
			AttachSlingshot();
	}

	private void OnShootCanceledReceived()
	{
		DetachSlingshot();
	}

	private void AttachSlingshot()
	{
		Debug.Log( "hitting" );

		joint.anchor = transform.InverseTransformPoint( _hit.point );

		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Limited;
		joint.zMotion = ConfigurableJointMotion.Limited;
	}

	private void DetachSlingshot()
	{
		/*Debug.Log( "detaching" );

		joint.connectedAnchor = Vector3.zero;
		joint.xMotion = ConfigurableJointMotion.Free;
		joint.yMotion = ConfigurableJointMotion.Free;
		joint.zMotion = ConfigurableJointMotion.Free;*/
	}

	private void CheckForTarget()
	{
		Vector3 rayDirection = cameraTransform.forward;

		_isHitting = Physics.Raycast( cameraTransform.position, rayDirection, out _hit, rayMaxLength, rayLayerMask );

		if( _isHitting )
			Debug.Log( $"hitting: {_hit.point}" );
	}
}