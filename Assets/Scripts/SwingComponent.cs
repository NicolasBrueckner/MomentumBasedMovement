using UnityEngine;

[ RequireComponent( typeof( Rigidbody ) ) ]
public class SwingComponent : MonoBehaviour
{
	public Transform swingAnchorTransform;
	public float maxSwingLength;

	private Rigidbody _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		float currentDistance = Vector3.Distance( swingAnchorTransform.position, transform.position );
		if( currentDistance >= maxSwingLength )
		{
			_rb.linearVelocity = ComputeNewVelocity( transform.position, _rb.linearVelocity, swingAnchorTransform.position );
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawLine( transform.position, swingAnchorTransform.position );
	}

	private Vector3 ComputeNewVelocity( Vector3 position, Vector3 velocity, Vector3 anchorPoint )
	{
		Vector3 radialDirection = ( position - anchorPoint ).normalized;
		Vector3 radialVelocity = Vector3.Dot( velocity, radialDirection ) * radialDirection;
		Vector3 tangentialVelocity = velocity - radialVelocity;

		return tangentialVelocity;
	}
}