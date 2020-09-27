/*
    DESCRIPTION: data model for item


    DATE        USER        ACTION
    26.04.2020  SH          Created
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Item data")]

public class ItemDataModel : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int itemID;
    public string ItemName { get => itemName; }
    [SerializeField] private string minSellingPrice;
    public string MinSellingPrice { get => minSellingPrice; }
    [SerializeField] private string maxSellingPrice;
    public string MaxSellingPrice { get => maxSellingPrice; }
    [SerializeField] private string minBuyingPrice;
    public string MinBuyingPrice { get => minBuyingPrice; }
    [SerializeField] private string maxBuyingPrice;
    public string MaxBuyingPrice { get => maxBuyingPrice; }
    [SerializeField] private Sprite itemIcon;
    public Sprite ItemIcon { get => itemIcon; }
}