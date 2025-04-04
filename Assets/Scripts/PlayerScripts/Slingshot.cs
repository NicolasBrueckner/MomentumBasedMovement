#region

using UnityEngine;

#endregion

[ RequireComponent( typeof( ConfigurableJoint ) ) ]
[ RequireComponent( typeof( AimTracer ) ) ]
public class Slingshot : MonoBehaviour
{
	private ConfigurableJoint _joint;

	private InputEventManager InputEm => InputEventManager.Instance;
	private AimTracer _aimTracer;
	private Rigidbody _targetBody;

	private void Awake()
	{
		_joint = GetComponent<ConfigurableJoint>();
		_aimTracer = GetComponent<AimTracer>();
	}

	private void Start()
	{
		_aimTracer.TargetHit += UpdateTarget;
		InputEm.ShootPerformed += AttachSlingshot;
	}

	private void UpdateTarget( RaycastHit hit ) => _targetBody = hit.collider.attachedRigidbody;

	private void AttachSlingshot()
	{
		if( _targetBody )
			_joint.connectedBody = _targetBody;
	}
}