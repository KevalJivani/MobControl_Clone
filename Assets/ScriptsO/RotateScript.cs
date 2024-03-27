using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateScript : MonoBehaviour
{
    [Header("Keep On Rotating")]
    [SerializeField] private bool keepOnRotating;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 30, 0);

    [Space(10)]
    [Header("Rotate Between Given Angles")]
    [SerializeField] private bool RotateBetweenAngles;
    [SerializeField] private Vector3 fromAngle = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 toAngle = new Vector3(0, 180, 0);
    [SerializeField] private float duration = 1f;
    [SerializeField] private float waitDuration = 1f;

    private void Start()
    {
        if (RotateBetweenAngles && !keepOnRotating)
        {
            StartCoroutine(RotateObject());
        }
    }

    void Update()
    {
        if (keepOnRotating && !RotateBetweenAngles)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator RotateObject()
    {
        float timeElapsed = 0f;
        Quaternion fromRotation = Quaternion.Euler(fromAngle);
        Quaternion toRotation = Quaternion.Euler(toAngle);

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = toRotation;

        Vector3 temp = fromAngle;
        fromAngle = toAngle;
        toAngle = temp;

        yield return new WaitForSeconds(waitDuration);
        StartCoroutine(RotateObject());
    }

}
