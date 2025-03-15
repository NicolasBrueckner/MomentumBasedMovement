#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

[ RequireComponent( typeof( Camera ) ) ]
public class CameraController : MonoBehaviour
{
	public float camSpeed;
	public Transform playerTransform;

	private Vector2 _cachedLookInput;
	private float _pitch;

	private IAEventManager IA_EM => IAEventManager.Instance;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Start()
	{
		IA_EM.LookPerformed += OnLookPerformedReceived;
		IA_EM.LookCanceled += OnLookCanceledReceived;
	}

	private void LateUpdate()
	{
		RotateToDirection( _cachedLookInput );
	}

	private void OnLookPerformedReceived( InputAction.CallbackContext ctx )
	{
		_cachedLookInput = ctx.ReadValue<Vector2>();
	}

	private void OnLookCanceledReceived( InputAction.CallbackContext ctx )
	{
		_cachedLookInput = ctx.ReadValue<Vector2>();
	}

	private void RotateToDirection( Vector2 dir )
	{
		float mouseX = dir.x * camSpeed * Time.deltaTime;
		float mouseY = dir.y * camSpeed * Time.deltaTime;

		_pitch -= mouseY;
		_pitch = Mathf.Clamp( _pitch, -90f, 90f );

		transform.localRotation = Quaternion.Euler( _pitch, 0f, 0f );
		playerTransform.Rotate( Vector3.up * mouseX );
	}
}