#region

using UnityEngine;

#endregion

public class ForceTest : MonoBehaviour
{
	public Rigidbody rb;
	public float force;

	[ ContextMenu( "Push" ) ]
	public void Push()
	{
		rb.AddForce( Vector3.right * force, ForceMode.Impulse );
	}
}