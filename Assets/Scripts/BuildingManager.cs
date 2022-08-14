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
    private Vector2Int _aimedCell;

    [SerializeField] private TurretsLevels _turretsLevels;

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
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell gridCell = _gridCells[x, y];
                gridCell.SetVisible(true);
            }
        }
    }

    public void ShowCells(Building building)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell gridCell = _gridCells[x, y];
                gridCell.SetVisible(true);
            }
        }
        ShowMergebleBuldings(building);
    }

    public void ShowMergebleBuldings(Building building)
    {
        int neededLevel = building.turret.level;
        for (int i = 0; i < _buildings.Count; i++)
        {
            Building buildingOnGrid = _buildings[i];
            Debug.Log(i);
            Debug.Log(buildingOnGrid.currentCell);
            if (buildingOnGrid.turret.level == neededLevel)
            {
                _buildings[i].currentCell.SetMergeble(true);
            }
            else
            {
                _buildings[i].currentCell.SetAvailable(false);
            }
        }
    }

    public void SpawnBuilding(Building building)
    {
        if (IsThereEmptySpace() == false)
            return;

        Building spawnedBuilding = Instantiate(building , transform);

        Vector2Int position = GetRandomCellCoordinates();
        PlaceBuilding(spawnedBuilding , position);
    }

    public void SpawnBuilding(Building building , Vector2Int position)
    {
        if (IsThereEmptySpace() == false)
            return;

        Building spawnedBuilding = Instantiate(building, transform);

        PlaceBuilding(spawnedBuilding, position);
    }

    public void PlaceBuilding(Building building , Vector2Int position)
    {
        _emptyCells.Remove(position);
        _grid[position.x, position.y] = building;
        _buildings.Add(building);

        building.currentCell = _gridCells[position.x, position.y];
        building.currentCell.SetAvailable(false);

        building.transform.position = transform.position + new Vector3(position.x , transform.position.y, position.y);
        building.PlaceBumpScale();
    }

    public void ReplaceBuildingBack(Building building) // place back to original cell
    {
        building.currentCell.SetAvailable(false);
        Vector2Int position = GetCellIndex(building.currentCell.transform.position);

        building.transform.position = transform.position + new Vector3(position.x, transform.position.y, position.y);
        building.PlaceBumpScale();
    }

    public Vector2Int GetRandomCellCoordinates()
    {
        int randomFreePositionIndex = Random.Range(0, _emptyCells.Count);
        return _emptyCells[randomFreePositionIndex];
    } 

    public bool IsCellEmpty(Vector2Int cell)
    {
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
        _buildings.Remove(_grid[index.x, index.y]);
        _grid[index.x, index.y] = null;
        _gridCells[index.x, index.y].Reset();
        _emptyCells.Add(index);
    }

    public bool IsThereEmptySpace()
    {
        return _emptyCells.Count > 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_selectedBuilding != null) // Try to place if holds building
            {
                HideCells(); 
                _selectedBuilding.SetSelected(false);

                Vector2Int oldCell = GetCellIndex(_selectedBuilding.currentCell.transform.position);
                Vector2Int targetCell = GetCellIndex(_selectedBuilding.transform.position);
                
                if(oldCell == targetCell) // Trying to place on the same cell where were placed
                {
                    _selectedBuilding.currentCell.SetAvailable(true);
                    PlaceBuilding(_selectedBuilding, oldCell);
                    _selectedBuilding = null;

                    return;
                }

                if (IsCellEmpty(targetCell)) // Placing on new cell
                {
                    _selectedBuilding.currentCell.SetAvailable(true);
                    DeleteBuildingOnGrid(oldCell);
                    PlaceBuilding(_selectedBuilding, targetCell);
                    _selectedBuilding = null;

                    return;
                }
                else // If cell isnt empty
                {
                    Building targetedBuilding = _grid[targetCell.x, targetCell.y];
                    int neededLevel = _selectedBuilding.turret.level;
                    if (targetedBuilding.turret.level == neededLevel) // if the targeted turret is the same level ( Merge )
                    {
                        DeleteBuildingOnGrid(oldCell);
                        Destroy(_selectedBuilding.gameObject);

                        DeleteBuildingOnGrid(targetCell);
                        Destroy(targetedBuilding.gameObject);

                        SpawnBuilding(_turretsLevels.GetTurretLevel(neededLevel + 1) , targetCell);

                        _selectedBuilding = null;
                        return;
                    }
                }

                //if its impossible to place on targeted cell , place back to original cell
                ReplaceBuildingBack(_selectedBuilding);
                _selectedBuilding = null;

                return;
            }

            // Choose building on grid
            RaycastHit hit;
            Ray inputRay = _camera.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast(inputRay , out hit , 20 , _turrets)){  
                _selectedBuilding = hit.collider.GetComponent<Building>();
                _selectedBuilding.SetSelected(true);

                ShowCells(_selectedBuilding);
            }
        }

        if (_selectedBuilding == null)
            return;

        // Highlight the cell below the buillding
        _gridCells[_aimedCell.x, _aimedCell.y].SetAimed(false);
        _aimedCell = GetCellIndex(_selectedBuilding.transform.position);
        _gridCells[_aimedCell.x, _aimedCell.y].SetAimed(true);
        //

        // Moving the building
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if(groundPlane.Raycast(ray , out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            float clampedX = Mathf.Clamp(worldPosition.x , transform.position.x, transform.position.x + gridSize.x - _selectedBuilding.size.x);
            float clampedZ = Mathf.Clamp(worldPosition.z , transform.position.z, transform.position.z + gridSize.y - _selectedBuilding.size.y);
            _selectedBuilding.transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
        //
    }
}
