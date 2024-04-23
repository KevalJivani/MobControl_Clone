using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineAdjusterScript : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CannonFollowCamera;

    [Space(10)]
    [SerializeField] private List<CinemachineVirtualCamera> levelPartsVirtualCameraList;

    private int cameraNo;


    private void OnEnable()
    {
        EventManager.cannonEvents.OnMovementCompleted.Get().AddListener(CannonScript_OnMovementCompleted);
        EventManager.castleEvents.OnCastleDestroyed.Get().AddListener(CastleManager_OnCastleDestroyed);
    }


    private void OnDisable()
    {
        EventManager.cannonEvents.OnMovementCompleted.Get().RemoveListener(CannonScript_OnMovementCompleted);
        EventManager.castleEvents.OnCastleDestroyed.Get().RemoveListener(CastleManager_OnCastleDestroyed);
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        SwapCams(CannonFollowCamera, levelPartsVirtualCameraList[cameraNo]);
        cameraNo++;
    }

    private void CannonScript_OnMovementCompleted()
    {
        SwapCams(CannonFollowCamera, levelPartsVirtualCameraList[cameraNo]);
    }

    public void SwapCams(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        var tempPriority = cam1.Priority;

        cam1.Priority = cam2.Priority;
        cam2.Priority = tempPriority;
    }
}
