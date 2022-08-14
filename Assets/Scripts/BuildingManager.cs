using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    public Vector2Int gridSize;
    [SerializeField] private Transform _gridCellsParent;
    [SerializeField] private GridCell _gridCellPrefab;

    private Building[,] _grid;
    private GridCell[,] _gridCells;

    private List<Building> _buildings;
    private List<Vector2Int> _emptyCells;

    [SerializeField] private LayerMask _turrets;
    private Camera _camera;
    private Building _selectedBuilding;

    private void Awake()
    {
        _grid = new Building[gridSize.x, gridSize.y];
        _gridCells = new GridCell[gridSize.x, gridSize.y];

        _buildings = new List<Building>();
        _emptyCells = new List<Vector2Int>();
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                _emptyCells.Add(new Vector2Int(x, y));
            }
        }

        SpawnCells();
        HideCells();
        _camera = Camera.main;
    }

    private void SpawnCells()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell gridCell = Instantiate(_gridCellPrefab, _gridCellsParent);
                gridCell.transform.localPosition = new Vector3(x, _gridCellsParent.transform.position.y, y);
                gridCell.SetAvailable(true);
                _gridCells[x, y] = gridCell;
            }
        }
    }

    public void HideCells()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell gridCell = _gridCells[x, y];
                gridCell.SetVisible(false);
            }
        }
    }

    public void ShowCells()
    {

    }

    public void SpawnBuilding(Building building)
    {
        if (IsThereEmptySpace() == false)
            return;

        Building spawnedBuilding = Instantiate(building);
        _buildings.Add(building);

        Vector2Int position = GetRandomCellCoordinates();
        PlaceBuilding(spawnedBuilding , position);
    }

    public void PlaceBuilding(Building building , Vector2Int position)
    {
        _emptyCells.Remove(position);
        _grid[position.x, position.y] = building;

        building.currentCell = _gridCells[position.x, position.y];
        building.currentCell.SetAvailable(false);

        building.transform.position = transform.position + new Vector3(position.x , transform.position.y, position.y);
    }

    public Vector2Int GetRandomCellCoordinates()
    {
        int randomFreePositionIndex = Random.Range(0, _emptyCells.Count);
        return _emptyCells[randomFreePositionIndex];
    } 

    public bool IsCellEmpty(Vector2Int cell)
    {
        Debug.Log(cell);
        return _grid[cell.x, cell.y] == null;
    }

    public Vector2Int GetCellIndex(Vector3 position)
    {
        Vector2Int index = Vector2Int.zero;
        index.x = Mathf.RoundToInt(position.x - transform.position.x);
        index.y = Mathf.RoundToInt(position.z - transform.position.z);

        return index;
    }

    public void DeleteBuildingOnGrid(Vector2Int index)
    {
        _grid[index.x, index.y] = null;
    }

    public bool IsThereEmptySpace()
    {
        return _emptyCells.Count > 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_selectedBuilding != null)
            {
                Vector2Int oldCell = GetCellIndex(_selectedBuilding.currentCell.transform.position);
                Vector2Int aimedCell = GetCellIndex(_selectedBuilding.transform.position);

                if(oldCell == aimedCell) // If trying to place on the same cell where were placed
                {
                    _selectedBuilding.currentCell.SetAvailable(true);
                    PlaceBuilding(_selectedBuilding, oldCell);
                    _selectedBuilding = null;
                    return;
                }

                if (IsCellEmpty(aimedCell)) // Placing on new cell
                {
                    _selectedBuilding.currentCell.SetAvailable(true);
                    DeleteBuildingOnGrid(oldCell);
                    PlaceBuilding(_selectedBuilding, aimedCell);
                    _selectedBuilding = null;
                }
                return;
            }

            Ray inputRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay , out hit , 20 , _turrets)){
                Debug.Log("Hittet Turret");
                _selectedBuilding = hit.collider.GetComponent<Building>();
            }
        }

        if (_selectedBuilding == null)
            return;

        _selectedBuilding.currentCell.SetSelected(true);

        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if(groundPlane.Raycast(ray , out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            float clampedX = Mathf.Clamp(worldPosition.x , transform.position.x, transform.position.x + gridSize.x - _selectedBuilding.size.x);
            float clampedZ = Mathf.Clamp(worldPosition.z , transform.position.z, transform.position.z + gridSize.y - _selectedBuilding.size.y);
            _selectedBuilding.transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
    }
}
