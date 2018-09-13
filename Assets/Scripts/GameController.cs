using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the main gameplay
/// </summary>
public class GameController : MonoBehaviour {

    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tiles to spawn initially with no obstacles")]
    public int initNoObstacles = 4;

    /// <summary>
    /// Where the next tile should be spawned
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// How should the next tile be rotated
    /// </summary>
    private Quaternion nextTileRotation;

    /// <summary>
    /// Use this for initialization
    /// </summary>
	void Start()
    {
        // set our starting point
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for (int i = 0; i < initSpawnNum; i++)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }

    /// <summary>
    /// Will spawn a tile at a certain location and setup the next position
    /// </summary>
	public void SpawnNextTile (bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        // Figure out location & rotation for next tile spawn
        var nextTile = newTile.Find("NextSpawnPoint");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (!spawnObstacles)
        {
            return;
        }

        // now get all possible places to spawn obstacle
        var obstacleSpawnPoints = new List<GameObject>();

        // go through each of child game objects in our tile
        foreach(Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }

        // make sure there is at least one
        if (obstacleSpawnPoints.Count > 0)
        {
            // Get a random object from the ones in the List
            var spawnPoint = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Count)];

            // Store its position for us to use
            var spawnPos = spawnPoint.transform.position;

            // create our obstacle
            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            // have it parented to the tile
            newObstacle.SetParent(spawnPoint.transform);
        }
	}
}
