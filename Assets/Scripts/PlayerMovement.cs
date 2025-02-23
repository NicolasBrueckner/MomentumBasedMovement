using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed;
	public Rigidbody rb;
	
	private IAEventManager IA_EM=>IAEventManager.Instance;

	private void Start()
	{
		IA_EM.MovePerformed += OnMovePerformedReceived;
		IA_EM.MoveCanceled += OnMoveCanceledReceived;
	}

	private void OnMovePerformedReceived( InputAction.CallbackContext ctx )
	{
		Vector2 dir = ctx.ReadValue<Vector2>();
		MoveInDirection(dir);
	}

	private void OnMoveCanceledReceived( InputAction.CallbackContext ctx )
	{
		rb.velocity = Vector3.zero;
	}

	private void MoveInDirection( Vector2 dir )
	{
		Vector3 movement = new Vector3(dir.x, 0f, dir.y) * moveSpeed;
		rb.velocity = movement;
	}
}
