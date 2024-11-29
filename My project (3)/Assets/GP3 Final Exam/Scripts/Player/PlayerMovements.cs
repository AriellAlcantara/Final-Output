using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovements : MonoBehaviour
{
    public GameObject interactable;
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject interactionObject;

    private bool isInteractable;
    private bool hasWon;

    [Header("Coin Settings")]
    [SerializeField] private List<GameObject> coinPrefabs;
    [SerializeField] private int coinSpawnCount;
    [SerializeField] private Vector2 randomXRange;
    [SerializeField] private Vector2 randomZRange;
    [SerializeField] private Transform coinParent;

    private int coinsCollected;
    private int points;

    [Header("Chest Settings")]
    private int chestsOpened;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI chestsText;
    [SerializeField] private GameObject winPanel;

    // Setter for interactable GameObject
    public void SetInteractable(GameObject interactableObject)
    {
        interactable = interactableObject;
        isInteractable = true;
    }

    private void Start()
    {
        SpawnCoins();
        UpdateUI();
    }

    private void Update()
    {
        if (hasWon) return;

        HandleMovement();
        HandleInteraction();
    }

    // Clears interaction settings when leaving an interactable object
    public void ClearInteractable()
    {
        interactable = null;
        isInteractable = false;
    }

    // Handles player movement
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        playerRigidbody.velocity = movement * movementSpeed;
    }

    // Handles interaction with chests and doors
    private void HandleInteraction()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionObject.TryGetComponent(out Chest chest))
            {
                chest.Interacted();
                chestsOpened++;
                UpdateChestsUI();
            }
            else if (interactionObject.TryGetComponent(out Door door))
            {
                door.Interacted();
            }

            ClearInteraction();
        }
    }

    // Spawns coins at random positions
    private void SpawnCoins()
    {
        int spawnedCoins = 0;

        while (spawnedCoins < coinSpawnCount)
        {
            float randomX = Random.Range(randomXRange.x, randomXRange.y);
            float randomZ = Random.Range(randomZRange.x, randomZRange.y);

            Vector3 spawnPosition = new Vector3(randomX, 10, randomZ);
            if (TryGetGroundPosition(spawnPosition, out Vector3 groundPosition))
            {
                Collider[] overlappingColliders = Physics.OverlapSphere(groundPosition, 2);
                if (overlappingColliders.Length == 0)
                {
                    InstantiateRandomCoin(groundPosition);
                    spawnedCoins++;
                }
            }
        }
    }

    // Checks for ground position to spawn coins
    private bool TryGetGroundPosition(Vector3 origin, out Vector3 groundPosition)
    {
        Ray ray = new Ray(origin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, groundMask))
        {
            groundPosition = hit.point;
            return true;
        }

        groundPosition = Vector3.zero;
        return false;
    }

    // Instantiates a random coin at a given position
    private void InstantiateRandomCoin(Vector3 position)
    {
        int randomIndex = Random.Range(0, coinPrefabs.Count);
        Instantiate(coinPrefabs[randomIndex], new Vector3(position.x, 1, position.z), Quaternion.identity, coinParent);
    }

    // Handles coin collection
    public void CollectCoin(int pointsGained)
    {
        points += pointsGained;
        coinsCollected++;

        UpdateUI();

        if (coinsCollected == coinSpawnCount)
        {
            TriggerWinCondition();
        }
    }

    // Triggers win condition when all coins are collected
    private void TriggerWinCondition()
    {
        hasWon = true;
        winPanel.SetActive(true);
    }

    // Updates UI elements with the latest values
    private void UpdateUI()
    {
        scoreText.text = $"Score: {points}";
        coinsText.text = $"Coins: {coinsCollected}";
    }

    // Updates the chests UI
    private void UpdateChestsUI()
    {
        chestsText.text = $"Chests: {chestsOpened}";
    }

    // Clears the interaction when player leaves the interactable object
    private void ClearInteraction()
    {
        isInteractable = false;
        interactionObject = null;
    }

    // Detects when player enters an interactable object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isInteractable = true;
            interactionObject = other.gameObject;
        }
    }

    // Detects when player exits an interactable object
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            ClearInteraction();
        }
    }

}
