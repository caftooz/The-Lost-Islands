using System.Collections;
using System.Collections.Generic;
using DarkHorizon;
using UnityEngine;

public class InventoryManager : MonoBehaviour

{
    public GameObject UIBG;
    public GameObject main;

    public Player player;

    public Transform quickSlotPanel;
    public Transform inventoryPanel;
    public static bool inventoryIsOpened;
    public List<InventorySlot> slots = new List<InventorySlot>();
    private Camera mainCamera;
    public float reachDistance;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UIBG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        reachDistance = 5;
        mainCamera = Camera.main;
        for (int i = 0; i < quickSlotPanel.childCount; i++)
        {
            if (quickSlotPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(quickSlotPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        for(int i = 0; i < inventoryPanel.childCount; i++)
        {
            if(inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !MenuManager.isPaused)
        {
            InventoryAction();
        }

        

        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, reachDistance))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    player.PlaySound(1);
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void InventoryAction()
    {
        UIBG.SetActive(!inventoryIsOpened);
        inventoryPanel.gameObject.SetActive(!inventoryIsOpened);
        main.GetComponent<RotatePl>().enabled = inventoryIsOpened;
        Cursor.visible = !inventoryIsOpened;
        if (inventoryIsOpened)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        inventoryIsOpened = !inventoryIsOpened;
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maxAmount)
                { 
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
            }
        }
        
        foreach (InventorySlot slot in slots)
        {
            if(slot.isEmpty == true)
            { 
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if (slot.amount != 1)
                {
                    slot.itemAmountText.text = _amount.ToString();
                }
                break;
            }
        }
    }
}
