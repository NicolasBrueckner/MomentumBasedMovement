#region

using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using static Utility;

#endregion

[ RequireComponent( typeof( SphereCollider ) ) ]
public class ProximityChecker : MonoBehaviour, IUpdateObserver
{
	public LayerMask layerMask;
	public HashSet<Collider> collidersInProximity = new();

	public Collider Current{ get; private set; }

	private void Start()
	{
		UpdateManager.RegisterObserver( this );
	}

	public void ObservedUpdate()
	{
		using( new ProfilerMarker( "ObservedFixedUpdate in ProximityChecker" ).Auto() )
			SetClosestCollider();
	}

	private void OnTriggerEnter( Collider other )
	{
		if( IsInLayerMask( other.gameObject, layerMask ) )
			collidersInProximity.Add( other );
	}

	private void OnTriggerExit( Collider other )
	{
		if( IsInLayerMask( other.gameObject, layerMask ) )
			collidersInProximity.Remove( other );
	}

	private void SetClosestCollider()
	{
		Debug.Log( $"possible colliders: {collidersInProximity.Count}" );
		switch( collidersInProximity.Count )
		{
			case > 1:
				Collider other = GetClosestCollider( collidersInProximity );
				if( other == Current )
					break;
				Current = other;
				break;
			case 1:
				Current = collidersInProximity.First();
				Debug.Log( $"one collider: {Current.gameObject.name}" );
				break;
			case 0:
				Current = null;
				break;
		}
	}

	private Collider GetClosestCollider( HashSet<Collider> colliders )
	{
		if( colliders == null || colliders.Count == 0 )
			return null;

		Collider closest = null;
		Vector3 position = transform.position;
		float minDistanceSquared = float.MaxValue;

		foreach( Collider col in colliders )
		{
			Vector3 closestPoint = col.ClosestPoint( position );
			float distanceSquared = ( closestPoint - position ).sqrMagnitude;

			if( !( distanceSquared < minDistanceSquared ) )
				continue;

			minDistanceSquared = distanceSquared;
			closest = col;

			if( minDistanceSquared < float.Epsilon )
				break;
		}

		return closest;
	}


	// don't use this method often as its quite expensive (also resets the colliders hashset)
	private void ManualProximityCheck()
	{
		Collider[] colliders =
			Physics.OverlapSphere( transform.position, GetComponent<SphereCollider>().radius, layerMask );
		collidersInProximity = new HashSet<Collider>( colliders );

		SetClosestCollider();
	}
}