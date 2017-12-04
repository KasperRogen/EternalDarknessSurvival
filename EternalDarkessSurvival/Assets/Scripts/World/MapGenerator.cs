using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public GenerateTerrainFromNoise TerrainGenerater;

	public float MapWidth = 400;
	public float MapHeight = 400;

	public const int SegmentWidthCount = 8;
	public const int SegmentHeightCount = 8;

	private float SegmentWidth, SegmentHeight;
	private MapSegment[,] Segments;

	public int Frequency = 3;
	
	void Start()
	{	
		initSegments();
		for(int i = 0; i < SegmentWidthCount-1; i++){
			for(int j = 0; j < SegmentHeightCount-1; j++){
				TerrainGenerater.GenerateTerrain(new Vector3(Segments[i,j].X, 0, Segments[i,j].Y), (int)SegmentWidth, (int)SegmentHeight, Segments[i,j].TerrainType);
			}
		}
	}

	// Algorithm
	// Make counters for resource type fields
	// Place Empty Camp Segment in the middle
	// Go through all segments i, j
	// 	Check [i-1,j]x, [i, j-1]x, [i+1, j]x, [i, j+1]x, [i+1, j+1]x, [i-1, j-1]x, [i+1, j-1], [i-1, j+1]; All empty => Make new resource
	// 		Count resource up
	//  else=> Do nothing (Maybe mountains, etc.) 

	void initSegments(){
		SegmentWidth = MapWidth / SegmentWidthCount;
		SegmentHeight = MapHeight / SegmentHeightCount;

		Segments = new MapSegment[SegmentWidthCount, SegmentHeightCount];

		for(int i = 0; i < SegmentWidthCount-1; i++){
			for(int j = 0; j < SegmentHeightCount-1; j++){
				Segments[i, j] = new MapSegment(-MapWidth/2 + (SegmentWidth * i + (SegmentWidth / 2)), -MapHeight/2 + (SegmentHeight * j + (SegmentHeight / 2)), SegmentWidth, SegmentHeight);

				if (i % Frequency == 0 && j % Frequency == 0)
				{
					Segments[i, j].TerrainType =
						TerrainGenerater.GatherTypes[Random.Range(0, TerrainGenerater.GatherTypes.Count)].TerrainType;
				}
			}
		}
		
		Segments[SegmentWidthCount / 2, SegmentHeightCount / 2].TerrainType = PublicEnums.TerrainType.Spawn;
	}

	
}
