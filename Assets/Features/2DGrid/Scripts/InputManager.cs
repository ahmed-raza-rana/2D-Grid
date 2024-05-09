using System.Collections;
using System.Collections.Generic;
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
        private Renderer _highlighter; // Reference to the highlighter renderer
        private int _tableRot; // Rotation of the table
        
        public GridTile gridTile;
        public GridTile nextGridTile;
        [SerializeField] private TerrainGridLoaderRenderer TerrainGridLoaderRenderer;

        private void Update()
        {
            UpdateCellIndicatorPosition();
            HandleMouseActions();
        }

        private void UpdateCellIndicatorPosition()
        {
            if (_instantiatedObject != null)
            {
                // Move the instantiated object to follow the cell indicator
                _instantiatedObject.transform.position = cellIndicator.transform.position;
            }
        }

        private void HandleMouseActions()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask))
                {
                    gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                    if (gridTile != null)
                    {
                        // Update cell indicator position and color
                        UpdateCellIndicator(hit.point);

                        // Show the cell indicator
                        ShowCellIndicator();

                        // Draw the raycast
                        DrawRay(ray, hit.distance);

                        // Highlight the indicator based on table placement availability
                        HighlightIndicator(CanPlaceLastTable());
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PlaceOrDestroyTable();
            }
        }

        private void UpdateCellIndicator(Vector3 hitPoint)
        {
            Vector3Int gridPos = grid.WorldToCell(hitPoint);
            cellIndicator.transform.position = grid.CellToWorld(gridPos);
        }

        private void ShowCellIndicator()
        {
            cellIndicator.SetActive(true);
        }

        private void DrawRay(Ray ray, float distance)
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        }

        private void HighlightIndicator(bool canPlaceTable)
        {
            if (_highlighter)
            {
                _highlighter.material.color = canPlaceTable ? Color.green : Color.red;
            }
        }

        private void PlaceOrDestroyTable()
        {
            if (CanPlaceLastTable())
            {
                // Place the table
                PlaceTable();
            }
            else
            {
                // Destroy the instantiated object if it cannot be placed
                DestroyInstantiatedObject();
            }

            // Hide the cell indicator
            HideCellIndicator();
        }

        private void PlaceTable()
        {
            Vector3Int gridPos = grid.WorldToCell(cellIndicator.transform.position);
            tablePlacer.PlaceTableAt(gridPos);
            gridTile.canPlaceTable = false;
            nextGridTile.canPlaceTable = false;
            if (_highlighter)
            {
                _highlighter.gameObject.SetActive(false);
            }
            _instantiatedObject = null;
        }

        private void DestroyInstantiatedObject()
        {
            Destroy(_instantiatedObject);
            if (_highlighter)
            {
                _highlighter.gameObject.SetActive(false);
            }
            _instantiatedObject = null;
        }

        private void HideCellIndicator()
        {
            cellIndicator.SetActive(false);
            cellIndicator.transform.position = Vector3.zero;
        }

        public void InstantiateAndPlaceTable(GameObject tablePrefab, int tableRot)
        {
            _tableRot = tableRot;
            Vector3Int gridPos = grid.WorldToCell(cellIndicator.transform.position);
            _instantiatedObject = tablePlacer.InstantiateAndPlaceTable(tablePrefab, gridPos, tablePrefab.transform.rotation, _tableRot);
            _highlighter = cellIndicator.transform.GetChild(_tableRot).GetComponent<Renderer>();
            _highlighter.gameObject.SetActive(true);
        }

        private bool CanPlaceLastTable()
        {
            if (_instantiatedObject != null)
            {
                // Get the tiles array from the terrain grid renderer
                GridTile[,] tilesArray = TerrainGridLoaderRenderer.TilesArray;
                // Given object
                GridTile givenObject = gridTile;

                // Find x and y position of the given object
                int xPos = -1;
                int yPos = -1;

                // Iterate through the array to find the object
                for (int x = 0; x < tilesArray.GetLength(0); x++)
                {
                    for (int y = 0; y < tilesArray.GetLength(1); y++)
                    {
                        if (tilesArray[x, y] == givenObject)
                        {
                            // Object found, save its position
                            xPos = x;
                            yPos = y;
                            break;
                        }
                    }

                    if (xPos != -1 && yPos != -1)
                    {
                        // Object found, break out of the loop
                        break;
                    }
                }

                // Get the world position of the cell indicator
                Vector3 position = cellIndicator.transform.position;

                // Convert the world position to grid coordinates
                Vector3Int currentGridPos = grid.WorldToCell(position);
                Vector3Int nextGridPos = Vector3Int.zero;

                // Get the rotation of the instantiated object
                int tableRot = _instantiatedObject.transform.rotation.eulerAngles.y == 0 ? 0 : 1;
                nextGridTile = null;
                // Calculate the next grid position based on the rotation
                switch (_tableRot)
                {
                    case 0:
                        if (xPos + 1 < tilesArray.GetLength(0))
                        {
                            nextGridTile = tilesArray[xPos + 1, yPos];
                        }
                        break;
                    case 1:
                        if (yPos + 1 < tilesArray.GetLength(1))
                        {
                            nextGridTile = tilesArray[xPos, yPos + 1];
                        }
                        break;
                }

                return gridTile != null && gridTile.canPlaceTable &&
                       nextGridTile != null && nextGridTile.canPlaceTable;
            }

            return false;
        }
    }
}
