using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    Rigidbody playerRigidBody;
    PlayerHealth playerHealth;
    GameObject floor;
    UIManager UIManager;

    int floorMask;
    internal bool isFalling;
    float camRayLenght = 100f;
    public float speed = 200f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        floor = GameObject.FindGameObjectWithTag("Floor").transform.GetChild(0).gameObject;
        playerRigidBody = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
        UIManager = GameObject.Find("GameController").GetComponent<UIManager>();
    }

    void FixedUpdate()
    {
        if (UIManager.state == GameState.Playing)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Move(h, v);
            Turning();
        }
        else if (!isFalling)
        {
            playerRigidBody.velocity = Vector3.zero;
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        if (!isFalling)
        {
            playerRigidBody.velocity = movement;
        }
        else
        {
            playerRigidBody.velocity = Vector3.down * 10;
            playerHealth.TakeDamage((int)(playerHealth.startingHealth * 0.1f));
        }
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        if(Physics.Raycast (camRay, out RaycastHit floorHit, camRayLenght, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isFalling = true;
            floor.SetActive(false);
        }
    }
}
