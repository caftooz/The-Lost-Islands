using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

    public enum ItemType { Default, Food, Weapon, Instrument}

public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    
    public GameObject itemPrefab;

    public string itemName;
    public string itemDescription;
    
    public int maxAmount;
    
    public Sprite icon;

    public bool isConsumeable;

    [Header("Consumeable Characteristics")]
    public float changeHealth;
    public float changeSatiety;
}
