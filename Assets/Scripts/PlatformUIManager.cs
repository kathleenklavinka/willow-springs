using UnityEngine;

public class PlatformUIManager : MonoBehaviour
{
    public GameObject mobileControlsPanel;
    public GameObject pcHelpText; // Optional: text "Press SPACE to action"
    
    void Start()
    {
        // Show mobile controls hanya di Android/iOS
#if UNITY_ANDROID || UNITY_IOS
        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(true);
        if (pcHelpText != null)
            pcHelpText.SetActive(false);
#else
        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(false);
        if (pcHelpText != null)
            pcHelpText.SetActive(true);
#endif
    }
}