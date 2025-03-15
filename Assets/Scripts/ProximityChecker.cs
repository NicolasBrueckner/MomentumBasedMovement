#region

using System;
using System.Collections;
using UnityEngine;

#endregion

[ RequireComponent( typeof( SphereCollider ) ) ]
public class ProximityChecker : MonoBehaviour
{
	public LayerMask layerMask;
	public float checkFrequency;

	public event Action<Collider> ProximityEntered;
	public event Action ProximityExited;

	private SphereCollider _self;
	private Collider _current;

	private void Awake()
	{
		_self = GetComponent<SphereCollider>();
	}

	private void Start()
	{
		StartCoroutine( CheckProximity() );
	}

	private IEnumerator CheckProximity()
	{
		while( true )
		{
			Collider[] colliders = Physics.OverlapSphere( transform.position, _self.radius, layerMask );

			switch( colliders.Length )
			{
				case > 1:
					Collider other = GetClosestCollider( colliders );
					if( other == _current )
						break;
					_current = other;
					ProximityEntered?.Invoke( other );
					break;
				case 1:
					_current = colliders[ 0 ];
					ProximityEntered?.Invoke( colliders[ 0 ] );
					break;
				case 0:
					_current = null;
					ProximityExited?.Invoke();
					break;
			}

			yield return new WaitForSeconds( checkFrequency );
		}
	}

	// expects a valid Collider array with size > 0
	private Collider GetClosestCollider( Collider[] colliders )
	{
		Collider closest = colliders[ 0 ];
		float minDistanceSquared = float.MaxValue;

		foreach( Collider other in colliders )
		{
			float distanceSquared = ( transform.position - other.transform.position ).sqrMagnitude;

			if( !( distanceSquared < minDistanceSquared ) )
				continue;

			closest = other;
			minDistanceSquared = distanceSquared;
		}

		return closest;
	}
}