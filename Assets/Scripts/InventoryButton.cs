using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Inventory_UI inventoryUI;
    
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnInventoryButtonClick);
        }
        
        // Auto-find inventory UI kalo belum di-assign
        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<Inventory_UI>();
        }
        
#if !UNITY_ANDROID && !UNITY_IOS
        gameObject.SetActive(false);
#endif
    }
    
    void OnInventoryButtonClick()
    {
        if (inventoryUI != null)
        {
            inventoryUI.ToggleInventory();
        }
        else
        {
            Debug.LogWarning("Inventory_UI not assigned!");
        }
    }
}