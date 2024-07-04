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
    [SerializeField] string closeItem = "";
    [SerializeField] string carryItem = "";

    [SerializeField] GameObject closeCustomer = null;

    void Update()
    {
        // Check if the player presses the pick up key (e.g., spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (closeItem != "")
            {
                PickUpItem();
            }
            else if (carryItem != "" && closeCustomer != null)
            {
                GiveItem();
            }
        }
    }

    // This function is called when the character enters a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Item"
        if (other.CompareTag("ItemTable"))
        {
            closeItem = other.GetComponent<ItemHandler>().GetItemName();
            //nearbyItem = other.gameObject;

            Debug.Log("Item nearby: " + closeItem);
        }
        else if(other.CompareTag("Customer"))
        {
            closeCustomer = other.gameObject;
        }
    }

    // This function is called when the character exits a trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider is tagged as "Item"
        if (other.CompareTag("ItemTable"))
        {
            Debug.Log("Left item: " + closeItem);
            
            // Clear the reference to the nearby item
            closeItem = "";
                    }
        else if (other.CompareTag("Customer"))
        {
            closeCustomer = null;
        }
    }

    // Function to handle item pickup
    void PickUpItem()
    {
        //Debug.Log("Picked up: " + nearbyItem.name);
        carryItem = closeItem;
        // Instantiate the prefab item into the ItemHolderSlot
        Instantiate(prefabItem, itemHolderSlot.position, itemHolderSlot.rotation, itemHolderSlot);
    }

    void GiveItem()
    {
        closeCustomer.GetComponent<CustomerController>().ReceiveItem(carryItem);
        DropItem();
    }

    void DropItem()
    {
        Destroy(itemHolderSlot.GetChild(0).gameObject);
        carryItem = "";
    }


}
