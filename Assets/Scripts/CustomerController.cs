using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerController : MonoBehaviour
{
    Transform originalSpawnPos;
    GameObject targetBench;
    int benchId;
    public float moveSpeed = 2f;
    float originalMoveSpeed = 10f;
    [SerializeField] bool movingToBenchHorizontal = true;
    [SerializeField] bool movingToBenchVertical = false;

    [SerializeField] bool moveBackDown = false;
    [SerializeField] bool moveOut = false;
    [SerializeField] bool sunBathing = false;
    [SerializeField] bool requestingSomething = false;

    [SerializeField] float timer = 15f;
    [SerializeField] float maxTimer = 15f;

    [SerializeField] float burnTimer = 0f;
    [SerializeField] float maxBurnTimer = 20f;

    [SerializeField] float burnModifier = 1f;

    int triggerRequestChance = 9;
    float intervalTicker = 0f;

    [SerializeField] string requestedItem = "";
    [SerializeField] Image requestedItemImage;
       
    public Slider burnSlider;
    public Slider timerSlider;
    public void SetStartValues(GameObject target, Transform spawnPos)
    {
        targetBench = target;
        originalSpawnPos = spawnPos;
        benchId = targetBench.GetComponent<ChairController>().GetChairId();
        timerSlider.value = timerSlider.maxValue;
        requestedItemImage.gameObject.SetActive(false);

        timer = maxTimer;
        burnTimer = 0;
    }


    private void Update()
    {
        if (targetBench != null && movingToBenchHorizontal)
        {
            MoveToBenchHorizontal();
        }
        else if (movingToBenchVertical)
        {
            MoveToBenchVertical();
        }
        else if (moveBackDown)
        {
            MoveDown();
        }
        else if(moveOut)
        {
            MoveOutScreen();
        }
        else if (sunBathing && !requestingSomething)
        {
            timer -= Time.deltaTime;
            intervalTicker += Time.deltaTime;
            TickBurnTimer();
            if (intervalTicker > 1)
            {
                intervalTicker = 0;
                RollForRandomRequest();
            }
            RefreshSlider();
            if (timer <= 0)
            {
                StopBathing();
            }
        }
        else if(requestingSomething)
        {
            TickBurnTimer();
        }
    }

    void TickBurnTimer()
    {
        if (requestingSomething)
            burnModifier = 1.5f;
        else
            burnModifier = 1f;

        burnTimer += Time.deltaTime * burnModifier;
        burnSlider.value = Tools.instance.Normalize(0, maxBurnTimer, burnTimer);
        if (burnTimer >= maxBurnTimer)
        {
            Debug.Log("Customer burned in the sun!");
            DestroySelf();
        }
    }
    
    void RollForRandomRequest()
    {
        int randomChance = UnityEngine.Random.Range(0, 10);
        if(randomChance < triggerRequestChance) //Default 20% chance per second
        {
            TriggerRequest();
        }
    }

    void TriggerRequest()
    {
        requestingSomething = true;
        requestedItem = RequestManager.instance.GetRandomItemRequest();
        requestedItemImage.sprite = RequestManager.instance.GetItemSpriteByName(requestedItem);
        requestedItemImage.gameObject.SetActive(true);
        //requestText.text = requestedItem;
    }

    public void ReceiveItem(string itemName)
    {
        if(itemName == requestedItem)
        {
            Debug.Log("Correct item received");
            requestedItem = "";
            requestingSomething = false;
            requestedItemImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Incorrect item received");
        }
    }

    public void RefreshSlider()
    {
        timerSlider.value = Tools.instance.Normalize(0, maxTimer, timer);
    }

    private void MoveToBenchHorizontal()
    {
        // Move only on the x axis towards the target bench
        if (transform.position.x < targetBench.transform.position.x )
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Call MoveEnded when the customer reaches or surpasses the target bench's x position
            MoveEndedRight();
        }
    }
    private void MoveEndedRight()
    {
        // Start up movement
        movingToBenchHorizontal = false;
        movingToBenchVertical = true;
    }

    private void MoveToBenchVertical()
    {
        // Move only on the x axis towards the target bench
        if (transform.position.y < targetBench.transform.position.y - 0.64f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            StartSunBathing();
        }
    }

    void StartSunBathing()
    {

        // Stop the movement
        moveSpeed = 0f;
        movingToBenchHorizontal = false;
        movingToBenchVertical = false;

        // Additional logic can be added here when the move ends
        Debug.Log("Customer has reached the target bench: " + targetBench.name);
        sunBathing = true;
    }

    void StopBathing()
    {
        sunBathing = false;
        moveBackDown = true;
        CustomerSpawner.instance.ReleaseBench(benchId);
        moveSpeed = originalMoveSpeed;

    }

    void MoveDown()
    {
        // Move only on the x axis towards the target bench
        //Debug.Log("T: "+ transform.position.y + " ---- " + originalSpawnPos.position.y);
        if (transform.position.y > originalSpawnPos.position.y - 0.92f)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveBackDown = false;
            moveOut = true;
        }
    }

    void MoveOutScreen()
    {
        if (transform.position.x < originalSpawnPos.position.x * -1)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void DestroySelf()
    {
        CustomerSpawner.instance.ReleaseBench(benchId);
        Destroy(this.gameObject);
    }

}
