using System;
using System.Collections;
using System.Collections.Generic;
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
        GameManagerScript.Instance.OnGameCompleted += GameManagerScript_OnGameCompleted;
    }

    private void OnDisable()
    {
        BaseLineScript.OnGameFailed -= BaseLineScript_OnLevelFailed;
        GameManagerScript.Instance.OnGameCompleted -= GameManagerScript_OnGameCompleted;
    }

    private void GameManagerScript_OnGameCompleted()
    {
        var winPanelShowDelay = 0.5f;
        DOVirtual.DelayedCall(winPanelShowDelay, () =>
        {
            Debug.Log("GameComletedUI");
            winPanel.Show();
            winPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).From(new Vector3(0.1f, 0.1f, 0.1f)).SetEase(Ease.InOutBack);
        });
    }

    private void BaseLineScript_OnLevelFailed()
    {
        var loosePanelShowDelay = 0.5f;
        DOVirtual.DelayedCall(loosePanelShowDelay, () =>
         {
             loosePanel.Show();
             loosePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).From(new Vector3(0.1f, 0.1f, 0.1f)).SetEase(Ease.InOutBack);
         });
    }
}
