using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _selected = Color.cyan;
    [SerializeField] private float _aimedAlpha = 210;
    [SerializeField] private Color _merge = Color.yellow;
    [SerializeField] private Color _available = Color.green;
    [SerializeField] private Color _notAvailable = Color.red;

    private bool _isAimed;

    private bool _isSelected;
    private bool _canMerge;
    private bool _isAvailable;

    public void UpdateState()
    {
        if (_isSelected)
        {
            _meshRenderer.material.color = _selected;
        }
        else if (_canMerge)
        {
            _meshRenderer.material.color = _merge;
        }
        else if (_isAvailable)
        {
            _meshRenderer.material.color = _available;
        }
        else
        {
            _meshRenderer.material.color = _notAvailable;
            return;
        }

        if (_isAimed)
        {
            Color modifiedColor = _meshRenderer.material.color;
            modifiedColor.a = _aimedAlpha;
            _meshRenderer.material.color = modifiedColor;
        }
    }

    public void SetSelected(bool selected)
    {
        _isSelected = selected;

        UpdateState();
    }

    public void SetMergeble(bool canMerge)
    {
        _canMerge = canMerge;

        UpdateState();
    }

    public void SetAimed(bool aimed)
    {
        _isAimed = aimed;

        UpdateState();
    }

    public void SetAvailable(bool available)
    {
        _isAvailable = available;

        UpdateState();
    }

    public void Reset()
    {
        _isAimed = false;
        _canMerge = false;
        _isAvailable = true;

        UpdateState();
    }

    public void SetVisible(bool visible)
    {
        _isAimed = false;
        _canMerge = false;

        _meshRenderer.gameObject.SetActive(visible);
    }
}
