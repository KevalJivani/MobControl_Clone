using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler instance;

    public static ObjectPooler Instance
    {
        get { return instance; }
    }

    [HideInInspector] public Dictionary<string, Queue<GameObject>> PoolDict;

    [SerializeField] private List<Pool> pools;

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
        PoolDict = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            var gameobjQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.spawnCount; i++)
            {
                var obj = Instantiate(pool.gameObj.gameObject, new Vector3(0,2000f,0), Quaternion.identity);
                obj.SetActive(false);
                gameobjQueue.Enqueue(obj);
            }

            PoolDict.Add(pool.objName, gameobjQueue);
        }
    }

    public GameObject SpawnObjectFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDict.ContainsKey(tag))
        {
            Debug.Log("No such Key Present");
            return null;
        }
        var obj = PoolDict[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;


        PoolDict[tag].Enqueue(obj);

        return obj;
    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }

    [System.Serializable]
    public class Pool
    {
        public string objName;
        public Transform gameObj;
        public int spawnCount;
    }
}
