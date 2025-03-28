#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public interface ILateUpdateObserver
{
	void ObservedLateUpdate();
}

public class LateUpdateManager : MonoBehaviour
{
	private static readonly List<ILateUpdateObserver> _observers = new();
	private static readonly List<ILateUpdateObserver> _pendingObservers = new();
	private static int _currentIndex;

	private void Update()
	{
		for( _currentIndex = _observers.Count - 1; _currentIndex >= 0; _currentIndex-- )
			_observers[ _currentIndex ].ObservedLateUpdate();

		_observers.AddRange( _pendingObservers );
		_pendingObservers.Clear();
	}

	public static void RegisterObserver( ILateUpdateObserver observer )
	{
		_pendingObservers.Add( observer );
	}

	public static void UnregisterObserver( ILateUpdateObserver observer )
	{
		_observers.Remove( observer );
		_currentIndex--;
	}
}