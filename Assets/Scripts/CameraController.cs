#region

using Unity.Cinemachine;
using UnityEngine;

#endregion

public class CameraController : MonoBehaviour
{
	[ Range( 0f, 10f ) ]
	public float xCameraSpeed;

	[ Range( 0f, 10f ) ]
	public float yCameraSpeed;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

		CinemachineInputAxisController ctr = GetComponent<CinemachineInputAxisController>();

		foreach( InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller c in ctr.Controllers )
		{
			c.Input.Gain = c.Name switch
			{
				"Look X (Pan)"  => xCameraSpeed,
				"Look Y (Tilt)" => -yCameraSpeed,
				_               => c.Input.Gain,
			};
		}
	}
}