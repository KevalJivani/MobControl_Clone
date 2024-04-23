using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private LevelDataScript levelDataScript;


    private void OnEnable()
    {
       levelDataScript =  GetComponentInParent<LevelDataScript>();
    }

    private void Start()
    {
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
            multipliable.MultiplyMinions(multiplyAmount, levelDataScript , instanceID);
            //Debug.Log("Multipliable");
        }
    }
}
