using System;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public Action<GameObject> OnLevelInstantiated;
    public Action OnLevelCompleted;

    [HideInInspector] public const string LEVEL_NO = "LevelNo";
    [HideInInspector] public int levelNo = 1;

    public LevelDataListSO LevelDataListSO;

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

    public void InstantiateLevel()
    {
        levelNo = PlayerPrefs.GetInt(LEVEL_NO, 1);

        if (levelNo > 0 && levelNo <= LevelDataListSO.LevelsDataList.Count)
        {
            GameObject obj = LevelDataListSO.LevelsDataList.Find(x => x.LevelNumber == levelNo).LevelGameObj;
           var instantiatedLevel = Instantiate(obj, transform.position, Quaternion.identity);
            OnLevelInstantiated?.Invoke(instantiatedLevel);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(LEVEL_NO, levelNo);
        PlayerPrefs.Save();
    }
}
