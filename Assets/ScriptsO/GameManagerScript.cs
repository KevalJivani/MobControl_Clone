using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    public static GameManagerScript Instance
    {
        get { return instance; }
    }

    public int gamePartNo = 0;

    public List<GameObject> GamePartsList;

    public Action OnGameCompleted;
    public bool isGameComplete;


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

    private void Start()
    {
        foreach (var part in GamePartsList)
        {
            part.SetActive(false);
        }
        GamePartsList[0].SetActive(true);
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

        foreach (var gamePart in GamePartsList)
        {
            var castleObj = gamePart.GetComponentInChildren<CastleScript>().gameObject;
            if (castleObj == destroyedCastleObj)
            {
                Debug.Log("The Castle to be destroyed " + castleObj.name);
                gamePart.SetActive(false);
                GamePartsList.Remove(gamePart);
                if (GamePartsList.Count <= 0)
                {
                    Debug.Log("CompletedGameManager");
                    isGameComplete = true;
                    OnGameCompleted?.Invoke();
                    return;
                }
                GamePartsList[0].SetActive(true);
                return;
            }
        }

    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }
}
