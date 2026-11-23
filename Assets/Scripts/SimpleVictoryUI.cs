using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleVictoryUI : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public GameObject victoryPanel;
    public Image trophyImage; // Untuk PNG trophy

    [Header("Hide on Victory")]
    public GameObject toolbar;  // Drag Toolbar GameObject kesini
    public GameObject counterPanel; // Drag CounterPanel (wheat/beet counter)
    
    void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            
            Canvas canvas = victoryPanel.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = victoryPanel.AddComponent<Canvas>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = 999;
            
            Debug.Log("Victory Panel setup with sorting order 999!");
        }
    }
    
    public void ShowVictory(int wheat, int beet)
    {
        Debug.Log("ShowVictory called!");
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            
            victoryPanel.transform.SetAsLastSibling();
            
            CanvasGroup cg = victoryPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            
            Debug.Log("Victory Panel activated and moved to front!");
        }
        
        if (trophyImage != null)
        {
            StartCoroutine(BounceTrophy());
        }
    }
    
    System.Collections.IEnumerator BounceTrophy()
    {
        float time = 0;
        Vector3 originalScale = trophyImage.transform.localScale;
        
        while (time < 1f)
        {
            time += Time.deltaTime * 2;
            float scale = 1 + Mathf.Sin(time * 10) * 0.1f;
            trophyImage.transform.localScale = originalScale * scale;
            yield return null;
        }
        
        trophyImage.transform.localScale = originalScale;
    }
}