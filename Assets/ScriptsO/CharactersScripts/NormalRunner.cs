using UnityEngine;

public class NormalRunner : Character, IMultipliable
{
    private Rigidbody rb;
   
    [HideInInspector] public int cloningWallInstanceID = 0;

    private int health;

    private void Awake()
    {
        health = Health;
    }

    void Start()
    {
        //Speed = MovementSpeed;
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Health = health;
    }

    private void FixedUpdate()
    {
        //FlockFunction();
        Flocking(gameObject);
        //Move();
    }

    public override void Move()
    {
        rb.velocity = transform.forward * MovementSpeed * Time.deltaTime;
    }

    public override void Flocking(GameObject characterObj)
    {
        base.Flocking(characterObj);
    }

    //private void FlockFunction()
    //{
    //    var normalRunnerList = FlockingManager.Instance.prefabsList;

    //    Vector3 groupCenter = Vector3.zero;
    //    Vector3 prefabAvoid = Vector3.zero;
    //    float groupSpeed = 0.01f;
    //    float prefabDistance;
    //    int groupSize = 0;

    //    foreach (var smallRunner in normalRunnerList)
    //    {
    //        if (smallRunner == this.gameObject) continue;
    //        prefabDistance = Vector3.Distance(smallRunner.transform.position, transform.position);
    //        if (prefabDistance <= FlockingManager.Instance.neighbourDistance)
    //        {
    //            groupCenter += smallRunner.transform.position;
    //            groupSize++;

    //            if (prefabDistance < 3.0f)
    //            {
    //                prefabAvoid += (transform.position - smallRunner.transform.position);
    //            }

    //            groupSpeed += smallRunner.GetComponent<NormalRunner>().Speed;
    //        }
    //    }

    //    if (groupSize > 0)
    //    {
    //        groupCenter = groupCenter / groupSize + (FlockingManager.Instance.Goal.position - transform.position);

    //        Speed = groupSpeed / groupSize;
    //        if (Speed > maxMovementSpeed) Speed = maxMovementSpeed;

    //        Vector3 direction = (groupCenter + prefabAvoid) - transform.position;
    //        direction.Normalize();

    //        if (direction != Vector3.zero)
    //        {
    //            transform.rotation = Quaternion.Slerp(transform.rotation,
    //                Quaternion.LookRotation(direction), FlockingManager.Instance.rotationSpeed * Time.deltaTime);
    //        }
    //        rb.velocity = Speed * Time.deltaTime * direction;
    //    }
    //    else
    //    {
    //        rb.velocity = transform.forward * Speed * Time.deltaTime;
    //    }

    //}

    public override void Attack(Collision coll)
    {
        var otherGameObj = coll.gameObject.GetComponent<Character>();
        otherGameObj.Health -= AttackPower;
        //Debug.Log("Attacking " + otherGameObj.name);

        otherGameObj.TakeDamage(AttackPower);

        if (otherGameObj.Health <= 0)
        {
            coll.gameObject.SetActive(false);
            coll.transform.position = DequedObjectPos;

            if (otherGameObj.characterType == CharacterType.Player)
            {
                var templist = GameManagerScript.Instance.PlayersActiveInScene;
                if (templist.Contains(coll.gameObject)) templist.Remove(coll.gameObject);
            }
            else if (otherGameObj.characterType == CharacterType.Enemy)
            {
                var templist = GameManagerScript.Instance.EnemiesActiveInScene;
                if (templist.Contains(coll.gameObject)) templist.Remove(coll.gameObject);
            }
        }
    }

    public override void TakeDamage(int attackPower)
    {
        //Debug.Log("taking Damage " + name);
        Health -= attackPower;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            transform.position = DequedObjectPos;

            if (characterType == CharacterType.Player)
            {
                var templist = GameManagerScript.Instance.PlayersActiveInScene;
                if (templist.Contains(gameObject)) templist.Remove(gameObject);
            }
            else if (characterType == CharacterType.Enemy)
            {
                var templist = GameManagerScript.Instance.EnemiesActiveInScene;
                if (templist.Contains(gameObject)) templist.Remove(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collsion)
    {
        switch (characterType)
        {
            case CharacterType.Enemy:
                if (collsion.gameObject.CompareTag("Player"))
                {
                    Attack(collsion);
                    //Debug.Log("Attack Player");
                }

                break;

            case CharacterType.Player:
                if (collsion.gameObject.CompareTag("Enemy"))
                {
                    Attack(collsion);
                    //Debug.Log("Attack Enemy");
                }
                break;

            default:
                break;
        }
    }

    public void MultiplyMinions(int multiplynumber, LevelDataScript currLevelData, int instanceID)
    {
        if (characterType == CharacterType.Enemy || cloningWallInstanceID == instanceID) return;
        //Debug.Log(name);
        int multiplier = multiplynumber;

        for (int i = 0; i < multiplier; i++)
        {
            var rnd = Random.Range(-1f, 1f);
            Vector3 newPosition = transform.position + new Vector3(rnd, 0, 0);

            string originalString = name;
            string modifiedString = originalString.Replace("(Clone)", "");

            //Debug.Log("prefabInstantiated");
            var cloneSmall = ObjectPooler.Instance.SpawnObjectFromPool(modifiedString, newPosition, transform.rotation);
            cloneSmall.GetComponent<NormalRunner>().cloningWallInstanceID = instanceID;

            var templist = GameManagerScript.Instance.PlayersActiveInScene;
            templist.Add(cloneSmall);

            if (currLevelData.LevelPartsList.Count > 0)
            {
                cloneSmall.transform.SetParent(currLevelData.LevelPartsList[0].transform);
            }
        }

        gameObject.SetActive(false);
        transform.position = new Vector3(0, 1000f, 0);
    }
}
