using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CastleScript : MonoBehaviour
{
    [SerializeField] TMP_Text castleHealthText;

    [SerializeField] private ParticleSystem castleParticles;
    private Animator castleAnim;
    private float hitTime = 1f;
    public int castleHealth = 50;

    public static Action<GameObject> OnCastleDestroyed;

    private Vector3 dequedObjectPos = new Vector3(0f, 1000f, 0f);

    void Start()
    {
        castleAnim = GetComponent<Animator>();
        castleHealthText.text = castleHealth.ToString();
    }

    void Update()
    {
        castleHealthText.text = castleHealth.ToString();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Character character)) return;
        if (character.characterType == Character.CharacterType.Player && castleHealth > 0)
        {
            StartCoroutine(DamageCastle(character, other));
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Character character)) return;
        if (character.characterType == Character.CharacterType.Player)
        {
            StopCoroutine(DamageCastle(character, other));
        }
    }

    IEnumerator DamageCastle(Character character, Collision other)
    {
        while (castleHealth > 0 && character.Health > 0)
        {
            castleHealth -= character.AttackPower;
            OnDamage();

            if (castleHealth <= 0)
            {
                OnCastleDestroyed?.Invoke(gameObject);
                yield break;
            }

            character.Health--;
            //Debug.Log("Character Health going down" + character.gameObject.name);

            if (character.Health <= 0)
            {
                other.gameObject.SetActive(false);
                other.transform.position = dequedObjectPos;
            }
            yield return new WaitForSeconds(hitTime);
        }
    }

    private void OnDamage()
    {
        //castleAnim.SetTrigger("Damage");
        castleParticles.Play();
    }
}
