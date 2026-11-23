using UnityEngine;
using TMPro;

public class HarvestCounterUI : MonoBehaviour
{
    public TextMeshProUGUI wheatText;
    public TextMeshProUGUI beetText;
    
    void Update()
    {
        if (WinConditionManager.instance != null)
        {
            if (wheatText != null)
            {
                int wheat = WinConditionManager.instance.GetWheatCount();
                wheatText.text = wheat + "/10";
            }
            
            if (beetText != null)
            {
                int beet = WinConditionManager.instance.getBeetCount();
                beetText.text = beet + "/10";
            }
        }
    }
}