#region

using System;
using JetBrains.Annotations;
using UnityEngine;

#endregion

public class GameplayEventManager : MonoBehaviour
{
	public static GameplayEventManager Instance{ get; private set; }

	public event Action<Collider> ProximityUpdate; //always check for null collider when using this

	private void Awake()
	{
		Instance = Utility.CreateSingleton( Instance, gameObject );
	}

	public void OnProximityUpdate( [ CanBeNull ] Collider other ) => ProximityUpdate?.Invoke( other );
}