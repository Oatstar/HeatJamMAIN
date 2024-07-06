using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMasterManager : MonoBehaviour
{
    [SerializeField] float timer = 0f;
    [SerializeField] float gameTimer = 0f;
    [SerializeField] GameObject backdropPanel;

    int maxSpawnCount = 10;
    [SerializeField] float spawnInterval = 10f;
    [SerializeField] int currentSpawnCount = 0;
    [SerializeField] int burnCount = 0;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject victoryMenu;

    [SerializeField] TMP_Text customerSpawnCountText;
    [SerializeField] TMP_Text failCountText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text currentDayText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] int currentDay;

    [SerializeField] GameObject startButton;

    public static GameMasterManager instance;
    private void Awake()
    {
        instance = this;
        gameOverText.SetActive(false);

        Time.timeScale = 0f;
        customerSpawnCountText.text = "SPAWNS: 0";
        timerText.text = "TIME: 0";
        failCountText.text = "BURNS: 0";
        currentDayText.text = "DAY: 0";
        //SunchairController.instance.CreateChairs();

    }


    public void StartGame()
    {
        SoundManager.instance.PlayBasicClick();

        CloseMenu();
        SunchairController.instance.CreateChairs();
        Time.timeScale = 1f;
        Invoke("SpawnCustomer", 0.1f);
        Invoke("SpawnCustomer", 2f);
        Invoke("SpawnCustomer", 5f);
    }



    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        backdropPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SoundManager.instance.PlayBasicClick();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            TriggerGameOver();
        if (Input.GetKeyDown(KeyCode.B))
            TriggerVictoryMenu();

        gameTimer += Time.deltaTime;
        timerText.text = "TIME: " + Mathf.RoundToInt(gameTimer).ToString();

        timer += Time.deltaTime;
        if (timer > spawnInterval)
        {
            FindAllCustomers();

            if (currentSpawnCount >= maxSpawnCount)
                return;

            SpawnCustomer();
            timer = 0f;
        }
    }

    public void OnDelayFindAllCustomers()
    {
        Invoke("FindAllCustomers", 1f);
    }


    public void FindAllCustomers()
    {
        List<GameObject> remainingCustomers = new List<GameObject> { };
        remainingCustomers.AddRange(GameObject.FindGameObjectsWithTag("Customer"));

        bool noCustomers = false;
        bool noSpawnsLeft = false;
        if(remainingCustomers.Count <= 0)
        {
            Debug.Log("No more customers in game");
            noCustomers = true;
        }
        if(currentSpawnCount >= maxSpawnCount)
        {
            Debug.Log("All customers have been spawned");
            noSpawnsLeft = true;
        }

        if(noCustomers && noSpawnsLeft)
        {
            Debug.Log("Game ends to victory!");
            TriggerVictoryMenu();
        }
    }

    public void CustomerBurned()
    {
        SoundManager.instance.PlayBurning();

        burnCount++;
        failCountText.text = "BURNS: " + burnCount;
        if(burnCount >= 3)
        {
            Debug.Log("Three burn victims! Game over!");
            TriggerGameOver();
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    void SpawnCustomer()
    {
        bool customerSpawned = CustomerSpawner.instance.SpawnCustomer();
        if (customerSpawned)
        {
            currentSpawnCount++;
            customerSpawnCountText.text = "SPAWNS: "+currentSpawnCount.ToString();
        }

        FindAllCustomers();
    }

    public void NextDayButton()
    {
        SoundManager.instance.PlayBasicClick();

        //Reset current game state
        ResetGameState();
        victoryMenu.SetActive(false);
        currentDay++;
        currentDayText.text = "DAY: "+ currentDay.ToString();
        
        //Set next day variables
        maxSpawnCount += 2;
        spawnInterval -= 0.5f;

        //Start new day
        CloseMenu();
        StartGame();
    }

    public void ResetGameState()
    {
        GameObject playerCharacter = GameObject.Find("Character");
        playerCharacter.GetComponent<CharacterMover>().ResetCharacter();
        playerCharacter.GetComponent<CharacterItemPickupper>().ResetScript();
        CustomerSpawner.instance.ResetState();
        SunchairController.instance.ResetState();

        currentSpawnCount = 0;
        gameTimer = 0;
        timer = 0;
        burnCount = 0;

        customerSpawnCountText.text = "SPAWNS: 0";
        timerText.text = "TIME: 0";
        failCountText.text = "BURNS: 0";
    }


    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        backdropPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        SoundManager.instance.PlayGameOver();

        OpenPauseMenu();
        Time.timeScale = 0;
        gameOverText.SetActive(true);
        startButton.SetActive(false);
    }


    public void TriggerVictoryMenu()
    {
        SoundManager.instance.PlayVictory();

        OpenPauseMenu();
        victoryMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
