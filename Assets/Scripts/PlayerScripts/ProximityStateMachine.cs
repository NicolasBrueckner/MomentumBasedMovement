#region

using System.Collections.Generic;
using System.Linq;
using PS = ProximityState;

#endregion

public enum ProximityState
{
	None = 0,
	Ground,
	Wall,
}

public static class ProximityStateMachine
{
	public static PS CurrentProximityState{ get; private set; } = PS.None;

	private static readonly Dictionary<PS, PS[]> Allowed = new()
	{
		{ PS.None, new[] { PS.Ground, PS.Wall } },
		{ PS.Ground, new[] { PS.None, PS.Wall } },
		{ PS.Wall, new[] { PS.None, PS.Ground } },
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