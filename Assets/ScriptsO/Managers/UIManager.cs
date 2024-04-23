using UnityEngine;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get { return instance; }
    }

    [HideInInspector] public static int CoinCount;
    [HideInInspector] public static int BlockCount;

    [Header("Win&LoosePanels")]
    [SerializeField] private WinPanelUIScript winPanel;
    [SerializeField] private LoosePanelUIScript loosePanel;
    [SerializeField] private CoinUIPanelScript coinsPanel;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        EventManager.gameManagerEvents.OnLevelInstantiated.Get().AddListener(Event_OnLevelInstantiated);
        EventManager.gameManagerEvents.OnLevelCompleted.Get().AddListener(GameManagerScript_OnLevelCompleted);
        EventManager.gameManagerEvents.OnGameFailed.Get().AddListener(GameManagerScript_OnLevelFailed);
    }

    private void OnDisable()
    {
        EventManager.gameManagerEvents.OnLevelInstantiated.Get().RemoveListener(Event_OnLevelInstantiated);
        EventManager.gameManagerEvents.OnLevelCompleted.Get().RemoveListener(GameManagerScript_OnLevelCompleted);
        EventManager.gameManagerEvents.OnGameFailed.Get().RemoveListener(GameManagerScript_OnLevelFailed);
    }

    private void Event_OnLevelInstantiated(GameObject obj)
    {
        coinsPanel.Show();
    }

    private void GameManagerScript_OnLevelCompleted()
    {
        CoinCount += 200;
        coinsPanel.Hide();
        ShowWinPanel();
    }

    private void GameManagerScript_OnLevelFailed()
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
