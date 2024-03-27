using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CastleScript : MonoBehaviour
{
    [SerializeField] TMP_Text castleHealthText;
    [SerializeField] private ParticleSystem castleParticles;
    private Animator castleAnim;
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

            castleHealth--;
            OnDamage();

            if (castleHealth <= 0)
            {
                OnCastleDestroyed?.Invoke(gameObject);
                return;
            }

            character.Health--;
            Debug.Log("Character Health going down" + character.gameObject.name);

            if (character.Health <= 0)
            {
                other.gameObject.SetActive(false);
                other.transform.position = dequedObjectPos;
            }
        }
    }

    private void OnDamage()
    {
        //castleAnim.SetTrigger("Damage");
        castleParticles.Play();
    }

}
