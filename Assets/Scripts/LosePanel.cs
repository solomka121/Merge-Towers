using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    public event System.Action OnRestartButtonClick;

    private void Awake()
    {
        _restartButton.onClick.AddListener(RestartButtonClicked);
    }

    private void RestartButtonClicked()
    {
        OnRestartButtonClick?.Invoke();
    }

    public void ShowPanel()
    {
        transform.localScale = Vector2.zero;
        gameObject.SetActive(true);

        gameObject.LeanScale(Vector2.one, 0.6f).setEaseOutBack();
        LeanTween.alphaCanvas(_canvasGroup, 1f, 1f).setEaseOutCubic();
    }

    public void HidePanel()
    {
        gameObject.LeanScale(Vector2.zero, 0.5f).setEaseOutCubic().setOnComplete(OnPanelHide);
    }

    public void OnPanelHide()
    {
        _canvasGroup.interactable = false;
        gameObject.SetActive(false);
    }
}
