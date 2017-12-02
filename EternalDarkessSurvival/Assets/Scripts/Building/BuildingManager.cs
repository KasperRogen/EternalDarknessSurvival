using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    //Call Build with the selected object as parameter. Should be called in update continually as long as the player is building.
    //Returns false every time the player hasn't built anything, and true when the player has built, to make it easier to remove the 
    //Resources from the players inventory.
    public Material TransparentMaterial;
    private Material _objectMat;
    private int _rotation = 0;
    private bool _firstRun = true;
    private LayerMask layerMask;
    public GameObject wall;
    public LayerMask BuildCheckMask;
    private bool IsFree = false;

    private GameObject _buildable;
	// Use this for initialization

    void Update()
    {
        Build(wall);
    }

    public bool Build(GameObject Buildable)
    {
        IsFree = true;
        if (_firstRun)
        {
            _objectMat = Buildable.GetComponent<Renderer>().sharedMaterial;
            _buildable = Instantiate(Buildable, Vector3.zero, Quaternion.identity);
            _buildable.gameObject.GetComponent<Collider>().isTrigger = true;
            _buildable.gameObject.AddComponent<Rigidbody>();
            _buildable.gameObject.GetComponent<Rigidbody>().useGravity = false;
            InitBuildable(_buildable);
            layerMask = LayerMask.GetMask("Floor");
            _firstRun = false;
        }


        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), layerMask);
        RaycastHit hit = hits[0];
        Vector3 mousePos = hit.point;
        mousePos.y = _buildable.transform.localScale.y / 2;
        _buildable.transform.position = mousePos;



        float colliderSize = _buildable.transform.localScale.x;

        for (int i = 0; i < 3; i++)
        {
            if (_buildable.transform.localScale[i] > colliderSize)
                colliderSize = _buildable.transform.localScale[i];
        }

        colliderSize *= 0.5f;


        _buildable.transform.Rotate(new Vector3(transform.rotation.x, Input.GetAxis("Mouse ScrollWheel") * 30, 0));

        RaycastHit[] sphereHit;
        sphereHit = Physics.SphereCastAll(_buildable.transform.position, colliderSize, Vector3.down);
        foreach (RaycastHit raycastHit in sphereHit)
        {
            if(_buildable.gameObject.GetComponent<Collider>().bounds.Intersects(raycastHit.transform.gameObject.GetComponent<Collider>().bounds))
                if (raycastHit.transform.gameObject.tag != "Floor" && raycastHit.transform.gameObject.tag != "Untagged")
                {
                    Debug.Log(raycastHit.transform.gameObject.tag);
                    _buildable.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
                    return false;
                }
        }

        _buildable.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaceBuildable(_buildable);
            _buildable = Instantiate(Buildable, Vector3.zero, Quaternion.identity);
            InitBuildable(_buildable);
            return true;
        }



        return false;
    }



    void InitBuildable(GameObject GO)
    {
        GO.GetComponent<Renderer>().material = TransparentMaterial;
        GO.GetComponent<NavMeshObstacle>().enabled = false;
        GO.GetComponent<Collider>().isTrigger = true;
        GO.AddComponent<Rigidbody>();
        GO.GetComponent<Rigidbody>().useGravity = false;
        GO.layer = 2;
    }


    void PlaceBuildable(GameObject GO)
    {
        GO.GetComponent<Renderer>().material = _objectMat;
        GO.GetComponent<Collider>().isTrigger = false;
        GO.GetComponent<NavMeshObstacle>().enabled = true;
        Destroy(GO.GetComponent<Rigidbody>());
        GO.layer = 9;
    }



	}
