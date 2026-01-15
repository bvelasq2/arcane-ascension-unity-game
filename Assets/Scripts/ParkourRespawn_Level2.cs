using UnityEngine;
using System.Collections;

public class ParkourRespawn : MonoBehaviour
{
    // These should be set by your MazeGenerator.
    public static Vector3 StartPoint;    // The starting position of the maze.
    public static Vector3 RespawnPoint;  // The end position of the maze (used for falling off).

    [Header("Respawn Settings")]
    [Tooltip("If the player's Y position falls below this value, they will be respawned at the maze's end.")]
    public float fallThresholdY = -5f;

    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // If MazeGenerator hasn't set the points, default to the current position.
        if (StartPoint == default(Vector3))
            StartPoint = transform.position;
        if (RespawnPoint == default(Vector3))
            RespawnPoint = transform.position;
    }

    private void Update()
    {
        // If the player falls off the ledge (Y position below threshold), respawn at the end.
        if (transform.position.y < fallThresholdY)
        {
            Debug.Log("Player fell off the ledge. Respawning at end of maze: " + RespawnPoint);
            RespawnToEnd();
        }
    }

    /// <summary>
    /// Respawns the player at the starting point (used for death by damage, etc.).
    /// </summary>
    public void RespawnToStart()
    {
        Debug.Log("Respawning to Start at " + StartPoint);
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = StartPoint;
            transform.rotation = Quaternion.identity;
            controller.enabled = true;
        }
        else
        {
            transform.position = StartPoint;
            transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Respawns the player at the end of the maze (used when falling off a ledge).
    /// </summary>
    public void RespawnToEnd()
    {
        Debug.Log("Respawning to End at " + RespawnPoint);
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = RespawnPoint;
            transform.rotation = Quaternion.identity;
            controller.enabled = true;
        }
        else
        {
            transform.position = RespawnPoint;
            transform.rotation = Quaternion.identity;
        }
    }
}
