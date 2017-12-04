using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSegment {
	public float X, Y, Width, Height;
	public PublicEnums.TerrainType TerrainType = PublicEnums.TerrainType.Empty;
	public MapSegment(float x, float y, float width, float height){
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
}
