#region

using UnityEngine;

#endregion

public static class Utility
{
	public static T CreateSingleton<T>( T instance, GameObject gameObject ) where T : Component
	{
		if( !instance )
		{
			instance = gameObject.GetComponent<T>();
			if( !instance )
				instance = gameObject.AddComponent<T>();
			Object.DontDestroyOnLoad( gameObject );
			return instance;
		}

		if( instance.gameObject != gameObject )
			Object.Destroy( gameObject );
		return instance;
	}

	public static bool IsInLayerMask( GameObject gameObject, LayerMask layerMask ) =>
		( layerMask.value & ( 1 << gameObject.layer ) ) != 0;
}