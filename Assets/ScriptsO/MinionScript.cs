using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionScript : MonoBehaviour
{

    public bool canMove;
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        if (canMove)
        {
            //To move Foreward
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            Debug.Log("MinionScript Moving");
        }
    }

}
