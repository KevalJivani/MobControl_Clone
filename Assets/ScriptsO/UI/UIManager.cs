using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Win&LoosePanels")]
    [SerializeField] private WinPanelUIScript winPanel;
    [SerializeField] private LoosePanelUIScript loosePanel;
    //[Space(10)]

    private void OnEnable()
    {
        BaseLineScript.OnGameFailed += BaseLineScript_OnLevelFailed;
        GameManagerScript.Instance.OnLevelCompleted += GameManagerScript_OnLevelCompleted;
    }

    private void OnDisable()
    {
        BaseLineScript.OnGameFailed -= BaseLineScript_OnLevelFailed;
        GameManagerScript.Instance.OnLevelCompleted -= GameManagerScript_OnLevelCompleted;
    }

    private void GameManagerScript_OnLevelCompleted()
    {
        ShowWinPanel();
    }

    private void BaseLineScript_OnLevelFailed()
    {
        ShowLoosePanel();
    }

    private void ShowLoosePanel()
    {
        var loosePanelShowDelay = 0.8f;
        DOVirtual.DelayedCall(loosePanelShowDelay, () =>
        {
            loosePanel.Show();
            loosePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).From(new Vector3(0.1f, 0.1f, 0.1f)).SetEase(Ease.InOutCubic);
        });
    }

    private void ShowWinPanel()
    {
        var winPanelShowDelay = 0.8f;
        DOVirtual.DelayedCall(winPanelShowDelay, () =>
        {
            winPanel.Show();
            winPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).From(new Vector3(0.1f, 0.1f, 0.1f)).SetEase(Ease.InOutCubic);
        });
    }
}
