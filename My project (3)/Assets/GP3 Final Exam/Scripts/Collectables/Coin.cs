using UnityEngine;

public class Coin : MonoBehaviour
{
    public int givePoints; // Points the coin gives to the player

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.TryGetComponent<PlayerMovements>(out PlayerMovements player))
        {
            // Call CollectCoin method to add points to the player
            player.CollectCoin(givePoints);
            // Destroy the coin after collection
            Destroy(gameObject);
        }
    }
}
