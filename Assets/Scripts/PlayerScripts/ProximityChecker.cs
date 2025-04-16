#region

using UnityEngine;

#endregion

public enum ProximityState
{
	OnGround,
	OnWall,
	InAir,
}

public class ProximityChecker : MonoBehaviour, IUpdateObserver
{
	public BoxCollider groundCollider;
	public BoxCollider wallCollider;
	public LayerMask groundLayers;
	public LayerMask wallLayers;

	public static ProximityState CurrentState{ get; private set; }

	private bool _isOnGround;
	private bool _isOnWall;

	private RaycastHit _hit;
	private Vector3 _halfSizeGround;
	private Vector3 _halfSizeWall;
	private static readonly Quaternion Identity = Quaternion.identity;

	private void Awake()
	{
		_halfSizeGround = groundCollider.size / 2.0f;
		_halfSizeWall = wallCollider.size / 2.0f;
	}

	private void Start()
	{
		UpdateManager.RegisterObserver( this );
	}

	public void ObservedUpdate()
	{
		Vector3 groundColliderCenter = groundCollider.transform.TransformPoint( groundCollider.center );
		Vector3 wallColliderCenter = wallCollider.transform.TransformPoint( wallCollider.center );
		_isOnGround = IsInProximity( groundColliderCenter, _halfSizeGround, groundLayers );
		_isOnWall = IsInProximity( wallColliderCenter, _halfSizeWall, wallLayers );

		switch( _isOnGround )
		{
			case false when !_isOnWall:
				CurrentState = ProximityState.InAir;
				break;
			case true:
				CurrentState = ProximityState.OnGround;
				break;
			default:
			{
				if( _isOnWall )
					CurrentState = ProximityState.OnWall;
				break;
			}
		}
	}

	private static bool IsInProximity( Vector3 center, Vector3 halfExtents, LayerMask mask ) =>
		Physics.CheckBox( center, halfExtents, Identity, mask );
}