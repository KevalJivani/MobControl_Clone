using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRunner : Character, IMultipliable
{
    private Rigidbody rb;
    private GameManagerScript gmInstance;

    private int healthBig;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gmInstance = GameManagerScript.Instance;
        healthBig = Health;
    }

    private void OnEnable()
    {
        Health = healthBig;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        rb.velocity = transform.forward * MovementSpeed;
    }

    public override void Attack(Collision coll)
    {
        var otherGameObj = coll.gameObject.GetComponent<Character>();
        otherGameObj.Health -= AttackPower;
        otherGameObj.TakeDamage(AttackPower);

        Debug.Log("Attacking " + otherGameObj.name);
        if (otherGameObj.Health <= 0)
        {
            coll.gameObject.SetActive(false);
            coll.transform.position = DequedObjectPos;
        }
    }

    public override void TakeDamage(int attackPower)
    {
        Health -= attackPower;
        if (Health <= 0)
        {
            Debug.Log("taking Damage " + name);
            gameObject.SetActive(false);
            transform.position = DequedObjectPos;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (characterType)
        {
            case CharacterType.Enemy:
                if (other.gameObject.CompareTag("Player"))
                {
                    Attack(other);
                    Debug.Log("Attack Player");

                }

                break;

            case CharacterType.Player:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Attack(other);
                    Debug.Log("Attack Enemy");
                }
                break;

            default:
                break;
        }
    }

    public void MultiplyMinions(int multiplynumber, Transform objTransform)
    {
        if (characterType == CharacterType.Enemy) return;

        int multiplier = multiplynumber;

        for (int i = 0; i < multiplier; i++)
        {
            var rnd = Random.Range(-1f, 1f);
            Vector3 newPosition = transform.position + new Vector3(rnd, 0, 0.05f);

            string originalString = name;
            string modifiedString = originalString.Replace("(Clone)", "");

            var cloneSmall = ObjectPooler.Instance.SpawnObjectFromPool(modifiedString, newPosition, transform.rotation);
            if (gmInstance.GamePartsList.Count > 0)
            {
                cloneSmall.transform.SetParent(gmInstance.GamePartsList[0].transform);
                //Debug.Log(modifiedString + " The multiplied Prefab");
            }
        }
    }
}
