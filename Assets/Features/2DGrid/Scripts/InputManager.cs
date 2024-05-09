using System;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask floorLayerMask;
        public GameObject cellIndicator;
        [SerializeField] private Grid grid;
        [SerializeField] private TablePlacer tablePlacer;
        private GameObject _instantiatedObject; // Reference to the instantiated object


        private void Update()
        {
            if (_instantiatedObject != null)
            {
                // Move the instantiated object to follow the cell indicator
                _instantiatedObject.transform.position = cellIndicator.transform.position;
            }
            
            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask))
                {
                    GridTile gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                    if (gridTile != null)
                    {
                        // Update cell indicator position and color
                        Vector3Int gridPos = grid.WorldToCell(hit.point);
                        cellIndicator.transform.position = grid.CellToWorld(gridPos);
                        cellIndicator.GetComponent<Renderer>().material.color = gridTile.canPlaceTable ? Color.green : Color.red;

                        // Draw the raycast
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

                        // Show the cellIndicator
                        cellIndicator.SetActive(true);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Place the table on mouse button up
                Vector3Int gridPos = grid.WorldToCell(cellIndicator.transform.position);
                tablePlacer.PlaceTableAt(gridPos);

                // Hide the cellIndicator on mouse up click
                cellIndicator.SetActive(false);
            }
        }

        public void InstantiateAndPlaceTable(GameObject tablePrefab)
        {
            Vector3Int gridPos = grid.WorldToCell(cellIndicator.transform.position);
            _instantiatedObject = tablePlacer.InstantiateAndPlaceTable(tablePrefab, gridPos, tablePrefab.transform.rotation);
        }
    }
}
