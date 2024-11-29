using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public bool isInteracted;
    public TextMeshProUGUI Tooltips;

    private void OnCollisionEnter(Collision collision)
    {
        Tooltips.gameObject.SetActive(true);
        Tooltips.text = "Press E to Open Door";
        if (collision.gameObject.TryGetComponent<PlayerMovements>(out PlayerMovements player))
        {
            player.SetInteractable(gameObject);  // Use the SetInteractable method
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Tooltips.gameObject.SetActive(false);
        if (collision.gameObject.TryGetComponent<PlayerMovements>(out PlayerMovements player))
        {
            player.ClearInteractable();  // Clear interactable object when collision ends
        }
    }

    public void Interacted()
    {
        if (!isInteracted)
        {
            isInteracted = true;
            gameObject.SetActive(false); // Hide door when interacted
        }
        else
        {
            isInteracted = false;
            gameObject.SetActive(true); // Show door when interacted again
        }
    }
}
