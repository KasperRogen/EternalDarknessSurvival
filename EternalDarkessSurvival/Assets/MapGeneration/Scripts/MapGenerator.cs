using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public float MapWidth = 400;
	public float MapHeight = 400;

	public const int SegmentWidthCount = 8;
	public const int SegmentHeightCount = 8;

	private float SegmentWidth, SegmentHeight;
	private MapSegment[,] Segments;
	
	void initSegments(){
		SegmentWidth = MapWidth / SegmentWidthCount;
		SegmentHeight = MapHeight / SegmentHeightCount;

		Segments = new MapSegment[SegmentWidthCount, SegmentHeightCount];

		for(int i = 0; i < SegmentWidthCount; i++){
			for(int j = 0; j < SegmentHeightCount; i++){
				Segments[i, j] = new MapSegment(SegmentWidth * i + (SegmentWidth / 2), SegmentHeight * j + (SegmentHeight / 2), SegmentWidth, SegmentHeight);
			}
		}
	}
}
