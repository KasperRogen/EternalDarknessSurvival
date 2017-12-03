using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrainFromNoise : MonoBehaviour {

    public enum TerrainType
    {
        trees,
        stone
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}





    public void GenerateTerrain(Vector3 center, int width, int height, TerrainType type)
    {
        Vector3 startPos = new Vector3(center.x - width/2, center.y, center.z - height/2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noise = Mathf.PerlinNoise(x, y);
            }
        }
    }


}
