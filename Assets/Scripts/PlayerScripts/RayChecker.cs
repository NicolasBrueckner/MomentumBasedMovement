#region

using UnityEngine;

#endregion

public class RayChecker : IUpdateObserver
{
	public bool IsHitting{ get; private set; }
	public RaycastHit Hit{ get; private set; }

	private bool _canCheck;
	private readonly Transform _origin;
	private readonly LayerMask _mask;
	private readonly float _maxLength;

	public RayChecker( Transform origin, LayerMask mask, float maxLength, bool canCheck )
	{
		_origin = origin;
		_mask = mask;
		_maxLength = maxLength;

		AllowCheck( canCheck );
		UpdateManager.RegisterObserver( this );
	}

	~RayChecker()
	{
		UpdateManager.UnregisterObserver( this );
	}

	public void ObservedUpdate()
	{
		if( !_canCheck )
			return;

		IsHitting = Physics.Raycast( _origin.position, _origin.forward, out RaycastHit hit, _maxLength, _mask );
		Hit = hit;
	}

	public void AllowCheck( bool value )
	{
		_canCheck = value;
	}
}