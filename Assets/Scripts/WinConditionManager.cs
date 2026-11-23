using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionManager : MonoBehaviour
{
    public static WinConditionManager instance;
    
    [Header("Win Condition")]
    public int wheatRequired = 10;
    public int beetRequired = 10;
    
    [Header("UI")]
    public SimpleVictoryUI victoryUI;
    
    private int wheatHarvested = 0;
    private int beetHarvested = 0;
    private bool hasWon = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Auto-find kalo belum di-assign
        if (victoryUI == null)
        {
            victoryUI = FindObjectOfType<SimpleVictoryUI>();
            
            if (victoryUI == null)
            {
                Debug.LogError("SimpleVictoryUI NOT FOUND in scene!");
            }
        }
    }
    
    public void OnHarvest(string cropName)
    {
        if (hasWon) return;
        
        Debug.Log("Crop name: " + cropName);
        
        if (cropName.ToLower().Contains("wheat"))
        {
            wheatHarvested++;
            Debug.Log("WHEAT COUNT: " + wheatHarvested + "/" + wheatRequired);
        }
        else if (cropName.ToLower().Contains("beet"))
        {
            beetHarvested++;
            Debug.Log("BEET COUNT: " + beetHarvested + "/" + beetRequired);
        }
        else
        {
            Debug.LogError("UNKNOWN CROP: " + cropName);
        }
        
        CheckWinCondition();
    }
    
    void CheckWinCondition()
    {
        if (wheatHarvested >= wheatRequired && beetHarvested >= beetRequired)
        {
            hasWon = true;
            ShowVictory();
        }
    }
    
    void ShowVictory()
    {
        Debug.Log($"Wheat: {wheatHarvested}/{wheatRequired}, Beet: {beetHarvested}/{beetRequired}");

        GameObject toolbar = GameObject.Find("Toolbar");
        if (toolbar != null) toolbar.SetActive(false);
        
        GameObject counterPanel = GameObject.Find("CounterPanel");
        if (counterPanel != null) counterPanel.SetActive(false);
        
        GameObject mobileControls = GameObject.Find("MobileControls");
        if (mobileControls != null) mobileControls.SetActive(false);
            
        if (victoryUI != null)
        {
            victoryUI.ShowVictory(wheatHarvested, beetHarvested);
            Debug.Log("Victory!");
        }
        else
        {
            Debug.LogError("Victory is NULL!");
        }
    }
    
    public int GetWheatCount() { return wheatHarvested; }
    public int getBeetCount() { return beetHarvested; }
    public bool HasWon() { return hasWon; }

    public void ResetCount()
    {
        wheatHarvested = 0;
        beetHarvested = 0;
        hasWon = false;
    }
}