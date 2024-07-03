using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItemPickupper : MonoBehaviour
{
    // Variable to store the reference to the item the character is next to
    private GameObject nearbyItem;

    // Reference to the prefab to instantiate when picking up an item
    public GameObject prefabItem;

    // Reference to the ItemHolderSlot on the character
    public Transform itemHolderSlot;

    void Update()
    {
        // Check if the player presses the pick up key (e.g., spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nearbyItem != null)
            {
                PickUpItem();
            }
        }
    }

    // This function is called when the character enters a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Item"
        if (other.CompareTag("Item"))
        {
            // Store the reference to the nearby item
            nearbyItem = other.gameObject;
            Debug.Log("Item nearby: " + nearbyItem.name);
        }
    }

    // This function is called when the character exits a trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider is tagged as "Item"
        if (other.CompareTag("Item"))
        {
            // Clear the reference to the nearby item
            nearbyItem = null;
            Debug.Log("Left item: " + other.gameObject.name);
        }
    }

    // Function to handle item pickup
    void PickUpItem()
    {
        Debug.Log("Picked up: " + nearbyItem.name);

        // Instantiate the prefab item into the ItemHolderSlot
        Instantiate(prefabItem, itemHolderSlot.position, itemHolderSlot.rotation, itemHolderSlot);

        // Optionally destroy the nearby item after "picking it up"
        Destroy(nearbyItem);
    }
}
