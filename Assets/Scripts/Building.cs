using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    public GridCell currentCell;
    public Turret turret;
    private bool _canScale = true;
    private int _selectedAnimationID;

    private void Start()
    {
        SpawnScale();
    }

    public void SpawnScale()
    {
        if (_canScale == false)
            return;

        _canScale = false;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.2f).setEaseOutBack().setOnComplete(ResetScaleFlag);
    }

    public void PlaceBumpScale()
    {
        if (_canScale == false)
            return;

        _canScale = false;

        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 1.2f , 0.1f).setEaseOutCubic().setLoopPingPong(1).setOnComplete(ResetScaleFlag);
    }

    private void ResetScaleFlag()
    {
        _canScale = true;
    }

    public void SetSelected(bool selected)
    {
        currentCell.SetSelected(selected);

        if (selected)
        {
            SelectedScale();
        }
        else
        {
            CancelSelectedScale();
        }
    }

    public void SelectedScale()
    {
        Debug.Log("start scale");
        _selectedAnimationID = LeanTween.scale(gameObject, Vector3.one * 1.2f , 0.6f).setEaseOutBack().setLoopPingPong().id;
    }

    public void CancelSelectedScale()
    {
        Debug.Log("end scale");
        LeanTween.cancel(_selectedAnimationID);
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }
}
