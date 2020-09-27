/*
    DESCRIPTION: data model for town


    DATE        USER        ACTION
    26.04.2020  SH          Created
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Town data")]
public class TownDataModel : ScriptableObject
{
    [SerializeField] private string townName;
    public string TownName { get => townName; }
    public enum Towntype { Governor, Collonial, Hideout, Fort };
    [SerializeField] private Towntype townType;
    public Towntype TownType { get => townType; }
    public enum Townnation { Spain, England, France, Netherland };
    [SerializeField] private Townnation townNation;
    public Townnation TownNation { get => townNation; }

    [Header("Size")]

    [SerializeField] private string defaultCitizens;
    public string DefaultCitizens { get => defaultCitizens; }
    [SerializeField] private string defaultSoldiers;
    public string DefaultSoldiers { get => defaultSoldiers; }
    [SerializeField] private string defaultCannons;
    public string DefaultCannons { get => defaultCannons; }
    [SerializeField] private List<ItemDataModel> productionList;
    public List<ItemDataModel> ProductionList { get => productionList; }

}
