using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<Slot_UI> slots = new List<Slot_UI>();
    
    private int selectedInventorySlot = -1;
    private bool isMovingMode = false;

    void Start() 
    {
        inventoryPanel.SetActive(false);
        
        // Set reference untuk semua slots
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                slots[i].slotIndex = i;
                slots[i].inventoryUI = this;
                slots[i].isToolbarSlot = false; // Ini inventory slot, bukan toolbar
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            ToggleInventory();
        }
        
        if (inventoryPanel.activeSelf)
        {
            HandleInventoryInput();
        }
    }

    void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.X) && selectedInventorySlot >= 0 && !isMovingMode)
        {
            Remove(selectedInventorySlot);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && isMovingMode)
        {
            CancelMove();
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        if (!isMovingMode)
        {
            // First click: pilih item untuk dipindah (TANPA HIGHLIGHT)
            if (slotIndex < player.inventory.slots.Count && 
                player.inventory.slots[slotIndex].itemName != "")
            {
                selectedInventorySlot = slotIndex;
                isMovingMode = true;
                
                Debug.Log("Selected slot " + slotIndex + " to move");
            }
        }
        else
        {
            // Second click: pindahin item
            SwapSlots(selectedInventorySlot, slotIndex);
            Debug.Log("Moved from " + selectedInventorySlot + " to " + slotIndex);
            CancelMove();
        }
    }

    void SwapSlots(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex)
        {
            CancelMove();
            return;
        }
        
        // Swap items
        var temp = new Inventory.Slot();
        temp.itemName = player.inventory.slots[fromIndex].itemName;
        temp.icon = player.inventory.slots[fromIndex].icon;
        temp.count = player.inventory.slots[fromIndex].count;
        temp.maxAllowed = player.inventory.slots[fromIndex].maxAllowed;
        
        player.inventory.slots[fromIndex].itemName = player.inventory.slots[toIndex].itemName;
        player.inventory.slots[fromIndex].icon = player.inventory.slots[toIndex].icon;
        player.inventory.slots[fromIndex].count = player.inventory.slots[toIndex].count;
        
        player.inventory.slots[toIndex].itemName = temp.itemName;
        player.inventory.slots[toIndex].icon = temp.icon;
        player.inventory.slots[toIndex].count = temp.count;
        
        Refresh();
    }

    void CancelMove()
    {
        selectedInventorySlot = -1;
        isMovingMode = false;
    }

    public void ToggleInventory() 
    {
        if (!inventoryPanel.activeSelf) 
        {
            inventoryPanel.SetActive(true);
            Refresh();
        }
        else 
        {
            CancelMove();
            inventoryPanel.SetActive(false);
        }
    }

    void Refresh() 
    {
        if (player == null || player.inventory == null) return;

        for (int i = 0; i < slots.Count; i++) 
        {
            if (slots[i] == null) continue;
            
            if (i < player.inventory.slots.Count)
            {
                slots[i].SetItem(player.inventory.slots[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove(int slotID) 
    {
        if (slotID < 0 || slotID >= player.inventory.slots.Count) return;
        if (player.inventory.slots[slotID].itemName == "") return;
            
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[slotID].itemName);

        if (itemToDrop != null) 
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    }
}