using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Vector2Int gridSize;

    private Building[,] _grid;

    private void Awake()
    {
        _grid = new Building[gridSize.x, gridSize.y];
    }
}
