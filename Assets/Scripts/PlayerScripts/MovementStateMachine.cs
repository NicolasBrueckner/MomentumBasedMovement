#region

using System;
using UnityEngine;

#endregion

public enum MovementState
{
	OnGround,
	OnWall,
	InAir,
}

public class MovementStateMachine : MonoBehaviour
{
	public MovementState currentState;

	public void ChangeState( MovementState newState )
	{
		currentState = newState;
		OnStateChange( newState );
	}

	private static void OnStateChange( MovementState newState )
	{
		switch( newState )
		{
			case MovementState.OnGround:
				break;
			case MovementState.OnWall:
				break;
			case MovementState.InAir:
				break;
			default:
				throw new ArgumentOutOfRangeException( nameof( newState ), newState, null );
		}
	}

	private void HandleOnGroundChange()
	{
	}

	private void HandleOnWallChange()
	{
	}

	private void HandleInAirChange()
	{
	}
}