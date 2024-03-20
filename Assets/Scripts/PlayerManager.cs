using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int bigPlayerHealth = 10;
    private Vector3 dequedObjectPos = new Vector3(0f, 1000f, 0f);

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "SmallPrefab")
        {
            if (other.CompareTag("EnemySmall"))
            {
                other.gameObject.SetActive(false);
                gameObject.SetActive(false);
                other.transform.position = transform.position = dequedObjectPos;

            }
        }
        else if (gameObject.CompareTag("Big"))
        {
            if(other.CompareTag("EnemySmall"))
            {
                if (bigPlayerHealth > 1)
                {
                    other.gameObject.SetActive(false);
                    other.transform.position = dequedObjectPos;

                    bigPlayerHealth--;
                    Debug.Log(bigPlayerHealth + " small enemy destroyed");
                }
                else
                {
                    other.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    other.transform.position = transform.position = dequedObjectPos;
                }
            }
        }
        
    }
}
