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
    private List<Vector2Int> _freePositions;

    private Building _selectedBuilding;

    private void Awake()
    {
        _grid = new Building[gridSize.x, gridSize.y];
        _gridCells = new GridCell[gridSize.x, gridSize.y];

        _buildings = new List<Building>();
        _freePositions = new List<Vector2Int>();
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                _freePositions.Add(new Vector2Int(x, y));
            }
        }

        SpawnCells();
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

    public void SpawnBuilding(Building building)
    {
        if (IsThereEmptySpace() == false)
            return;

        Building spawnedBuilding = Instantiate(building);
        Vector2Int position = GetRandomCoordinates();
        PlaceBuilding(spawnedBuilding , position);
    }

    public void PlaceBuilding(Building building , Vector2Int position)
    {
        _freePositions.Remove(position);
        _buildings.Add(building);
        _grid[position.x, position.y] = building;

        _gridCells[position.x, position.y].SetAvailable(false);

        building.transform.position = transform.position + new Vector3(position.x , transform.position.y, position.y);
    }

    public Vector2Int GetRandomCoordinates()
    {
        int randomFreePositionIndex = Random.Range(0, _freePositions.Count);
        return _freePositions[randomFreePositionIndex];
    }

    public bool IsThereEmptySpace()
    {
        return _freePositions.Count > 0;
    }
}
