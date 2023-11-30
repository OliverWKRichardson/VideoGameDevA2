using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    private int health;
    public HealthBar healthBar;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        
    }

    public void DamageBy(int amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    public int GetHealth()
    {
        return health;
    }
}
