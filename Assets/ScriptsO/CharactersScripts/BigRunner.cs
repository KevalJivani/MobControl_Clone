using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigRunner : Character, IMultipliable
{
    private Rigidbody rb;

    [HideInInspector] public int cloningWallInstanceID = 0;

    private int healthBig;


    private void Awake()
    {
        healthBig = Health;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Health = healthBig;
        cloningWallInstanceID = 0;
    }

    private void OnDisable()
    {
        cloningWallInstanceID = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        rb.velocity = transform.forward * MovementSpeed * Time.deltaTime;
    }

    public override void Attack(Collision coll)
    {
        var otherGameObj = coll.gameObject.GetComponent<Character>();
        otherGameObj.Health -= AttackPower;
        otherGameObj.TakeDamage(AttackPower);
        //Debug.Log("Attacking " + otherGameObj.name);
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
            //Debug.Log("taking Damage " + name);
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
                    // Debug.Log("Attack Player");
                }

                break;

            case CharacterType.Player:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Attack(other);
                    // Debug.Log("Attack Enemy");
                }
                break;

            default:
                break;
        }
    }

    public void MultiplyMinions(int multiplynumber, LevelDataScript currLevelData, int instanceID)
    {
        if (characterType == CharacterType.Enemy || cloningWallInstanceID == instanceID) return;

        int multiplier = multiplynumber;

        for (int i = 0; i < multiplier; i++)
        {
            var rnd = Random.Range(-2f, 2f);
            Vector3 newPosition = transform.position + new Vector3(rnd, 0, 0);

            string originalString = name;
            string modifiedString = originalString.Replace("(Clone)", "");

            var cloneBig = ObjectPooler.Instance.SpawnObjectFromPool(modifiedString, newPosition, transform.rotation);
            cloneBig.GetComponent<BigRunner>().cloningWallInstanceID = instanceID;

            if (currLevelData.LevelPartsList.Count > 0)
            {
                cloneBig.transform.SetParent(currLevelData.LevelPartsList[0].transform);
                //Debug.Log(modifiedString + " The multiplied Prefab");
            }
        }

        gameObject.SetActive(false);
        transform.position = new Vector3(0, 1000f, 0);
    }
}
