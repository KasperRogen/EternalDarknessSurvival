using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class ClickMovement : MonoBehaviour
{
	private Rigidbody rigidBody;
	private Camera camera;

	private float distanceToMove;
	private Vector3 mouseClickedVector;
	private Vector3 mousePointPosition;
	private Vector3 mousePointRecorded;
	
	
	public float MoveSpeed = 1.0f;
	
	
	
	
	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		camera = Camera.main;

		distanceToMove = 0f;
		mouseClickedVector = new Vector3();
		mousePointPosition = new Vector3();
		mousePointRecorded = new Vector3();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Rotate at mouse position
		GetMousePositionSpace();
		rigidBody.rotation = Quaternion.Euler(0, Mathf.Atan2(mousePointPosition.x, mousePointPosition.z) * Mathf.Rad2Deg - 90, 0);
		
		// Check if mouse has been clicked => Move towards position
		if (Input.GetMouseButtonDown(1))
		{
			GetMouseClickPosition();
		}

		if (distanceToMove > 1.7f)
		{
			moveCharacter();
		}
		else
		{
			//Debug.Log("Happend");
			distanceToMove = 0;
			rigidBody.velocity = Vector3.zero;
		}
	}

	private void GetMousePositionSpace()
	{
		RaycastHit rayHit;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		Vector3 hitPoint = new Vector3();
		
		if (Physics.Raycast(ray, out rayHit))
		{
			hitPoint = rayHit.point;

		}

		mousePointPosition = hitPoint;
	}

	private void GetUpdatedDistanceToMove()
	{
		distanceToMove = (mousePointRecorded - (transform.position)).magnitude;
	}

	private void GetMouseClickPosition()
	{
		mouseClickedVector = new Vector3((mousePointPosition - transform.position).normalized.x, 0, (mousePointPosition - transform.position).normalized.z);
		mousePointRecorded = mousePointPosition;
		GetUpdatedDistanceToMove();
		Debug.DrawLine(transform.position, mousePointRecorded);
	}

	private void moveCharacter()
	{
		GetUpdatedDistanceToMove();
		rigidBody.velocity = mouseClickedVector * MoveSpeed;
	}
}
