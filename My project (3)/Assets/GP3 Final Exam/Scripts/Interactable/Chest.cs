using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isInteracted = false;  // Tracks whether the chest has been interacted with
    public TextMeshProUGUI Tooltips;  // Tooltip UI to show interaction text
    public List<Coin> coins;  // List of available coins to spawn
    public GameObject coinParents;  // Parent GameObject to hold spawned coins

    // Called when a collision with the chest occurs
    private void OnCollisionEnter(Collision collision)
    {
        Tooltips.gameObject.SetActive(true);  // Show the tooltip
        Tooltips.text = "Press E to Open Chest";  // Update the tooltip text
        if (collision.gameObject.TryGetComponent<PlayerMovements>(out PlayerMovements player))  // Check if it's the player
        {
            player.SetInteractable(gameObject);  // Set the chest as interactable for the player
        }
    }

    // Called when a collision with the chest ends
    private void OnCollisionExit(Collision collision)
    {
        Tooltips.gameObject.SetActive(false);  // Hide the tooltip
        if (collision.gameObject.TryGetComponent<PlayerMovements>(out PlayerMovements player))  // Check if it's the player
        {
            player.SetInteractable(null);  // Clear the interactable object when the player leaves
        }
    }

    // Called when the player interacts with the chest (e.g., pressing "E")
    public void Interacted()
    {
        if (!isInteracted)  // If the chest hasn't been interacted with
        {
            isInteracted = true;  // Mark it as interacted
            SpawnCoin();  // Spawn a coin
            gameObject.SetActive(false);  // Disable the chest
        }
        else
        {
            isInteracted = false;  // Reset interaction state
            gameObject.SetActive(true);  // Re-enable the chest if needed
        }
    }

    // Spawns a random coin at the chest's position
    public void SpawnCoin()
    {
        int randomCoin = Random.Range(0, coins.Count);  // Randomly select a coin from the list
        Instantiate(coins[randomCoin], transform.position, Quaternion.identity, coinParents.transform);  // Spawn the selected coin
    }
}
