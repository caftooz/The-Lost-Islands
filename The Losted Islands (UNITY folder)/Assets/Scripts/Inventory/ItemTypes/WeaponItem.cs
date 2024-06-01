using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Items/New Weapon")]
public class WeaponItem : ItemScriptableObject
{
    public float damage;
    public float strenght;
    private void Start()
    {
        itemType = ItemType.Weapon;
    }
}
