#region

using UnityEngine;

#endregion

public class DebugGraphs : MonoBehaviour
{
	public Rigidbody playerRb;

	[ DebugGUIGraph() ]
	private float Height => playerRb.position.y;

	[ DebugGUIGraph( 1f, 0f, 0f, autoScale: true ) ]
	private float Speed => playerRb.linearVelocity.magnitude;
}