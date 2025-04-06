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

		if( IsGrounded )
		{
			Gizmos.DrawRay( transform.position, Direction * _hit.distance );
			Gizmos.DrawWireCube( transform.position + Direction * _hit.distance, halfExtents );
		}
		else
		{
			Gizmos.DrawRay( transform.position, Direction * maxDistance );
			Gizmos.DrawWireCube( transform.position + Direction * maxDistance, halfExtents );
		}
	}
}