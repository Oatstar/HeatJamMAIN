using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    [SerializeField] string[] allRequestItems = new string[] { };
    [SerializeField] Sprite[] requestItemSprites = new Sprite[] { };

    public static RequestManager instance;

    private void Awake()
    {
        instance = this;   
    }

    public string[] GetAllRequestItems()
    {
        return allRequestItems;
    }

    public string GetRandomItemRequest()
    {
        int randomVal = UnityEngine.Random.Range(0, allRequestItems.Length-1);
        return allRequestItems[randomVal];
    }

    public string GetRequestItem(string itemName)
    {
        return "";
    }

    public Sprite GetItemSpriteByName(string name)
    {
        int correctId = -1;
        for (int i = 0; i < allRequestItems.Length; i++)
        {
            if (name == allRequestItems[i])
            {
                correctId = i;
                break;
            }
        }

        return requestItemSprites[correctId];
    }
}
