using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonScript : MonoBehaviour
{
    private const string SMALL_RUNNER = "NormalRunner";
    private const string BIG_RUNNER = "BigGuyRunner";

    private Vector3 _offset;

    private Animator cannonTopAnimator;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private CannonDataSO cannonData;

    private BigGuyLauncherSliderUI bigGuyLauncherSliderUI;

    private LevelDataScript currLevelData;

    private bool canSpawnPlayers;


    private void Start()
    {
        cannonTopAnimator = GetComponentInChildren<Animator>();
        bigGuyLauncherSliderUI = GetComponentInChildren<BigGuyLauncherSliderUI>();
    }

    private void OnEnable()
    {
        GameManagerScript.Instance.OnLevelInstantiated += GameManagerScript_OnLevelInstantiated;
        //EventManager.cannonEvents.OnMovementCompleted.Get().AddListener(CannonMovementScript_OnMovementCompleted);
        CannonMovementScript.OnMovementCompleted += CannonMovementScript_OnMovementCompleted;
        CastleScript.OnCastleDestroyed += CastleManager_OnCastleDestroyed;
        cannonData.OnNormalCharacterSpawned += CannonData_OnNormalCharacterSpawned;
        cannonData.OnSpecialCharacterSpawned += CannonData_OnSpecialCharacterSpawned;
    }

    private void OnDisable()
    {
        GameManagerScript.Instance.OnLevelInstantiated -= GameManagerScript_OnLevelInstantiated;
        //EventManager.cannonEvents.OnMovementCompleted.Get().RemoveListener(CannonMovementScript_OnMovementCompleted);
        CannonMovementScript.OnMovementCompleted -= CannonMovementScript_OnMovementCompleted;
        CastleScript.OnCastleDestroyed -= CastleManager_OnCastleDestroyed;
        cannonData.OnNormalCharacterSpawned -= CannonData_OnNormalCharacterSpawned;
        cannonData.OnSpecialCharacterSpawned -= CannonData_OnSpecialCharacterSpawned;
    }

    private void GameManagerScript_OnLevelInstantiated(GameObject instantiatedLevelGameObj)
    {
        currLevelData = instantiatedLevelGameObj.GetComponent<LevelDataScript>();
    }

    private void CannonMovementScript_OnMovementCompleted()
    {
        canSpawnPlayers = true;
        Debug.Log("Callingthis");
    }

    private void CannonData_OnSpecialCharacterSpawned()
    {
        cannonTopAnimator.SetTrigger("CannonShoot");
    }

    private void CannonData_OnNormalCharacterSpawned()
    {
        cannonTopAnimator.SetTrigger("CannonShoot");
        if (!bigGuyLauncherSliderUI.isBarFilled) bigGuyLauncherSliderUI.fillAmount++;
        if (!bigGuyLauncherSliderUI.isBarFilled) bigGuyLauncherSliderUI.UpdateUIValue();
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        canSpawnPlayers = false;
        transform.DOLocalMove(new Vector3(0f, transform.position.y, 0f), 1f).SetEase(Ease.InOutCirc);
    }

    private void OnMouseDown()
    {
        var pos = transform.position;
        _offset = pos - MouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (canSpawnPlayers)
        {
            /*_offset = Quaternion.Euler(transform.parent.eulerAngles) * _offset;

            //var pos = MouseWorldPosition() + _offset;

            //var xPosition = Mathf.Clamp(pos.x, cannonData.minX, cannonData.maxX);
            //Vector3 position = new Vector3(xPosition, 0f, 0f);

            ////Spawn Minions
            //cannonData.SpawnMinions(SMALL_RUNNER, spawnPoint, currLevelData);
            //transform.localPosition = position;

            // Adjust the offset based on the parent's rotation
            _offset = Quaternion.Euler(transform.parent.eulerAngles) * _offset;
            */

            // Calculate the target position in world space
            var targetPosition = MouseWorldPosition() + _offset;

            // Convert the target position to local space
            var localTargetPosition = transform.parent.InverseTransformPoint(targetPosition);

            // Clamp the local X position
            localTargetPosition.x = Mathf.Clamp(localTargetPosition.x, cannonData.minX, cannonData.maxX);

            // Set the new local position of the child object
            transform.localPosition = new Vector3(localTargetPosition.x, 0f, 0f);

            //Spawn Minions
            cannonData.SpawnMinions(SMALL_RUNNER, spawnPoint, currLevelData);
        }
    }

    private Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main!.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private void OnMouseUp()
    {
        if (bigGuyLauncherSliderUI.isBarFilled && !CannonMovementScript.isMoving)
        {
            bigGuyLauncherSliderUI.fillAmount = 0;
            bigGuyLauncherSliderUI.isBarFilled = false;
            bigGuyLauncherSliderUI.UpdateUIValue();

            cannonData.SpawnSpecialMinion(BIG_RUNNER, spawnPoint, currLevelData);
        }
    }

}
