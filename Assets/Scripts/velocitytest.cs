#region

using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#endregion

public class velocitytest : MonoBehaviour
{
	private Stopwatch _stopwatch;
	private float _fixedElapsedTime;
	private Rigidbody _rb;
	private bool _started;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_stopwatch = new Stopwatch();
	}

	public void FixedUpdate()
	{
		if( _rb.linearVelocity.y < 0 )
		{
			_fixedElapsedTime += Time.deltaTime;

			if( !_started )
			{
				if( _stopwatch.IsRunning )
				{
					_stopwatch.Stop();
					_stopwatch.Reset();
				}

				_stopwatch.Start();
				_started = true;
			}
		}

		Debug.Log( $"velocity y: {_rb.linearVelocity.y}" );
	}


	private void OnTriggerEnter( Collider other )
	{
		_stopwatch.Stop();
		Debug.Log( $"end watch: {_stopwatch.ElapsedMilliseconds}" );
		Debug.Log( $"end fixed: {_fixedElapsedTime}" );
	}
}