/*
    DESCRIPTION: this class is used for town and convoy storage


    DATE        USER        ACTION
    02.05.2020  SH          Created

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemName {Brick, Cocoa, Coffee, Corn, Cotton, Dyes, Fruit, Garments, Hemp, Meat, Rope, Rum, Spices, Sugar, Tobacco, Tools, Vilagers, Wheat, Wine, Wood};
    [SerializeField] private ItemName itemName;
    [SerializeField] private ItemDataModel itemDataModel;
    public ItemName TownType { get => itemName; }
    [SerializeField] private int itemCurrentAmount;
    public int ItemCurrentAmount
    {
        get { return itemCurrentAmount; }
        set { itemCurrentAmount = value; }
    }
    //public int ItemCurrentAmount { get => itemCurrentAmount; }

}
