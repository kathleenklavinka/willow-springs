using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();
    [SerializeField] private Player player;

    private Slot_UI selectedSlot;
    private int selectedIndex = 0;

    private void Start()
    {
        // Auto find player kalo belum di-assign
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        
        if(toolbarSlots.Count == 0)
        {
            toolbarSlots.AddRange(GetComponentsInChildren<Slot_UI>());
            toolbarSlots.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }
        
        // Setup semua toolbar slots
        for (int i = 0; i < toolbarSlots.Count; i++)
        {
            if (toolbarSlots[i] != null)
            {
                toolbarSlots[i].slotIndex = i;
                toolbarSlots[i].toolbarUI = this; // Reference ke toolbar
                toolbarSlots[i].isToolbarSlot = true;
                toolbarSlots[i].SetHighlighted(false);
                toolbarSlots[i].SetEmpty();
            }
        }
        
        SelectSlot(0);
    }

    private void Update()
    {
        // PC keyboard support
        CheckAlphaNumericKeys();
        Refresh();
    }

    public void Refresh()
    {
        if (player == null || player.inventory == null) return;

        // Toolbar nampilin 7 slot pertama
        int slotsToShow = Mathf.Min(7, toolbarSlots.Count);
        
        for (int i = 0; i < slotsToShow; i++)
        {
            if (toolbarSlots[i] == null) continue;
            
            if (i < player.inventory.slots.Count)
            {
                toolbarSlots[i].SetItem(player.inventory.slots[i]);
            }
            else
            {
                toolbarSlots[i].SetEmpty();
            }
        }
    }

    public void SelectSlot(int index)
    {
        if(index < 0 || index >= toolbarSlots.Count) return;
        
        // Matiin highlight yang lama
        if(selectedSlot != null && selectedSlot.Highlight != null)
        {
            selectedSlot.SetHighlighted(false);
        }
        
        selectedIndex = index;
        selectedSlot = toolbarSlots[index];
        
        // Nyalain highlight yang baru
        if (selectedSlot != null && selectedSlot.Highlight != null)
        {
            selectedSlot.SetHighlighted(true);
        }
        
        Debug.Log("Selected toolbar slot: " + index);
    }
    
    // Dipanggil dari Slot_UI ketika di-tap (mobile)
    public void OnToolbarSlotClicked(int slotIndex)
    {
        SelectSlot(slotIndex);
    }

    public int GetSelectedSlotIndex()
    {
        return selectedIndex;
    }

    private void CheckAlphaNumericKeys()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        else if(Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        else if(Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        else if(Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        else if(Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        else if(Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
        else if(Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
    }
}