using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot_UI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public GameObject Highlight;
    
    [HideInInspector] public int slotIndex = -1;
    [HideInInspector] public Inventory_UI inventoryUI;
    [HideInInspector] public Toolbar_UI toolbarUI;
    [HideInInspector] public bool isToolbarSlot = false;
    
    private void Start()
    {
        SetHighlighted(false);
    }

    public void SetItem(Inventory.Slot slot) 
    {
        if(slot != null && slot.itemName != "")
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            itemIcon.enabled = true;
            quantityText.text = slot.count.ToString();
            quantityText.enabled = true;
        }
        else
        {
            SetEmpty();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        itemIcon.enabled = false;
        quantityText.text = "";
        quantityText.enabled = false;
    }

    public void SetHighlighted(bool isOn) 
    {
        if (Highlight != null)
        {
            Highlight.SetActive(isOn);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isToolbarSlot)
        {
            // Toolbar slot: pilih slot untuk dipakai
            if (toolbarUI != null && slotIndex >= 0)
            {
                toolbarUI.OnToolbarSlotClicked(slotIndex);
            }
        }
        else
        {
            // Inventory slot: move/swap item
            if (inventoryUI != null && slotIndex >= 0)
            {
                inventoryUI.OnSlotClicked(slotIndex);
            }
        }
    }
}