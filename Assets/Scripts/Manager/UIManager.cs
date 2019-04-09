using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Text enemiesText;
    public Text GameStateText;

    PlayerHealth playerHealth;
    GameController gameController;
    public GameState state = GameState.MainMenu;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (state == GameState.MainMenu && Input.GetMouseButtonDown(0))
        {
            state = GameState.Playing;

            enemiesText.gameObject.SetActive(true);
            GameStateText.gameObject.SetActive(false);

            gameController.CreateWalls();
            gameController.StartSpawn();
            Cursor.visible = true;
        }
        else if (state == GameState.Playing)
        {
            enemiesText.text = "Wave " + gameController.currentWave + " / " + gameController.maxWaves + "\nEnemies Left: " + (gameController.enemiesOnWave - gameController.enemiesKilled);
            Cursor.visible = false;
        }
        else if (state == GameState.GameOver)
        {
            Cursor.visible = true;
            enemiesText.gameObject.SetActive(false);
            GameStateText.gameObject.SetActive(true);

            if (playerHealth.currnetHealth > 0)
            {
                GameStateText.text = "You win! \n\nClick to continue";
            }
            else
            {
                GameStateText.text = "You Lose :( \n\nClick to continue";
            }
 
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
       
    }
}
