using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PublicEnums {
    public enum ItemType {
    Stone,
    Wood
    }


    public enum TerrainType
    {
        Tree,
        Stone,
        Empty,
        Spawn,
        Mountain
    }

    public enum ToolType
    {
        Gather,
        Weapoin
    }

    public enum ToolGatherType
    {
        None,
        Stone,
        Wood
    }
}
