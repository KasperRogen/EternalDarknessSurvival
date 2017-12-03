using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenerateTerrainFromNoise : MonoBehaviour
{

    public GameObject[] Trees;
    public GameObject[] Stones;


	// Use this for initialization
	void Start () {
	        int grassDensity = 800;
	        int patchDetail = 0;
	        Terrain terrainToPopulate = transform.gameObject.GetComponent<Terrain>();
	        terrainToPopulate.terrainData.SetDetailResolution(grassDensity, patchDetail);

	        int[,] newMap = new int[grassDensity, grassDensity];

	        for (int i = 0; i < grassDensity; i++)
	        {
	            for (int j = 0; j < grassDensity; j++)
	            {
	                    newMap[i, j] = 6;
	            }
	        }
	        terrainToPopulate.terrainData.SetDetailLayer(0, 0, 0, newMap);


    }
	
	// Update is called once per frame
	void Update () {
		
	}





    public void GenerateTerrain(Vector3 center, int width, int height, PublicEnums.TerrainType type)
    {
        Vector3 startPos = new Vector3(center.x - width / 2, center.y, center.z - height / 2);

        for (float x = (int)startPos.x ; x < (int)startPos.x + width; x++)
        {
            for (float z = (int)startPos.z; z < (int)startPos.z + height; z++)
            {
                double noise = Mathf.PerlinNoise(x.Remap(0f,width,0f,100f), z.Remap(0f,height,0f,100f));
                float distFromCenter = (float)(center - new Vector3(x, 0, z)).magnitude;
                distFromCenter = distFromCenter.Remap(0f, 50f, 0f, 1f);
                float rand = Random.Range(0, 20);
                
                if (noise - distFromCenter >= rand)
                {
                    Quaternion rot = Random.rotation;
                    rot.x = 0;
                    rot.z = 0; 

                    if(type == PublicEnums.TerrainType.Tree)
                        Instantiate(Trees[Random.Range(0,Trees.Length-1)], new Vector3(x, 2, z), rot);
                    else if(type == PublicEnums.TerrainType.Stone)
                        Instantiate(Stones[Random.Range(0, Stones.Length - 1)], new Vector3(x, 2, z), rot);
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
