using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public Vector3 CameraOffsetVector;
    public GameObject Player;

	// Use this for initialization
	void Start () {
	    Camera.main.transform.position = Player.transform.position + CameraOffsetVector;
	    Camera.main.transform.LookAt(Player.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
	{

	    Camera.main.transform.position = Player.transform.position + CameraOffsetVector;

	}
}
