#region

using UnityEngine;

#endregion

public class RotateWithTarget : MonoBehaviour, IUpdateObserver
{
	public Transform targetTransform;
	public bool x, y, z;

	private Vector3 TargetEuler => targetTransform.rotation.eulerAngles;
	private Vector3 CurrentEuler => transform.rotation.eulerAngles;

	private void Start()
	{
		UpdateManager.RegisterObserver( this );
	}

	public void ObservedUpdate()
	{
		AdjustToTargetRotation();
	}

	private void AdjustToTargetRotation()
	{
		transform.rotation = Quaternion.Euler(
			x ? TargetEuler.x : CurrentEuler.x,
			y ? TargetEuler.y : CurrentEuler.y,
			z ? TargetEuler.z : CurrentEuler.z
		);
	}
}