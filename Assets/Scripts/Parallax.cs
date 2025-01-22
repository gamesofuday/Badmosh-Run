using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;  // Speed of the parallax effect
    private Transform player;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        // Move the background based on the player's position
        Vector3 newPosition = transform.position;
        newPosition.x = player.position.x * parallaxSpeed;
        transform.position = newPosition;
    }
}
