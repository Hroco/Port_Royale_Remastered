/*
    DESCRIPTION: class handles town data , changing town icons, etc.


    DATE        USER        ACTION
    01.05.2020  SH          Created
    03.05.2020  SH          Added methods for selecting ships from otside of town

     TODO:   -create method that will handle town name text color

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownManager : MonoBehaviour
{

    [SerializeField] private int townId = 0;

    [SerializeField] private TownDataModel townDataModel;

    [SerializeField] private string townName;
    [SerializeField] private string townNation;
    [SerializeField] private string townType;

    [SerializeField] private int townSize;
    [SerializeField] private int townCurrentCitizens;
    [SerializeField] private int townCurrentCannons;
    [SerializeField] private int townCurrentSoldiers;
    [SerializeField] private int j = 0;

    [SerializeField] private TMP_Text townNameText;
    [SerializeField] private TMP_Text convoyCountText;

    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject convoyCountButton;
    [SerializeField] private GameObject townGameObject;

    [SerializeField] private Image townFlag;

    [SerializeField] private List<ConvoyData> convoyList;
    [SerializeField] private List<ConvoyData> convoyPlayerList;
    [SerializeField] private List<Item> itemList;
    [SerializeField] private List<Item> playerItemList;

    [SerializeField] private bool townHighlighted = false;

    void Start()
    {
        townId = GameManager.Instance.askForTownId();
        townName = townDataModel.TownName;
        townNameText.text = townName;
        townType = townDataModel.TownType.ToString();
        townNation = townDataModel.TownNation.ToString();
        townCurrentCitizens = int.Parse(townDataModel.DefaultCitizens);
        townCurrentCannons = int.Parse(townDataModel.DefaultCannons);
        townCurrentSoldiers = int.Parse(townDataModel.DefaultSoldiers);
        if (townType == "Collonial")
        {
            townNameText.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HUDManager.Instance.GetConvoyData().getConvoyId());
        showPlayerConnvoy();
        townFlag.sprite = GameManager.Instance.getFlag(townNation);
        setTownIcon(townHighlighted);
        removeConvoyFromTownList(PlayerController.Instance.getRemovedConvoyID());
        if (HUDManager.Instance.getActiveCity() != null && HUDManager.Instance.getActiveCity().GetComponent<TownManager>().getTownId() == townId)
        {
            writeWarehouseValues();
        }
    }

    // method add new convoy to the convoy list and save current town game object into convoy
    public void addConvoy(ConvoyData value)
    {
        value.setConvoyLocation(townGameObject);        //<------------------03.05.2020 SH----------------------------
        convoyList.Add(value);
    }

    // this method will put every convoy that belongs to player into new list 
    public void showPlayerConnvoy()
    {
        bool temp = true;
        convoyPlayerList.RemoveRange(0, convoyPlayerList.Count);
        if (convoyList.Count != 0)
        {
            for (int i = 0; i < convoyList.Count; i++)
            {
                if (convoyList[i].getConvoyOwner() == TestingProfile.Instance.getPlayerName())
                {
                    for (int j = 0; j < convoyPlayerList.Count; j++)
                    {
                        if (convoyPlayerList[j].getConvoyId() == convoyList[i].getConvoyId())
                            temp = false;
                    }
                    if (temp)
                        convoyPlayerList.Add(convoyList[i]);
                    temp = true;
                }
            }
        }
        if (convoyPlayerList != null && convoyPlayerList.Count != 0)
        {
            convoyCountButton.SetActive(true);
            convoyCountText.text = convoyPlayerList.Count.ToString();
        }
        else
        {
            convoyCountButton.SetActive(false);
        }

    }

    // this method handle selecting convoy from town by clicking on convoy count icon from map
    public void convoySelectingFromTownIcon()                   //<------------------03.05.2020 SH----------------------------
    {
        PlayerController.Instance.resetSelectedConvoy(PlayerController.Instance.getSelectedConvoy());
        if (j >= convoyPlayerList.Count)
        {
            j = 0;
        }
        HUDManager.Instance.setConvoyData(convoyPlayerList[j]);
        j++;
    }

    // remove convoy with specific ID from town convoy list
    public void removeConvoyFromTownList(int id)
    {
        if (id > 0)
        {
            for (int i = 0; i < convoyList.Count; i++)
            {
                if (convoyList[i].getConvoyId() == id)
                {
                    convoyList.RemoveAt(i);
                }
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Setters for max and current data-------------------------------------

    // set value of bool variable setTownHighlighted that is passed into setTownIcon method in update
    public void setTownHighlighted(bool value)
    {
        townHighlighted = value;
    }

    // change town icon to highlighted mode base on bool value
    public void setTownIcon(bool value)
    {
        if (value)
            GetComponent<MeshRenderer>().material = GameManager.Instance.getTownIcon(townCurrentCitizens, true);
        else
            GetComponent<MeshRenderer>().material = GameManager.Instance.getTownIcon(townCurrentCitizens, false);
        townHighlighted = false;
    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Getters for max and current data-------------------------------------

    // get spawner position
    public Transform getSpawnerPos()
    {
        return spawner.transform;
    }

    // get convoy data for convoy base on specific ID
    public ConvoyData getConvoyData(int id)
    {
        if (id > 0)
        {
            for (int i = 0; i < convoyList.Count; i++)
            {
                if (convoyList[i].getConvoyId() == id)
                {
                    return convoyList[i];
                }
            }
        }
        Debug.LogError("Imported value out of bounds!");
        return null;
    }

    // get town ID
    public int getTownId()
    {
        return townId;
    }
    
    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Town Economic--------------------------------------------------------

    //write Values of items in Town Warehouse List into HUD
    private void writeWarehouseValues()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (HUDManager.Instance.GetConvoyData() != null && HUDManager.Instance.GetConvoyData().getConvoyId() != 0)
            {
                HUDManager.Instance.getTownWarehouseLeft().transform.GetChild(i).GetComponent<TMP_Text>().text = itemList[i].ItemCurrentAmount.ToString();
                HUDManager.Instance.getTownWarehouseRight().transform.GetChild(i).GetComponent<TMP_Text>().text = HUDManager.Instance.GetConvoyData().getConvoyStorage(i).ItemCurrentAmount.ToString();
            }
                
        }
    }

    //move goods between warehouse bool variable represent side of HUD true is for left side, false is of right side
    public void moveGoods(int itemID,bool side)
    {
        if (side)
        {
            if (HUDManager.Instance.GetConvoyData() != null && HUDManager.Instance.GetConvoyData().getConvoyId() != 0)
                if (HUDManager.Instance.GetConvoyData().getConvoyStorage(itemID).ItemCurrentAmount != 0)
                {
                    itemList[itemID].ItemCurrentAmount += 1;
                    HUDManager.Instance.GetConvoyData().getConvoyStorage(itemID).ItemCurrentAmount -= 1;
                }
        }
        else 
        {
            if (itemList[itemID].ItemCurrentAmount != 0)
            {
                itemList[itemID].ItemCurrentAmount -= 1;
                if (HUDManager.Instance.GetConvoyData() != null && HUDManager.Instance.GetConvoyData().getConvoyId() != 0)
                    HUDManager.Instance.GetConvoyData().getConvoyStorage(itemID).ItemCurrentAmount += 1;
            } 
        }
    }

}