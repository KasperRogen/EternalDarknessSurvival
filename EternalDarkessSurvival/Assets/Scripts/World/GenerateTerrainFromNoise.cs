using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenerateTerrainFromNoise : MonoBehaviour
{
    public int TreeOnEmptyChance;
    public int StoneOnEmptyChance;
    public int EmptyOnEmptyChance;

    public GameObject StoneParent;
    public GameObject TreeParent;

	public List<Gatherable> GatherTypes; 
	
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



    IEnumerator PlaceLandscape()
    {
        yield return new WaitForSeconds(5);
    }

    public void GenerateTerrain(Vector3 center, int width, int height, PublicEnums.TerrainType type)
    {
            Vector3 startPos = new Vector3(center.x - width / 2, center.y, center.z - height / 2);
        
        if (type == PublicEnums.TerrainType.Empty)
        {
        
            for (float x = (int)startPos.x; x < (int)startPos.x + width; x+= 10)
            {
                for (float z = (int)startPos.z; z < (int)startPos.z + height; z+= 10)
                {

                    Quaternion rot = Random.rotation;
                    rot.x = 0;
                    rot.z = 0;

                    PublicEnums.TerrainType TypeToSet;

                    int randomType = Random.Range(0, EmptyOnEmptyChance + StoneOnEmptyChance + TreeOnEmptyChance);

            
                    if (randomType <= EmptyOnEmptyChance)
                    {
                        continue;
                    } if (randomType <= EmptyOnEmptyChance + StoneOnEmptyChance)
                    {
                        TypeToSet = PublicEnums.TerrainType.Stone;
                    } else
                    {
                        TypeToSet = PublicEnums.TerrainType.Tree;
                    }

                
                    Vector2 ToNoise = new Vector2(x, z);

                    double noise = Mathf.PerlinNoise(ToNoise.x.Remap(0f, width, 0f, 0.6f), ToNoise.y.Remap(0f, height, 0f, 0.6f));
                    float rand = Random.Range(0, 0.2f);
                    

                    if (noise >= rand)
                    {
                        if (GatherTypes.Any(it => it.TerrainType == TypeToSet))
                        {
                            Instantiate(GatherTypes.First(it => it.TerrainType == TypeToSet), new Vector3(ToNoise.x, 2, ToNoise.y), rot);
                        }
                    }

                }
            }
        }
        else { 


            for (float x = (int) startPos.x; x < (int) startPos.x + width; x++)
            {
                for (float z = (int) startPos.z; z < (int) startPos.z + height; z++)
                {
                    Quaternion rot = Random.rotation;
                    rot.x = 0;
                    rot.z = 0;
					
	                double noise = Mathf.PerlinNoise(x.Remap(0f,width,0f,0.9f), z.Remap(0f,height,100f,0.9f));
	                float distFromCenter = (float)(center - new Vector3(x, 0, z)).magnitude;
	                distFromCenter = distFromCenter.Remap(0f, 50f, 0f, 1f);
	                float rand = Random.Range(0, 30);
                
	                if (noise - distFromCenter >= rand)
	                {
		                if (GatherTypes.Any(it => it.TerrainType == type))
		                {
			                Instantiate(GatherTypes.First(it => it.TerrainType == type), new Vector3(x, 2, z), rot, TreeParent.transform);
		                }
	                }
	                
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
