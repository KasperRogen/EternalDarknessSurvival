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
    public LayerMask BuildCheckMask;
    private bool IsFree = false;
    public bool IsBuilding = false;
    public GameObject BuildingObject;
    public GameObject Player;
    public GameObject NavPathCheckerTarget;
    public NavMeshPath NavMeshPath;


    public List<GameObject> CollidingObjects;

    private GameObject _buildable;
	// Use this for initialization
    void Start()
    {
        NavMeshPath = new NavMeshPath();
    }



    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            IsBuilding = false;
            BuildingObject = null;
            Destroy(_buildable.transform.gameObject);
            _firstRun = true;
        }

        if (IsBuilding)
            Build(BuildingObject);

    }

    public bool Build(GameObject Buildable)
    {
        CollidingObjects = new List<GameObject>();
        IsFree = true;
        if (_firstRun)
        {
            _objectMat = Buildable.GetComponent<Renderer>().sharedMaterial;
            _buildable = Instantiate(Buildable, Vector3.zero, Quaternion.identity);
            _buildable.gameObject.GetComponent<Collider>().isTrigger = true;
            _buildable.gameObject.AddComponent<Rigidbody>();
            _buildable.gameObject.GetComponent<Rigidbody>().useGravity = false;
            InitBuildable(_buildable);
            layerMask = LayerMask.GetMask("Terrain");
            _firstRun = false;
        }

        ColliderCheck();

        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), layerMask);
        RaycastHit hit;
            hit = hits[0];
            Vector3 mousePos = hit.point;
            mousePos.y = _buildable.transform.localScale.y / 2;
            _buildable.transform.position = mousePos;


        


        _buildable.transform.Rotate(new Vector3(transform.rotation.x, Input.GetAxis("Mouse ScrollWheel") * 45, 0));

        
        
        if (Input.GetMouseButtonDown(0))
        {
            if(_buildable.GetComponent<NavMeshObstacle>() != null)
            _buildable.GetComponent<NavMeshObstacle>().enabled = true;
            StartCoroutine(BuildObject(Buildable));
        }



        return false;
    }



    IEnumerator BuildObject(GameObject Buildable)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (CalculateNewPath() && ColliderCheck())
        {
            int woodCount = 0;
            int stoneCount = 0;

            if (Player.GetComponent<Inventory>().Items.Any(i => i.ItemType == PublicEnums.ItemType.Wood))
            {
                woodCount = Player.GetComponent<Inventory>().Items.Where(i => i.ItemType == PublicEnums.ItemType.Wood)
                    .Sum(i => i.Quantity);
            }

            if (Player.GetComponent<Inventory>().Items.Any(i => i.ItemType == PublicEnums.ItemType.Stone))
            {
                stoneCount = Player.GetComponent<Inventory>().Items.Where(i => i.ItemType == PublicEnums.ItemType.Stone)
                    .Sum(i => i.Quantity);
            }

            if (stoneCount >= Buildable.GetComponent<DeployableStats>().StonePrice &&
                woodCount >= Buildable.GetComponent<DeployableStats>().WoodPrice)
            {
                PlaceBuildable(_buildable);
                _buildable = Instantiate(Buildable, Vector3.zero, Quaternion.identity);
                InitBuildable(_buildable);

                if (Buildable.GetComponent<DeployableStats>().WoodPrice > 0)
                    Player.GetComponent<Inventory>().DecrementResource(PublicEnums.ItemType.Wood, Buildable.GetComponent<DeployableStats>().WoodPrice);

                if (Buildable.GetComponent<DeployableStats>().StonePrice > 0)
                    Player.GetComponent<Inventory>().DecrementResource(PublicEnums.ItemType.Stone, Buildable.GetComponent<DeployableStats>().StonePrice);
            }

            //Buildable.GetComponent<DeployableStats>()
        }

        if (_buildable.GetComponent<NavMeshObstacle>() != null)
            _buildable.GetComponent<NavMeshObstacle>().enabled = false;
    }



    bool CalculateNewPath()
    {
        Player.GetComponent<NavMeshAgent>().CalculatePath(NavPathCheckerTarget.transform.position, NavMeshPath);
        print("New path calculated");
        if (NavMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    bool ColliderCheck()
    {

        float colliderSize = _buildable.transform.localScale.x;

        for (int i = 0; i < 3; i++)
        {
            if (_buildable.transform.localScale[i] > colliderSize)
                colliderSize = _buildable.transform.localScale[i];
        }

        colliderSize *= 0.5f;


        RaycastHit[] sphereHit;
        sphereHit = Physics.SphereCastAll(_buildable.transform.position, colliderSize, Vector3.down);
        foreach (RaycastHit raycastHit in sphereHit)
        {
            if (_buildable.gameObject.GetComponent<Collider>().bounds.Intersects(raycastHit.transform.gameObject.GetComponent<Collider>().bounds))
                if (raycastHit.transform.gameObject.tag != "Floor" &&
                    raycastHit.transform.gameObject.tag != "Terrain")
                {
                    CollidingObjects.Add(raycastHit.transform.gameObject);
//                    Debug.Log(raycastHit.transform.gameObject.tag);
                    _buildable.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
                    return false;
                }
        }
        _buildable.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
        return true;
    }


    void InitBuildable(GameObject GO)
    {
        GO.GetComponent<Renderer>().material = TransparentMaterial;

        if (GO.GetComponent<NavMeshObstacle>() != null)
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
        if (GO.GetComponent<NavMeshObstacle>() != null)
            GO.GetComponent<NavMeshObstacle>().enabled = true;
        Destroy(GO.GetComponent<Rigidbody>());
        GO.layer = 9;
    }



	}
