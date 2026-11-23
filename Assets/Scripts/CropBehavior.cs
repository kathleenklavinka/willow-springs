using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
    public Sprite[] growthStages;
    public float[] stageDurations;
    
    [Header("Harvest")]
    public ItemData harvestItem;
    public int harvestAmount = 1;
    
    [Header("Seed Drop")]
    public ItemData seedItem;
    public int seedDropAmount = 2;
    
    private SpriteRenderer spriteRenderer;
    private int currentStage = 0;
    private bool isFullyGrown = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Debug.Log("CropBehavior Start! Growth stages count: " + growthStages.Length);
        
        if (growthStages.Length > 0)
        {
            spriteRenderer.sprite = growthStages[0];
            StartCoroutine(GrowCrop());
            Debug.Log("Started GrowCrop coroutine!");
        }
    }
    
    IEnumerator GrowCrop()
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            currentStage = i;
            spriteRenderer.sprite = growthStages[i];
            
            Debug.Log("Changed to stage " + i);
            
            if (i < stageDurations.Length)
            {
                Debug.Log("Waiting " + stageDurations[i] + " seconds");
                yield return new WaitForSeconds(stageDurations[i]);
            }
        }
        
        isFullyGrown = true;
        Debug.Log("Crop is fully grown!");
    }
        
    public void Harvest(Player player)
    {
        if (!isFullyGrown)
        {
            Debug.Log("Not ready yet!");
            return;
        }
        
        // Kasih harvest item (wheat/beet)
        if (harvestItem != null)
        {
            for (int i = 0; i < harvestAmount; i++)
            {
                Item item = GameManager.instance.itemManager.GetItemByName(harvestItem.itemName);
                if (item != null)
                {
                    player.inventory.Add(item);
                }
                
                // Report ke WinConditionManager SETIAP ITEM
                if (WinConditionManager.instance != null)
                {
                    WinConditionManager.instance.OnHarvest(harvestItem.itemName);
                }
            }
        }
        
        // Kasih seed balik
        if (seedItem != null)
        {
            for (int i = 0; i < seedDropAmount; i++)
            {
                Item seed = GameManager.instance.itemManager.GetItemByName(seedItem.itemName);
                if (seed != null)
                {
                    player.inventory.Add(seed);
                }
            }
            Debug.Log("Got " + seedDropAmount + " seeds back!");
        }
        
        Destroy(gameObject);
    }
    
    public bool IsFullyGrown()
    {
        return isFullyGrown;
    }
}