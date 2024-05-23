using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;

public class CannonMovementScript : MonoBehaviour
{
    [HideInInspector] public static bool isMoving { private set; get; }

    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed;

    private float splineLength;
    public int splinescount = 0;
    private float distancePercentage = 0f;

    [Space(10)]
    [SerializeField] private GameObject cannonBottom;

    private LevelDataScript currLevelData;
   
    private void OnEnable()
    {
        EventManager.gameManagerEvents.OnLevelInstantiated.Get().AddListener(GameManagerScript_OnLevelInstantiated);
        EventManager.castleEvents.OnCastleDestroyed.Get().AddListener(CastleManager_OnCastleDestroyed);
    }

    private void OnDisable()
    {
        EventManager.gameManagerEvents.OnLevelInstantiated.Get().RemoveListener(GameManagerScript_OnLevelInstantiated);
        EventManager.castleEvents.OnCastleDestroyed.Get().RemoveListener(CastleManager_OnCastleDestroyed);
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        MoveToNextPart();
        RotateCannonBottom(true);
    }

    private void GameManagerScript_OnLevelInstantiated(GameObject instantiatedGameObj)
    {
        currLevelData = instantiatedGameObj.GetComponent<LevelDataScript>();
    }

    private void Start()
    {
        splineLength = spline.CalculateLength(splinescount);

        distancePercentage += speed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(splinescount, distancePercentage);
        transform.position = currentPosition;
        distancePercentage = 0f;

        transform.DOMove(new Vector3(currentPosition.x, transform.position.y, currentPosition.z), speed).
            SetSpeedBased(true).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                RotateCannonBottom(true);
            });
    }

    private void Update()
    {
        if (isMoving && splinescount < spline.Splines.Count)
        {
            distancePercentage += speed * Time.deltaTime / splineLength;

            Vector3 currentPosition = spline.EvaluatePosition(splinescount, distancePercentage);
            transform.position = currentPosition;

            if (distancePercentage <= 1f)
            {
                Vector3 nextPosition = spline.EvaluatePosition(splinescount, distancePercentage + 0.05f);
                Vector3 direction = nextPosition - currentPosition;
                transform.rotation = Quaternion.LookRotation(direction, transform.up);
            }

            if (distancePercentage > 1f)
            {
                currLevelData.LevelPartNo++;
                RotateCannonBottom(false);
                splinescount++;

                EventManager.cannonEvents.OnMovementCompleted.Get()?.Invoke();
                distancePercentage = 0f;
            }
        }
    }

    private void RotateCannonBottom(bool canCannonMove)
    {
        if (!canCannonMove)
        {
            isMoving = canCannonMove;
        }
        //Debug.Log("CannonBottom y value " + cannonBottom.transform.rotation.y);
        var rotateVal = new Vector3(cannonBottom.transform.rotation.x, cannonBottom.transform.rotation.eulerAngles.y + 90, cannonBottom.transform.rotation.z);
        cannonBottom.transform.DORotate(rotateVal, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            isMoving = canCannonMove;
            //Debug.Log("Rotate is called");
        });
    }

    private void MoveToNextPart()
    {
        if (spline.Splines.Count > splinescount)
        {
            var initialPos = spline.Splines[splinescount].ToArray()[0].Position.x;

            DOVirtual.DelayedCall(0.3f, () =>
             {
                 transform.DOMoveX(initialPos, speed).SetSpeedBased(true).SetEase(Ease.InOutCirc);
             });
        }
    }
}
