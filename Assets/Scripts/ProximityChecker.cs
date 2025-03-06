#region

using System;
using UnityEngine;

#endregion

[ RequireComponent( typeof( SphereCollider ) ) ]
public class ProximityChecker : MonoBehaviour
{
	public LayerMask layerMask;

	public event Action<Collider> ProximityEntered;
	public event Action ProximityExited;

	private void OnTriggerEnter( Collider other )
	{
		ProximityEntered?.Invoke( other );
	}

	private void OnTriggerExit( Collider other )
	{
		ProximityExited?.Invoke();
	}
}