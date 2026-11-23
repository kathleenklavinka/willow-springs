using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public TileManager tileManager;

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        else 
        {
            instance = this;
        }

        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}