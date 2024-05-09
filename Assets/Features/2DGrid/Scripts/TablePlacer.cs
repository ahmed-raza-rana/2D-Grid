using UnityEngine;

public class TablePlacer : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private GameObject instantiatedTable;

    public GameObject InstantiateAndPlaceTable(GameObject tablePrefab, Vector3Int gridPosition, Quaternion rotation)
    {
        if (instantiatedTable == null)
        {
            Vector3 worldPosition = grid.CellToWorld(gridPosition);
            instantiatedTable = Instantiate(tablePrefab, worldPosition, rotation);
            return instantiatedTable;
        }
        return null;
    }
    public void PlaceTableAt(Vector3Int gridPosition)
    {
        if (instantiatedTable != null)
        {
            Vector3 worldPosition = grid.CellToWorld(gridPosition);
            instantiatedTable.transform.position = worldPosition;
            instantiatedTable = null; // Reset the instantiated table reference
        }
    }
}