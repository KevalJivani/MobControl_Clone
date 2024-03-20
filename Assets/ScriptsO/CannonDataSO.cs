using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CannonSO", menuName = "CannonData/Cannon")]
public class CannonDataSO : ScriptableObject
{
    public event Action OnNormalCharacterSpawned;
    public event Action OnSpecialCharacterSpawned;

    public float lastSpawnTime = 0f;
    public float minionSpawnTime = 0.5f;
    public int minionSpawnAmount = 1;

    private void SpawnMinions(string minionName, Transform spawnPoint)
    {
        if (Time.time - lastSpawnTime >= minionSpawnTime)
        {
            for (int minion = 0; minion < minionSpawnAmount; minion++)
            {
                var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
                if (GameManagerScript.Instance.GamePartsList.Count > 0)
                {
                    spawnedGameObj.transform.SetParent(GameManagerScript.Instance.GamePartsList[0].transform);
                }
            }

            OnNormalCharacterSpawned?.Invoke();
            //Debug.Log("Spawned");
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnSpeacialMinion(string minionName, Transform spawnPoint)
    {
        var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
        if (GameManagerScript.Instance.GamePartsList.Count > 0)
        {
            spawnedGameObj.transform.SetParent(GameManagerScript.Instance.GamePartsList[0].transform);
        }
        OnSpecialCharacterSpawned?.Invoke();
    }
}
