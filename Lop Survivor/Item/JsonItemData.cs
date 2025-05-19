using Lop.Survivor.inventroy.Item;
using System;
using UnityEngine;

[Serializable]
public class JsonItemData
{
    public string itemName;           
    public ItemType itemType;         
    public int itemTier;              
    public int order;                 
    public bool isDurability;
    public float maxDurability;       
    public float healingValue;        
    public float temperatureRecovery; 
    public bool canMerge;             
    public int maxMerge;              
    public bool istemperatureDecrease;

    public string groundPrefabName;
    public string uiPrefabName;

    public void CopyItemData(ItemData itemData)
    {
        itemData.itemName = itemName;
        itemData.itemType = itemType;
        itemData.itemTier = itemTier;
        itemData.order = order;
        itemData.isDurability = isDurability;
        itemData.maxDurability = maxDurability;
        itemData.healingValue = healingValue;
        itemData.temperatureRecovery = temperatureRecovery;
        itemData.canMerge = canMerge;
        itemData.maxMerge = maxMerge;
        itemData.istemperatureDecrease = istemperatureDecrease;
    }
}
