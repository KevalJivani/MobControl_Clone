using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MultiplierWallScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Transform multiplierWallVisual;

    [SerializeField] private float rangeOfMovement = 5f;
    [SerializeField] private float speed;
    [SerializeField] private bool InvertMovement;

    private Vector3 originalPos;

    [Space(10)]
    public bool canMove;
    public int multiplyAmount = 2;
    private GameManagerScript gmInstance;

    private void Start()
    {
        gmInstance = GameManagerScript.Instance;
        originalPos = transform.localPosition;
        multiplierText.text = multiplyAmount + "x";
    }

    private void Update()
    {
        if (canMove)
        {
            WallMovementFunc();
        }
    }

    private void WallMovementFunc()
    {
        var posX = originalPos.x + Mathf.PingPong(Time.time * speed, rangeOfMovement);
        posX = InvertMovement ? -posX : posX;

        transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMultipliable multipliable))
        {
            int instanceID = GetInstanceID();
            multipliable.MultiplyMinions(multiplyAmount, transform , instanceID);
        }
    }

    //private void MultiplyMinions(int multiplynumber, GameObject gameObj)
    //{
    //    int multiplier = multiplynumber;

    //    for (int i = 0; i < multiplier; i++)
    //    {
    //        var rnd = Random.Range(-1f, 1f);
    //        Vector3 newPosition = gameObj.transform.position + new Vector3(rnd, 0, 0f);
    //        if (gameObj.CompareTag(SMALL_PREFAB))
    //        {
    //            var cloneSmall = ObjectPooler.Instance.SpawnObjectFromPool(SMALL_PREFAB, newPosition, transform.rotation);
    //            if (gmInstance.GamePartsList.Count > 0)
    //            {
    //                cloneSmall.transform.SetParent(gmInstance.GamePartsList[0].transform);
    //            }
    //        }
    //    }
    //}
}
