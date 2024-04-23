using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector] public const string LEVEL_NO = "LevelNo";
    [HideInInspector] public int levelNo = 1;

    public LevelDataListSO LevelDataListSO;

    [Space(10)]
    [Header("Flocking Settings")]
    [Range(1f, 15f)]
    public float neighbourDistance;
    [Range(5f, 15f)]
    public float rotationSpeed;

    public List<GameObject> PlayersActiveInScene;
    public List<GameObject> EnemiesActiveInScene;

    private static GameManagerScript instance;

    public static GameManagerScript Instance
    {
        get { return instance; }
    }

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
        Application.targetFrameRate = 60;
    }

    public void InstantiateLevel()
    {
        levelNo = PlayerPrefs.GetInt(LEVEL_NO, 1);

        if (levelNo > 0 && levelNo <= LevelDataListSO.LevelsDataList.Count)
        {
            GameObject obj = LevelDataListSO.LevelsDataList.Find(x => x.LevelNumber == levelNo).LevelGameObj;
            var instantiatedLevel = Instantiate(obj, transform.position, Quaternion.identity);
            PlayersActiveInScene.Clear();
            EnemiesActiveInScene.Clear();

            EventManager.gameManagerEvents.OnLevelInstantiated.Get()?.Invoke(instantiatedLevel);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(LEVEL_NO, levelNo);
        PlayerPrefs.Save();
    }
}
