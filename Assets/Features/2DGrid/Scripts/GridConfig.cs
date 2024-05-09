using System;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    [CreateAssetMenu(fileName = "2DGridConfig",menuName = "2dGrid/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        public GameObject gridObj;
        
        public Sprite dirt; 
        public Sprite grass;
        public Sprite stone;
        public Sprite wood;
    }
 
    [Serializable]
    public enum TileEnum
    {
        Dirt = 0,
        Grass = 1,
        Stone = 2,
        Wood = 3
    }
}