using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currnetHealth;

    GameController gameController;
    CapsuleCollider capsuleCollider;
    bool isDead;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        currnetHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currnetHealth -= amount;

        if(currnetHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        gameController.enemiesKilled++;
        isDead = true;
        capsuleCollider.isTrigger = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);

        //Left some time for dying animation
        Destroy(gameObject, 1);

        if (gameController.enemiesKilled == gameController.enemiesOnWave)
        {
            gameController.NextWave();
        }
    }
}
