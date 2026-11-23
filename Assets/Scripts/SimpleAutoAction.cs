using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleAutoAction : MonoBehaviour
{
    public Button actionButton;
    public TextMeshProUGUI actionButtonText;
    public Image actionButtonIcon;
    public CanvasGroup buttonCanvasGroup;
    
    [Header("Icons")]
    public Sprite plantIcon;
    public Sprite harvestIcon;
    public Sprite dropIcon;
    
    private Player player;
    private string currentAction = "none";
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        actionButton.onClick.AddListener(OnActionButtonPressed);
        
        if (buttonCanvasGroup == null)
            buttonCanvasGroup = actionButton.GetComponent<CanvasGroup>();
        
        actionButton.gameObject.SetActive(true);
        if (buttonCanvasGroup != null)
            buttonCanvasGroup.alpha = 1;
    }
    
    void Update()
    {
        DetectAvailableAction();
    }
    
    void DetectAvailableAction()
    {
        if (player == null) return;
        
        Vector3Int tilePos = GameManager.instance.tileManager.GetTilePosition(player.transform.position);
        
        GameObject cropObj = GameManager.instance.tileManager.GetCropAt(tilePos);
        if (cropObj != null)
        {
            CropBehavior crop = cropObj.GetComponent<CropBehavior>();
            if (crop != null && crop.IsFullyGrown())
            {
                ShowButton("Harvest", harvestIcon, "harvest");
                return;
            }
            else if (crop != null)
            {
                HideButton();
                return;
            }
        }
        
        if (GameManager.instance.tileManager.IsInteractable(tilePos) && 
            !GameManager.instance.tileManager.HasCrop(tilePos))
        {
            int selectedSlot = player.toolbarUI.GetSelectedSlotIndex();
            if (selectedSlot >= 0 && selectedSlot < player.inventory.slots.Count)
            {
                string itemName = player.inventory.slots[selectedSlot].itemName;
                if (itemName != "")
                {
                    Item item = GameManager.instance.itemManager.GetItemByName(itemName);
                    
                    if (item != null && item.data != null && item.data.isSeed)
                    {
                        ShowButton("Plant", plantIcon, "plant");
                        return;
                    }
                }
            }
        }
        
        int selectedSlotForDrop = player.toolbarUI.GetSelectedSlotIndex();
        if (selectedSlotForDrop >= 0 && selectedSlotForDrop < player.inventory.slots.Count)
        {
            if (player.inventory.slots[selectedSlotForDrop].itemName != "")
            {
                ShowButton("Drop", dropIcon, "drop");
                return;
            }
        }
        
        HideButton();
    }
    
    void ShowButton(string text, Sprite icon, string action)
    {
        if (!actionButton.gameObject.activeSelf)
            actionButton.gameObject.SetActive(true);
            
        actionButtonText.text = text;
        
        if (icon != null && actionButtonIcon != null)
        {
            actionButtonIcon.sprite = icon;
            actionButtonIcon.gameObject.SetActive(true);
        }
        
        currentAction = action;
        
        if (buttonCanvasGroup != null)
            buttonCanvasGroup.alpha = 1;
    }
    
    void HideButton()
    {
        currentAction = "none";
        
        if (buttonCanvasGroup != null)
            buttonCanvasGroup.alpha = 0.3f;
    }
    
    void OnActionButtonPressed()
    {
        if (player == null) return;
        
        Vector3Int tilePos = GameManager.instance.tileManager.GetTilePosition(player.transform.position);
        
        switch (currentAction)
        {
            case "plant":
                TryPlant(tilePos);
                break;
                
            case "harvest":
                TryHarvest(tilePos);
                break;
                
            case "drop":
                TryDrop();
                break;
        }
    }
    
    void TryPlant(Vector3Int tilePos)
    {
        int selectedSlot = player.toolbarUI.GetSelectedSlotIndex();
        if (selectedSlot < 0 || selectedSlot >= player.inventory.slots.Count)
            return;
        
        if (player.inventory.slots[selectedSlot].itemName == "")
            return;
        
        Item selectedItem = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[selectedSlot].itemName);
        
        if (selectedItem == null || selectedItem.data == null || !selectedItem.data.isSeed)
            return;
        
        if (GameManager.instance.tileManager.HasCrop(tilePos))
            return;
        
        if (selectedItem.data.cropPrefab != null)
        {
            Vector3 worldPos = GameManager.instance.tileManager.GetTileWorldPosition(tilePos);
            GameObject crop = Instantiate(selectedItem.data.cropPrefab, worldPos, Quaternion.identity);
            GameManager.instance.tileManager.PlantCrop(tilePos, crop);
            player.inventory.Remove(selectedSlot);
            
            Debug.Log("Planted " + selectedItem.data.itemName + "!");
        }
    }
    
    void TryHarvest(Vector3Int tilePos)
    {
        GameObject cropObj = GameManager.instance.tileManager.GetCropAt(tilePos);
        
        if (cropObj != null)
        {
            CropBehavior crop = cropObj.GetComponent<CropBehavior>();
            if (crop != null && crop.IsFullyGrown())
            {
                crop.Harvest(player);
                GameManager.instance.tileManager.RemoveCrop(tilePos);
                Debug.Log("Harvested!");
            }
        }
    }
    
    void TryDrop()
    {
        int selectedSlot = player.toolbarUI.GetSelectedSlotIndex();
        if (selectedSlot >= 0 && selectedSlot < player.inventory.slots.Count)
        {
            if (player.inventory.slots[selectedSlot].itemName != "")
            {
                Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
                    player.inventory.slots[selectedSlot].itemName);
                
                if (itemToDrop != null)
                {
                    player.DropItem(itemToDrop);
                    player.inventory.Remove(selectedSlot);
                    Debug.Log("Dropped item!");
                }
            }
        }
    }
}