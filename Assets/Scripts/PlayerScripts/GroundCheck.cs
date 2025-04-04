#region

using UnityEngine;

#endregion

public class GroundCheck : MonoBehaviour, IFixedUpdateObserver
{
	public bool IsGrounded{ get; private set; }

	public Vector3 halfExtents;
	public float maxDistance;
	public LayerMask groundLayers;

	private RaycastHit _hit;
	private static GameplayEventManager GameplayManager => GameplayEventManager.Instance;
	private static readonly Vector3 Direction = Vector3.down;

	private void Start()
	{
		FixedUpdateManager.RegisterObserver( this );
	}

	public void ObservedFixedUpdate()
	{
		IsGrounded = Check();
	}

	private bool Check() => Physics.BoxCast( transform.position, halfExtents, Direction, out _hit,
		Quaternion.identity, maxDistance, groundLayers );

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		//Check if there has been a hit yet
		if( IsGrounded )
		{
			//Draw a Ray forward from GameObject toward the hit
			Gizmos.DrawRay( transform.position, Direction * _hit.distance );
			//Draw a cube that extends to where the hit exists
			Gizmos.DrawWireCube( transform.position + Direction * _hit.distance, halfExtents );
		}
		//If there hasn't been a hit yet, draw the ray at the maximum distance
		else
		{
			//Draw a Ray forward from GameObject toward the maximum distance
			Gizmos.DrawRay( transform.position, Direction * maxDistance );
			//Draw a cube at the maximum distance
			Gizmos.DrawWireCube( transform.position + Direction * maxDistance, halfExtents );
		}
	}
}