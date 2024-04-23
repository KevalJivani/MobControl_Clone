using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[System.Diagnostics.DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public abstract class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Enemy,
        Player
    }

    public CharacterType characterType;

    public int Health;

    //public float MovementSpeed = 10f;
    [Range(50f, 500f)]
    public float maxMovementSpeed;
    [Range(50f, 500f)]
    public float minMovementSpeed;
    public float MovementSpeed { get { return Random.Range(minMovementSpeed, maxMovementSpeed); } set { } }

    public int AttackPower;

    protected Vector3 DequedObjectPos = new Vector3(0f, 1000f, 0f);

    public abstract void Attack(Collision coll);

    public abstract void TakeDamage(int attackPower);

    public abstract void Move();

    public virtual void Flocking(GameObject characterObj)
    {
        var gM = GameManagerScript.Instance;
        var chScript = characterObj.GetComponent<Character>();

        var runnerList = chScript.characterType == CharacterType.Player ? gM.PlayersActiveInScene : gM.EnemiesActiveInScene;

        FlockingFunc(characterObj, runnerList);
    }

    private void FlockingFunc(GameObject characterObj, List<GameObject> runnerList)
    {
        Vector3 groupCenter = Vector3.zero;
        Vector3 prefabAvoid = Vector3.zero;
        float groupSpeed = 0.01f;
        float prefabDistance;
        int groupSize = 0;
        float Speed = MovementSpeed;
        Rigidbody rb = characterObj.GetComponent<Rigidbody>();

        foreach (var runner in runnerList)
        {
            if (runner == characterObj) continue;
            prefabDistance = Vector3.Distance(runner.transform.position, characterObj.transform.position);
            if (prefabDistance <= GameManagerScript.Instance.neighbourDistance)
            {
                groupCenter += runner.transform.position;
                groupSize++;

                if (prefabDistance < 3.0f)
                {
                    prefabAvoid += (characterObj.transform.position - runner.transform.position);
                }

                groupSpeed += runner.GetComponent<Character>().MovementSpeed;
            }
        }

        if (groupSize > 0)
        {
            Vector3 goal = Vector3.zero;
            groupCenter = groupCenter / groupSize + (goal - characterObj.transform.position);

            Speed = groupSpeed / groupSize;
            if (Speed > maxMovementSpeed) Speed = maxMovementSpeed;

            Vector3 direction = (groupCenter + prefabAvoid) - characterObj.transform.position;
            direction.Normalize();

            if (direction != Vector3.zero && goal != Vector3.zero)
            {
                characterObj.transform.rotation = Quaternion.Slerp(characterObj.transform.rotation,
                    Quaternion.LookRotation(direction), GameManagerScript.Instance.rotationSpeed * Time.deltaTime);
            }

            //if (goal != Vector3.zero)
            //{
            //    rb.velocity = Speed* Time.deltaTime* direction;
            //    characterObj.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.yellow;
            //}
            //else
            //{
            //    rb.velocity = Speed * Time.deltaTime * characterObj.transform.forward;
            //    characterObj.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.green;
            //    Debug.Log("<color=green>" + characterObj.transform.forward + "</color>");
            //}

            rb.velocity = goal != Vector3.zero ? Speed * Time.deltaTime * direction :
                Speed * Time.deltaTime * characterObj.transform.forward;
        }
        else {
            rb.velocity = MovementSpeed * Time.deltaTime * characterObj.transform.forward;
            //characterObj.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.magenta;
            //Debug.Log("<color=blue>" + characterObj.transform.forward + "</color>");
        }
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}