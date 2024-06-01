using DarkHorizon;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    // ќбъект у которого дети €вл€ютс€ слотами
    public Transform quickslotParent;

    public int currentQuickslotID = 0;

    public Sprite selectedSprite;
    public Sprite notSelectedSprite;

    private Transform player;

    private void Start()
    {
        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID >= quickslotParent.childCount - 1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                currentQuickslotID++;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;

        }

        if (mw < -0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount - 1;
            }
            else
            {
                currentQuickslotID--;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        }

        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (currentQuickslotID == i)
                {
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    }
                }
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    currentQuickslotID = i;
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
            {
                if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.isConsumeable && !InventoryManager.inventoryIsOpened && quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    ChangeCharacteristics();

                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount <= 1)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount--;
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().itemAmountText.text = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount.ToString();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
            {
                if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount > 0)
                {
                    GameObject itemObject = Instantiate(quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                    itemObject.GetComponent<Item>().amount = 1;
                    switch (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount)
                    {
                        case 1:
                            quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount--;
                            quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                            break;
                        case 2:
                            quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount--;
                            quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().itemAmountText.text = "";
                            break;
                        case > 2:
                            quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount--;
                            quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().itemAmountText.text = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount.ToString();
                            break;
                    }
                }
            }
        }
    }
    
    private void ChangeCharacteristics()
    {
        if (Player.currentHealth + quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeHealth <= 100)
        {
            Player.currentHealth += quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeHealth;
        }
        else
        {
            Player.currentHealth = 100;
        }

        if (Player.currentSatiety + quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeSatiety <= 100)
        {
            Player.currentSatiety += quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeSatiety;
        }
        else
        {
            Player.currentSatiety = 100;
        }
    }
}