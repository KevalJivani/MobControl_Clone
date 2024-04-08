using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataList", menuName = "LevelData")]
public class LevelDataListSO : ScriptableObject
{
    public List<LevelData> LevelsDataList;
}

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public int LevelNumber;
    public GameObject LevelGameObj;
}
