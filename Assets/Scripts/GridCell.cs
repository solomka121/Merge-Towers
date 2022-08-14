using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _available = Color.green;
    private bool _isAvailable;
    [SerializeField] private Color _notAvailable = Color.red;
    [SerializeField] private Color _selected = Color.cyan;
    [SerializeField] private Color _aimed = Color.green;

    public void SetAvailable(bool available)
    {
        _isAvailable = available;

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

    public void SetAimed(bool aimed)
    {
        if (aimed)
        {
            _meshRenderer.material.color = _aimed;
        }
        else
        {
            SetAvailable(_isAvailable);
        }
    }

    public void SetVisible(bool visible)
    {
        _meshRenderer.gameObject.SetActive(visible);
    }
}
