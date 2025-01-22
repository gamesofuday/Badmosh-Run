using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;       // UI text to display the score
    public Transform player;     // Reference to the player
    private float score;

    void Update()
    {
        // Update score based on player's X position
        score = player.position.x;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }
}
