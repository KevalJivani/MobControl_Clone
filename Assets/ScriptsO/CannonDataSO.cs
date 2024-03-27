using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CannonSO", menuName = "CannonData/Cannon")]
public class CannonDataSO : ScriptableObject
{
    public event Action OnNormalCharacterSpawned;
    public event Action OnSpecialCharacterSpawned;

    [Space(10)]
    [Header("Range of Cannon Movement")]
    public float minX;
    public float maxX;

    [Space(10)]
    [Header("SpawnData Of Minions")]
    public float minionSpawnTime = 0.5f;
    public int minionSpawnAmount = 1;

    private float lastSpawnTime = 0f;

    private void OnEnable()
    {
        lastSpawnTime = 0f;
    }

    public void SpawnMinions(string minionName, Transform spawnPoint)
    {
        //Debug.Log("SpawnMinionCalled
        if (Time.time - lastSpawnTime >= minionSpawnTime)
        {
            for (int minion = 0; minion < minionSpawnAmount; minion++)
            {
                var Offset = UnityEngine.Random.Range(0, 1f);
                var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName,
                    new Vector3(spawnPoint.position.x + Offset, spawnPoint.position.y, spawnPoint.position.z + Offset), spawnPoint.rotation);

                if (GameManagerScript.Instance.GamePartsList.Count > 0)
                {
                    spawnedGameObj.transform.SetParent(GameManagerScript.Instance.GamePartsList[0].transform);
                }
            }

            //Debug.Log("Spawned");
            OnNormalCharacterSpawned?.Invoke();
            lastSpawnTime = Time.time;
        }
    }

    public void SpawnSpecialMinion(string minionName, Transform spawnPoint)
    {
        var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Big guy position " + spawnedGameObj.transform.position);
        Debug.Log("SpawnPoint " + spawnPoint.position);

        if (GameManagerScript.Instance.GamePartsList.Count > 0)
        {
            spawnedGameObj.transform.SetParent(GameManagerScript.Instance.GamePartsList[0].transform);
        }
        //Debug.Log("Big guy position 2 " + spawnedGameObj.transform.position);
        OnSpecialCharacterSpawned?.Invoke();
    }
}
