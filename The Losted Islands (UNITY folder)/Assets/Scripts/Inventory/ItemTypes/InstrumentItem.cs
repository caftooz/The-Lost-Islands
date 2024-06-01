using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instrument", menuName = "Inventory/Items/New Instrument")]
public class 
    InstrumentItem : ItemScriptableObject
{
    public float strenght;
    private void Start()
    {
        itemType = ItemType.Instrument;
    }
}
