using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    Animator animator;

    AudioSource audioSource;

    [SerializeField] MeleeHitBox hitbox;

    [SerializeField] float damage;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Swing()
    {
        animator.SetTrigger("KatanaSwing");
        audioSource.Play();

        List<GameObject> enemies = hitbox.enemies;
        foreach (GameObject enemy in enemies)
        {
            Debug.Log(enemy.name);
            // TODO hit enemy for damage
        }
    }
}
