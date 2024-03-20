using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{

    [SerializeField] private string VIEWER = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(VIEWER))
        {
            other.transform.LookAt(transform);
            //Debug.Log("Looking");
        }
    }
}