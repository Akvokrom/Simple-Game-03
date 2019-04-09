using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currnetHealth;
    public Slider HealthSlider;
    public Image damage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    PlayerController playerController;
    PlayerShooting playerShooting;
    UIManager UIManager;

    bool isDead;
    bool damaged;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        HealthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        damage = GameObject.Find("Damage").GetComponent<Image>();
        UIManager = GameObject.Find("GameController").GetComponent<UIManager>();

        HealthSlider.maxValue = startingHealth;
        HealthSlider.value = startingHealth;
        currnetHealth = startingHealth;
    }

    void Update()
    {
        if (damaged)
        {
            damage.color = flashColor;
        }
        else
        {
            damage.color = Color.Lerp(damage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;

        currnetHealth -= amount;

        HealthSlider.value = currnetHealth;

        if(currnetHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        playerController.enabled = false;
        playerShooting.enabled = false;
        playerShooting.DisableEffects();

        //Left some time for dying animation
        Invoke("GameOver", 1);
        Destroy(gameObject, 1);
    }

    void GameOver()
    {
        UIManager.state = GameState.GameOver;
    }
}
