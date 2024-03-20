using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Enemy,
        Player
    }

    public CharacterType characterType;

    public int Health;
    public int MovementSpeed;
    public int AttackPower;

    protected Vector3 DequedObjectPos = new Vector3(0f, 1000f, 0f);

    public abstract void Attack(Collision coll);

    public abstract void TakeDamage(int attackPower);

    public abstract void Move();
}