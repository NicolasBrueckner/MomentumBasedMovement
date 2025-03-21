#region

using UnityEngine;

#endregion

public class velocitytest : MonoBehaviour
{
	private Rigidbody _rb;
	private IAEventManager IA_EM => IAEventManager.Instance;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		IA_EM.VelocityTestPerformed += OnTest;
	}

	private void OnTest()
	{
		_rb.AddForce( new( 3, 0, 0 ), ForceMode.Impulse );
	}
}