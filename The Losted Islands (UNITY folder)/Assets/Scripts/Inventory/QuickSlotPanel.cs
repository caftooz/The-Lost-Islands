using DarkHorizon;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class QuickslotInventory : MonoBehaviour
{
    // ќбъект у которого дети €вл€ютс€ слотами
    public Transform quickslotParent;

    public int currentQuickslotID = 0;

    public Sprite selectedSprite;
    public Sprite notSelectedSprite;

    private Transform player;
    public Transform cam;

    public Transform hand;
    public float dropX = 200;
    public float dropY = 100;

    public GameObject rodFloat;
    private GameObject gameObjectRodFloat;

    public int rodFloatDistance = 1000;

    public bool isFishing = false;
    [SerializeField] GameObject _fishingPanel;

    private GameObject handItem;

    private void Start()
    {
        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw < -0.1)
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

        if (mw > 0.1)
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
                    GameObject itemObject = Instantiate(quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemPrefab, hand.position, quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemPrefab.transform.rotation);
                    itemObject.GetComponent<Rigidbody>().AddForce(player.forward * dropX + Vector3.up * dropY);
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
        if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount > 0)
            {
                if(hand.childCount == 0)
                {
                    handItem = Instantiate(quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemPrefab, hand);
                    handItem.transform.position = hand.position;
                    handItem.transform.localScale /= (100* 1.67f * 0.944f);
                    handItem.GetComponent<Rigidbody>().useGravity = false;
                    handItem.GetComponent<Rigidbody>().isKinematic = true;
                    handItem.GetComponent<Collider>().enabled = false;
                }
                else if (hand.GetChild(0).GetComponent<Item>().item != quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item)
                {
                    Destroy(hand.GetChild(0).gameObject);
                    handItem = Instantiate(quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemPrefab, hand);
                    handItem.transform.position = hand.position;
                    handItem.transform.localScale /= (100 * 1.67f * 0.944f);
                    handItem.GetComponent<Rigidbody>().useGravity = false;
                    handItem.GetComponent<Rigidbody>().isKinematic = true;
                    handItem.GetComponent<Collider>().enabled = false;
                }
            }
        } else if(hand.childCount >0) Destroy(hand.GetChild(0).gameObject);

        if(quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemName == "rod")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Fishing();
                }
                if (gameObjectRodFloat != null)
                {
                    handItem.GetComponent<LineRenderer>().SetPosition(0, handItem.transform.GetChild(3).position);
                    handItem.GetComponent<LineRenderer>().SetPosition(1, gameObjectRodFloat.transform.position);

                }
            }

        }
        if (gameObjectRodFloat != null)
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item == null)
            {
                isFishing = false;
                gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position = new Vector3(gameObjectRodFloat.GetComponent<Fishing>()._bluePositionX, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.y, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.z);
                gameObjectRodFloat.GetComponent<Fishing>()._red.transform.position = gameObjectRodFloat.GetComponent<Fishing>()._redPosition;
                Destroy(gameObjectRodFloat);
                _fishingPanel.SetActive(false);
            }
            else if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.itemName != "rod")
            {
                isFishing = false;
                gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position = new Vector3(gameObjectRodFloat.GetComponent<Fishing>()._bluePositionX, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.y, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.z);
                gameObjectRodFloat.GetComponent<Fishing>()._red.transform.position = gameObjectRodFloat.GetComponent<Fishing>()._redPosition;
                Destroy(gameObjectRodFloat);
                _fishingPanel.SetActive(false);
            }
            else if ((gameObjectRodFloat.transform.position - handItem.transform.GetChild(3).position).sqrMagnitude > rodFloatDistance)
            {
                isFishing = false;
                gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position = new Vector3(gameObjectRodFloat.GetComponent<Fishing>()._bluePositionX, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.y, gameObjectRodFloat.GetComponent<Fishing>()._blue.transform.position.z);
                gameObjectRodFloat.GetComponent<Fishing>()._red.transform.position = gameObjectRodFloat.GetComponent<Fishing>()._redPosition;
                Destroy(gameObjectRodFloat);
                handItem.GetComponent<LineRenderer>().enabled = false;
                _fishingPanel.SetActive(false);

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

    private void Fishing()
    {

        if (!isFishing)
        {
            gameObjectRodFloat = Instantiate(rodFloat, hand.position, Quaternion.identity);
            gameObjectRodFloat.GetComponent<Rigidbody>().AddForce(cam.forward * 2000 + Vector3.up * 500);
            gameObjectRodFloat.GetComponent<Fishing>()._fishingPanel = _fishingPanel;
            handItem.GetComponent<LineRenderer>().enabled = true;
            isFishing = true;
        }
        else
        {
            StopFishing();
        }

    }
    public void StopFishing()
    {

        if (!gameObjectRodFloat.GetComponent<Fishing>()._isMinigameNow)
        {
            if (isFishing == true)
            {
                isFishing = false;
                Destroy(gameObjectRodFloat);
                handItem.GetComponent<LineRenderer>().enabled = false;

            }

        }
          
    }
}