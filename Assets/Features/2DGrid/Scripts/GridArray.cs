using UnityEngine;

namespace Features._2DGrid.Scripts
{ 
    [CreateAssetMenu(fileName = "GridArray",menuName = "2dGrid/GridArray")]
    public class GridArray : ScriptableObject
    {
        public readonly GridTile[,] TilesArray;

        public GridArray(GridTile[,] tilesArray)
        {
            TilesArray = tilesArray;
        }


        public GridTile[,] GetTilesArray()
        {
            return TilesArray;
        }
    }
}
