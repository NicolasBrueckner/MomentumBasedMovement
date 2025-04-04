#region

using UnityEngine;

#endregion

public class Example : MonoBehaviour
{
	private readonly float m_MaxDistance = 100f;
	private float m_Speed;
	private bool m_HitDetect;

	private RaycastHit m_Hit;

	private void FixedUpdate()
	{
		//Test to see if there is a hit using a BoxCast
		//Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
		//Also fetch the hit data
		m_HitDetect = Physics.BoxCast( transform.position, new Vector3( 1, 1, 1 ), transform.forward,
			out m_Hit, transform.rotation, m_MaxDistance );
		if( m_HitDetect )
			//Output the name of the Collider your Box hit
			Debug.Log( "Hit : " + m_Hit.collider.name );
	}

	//Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		//Check if there has been a hit yet
		if( m_HitDetect )
		{
			//Draw a Ray forward from GameObject toward the hit
			Gizmos.DrawRay( transform.position, transform.forward * m_Hit.distance );
			//Draw a cube that extends to where the hit exists
			Gizmos.DrawWireCube( transform.position + transform.forward * m_Hit.distance, transform.localScale );
		}
		//If there hasn't been a hit yet, draw the ray at the maximum distance
		else
		{
			//Draw a Ray forward from GameObject toward the maximum distance
			Gizmos.DrawRay( transform.position, transform.forward * m_MaxDistance );
			//Draw a cube at the maximum distance
			Gizmos.DrawWireCube( transform.position + transform.forward * m_MaxDistance, transform.localScale );
		}
	}
}