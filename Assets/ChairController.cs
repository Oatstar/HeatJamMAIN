using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    [SerializeField] int chairId;

    SpriteRenderer spriteRen;
    SpriteRenderer parasolSpriteRen;
    [SerializeField] Sprite parasolDefault;
    [SerializeField] Sprite parasol20;
    [SerializeField] Sprite parasolBroken;
    [SerializeField] GameObject parasolObject;
    [SerializeField] float parasolHealth = 30f;
    float parasolMaxHealth = 30f;
    bool parasolActive = false;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        int randomSpriteIndex = UnityEngine.Random.Range(0, CustomerSpawner.instance.chairSprites.Length);
        spriteRen.sprite = CustomerSpawner.instance.chairSprites[randomSpriteIndex];
        parasolSpriteRen = parasolObject.GetComponent<SpriteRenderer>();
        parasolObject.SetActive(false);
    }

    public bool GetParasolActive()
    {
        return parasolActive;
    }

    public void SetChairId(int value)
    {
        chairId = value;
    }
 
    public int GetChairId()
    {
        return chairId;
    }

    private void Update()
    {
        if(parasolObject.activeSelf)
        {
            parasolHealth -= Time.deltaTime;
            if (parasolHealth <= parasolMaxHealth * 0.33f)
                parasolSpriteRen.sprite = parasolBroken;
            else if (parasolHealth <= parasolMaxHealth *0.66f)
                parasolSpriteRen.sprite = parasol20;
            else
                parasolSpriteRen.sprite = parasolDefault;

            if (parasolHealth <= 0)
                UnequipParasol();
        }
    }

    public void SetParasol()
    {
        parasolActive = true;
        parasolObject.SetActive(true);
        parasolHealth = parasolMaxHealth;
        parasolSpriteRen.sprite = parasolDefault;

    }

    void UnequipParasol()
    {
        parasolActive = false;
        parasolObject.SetActive(false);
    }

    public void ResetState()
    {
        UnequipParasol();
        Destroy(this.gameObject);
    }
}
