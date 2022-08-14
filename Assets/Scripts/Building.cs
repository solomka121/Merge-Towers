using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;

    private void OnDrawGizmosSelected()
    {
        for(int x = 0 ; x < size.x; x++)
        {
            for(int y = 0 ; y < size.y; y++)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }
}
