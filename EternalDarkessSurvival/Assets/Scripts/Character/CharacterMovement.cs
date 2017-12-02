using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody rigidbody;

	private Vector3 mousePositionInWorld;
	private Vector3 mouseClickedVector;
	private Vector3 mousePositionRecorded;

	private float distanceToMouseClickedPoint;

	public float MoveSpeed = 1;
	
	// Use this for initialization
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		
		mousePositionInWorld = new Vector3();
		mouseClickedVector = new Vector3();
		mousePositionRecorded = new Vector3();

		distanceToMouseClickedPoint = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Rotate rigidbody
		LookAtMousePos();

		if (Input.GetMouseButtonDown(0))
		{
			OnMouseClickRecord();
		}

		if (distanceToMouseClickedPoint > 0.7)
		{
			MoveCharacter();
		}
		else
		{
			distanceToMouseClickedPoint = 0;
			rigidbody.velocity = Vector3.zero;
		}
		
		Debug.DrawLine(transform.position, mousePositionInWorld);
	}

	void LookAtMousePos()
	{
		GetMousePositionWorld();
		rigidbody.rotation = Quaternion.LookRotation(mousePositionInWorld);
	}

	void GetMousePositionWorld()
	{
		RaycastHit rayHit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out rayHit))
		{
			mousePositionInWorld = ray.GetPoint(rayHit.distance);
		}
	}

	void OnMouseClickRecord()
	{
		Vector3 moveVector = (mousePositionInWorld - transform.position).normalized;
		mousePositionRecorded = mousePositionInWorld;
		mouseClickedVector = new Vector3(moveVector.x, 0, moveVector.z);
		
		UpdateDistance();
	}

	void UpdateDistance()
	{
		distanceToMouseClickedPoint = (mousePositionRecorded - transform.position).magnitude;
	}

	void MoveCharacter()
	{
		UpdateDistance();
		rigidbody.velocity = mouseClickedVector * MoveSpeed;
	}
	
	
	
	
}
