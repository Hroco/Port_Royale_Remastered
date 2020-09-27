/*
    DESCRIPTION: data model for ship


    DATE        USER        ACTION
    26.04.2020  PM          Created
    29.04.2020  PM          Added list of materials and sprite
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Data", menuName = "Ship data")]
public class ShipDataModel : ScriptableObject
{
    [SerializeField] private string shipName;
    public string ShipName { get => shipName; }
    [SerializeField] private int shipTier;
    public int ShipTier { get => shipTier; }
    [SerializeField] private int maxSpeed;
    public int MaxSpeed { get => maxSpeed; }

    [SerializeField] private int shipMaxHold;
    public int ShipMaxHold { get => shipMaxHold; }
    [SerializeField] private int shipMaxCannons;
    public int ShipMaxCannons { get => shipMaxCannons; }
    [SerializeField] private int shipMaxHealth;
    public int ShipMaxHealth { get => ShipMaxHealth; }
    [SerializeField] private int shipMaxCrew;
    public int ShipMaxCrew { get => shipMaxCrew; }

    [SerializeField] private List<Sprite> shipRotations = new List<Sprite>();                   // 29.04.2020 PM
    [SerializeField] private Sprite shipImage;
    public Sprite ShipImage { get => shipImage; }

    public Sprite getShipRotation(int index)
    {
        return shipRotations[index];
    }
}
