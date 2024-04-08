using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLineScript : MonoBehaviour
{
    private bool gameEnded;

    public static Action OnGameFailed;

    private CameraShake cameraShake;

    private void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character)) return;
        if (character.characterType == Character.CharacterType.Enemy && !gameEnded)
        {
            OnGameFailed?.Invoke();
            StartCoroutine(GameEndedFunc());
            gameEnded = true;
            Debug.Log("GameEnded");
        }
    }

    private IEnumerator GameEndedFunc()
    {
        Time.timeScale = 0.2f;
        cameraShake.ShakeCamera(shakeDuration: 0.2f);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }
}
