/*
    DESCRIPTION: handles convoy data. Direct communication with individual ships
                 as well as convoy object. 

    DATE        USER        ACTION
    02.05.2020  SH          Created
    03.05.2020  PM          Added logic for convoy ships hud

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private ConvoyData convoyData;
    [SerializeField] private GameObject convoyShipsHUD;
    [SerializeField] private GameObject HUDSelectedConvoy;
    [SerializeField] private GameObject HUDTownWarehouse;
    [SerializeField] private GameObject HUDWarehouseLeft;
    [SerializeField] private GameObject HUDWarehouseRight;
    [SerializeField] private GameObject activeCity;
    [SerializeField] private Text convoyNameTxt, destinationTxt, speedTxt, cannonsTxt, crewTxt;
    [SerializeField] private Image healthImg, holdImg;


    //setup a singleton for HUDManager
    private static HUDManager _HUDManager;
    public static HUDManager Instance { get { return _HUDManager; } }

    private void Awake()
    {
        if (_HUDManager != null && _HUDManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _HUDManager = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (convoyData == null || convoyData.getConvoyId() < 1)
        {
            HUDSelectedConvoy.SetActive(false);
        }
        else
        {
            HUDSelectedConvoy.SetActive(true);
        }
    }

    //
    public void itemButtonLeft(int itemID)
    {
        activeCity.GetComponent<TownManager>().moveGoods(itemID, true);
    }

    public void itemButtonRight(int itemID)
    {
        activeCity.GetComponent<TownManager>().moveGoods(itemID, false);
    }
    //remove sellected convoy from HUD
    public void removeSelectedConvoyHUD()
    {
        convoyData = null;
    }

    //Write ConvoyData informations like Name, Speed etc. to Convoy HUD
    private void parseConvoyData()                                      //    03.05.2020  PM   
    {
        if (convoyData != null)
        {
            List<Ship> convoyShips = convoyData.getAllShips();

            convoyNameTxt.text = convoyData.getConvoyName();
            speedTxt.text = "0";
            destinationTxt.text = "Not implemented";
            cannonsTxt.text = convoyData.getConvoyCurrentCannons().ToString();
            crewTxt.text = convoyData.getConvoyCurrentCrew().ToString();

            for (int i = 0; i < convoyShipsHUD.transform.childCount; i++)
            {
                convoyShipsHUD.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < convoyShips.Count; i++)
            {
                convoyShipsHUD.transform.GetChild(i).gameObject.SetActive(true);
                convoyShipsHUD.transform.GetChild(i).GetComponent<Image>().sprite = convoyShips[i].getShipData().ShipImage;
            }
        }
        else
        {
            convoyNameTxt.text = "";
            speedTxt.text = "";
            destinationTxt.text = "";
            cannonsTxt.text = "";
            crewTxt.text = "";

            for (int i = 0; i < convoyShipsHUD.transform.childCount; i++)
            {
                convoyShipsHUD.transform.GetChild(i).gameObject.SetActive(false);
            }
        }       
    }

    //enable/disable TownWarehouse HUD
    public void activateTownWarehouseHUD(bool value)
    {
        HUDTownWarehouse.SetActive(value);
    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Getters for max and current data-------------------------------------

    // ConvoyData getter
    public ConvoyData GetConvoyData()
    {
        return convoyData;
    }

    // Get GameObject ActiveCity
    public GameObject getActiveCity()
    {
        return activeCity;
    }

    // Get TownWareHouseLeft GameObject (Left side of Town Warehouse HUD)
    public GameObject getTownWarehouseLeft()
    {
        return HUDWarehouseLeft;
    }

    // Get TownWareHouseRight GameObject (Right side of Town Warehouse HUD)
    public GameObject getTownWarehouseRight()
    {
        return HUDWarehouseRight;
    }

    //-------------------------------------------------------------------------------------------------------------

    //--------------------------------------------------------Setters----------------------------------------------
    
    // Set GameObject ActiveCity
    public void setActiveCity(GameObject value)
    {
        activeCity = value;
    }

    // Set ConvoyData and write that data to HUD using method parseConvoyData
    public void setConvoyData(ConvoyData data)
    {
        convoyData = data;
        parseConvoyData();
    }
}
