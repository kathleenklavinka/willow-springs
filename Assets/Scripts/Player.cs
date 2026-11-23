using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public Toolbar_UI toolbarUI;
    
    private void Awake() 
    {
        inventory = new Inventory(10);
        
        if (toolbarUI == null)
        {
            toolbarUI = FindObjectOfType<Toolbar_UI>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleAction();
        }
    }
    
    // Dipanggil dari SimpleAutoAction atau input PC
    void HandleAction()
    {
        Vector3Int tilePos = GameManager.instance.tileManager.GetTilePosition(transform.position);
        
        GameObject cropObj = GameManager.instance.tileManager.GetCropAt(tilePos);
        if (cropObj != null)
        {
            CropBehavior crop = cropObj.GetComponent<CropBehavior>();
            if (crop != null && crop.IsFullyGrown())
            {
                crop.Harvest(this);
                GameManager.instance.tileManager.RemoveCrop(tilePos);
                return;
            }
        }
        
        if (GameManager.instance.tileManager.IsInteractable(tilePos))
        {
            TryPlantSeed(tilePos);
            return;
        }
    } 

    void TryPlantSeed(Vector3Int tilePosition)
    {
        int selectedSlot = toolbarUI.GetSelectedSlotIndex();
        
        if (selectedSlot < 0 || selectedSlot >= inventory.slots.Count)
            return;
        
        if (inventory.slots[selectedSlot].itemName == "")
            return;
        
        Item selectedItem = GameManager.instance.itemManager.GetItemByName(
            inventory.slots[selectedSlot].itemName);
        
        if (selectedItem == null || selectedItem.data == null)
            return;
        
        if (!selectedItem.data.isSeed)
            return;
        
        if (GameManager.instance.tileManager.HasCrop(tilePosition))
            return;
        
        if (selectedItem.data.cropPrefab != null)
        {
            Vector3 worldPos = GameManager.instance.tileManager.GetTileWorldPosition(tilePosition);
            GameObject crop = Instantiate(selectedItem.data.cropPrefab, worldPos, Quaternion.identity);
            GameManager.instance.tileManager.PlantCrop(tilePosition, crop);
            inventory.Remove(selectedSlot);
        }
    }

    public void DropItem(Item item) 
    {
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.2f;

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, 
        Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);

        StartCoroutine(EnableColliderAfterDelay(droppedItem));  
    }

    private IEnumerator EnableColliderAfterDelay(Item item)
    {
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            yield return new WaitForSeconds(0.5f);
            if (item != null && col != null)
                col.enabled = true;
        }
    }
}
