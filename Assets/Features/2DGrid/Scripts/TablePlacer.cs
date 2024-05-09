using System.Collections.Generic;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    public class TablePlacer : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        private GameObject _instantiatedTable;
        public Dictionary<Vector3Int, GameObject> PlacedTables = new Dictionary<Vector3Int, GameObject>();
        private int _tableRot;

        public GameObject InstantiateAndPlaceTable(GameObject tablePrefab, Vector3Int gridPosition, Quaternion rotation, int tableRot)
        {
            if (_instantiatedTable == null)
            {
                Vector3 worldPosition = grid.CellToWorld(gridPosition);
                _instantiatedTable = Instantiate(tablePrefab, worldPosition, rotation);
                _tableRot = tableRot;
                return _instantiatedTable;
            }
            return null;
        }
        public void PlaceTableAt(Vector3Int gridPosition)
        {
            if (!PlacedTables.ContainsKey(gridPosition))
            {
                if (_instantiatedTable != null)
                {
                    Vector3 worldPosition = grid.CellToWorld(gridPosition);
                    Vector3Int nextGridPos = Vector3Int.zero;
                    switch (_tableRot)
                    {
                        case 0:
                            nextGridPos = grid.WorldToCell(gridPosition + new Vector3(grid.cellSize.x, 0, 0));
                            break;
  
                        case 1:
                            nextGridPos = grid.WorldToCell(gridPosition + new Vector3(0, 0, grid.cellSize.z));
                            break;
                        
                    }
                    _instantiatedTable.transform.position = worldPosition;
                    PlacedTables.Add(gridPosition, _instantiatedTable);
                    PlacedTables.Add(nextGridPos, _instantiatedTable);
                    _instantiatedTable = null;
                }
            }
        }
    }
}