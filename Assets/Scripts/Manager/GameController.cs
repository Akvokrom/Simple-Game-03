using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public GameObject wall;
    public GameObject enemy;
    PlayerHealth playerHealth;
    UIManager UIManager;
    public int maxWaves = 3;
    public int enemiesOnWave = 10;
    public int mapSize = 500;
    public float enemiesSpawnDelay = 1.5f;
    public int wallOnMap = 5;
    public int DelayBetweenWaves = 3;

    internal int currentWave = 1;
    internal int enemiesSpawned = 0;
    internal int enemiesKilled = 0;
    internal int mapWidth;
    internal int mapHeight;

    GameObject floor;
    public NavMeshSurface surface;

    void Awake()
    {
        floor = GameObject.Find("Floor");
        ResizeFloor();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        UIManager = GameObject.Find("GameController").GetComponent<UIManager>();
    }

    void ResizeFloor()
    {
        mapWidth = (int)Mathf.Round(Screen.width * Mathf.Sqrt((float)mapSize / (Screen.width * Screen.height)));
        mapHeight = (int)Mathf.Round(Screen.height * Mathf.Sqrt((float)mapSize / (Screen.width * Screen.height)));

        floor.transform.localScale = new Vector3(mapWidth, 1, mapHeight);
    }

    public void CreateWalls()
    {
        for (int i = 0; i < wallOnMap; i++)
        {
            bool placeselected = false;
            Vector3 randomPosition = Vector3.zero;

            while (!placeselected)
            {
                randomPosition = new Vector3(Random.Range(-mapWidth / 2 + 1, mapWidth / 2 - 1), 0, Random.Range(-mapHeight / 2 + 1, mapHeight / 2 - 1));

                Ray ray = new Ray(new Vector3(randomPosition.x, 2, randomPosition.z), Vector3.down);
                Physics.Raycast(ray, out RaycastHit hit);

                if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
                {
                    placeselected = true;
                    break;
                }
            }
            var wallPlaced =Instantiate(wall, randomPosition, Quaternion.identity);
            wallPlaced.transform.parent = GameObject.Find("Walls").gameObject.transform;
        }

        surface.BuildNavMesh();
    }

    public void NextWave()
    {
        Invoke("StartNewWave", DelayBetweenWaves);
    }

    void StartNewWave()
    {
        if (currentWave < maxWaves)
        {
            currentWave++;
            enemiesSpawned = 0;
            enemiesKilled = 0;
        }
        else
        {
            UIManager.state = GameState.GameOver;
        }
    }

    public void StartSpawn()
    {
        InvokeRepeating("Spawn", enemiesSpawnDelay, enemiesSpawnDelay);
    }

    void Spawn()
    {
        if (playerHealth.currnetHealth <= 0f || enemiesSpawned == enemiesOnWave)
            return;

        bool placeselected = false;
        Vector3 randomPosition = Vector3.zero;

        while (!placeselected)
        {
            randomPosition = new Vector3(Random.Range(-mapWidth / 2 + 1, mapWidth / 2 - 1), 0, Random.Range(-mapHeight / 2 + 1, mapHeight / 2 - 1));

            Ray ray = new Ray(new Vector3(randomPosition.x, 2, randomPosition.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit);

            if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
            {
                placeselected = true;
                break;
            }
        }
        var enemyPlaced = Instantiate(enemy, randomPosition, Quaternion.identity);
        enemyPlaced.transform.parent = GameObject.Find("Enemies").gameObject.transform;
        enemiesSpawned++;
    }
}
