using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    
    void Start()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
            
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Main");
    }
    
    public void ShowCredits()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
            
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }
    
    public void HideCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
            
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}