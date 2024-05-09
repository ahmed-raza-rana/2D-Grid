using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features._2DGrid.Scripts
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField] private Vector3 _girdPos;
        public bool canPlaceTable;
        public List<GridTile> adjustObjs= new List<GridTile>();
        
        public void TileUpdate(Vector3 girdPos,TileEnum tileEnum)
        {
            _girdPos = girdPos;
            canPlaceTable = tileEnum == TileEnum.Wood;
        }
    }
}
