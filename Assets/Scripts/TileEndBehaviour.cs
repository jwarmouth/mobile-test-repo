using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles spawing a new tile & destroying this one upon the player reaching the end
/// </summary>
public class TileEndBehaviour : MonoBehaviour {

    [Tooltip("How much time to wait until destroying " + "the tile after reaching the end")]
    public float destroyTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        // First, check to see if we collided with the player
        if (other.gameObject.GetComponent<PlayerBehaviour>())
        {
            // If we did, spawn a new tile
            GameObject.FindObjectOfType<GameController>().SpawnNextTile();

            // And destroy this entire tile after a short delay
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
