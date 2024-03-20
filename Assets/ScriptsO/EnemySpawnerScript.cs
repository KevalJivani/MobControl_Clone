using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawnerScript : MonoBehaviour
{
    private const string ENEMYSMALL_PREFAB = "NormalRunnerEnemy";

    [SerializeField] private float EnemyShootSpeed = 3f;

    [Space(10)]
    [SerializeField] private float EnemyFlowDuration = 3f;
    private float lastSpawnTime;
    private float lastFlowTime;
    private float spawnAfterTime = 0.2f;
    private bool isCannonPlaced;

    private bool canSpawn;

    private void Start()
    {
        //InvokeRepeating("SpawnEnemy", 0f, spawnAfterTime);
    }

    private void OnEnable()
    {
        CannonMovementScript.OnMovementCompleted += CannonScript_OnMovementCompleted;
        BaseLineScript.OnGameFailed += BaseLineScript_OnLevelFailed;
    }


    private void OnDisable()
    {
        CannonMovementScript.OnMovementCompleted -= CannonScript_OnMovementCompleted;
        BaseLineScript.OnGameFailed -= BaseLineScript_OnLevelFailed;
    }

    private void BaseLineScript_OnLevelFailed()
    {
        isCannonPlaced = false;
    }

    private void CannonScript_OnMovementCompleted()
    {
        DOVirtual.DelayedCall(EnemyFlowDuration, () =>
        {
            isCannonPlaced = true;
        });
    }

    private void Update()
    {
        if (isCannonPlaced)
        {
            if (canSpawn)
            {
                if (Time.time - lastSpawnTime >= spawnAfterTime)
                {
                    SpawnEnemy();
                    lastSpawnTime = Time.time;
                }
            }

            if (Time.time - lastFlowTime >= EnemyFlowDuration)
            {
                canSpawn = true;
                StartCoroutine(Timer());
            }
        }
    }

    private void SpawnEnemy()
    {
        int rnd = Random.Range(-5, 5);
        GameObject enemySmall = ObjectPooler.Instance.SpawnObjectFromPool(ENEMYSMALL_PREFAB, transform.position + new Vector3(rnd, 0, 0), transform.rotation);
        enemySmall.transform.SetParent(GameManagerScript.Instance.GamePartsList[0].transform);

        var rb = enemySmall.GetComponent<Rigidbody>();
        StartCoroutine(ApplyForce(rb));
    }

    IEnumerator ApplyForce(Rigidbody rb)
    {
        rb.AddForce(-transform.forward * EnemyShootSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(.4f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(EnemyFlowDuration);
        lastFlowTime = Time.time;
        canSpawn = false;
    }

}
