using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public GenerateTerrainFromNoise TerrainGenerater;

	public float MapWidth = 400;
	public float MapHeight = 400;

	public const int SegmentWidthCount = 8;
	public const int SegmentHeightCount = 8;

	private float SegmentWidth, SegmentHeight;
	private MapSegment[,] Segments;
	
	
	void Start()
	{
		initSegments();
		for(int i = 0; i < SegmentWidthCount-1; i++){
			for(int j = 0; j < SegmentHeightCount-1; j++){
				TerrainGenerater.GenerateTerrain(new Vector3(Segments[i,j].X, 0, Segments[i,j].Y), (int)SegmentWidth, (int)SegmentHeight, GenerateTerrainFromNoise.TerrainType.trees);
			}
		}
	}

	void initSegments(){
		SegmentWidth = MapWidth / SegmentWidthCount;
		SegmentHeight = MapHeight / SegmentHeightCount;

		Segments = new MapSegment[SegmentWidthCount, SegmentHeightCount];

		for(int i = 0; i < SegmentWidthCount-1; i++){
			for(int j = 0; j < SegmentHeightCount-1; j++){
				Segments[i, j] = new MapSegment(-MapWidth/2 + (SegmentWidth * i + (SegmentWidth / 2)), -MapHeight/2 + (SegmentHeight * j + (SegmentHeight / 2)), SegmentWidth, SegmentHeight);
			}
		}
	}
}
