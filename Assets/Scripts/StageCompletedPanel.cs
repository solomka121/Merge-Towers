using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCompletedPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _levelNumber;
    [SerializeField] private Button _continueButton;
    private CanvasGroup _continueButtonCanvasGroup;

    public event System.Action OnContinueButtonClick;

    private void Awake()
    {
        _continueButtonCanvasGroup = _continueButton.GetComponent<CanvasGroup>();
        _continueButton.onClick.AddListener(ContinueButtonClick);
        gameObject.SetActive(false);
    }

    public void ContinueButtonClick()
    {
        OnContinueButtonClick?.Invoke();
    }

    public void ShowPanel(int level)
    {
        transform.localScale = Vector2.zero;
        gameObject.SetActive(true);

        StartCoroutine(PanelShow(level));
    }

    public IEnumerator PanelShow(int level)
    {
        gameObject.LeanScale(Vector2.one, 0.9f).setEaseOutBack();
        yield return new WaitForSeconds(0.6f);
        _levelNumber.gameObject.LeanScale(new Vector2(1.6f, 1.6f), 0.15f).setEaseOutCubic().setLoopPingPong(1);
        yield return new WaitForSeconds(0.15f);
        UpdateLevelText(level);
        _continueButtonCanvasGroup.interactable = true;
        LeanTween.alphaCanvas(_continueButtonCanvasGroup, 1f, 0.5f).setEaseOutCubic();
    }

    public void UpdateLevelText(int level)
    {
        _levelNumber.text = level.ToString();
    }

    public void HidePanel()
    {
        gameObject.LeanScale(Vector2.zero, 0.5f).setEaseOutCubic().setOnComplete(OnPanelHide);
    }

    public void OnPanelHide()
    {
        _continueButtonCanvasGroup.interactable = false;
        gameObject.SetActive(false);
    }
}
