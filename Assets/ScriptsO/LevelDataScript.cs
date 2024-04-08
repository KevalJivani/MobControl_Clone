using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelDataScript : MonoBehaviour
{
    public int LevelPartNo = 0;

    public List<GameObject> LevelPartsList;

    private void Start()
    {
        foreach (var part in LevelPartsList)
        {
            part.SetActive(false);
        }
        LevelPartsList[0].SetActive(true);
    }

    private void OnEnable()
    {
        CastleScript.OnCastleDestroyed += CastleManager_OnCastleDestroyed;
    }

    private void OnDisable()
    {
        CastleScript.OnCastleDestroyed -= CastleManager_OnCastleDestroyed;
    }

    private void CastleManager_OnCastleDestroyed(GameObject destroyedCastleObj)
    {
        LoadNextLevel(destroyedCastleObj);
    }

    private void LoadNextLevel(GameObject destroyedCastleObj)
    {
        DOVirtual.DelayedCall(0.1f, () =>
        {
            foreach (var gamePart in LevelPartsList)
            {
                var castleObj = gamePart.GetComponentInChildren<CastleScript>().gameObject;
                if (castleObj == destroyedCastleObj)
                {
                    Debug.Log("The Castle to be destroyed " + castleObj.name);
                    gamePart.SetActive(false);
                    LevelPartsList.Remove(gamePart);
                    if (LevelPartsList.Count <= 0)
                    {
                        Debug.Log("CompletedGameManager");
                        GameManagerScript.Instance.OnLevelCompleted?.Invoke();
                        return;
                    }
                    LevelPartsList[0].SetActive(true);
                    return;
                }
            }
        });
    }
}
