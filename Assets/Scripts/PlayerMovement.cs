using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    
    

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("SmallPrefab") || gameObject.CompareTag("Big"))
        {
            if(other.CompareTag("CastleRotate"))
            {
                gameObject.transform.LookAt(other.gameObject.transform);
            }
        }

    }
    
}
