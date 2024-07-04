using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public bool[] chairTaken = new bool[16];
    public GameObject[] allChairs = new GameObject[16];

    public GameObject customerPrefab;
    public Transform spawnpointTop;
    public Transform spawnpointBot;

    public float moveSpeed = 2f;

    public static CustomerSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        Transform spawnPos;
        spawnPos = GetSpawnPos();

        if (spawnPos == null)
            return;

        GameObject spawnedCustomer = Instantiate(customerPrefab, spawnPos);
        GameObject targetBench = SetTarget(spawnPos);

        spawnedCustomer.GetComponent<CustomerController>().SetStartValues(targetBench, spawnPos);
    }

    GameObject SetTarget(Transform spawnPos)
    {
        GameObject targetBench = null;

        targetBench = (spawnPos == spawnpointTop) ? GetRandomFreeBench("top") : GetRandomFreeBench("bot");
               
        chairTaken[targetBench.GetComponent<ChairController>().GetChairId()] = true;
        return targetBench;
    }

    GameObject GetRandomFreeBench(string side)
    {
        int rangeMin = 0;
        int rangeMax = 8;
        int benchId = -1;
        if(side == "bot")
        {
            rangeMin = 8;
            rangeMax = allChairs.Length;
        }

        while (true)
        {
            benchId = Random.Range(rangeMin, rangeMax);
            if (!chairTaken[benchId])
            {
                break;
            }
        }
        return allChairs[benchId];
    }

    Transform GetSpawnPos()
    {
        Transform spawnPos = null;
        int chairFreeState = FreeChairs();

        if (chairFreeState == 0)
        {
            Debug.Log("No free chairs");
        }
        else if (chairFreeState == 1)
        {
            spawnPos = spawnpointTop.transform;
        }
        else if (chairFreeState == 2)
        {
            spawnPos = spawnpointBot.transform;
        }
        else if (chairFreeState == 3)
        {
            int randomVal = UnityEngine.Random.Range(0, 2);
            if (randomVal == 0)
                spawnPos = spawnpointTop.transform;
            else
                spawnPos = spawnpointBot.transform;
        }
        else
            spawnPos = null;

        return spawnPos;
    }

    //Returns int. 0=no chairs, 1= top, 2 = bot, 3 both
    int FreeChairs()
    {
        bool freeTop = false;
        bool freeBot = false;
        //top
        for (int i = 0; i < 8; i++)
        {
            if (!chairTaken[i])
            {
                freeTop = true;
                break;
            }
        }
        for (int i = 8; i < chairTaken.Length; i++)
        {
            if (!chairTaken[i])
            {
                freeBot = true;
                break;
            }
        }

        if (freeTop && freeBot)
            return 3;
        else if (freeTop)
            return 1;
        else if (freeBot)
            return 2;
        else return 0;

    }

    public void ReleaseBench(int id)
    {
        chairTaken[id] = false;
    }
}
