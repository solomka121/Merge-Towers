using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _available = Color.green;
    [SerializeField] private Color _notAvailable = Color.red;
    [SerializeField] private Color _selected = Color.cyan;

    public void SetAvailable(bool available)
    {
        if (available)
        {
            _meshRenderer.material.color = _available;
        }
        else
        {
            _meshRenderer.material.color = _notAvailable;
        }
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            _meshRenderer.material.color = _selected;
        }
        else
        {
            SetVisible(false);
        }
    }

    public void SetVisible(bool visible)
    {
        _meshRenderer.gameObject.SetActive(visible);
    }
}
