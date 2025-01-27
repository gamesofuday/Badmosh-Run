using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Spawner spawner;
    public GameObject menuUI;
    public GameObject gameOverUI;

    private bool isGameRunning = false;

    void Start()
    {
        ShowMenu();
    }

    public void StartGame()
    {
        isGameRunning = true;
        Time.timeScale = 1f;

        menuUI.SetActive(false);
        gameOverUI.SetActive(false);

        playerMovement.StartGame();
        spawner.StartSpawning();
    }

    public void GameOver()
    {
        isGameRunning = false;
        Time.timeScale = 0f;

        playerMovement.StopGame();
        spawner.StopSpawning();

        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        spawner.RestartSpawning();
        playerMovement.RestartGame();

        StartGame();
    }

    public void ShowMenu()
    {
        isGameRunning = false;
        Time.timeScale = 0f;

        menuUI.SetActive(true);
        gameOverUI.SetActive(false);
    }
}
