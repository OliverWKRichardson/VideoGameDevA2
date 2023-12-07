using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public HealthBar healthBar;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        
    }

    public void DamageBy(float amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    public float GetHealth()
    {
        return health;
    }
}
