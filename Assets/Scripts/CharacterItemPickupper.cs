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
    [SerializeField] GameObject closeChair = null;

    [SerializeField] GameObject closeCustomer = null;


    private void Start()
    {
        itemHolderSlot.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ResetScript()
    {
        if(itemHolderSlot.childCount > 0)
            DropItem();
        closeItem = "";
        carryItem = "";
        closeChair = null;
        closeCustomer = null;
        nearbyItem = null;
    }

    void Update()
    {
        if (Time.timeScale <= 0)
            return;

        // Check if the player presses the pick up key (e.g., spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(closeItem != "" && carryItem != "")
            {
                Debug.Log("Repicking");
                DropItem();
                PickUpItem();
            }
            else if (closeItem != "")
            {
                Debug.Log("Picking");
                PickUpItem();
            }
            else if (carryItem == "Parasol" && closeChair != null)
            {
                Debug.Log("Placing Parasol");
                PlaceParasol();
            }
            else if (carryItem != "" && carryItem != "Parasol" && closeCustomer != null)
            {
                Debug.Log("Giving Item");
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
        else if (other.CompareTag("ChairTrigger"))
        {
            closeChair = other.gameObject;
        }


    }

    // This function is called when the character exits a trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider is tagged as "Item"
        if (other.CompareTag("ItemTable"))
        {
            Debug.Log("Left proximity of: " + closeItem);
            
            // Clear the reference to the nearby item
            closeItem = "";
        }
        else if (other.CompareTag("Customer"))
        {
            closeCustomer = null;
        }
        else if (other.CompareTag("ChairTrigger"))
        {
            closeChair = null;
        }
    }

    // Function to handle item pickup
    void PickUpItem()
    {
        //Debug.Log("Picked up: " + nearbyItem.name);

        if(itemHolderSlot.childCount != 0)
            DropItem();

        if (closeItem == "")
            return;

        carryItem = closeItem;
        Debug.Log("Carry item: " + carryItem);
        // Instantiate the prefab item into the ItemHolderSlot
        //GameObject spawnedCarryItem = Instantiate(prefabItem, itemHolderSlot.position, itemHolderSlot.rotation, itemHolderSlot);
        GameObject spawnedCarryItem = itemHolderSlot.transform.GetChild(0).gameObject;
        spawnedCarryItem.GetComponent<ItemHandler>().SetName(carryItem);
        SpriteRenderer spawnedCarrySpriteRen = spawnedCarryItem.GetComponent<SpriteRenderer>();
        spawnedCarrySpriteRen.sprite = RequestManager.instance.GetItemSpriteByName(carryItem);
        spawnedCarryItem.SetActive(true);
        SoundManager.instance.PlayBoxClick();
    }

    void GiveItem()
    {
        closeCustomer.GetComponent<CustomerController>().ReceiveItem(carryItem);
        DropItem();
    }

    void PlaceParasol()
    {
        if (closeChair == null)
        {
            Debug.Log("No close chair");
            return;
        }

        closeChair.GetComponentInParent<ChairController>().SetParasol();
        DropItem();

        SoundManager.instance.PlayParasoldrop();
    }

    void DropItem()
    {
        itemHolderSlot.GetChild(0).gameObject.SetActive(false);
        carryItem = "";
    }


}
