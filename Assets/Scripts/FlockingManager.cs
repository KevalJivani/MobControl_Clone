using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public static FlockingManager Instance
    {
        get { return _instance; }
    }
    private static FlockingManager _instance;

    [SerializeField] private GameObject smallPrefabs;
    [SerializeField] private Vector3 spawnPos = new Vector3(5, 5, 5);

    [Header("Fish Settings")]
    [Range(1f, 15f)]
    public float neighbourDistance;
    [Range(5f, 15f)]
    public float rotationSpeed;

    public List<GameObject> prefabsList;
    public Transform Goal;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < 30; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-spawnPos.x, spawnPos.x),
                0.5f, Random.Range(-spawnPos.z, spawnPos.z));

            GameObject smallPrefab = Instantiate(smallPrefabs, spawnPosition, Quaternion.identity);
            prefabsList.Add(smallPrefab);

            yield return new WaitForSeconds(0.3f);
        }
    }
}
