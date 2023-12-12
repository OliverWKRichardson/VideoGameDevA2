using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public HealthBar healthBar;
    public GameObject gameoverScreen;
    private float timer;
    private bool timerActive;
    public bool gameOver = false;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        timerActive = false;
        timer = 3.0f;
    }

    void Update()
    {
        if(timerActive)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0.0f)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void DamageBy(float amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0, maxHealth);
        healthBar.SetHealth(health);
        if(health <= 0 && gameOver)
        {
            Die();
        }
    }

    public void Die()
    {
        gameoverScreen.SetActive(true);
        FirstPersonAIO FPAIO = GetComponent<FirstPersonAIO>();
        FPAIO.playerCanMove = false;
        FPAIO.enableCameraMovement = false;
        timerActive = true;
    }

    public float GetHealth()
    {
        return health;
    }
}
