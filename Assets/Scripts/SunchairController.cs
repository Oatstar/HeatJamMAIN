using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunchairController : MonoBehaviour
{
    [SerializeField] Vector2[] sunchairPositions;
    [SerializeField] GameObject sunchairPrefab;
    // Start is called before the first frame update
    public GameObject[] allChairs = new GameObject[16];

    public static SunchairController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void CreateChairs()
    {
        for (int i = 0; i < sunchairPositions.Length; i++)
        {
            GameObject spawnedChair = Instantiate(sunchairPrefab, sunchairPositions[i], Quaternion.identity, this.transform);
            spawnedChair.GetComponent<ChairController>().SetChairId(i);
            allChairs[i] = spawnedChair;
        }
    }

    public void ResetState()
    {
        for (int i = 0; i < allChairs.Length; i++)
        {
            allChairs[i].transform.GetComponent<ChairController>().ResetState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
