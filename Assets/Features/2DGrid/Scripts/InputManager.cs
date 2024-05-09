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

        private void Update()
        {
            // Check for mouse input
            if (Input.GetMouseButton(0))
            {
                // Cast a ray from the mouse position
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask))
                {
                    // Get the GridTile component of the selected object
                    GridTile gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                    if (gridTile != null)
                    {
                        // Access the canPlaceTable property
                        bool canPlaceTable = gridTile.canPlaceTable;

                        // Update color of cellIndicator based on canPlaceTable
                        cellIndicator.GetComponent<Renderer>().material.color = canPlaceTable ? Color.green : Color.red;
                    }

                    // Update cell indicator position
                    Vector3Int gridPos = grid.WorldToCell(hit.point);
                    cellIndicator.transform.position = grid.CellToWorld(gridPos);

                    // Draw the raycast
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

                    // Show the cellIndicator
                    cellIndicator.SetActive(true);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Hide the cellIndicator on mouse up click
                cellIndicator.SetActive(false);
            }
        }
    }
}
