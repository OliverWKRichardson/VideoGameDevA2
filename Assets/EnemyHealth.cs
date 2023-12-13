using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthManager
{
    // Destroy enemy on 0 health and drop loot
    public void DamageBy(float amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0, maxHealth);
        healthBar.SetHealth(health);

        if (health == 0)
        {
            DropLoot dropLoot = GetComponent<DropLoot>();
            if (dropLoot != null) dropLoot.Loot();
            Destroy(gameObject);
        }
    }
}
