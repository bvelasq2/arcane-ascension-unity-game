using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public GameObject mazeTilePrefab;
    public int width = 5;
    public int height = 5;
    public float tileWidth = 5f;
    public float tileDepth = 5f;

    [Header("Player")]
    public Transform player;

    [Header("Castle Decoration")]
    public GameObject wallPrefab;
    public GameObject towerPrefab;

    private MazeTileController[,] grid;
    private bool[,] visited;

    private void Start()
    {
        GenerateMazeGrid();
    }

    // This function sets up the maze grid and generates the maze layout
    void GenerateMazeGrid()
    {
        grid = new MazeTileController[width, height];
        visited = new bool[width, height];

        // Spawn all the tiles that make up the maze
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 spawnPos = new Vector3(x * tileWidth, 0, z * tileDepth);
                GameObject tileGO = Instantiate(mazeTilePrefab, spawnPos, Quaternion.identity, transform);
                grid[x, z] = tileGO.GetComponent<MazeTileController>();
            }
        }

        // Generate the actual maze paths using a recursive backtracking algorithm
        VisitTile(0, 0);

        // Open up the entrance (back wall of first tile) and the exit (front wall of last tile)
        if (grid[0, 0].backWall != null)
            grid[0, 0].backWall.SetActive(false);
        if (grid[width - 1, height - 1].frontWall != null)
            grid[width - 1, height - 1].frontWall.SetActive(false);

        // Move the player to the starting position of the maze
        if (player != null)
        {
            Vector3 mazeStart = grid[0, 0].transform.position + new Vector3(0, 1, 0);
            player.position = mazeStart;
            player.rotation = Quaternion.Euler(0, 0, 0);

            // Set start and end positions for respawn logic (e.g., falling off the map, checkpoints, etc.)
            ParkourRespawn.StartPoint = mazeStart;
            ParkourRespawn.RespawnPoint = grid[width - 1, height - 1].transform.position + new Vector3(0, 1, 0);
        }

        Debug.Log("Maze ends at: " + grid[width - 1, height - 1].transform.position + " | Respawn point set.");
    }

    // Recursive function that visits each tile and removes walls to carve out paths
    void VisitTile(int x, int z)
    {
        visited[x, z] = true;

        // List all possible directions to move from the current tile
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        // Randomize the direction order to create unique maze layouts
        Shuffle(directions);

        // Try each direction
        foreach (Vector2Int dir in directions)
        {
            int nx = x + dir.x;
            int nz = z + dir.y;

            // Check if the neighbor is within bounds and hasn't been visited yet
            if (nx >= 0 && nx < width && nz >= 0 && nz < height && !visited[nx, nz])
            {
                // Depending on which direction we're going, knock down the appropriate walls
                if (dir == Vector2Int.up)
                {
                    grid[x, z].frontWall.SetActive(false);
                    grid[nx, nz].backWall.SetActive(false);
                }
                else if (dir == Vector2Int.down)
                {
                    grid[x, z].backWall.SetActive(false);
                    grid[nx, nz].frontWall.SetActive(false);
                }
                else if (dir == Vector2Int.left)
                {
                    grid[x, z].leftWall.SetActive(false);
                    grid[nx, nz].rightWall.SetActive(false);
                }
                else if (dir == Vector2Int.right)
                {
                    grid[x, z].rightWall.SetActive(false);
                    grid[nx, nz].leftWall.SetActive(false);
                }

                // Recursively visit the next tile
                VisitTile(nx, nz);
            }
        }
    }

    // Fisher-Yates shuffle to randomize direction order
    void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2Int temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
