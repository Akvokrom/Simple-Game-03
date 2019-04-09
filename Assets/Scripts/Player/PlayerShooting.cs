using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public GameObject damageText;
    UIManager UIManager;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        UIManager = GameObject.Find("GameController").GetComponent<UIManager>();
    }

    void Update()
    {
        if (UIManager.state == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
            {
                Shoot();
            }

            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;

        gunLight.enabled = true;
        gunLine.enabled = true;

        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null && enemyHealth.currnetHealth > 0)
            {
                enemyHealth.TakeDamage(damagePerShot);

                GameObject damageTextHit = Instantiate(damageText, shootHit.point, Quaternion.identity);
                damageTextHit.GetComponentInChildren<TextMesh>().text = "-" + damagePerShot.ToString();
                Destroy(damageTextHit, 1);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
