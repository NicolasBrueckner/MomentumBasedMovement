#region

using System.Collections.Generic;
using System.Linq;
using GS = GrapplingState;

#endregion

public enum GrapplingState
{
	None = 0,
	Grapple,
	Pull,
}

public static class GrapplingStateMachine
{
	public static GS CurrentGrapplingState{ get; private set; } = GS.None;

	private static readonly Dictionary<GS, GS[]> Allowed = new()
	{
		{ GS.None, new[] { GS.Grapple, GS.Pull } },
		{ GS.Grapple, new[] { GS.None } },
		{ GS.Pull, new[] { GS.None } },
	};

	/*
	//allows for self transition
	private static readonly Dictionary<GS, GS[]> AllowedWithSelf =
		BaseAllowed.ToDictionary( kvp => kvp.Key, kvp => kvp.Value.Append( kvp.Key ).ToArray() );
	*/

	public static void ChangeGrapplingState( GS newState )
	{
		if( CurrentGrapplingState == newState )
			return;

		if( Allowed[ CurrentGrapplingState ].Contains( newState ) )
		{
			CurrentGrapplingState = newState;
			return;
		}

		CurrentGrapplingState = GS.None;
	}
}