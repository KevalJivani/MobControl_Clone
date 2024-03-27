using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CannonScript : MonoBehaviour
{
    private const string SMALL_RUNNER = "NormalRunner";
    private const string BIG_RUNNER = "BigGuyRunner";
   
    private Vector3 _offset;

    private Animator cannonTopAnimator;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private CannonDataSO cannonData;

    private BigGuyLauncherSliderUI bigGuyLauncherSliderUI;

    private bool canSpawnPlayers;

    private void Start()
    {
        cannonTopAnimator = GetComponentInChildren<Animator>();
        bigGuyLauncherSliderUI = GetComponentInChildren<BigGuyLauncherSliderUI>();
    }

    private void OnEnable()
    {
        CannonMovementScript.OnMovementCompleted += CannonMovementScript_OnMovementCompleted;
        CastleScript.OnCastleDestroyed += CastleManager_OnCastleDestroyed;
        cannonData.OnNormalCharacterSpawned += CannonData_OnNormalCharacterSpawned;
        cannonData.OnSpecialCharacterSpawned += CannonData_OnSpecialCharacterSpawned;
    }


    private void OnDisable()
    {
        CannonMovementScript.OnMovementCompleted -= CannonMovementScript_OnMovementCompleted;
        CastleScript.OnCastleDestroyed -= CastleManager_OnCastleDestroyed;
        cannonData.OnNormalCharacterSpawned -= CannonData_OnNormalCharacterSpawned;
        cannonData.OnSpecialCharacterSpawned -= CannonData_OnSpecialCharacterSpawned;
    }

    private void CannonMovementScript_OnMovementCompleted()
    {
        canSpawnPlayers = true;
    }

    private void CannonData_OnSpecialCharacterSpawned()
    {
        cannonTopAnimator.SetTrigger("CannonShoot");
    }

    private void CannonData_OnNormalCharacterSpawned()
    {
        cannonTopAnimator.SetTrigger("CannonShoot");
        if (!bigGuyLauncherSliderUI.isBarFilled) bigGuyLauncherSliderUI.fillAmount++;
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        transform.DOLocalMove(new Vector3(0f, transform.position.y, 0f), 1f).SetEase(Ease.InOutCirc);
        canSpawnPlayers = false;
    }

    private void OnMouseDown()
    {
        var pos = transform.localPosition;
        var position = new Vector3(pos.x, pos.y, pos.z);
        _offset = position - MouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (canSpawnPlayers)
        {
            var pos = MouseWorldPosition() + _offset;
            Vector3 position;

            var xPosition = Mathf.Clamp(pos.x, cannonData.minX, cannonData.maxX);
            position = new Vector3(xPosition, 0f, 0f);
           
            //Spawn Minions
            cannonData.SpawnMinions(SMALL_RUNNER, spawnPoint);

            transform.localPosition = position;
        }
    }

    private void OnMouseUp()
    {
        if (bigGuyLauncherSliderUI.isBarFilled && !CannonMovementScript.isMoving)
        {
            bigGuyLauncherSliderUI.fillAmount = 0;
            bigGuyLauncherSliderUI.isBarFilled = false;
            cannonData.SpawnSpecialMinion(BIG_RUNNER, spawnPoint);
        }
    }

    private Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main!.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    //private void SpawnMinions(string minionName)
    //{
    //    if (Time.time - lastSpawnTime >= minionSpawnTime)
    //    {
    //        cannonTopAnimator.SetTrigger("CannonShoot");

    //        var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
    //        if (gmInstance.GamePartsList.Count > 0)
    //        {
    //            spawnedGameObj.transform.SetParent(gmInstance.GamePartsList[0].transform);
    //        }
    //        if (!bigGuyLauncherSliderUI.isBarFilled) bigGuyLauncherSliderUI.fillAmount++;
    //        //Debug.Log("Spawned");
    //        lastSpawnTime = Time.time;
    //    }
    //}

    //private void SpawnSpeacialMinion(string minionName)
    //{
    //    cannonTopAnimator.SetTrigger("CannonShoot");

    //    var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
    //    if (gmInstance.GamePartsList.Count > 0)
    //    {
    //        spawnedGameObj.transform.SetParent(gmInstance.GamePartsList[0].transform);
    //    }
    //}
}
