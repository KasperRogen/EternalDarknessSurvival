using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenerateTerrainFromNoise : MonoBehaviour
{

    public GameObject tree;
    
    public enum TerrainType
    {
        trees,
        stone
    }


	// Use this for initialization
	void Start () {
        GenerateTerrain(Vector3.zero, 50, 50, TerrainType.trees);
	}
	
	// Update is called once per frame
	void Update () {
		
	}





    public void GenerateTerrain(Vector3 center, int width, int height, TerrainType type)
    {
        Vector3 startPos = new Vector3(center.x - width / 2, center.y, center.z - height / 2);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float noise = Mathf.PerlinNoise(x, z);
                float distFromCenter = (center - new Vector3(x, 0, z)).magnitude;
         //       if (noise - distFromCenter.Remap((width + height) / 4 ) >= Random.Range(0, 60))
                {
                    Quaternion rot = Random.rotation;
                    rot.x = 0;
                    rot.z = 0; 
                    Instantiate(tree, new Vector3(x, 2, z), rot);
                }
            }
        }
    }


}



public static class ExtensionMethods
{

public static float Remap(this float value, float from1, float to1, float from2, float to2)
{
return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}

}
