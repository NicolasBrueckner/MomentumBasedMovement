#region

using System.Collections.Generic;
using System.Linq;
using PS = ProximityState;

#endregion

public enum ProximityState
{
	InAir = 0,
	OnGround,
	OnWall,
}

public static class ProximityStateMachine
{
	public static PS CurrentProximityState{ get; private set; } = PS.InAir;

	private static readonly Dictionary<PS, PS[]> Allowed = new()
	{
		{ PS.InAir, new[] { PS.OnGround, PS.OnWall } },
		{ PS.OnGround, new[] { PS.InAir, PS.OnWall } },
		{ PS.OnWall, new[] { PS.InAir, PS.OnGround } },
	};

	/*
	//allows for self transition
	private static readonly Dictionary<PS, PS[]> AllowedWithSelf =
		BaseAllowed.ToDictionary( kvp => kvp.Key, kvp => kvp.Value.Append( kvp.Key ).ToArray() );
	*/

	public static void ChangeProximityState( PS newState )
	{
		if( CurrentProximityState != newState && Allowed[ CurrentProximityState ].Contains( newState ) )
			CurrentProximityState = newState;
	}
}