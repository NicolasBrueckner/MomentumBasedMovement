#region

using System.Collections.Generic;
using System.Linq;
using GS = GrapplingState;

#endregion

public enum GrapplingState
{
	None = 0,
	Aiming,
	Hitting,
	Grappling,
}

public static class GrapplingStateMachine
{
	public static GS CurrentGrapplingState{ get; private set; } = GS.None;

	private static readonly Dictionary<GS, GS[]> Allowed = new()
	{
		{ GS.None, new[] { GS.Aiming } },
		{ GS.Aiming, new[] { GS.Hitting, GS.None } },
		{ GS.Hitting, new[] { GS.Grappling, GS.None } },
		{ GS.Grappling, new[] { GS.Aiming, GS.None } },
	};

	/*
	//allows for self transition
	private static readonly Dictionary<GS, GS[]> AllowedWithSelf =
		BaseAllowed.ToDictionary( kvp => kvp.Key, kvp => kvp.Value.Append( kvp.Key ).ToArray() );
	*/

	public static void ChangeGrapplingState( GS newState )
	{
		if( CurrentGrapplingState != newState && Allowed[ CurrentGrapplingState ].Contains( newState ) )
			CurrentGrapplingState = newState;
	}
}