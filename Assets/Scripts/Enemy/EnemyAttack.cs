using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetwennAttacks = 0.5f;
    public int attackDamage = 10;
    public GameObject damageText;

    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;

    bool playerInRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetwennAttacks && playerInRange && enemyHealth.currnetHealth > 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        timer = 0f;
        if (playerHealth.currnetHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);

            GameObject damageTextHit = Instantiate(damageText, player.transform.position, Quaternion.identity);
            damageTextHit.GetComponentInChildren<TextMesh>().text = "-" + attackDamage.ToString();
            damageTextHit.GetComponentInChildren<TextMesh>().color = new Color (0, 0, 0, 1);
            Destroy(damageTextHit, 1);
        }
    }
}
