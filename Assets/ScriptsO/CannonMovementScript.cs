using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;

public class CannonMovementScript : MonoBehaviour
{
    public static Action OnMovementCompleted;

    [HideInInspector] public bool isMoving { private set; get; }

    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed;

    private float splineLength;
    public int splinescount = 0;
    private float distancePercentage = 0f;

    [Space(10)]
    [SerializeField] private GameObject cannonBottom;

    private void OnEnable()
    {
        CastleScript.OnCastleDestroyed += CastleManager_OnCastleDestroyed;
    }

    private void OnDisable()
    {
        CastleScript.OnCastleDestroyed -= CastleManager_OnCastleDestroyed;
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        MoveToNextPart();
    }

    private void Start()
    {
        splineLength = spline.CalculateLength(splinescount);

        var lastKnotPos = spline.Splines[splinescount].ToArray()[0].Position;
        transform.DOMove(new Vector3(lastKnotPos.x, transform.position.y, lastKnotPos.z), speed).
            SetSpeedBased(true).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            RotateCannonBottom(true);
        });
    }

    private void Update()
    {
        if (isMoving)
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
                GameManagerScript.Instance.gamePartNo++;
                RotateCannonBottom(false);
                splinescount++;
                OnMovementCompleted?.Invoke();
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
                 transform.DOMoveX(initialPos, 0.2f).SetEase(Ease.InOutCirc).OnComplete(() =>
                 {       
                     RotateCannonBottom(true);
                 });
             });
        }
    }
}
