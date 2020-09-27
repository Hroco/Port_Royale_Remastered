/*
    DESCRIPTION: handles convoy data. Direct communication with individual ships
                 as well as convoy object. 

    DATE        USER        ACTION
    27.04.2020  PM          Created
    30.04.2020  PM          Added getter for highest ship tier
    01.05.2020  SH          Added convoy owner getter and setter for id
    02.05.2020  PM          Added convoy type and convoy nation including getters
    03.05.2020  SH          Added game object variable for convoy location aswell get/set methods for that variable
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConvoyData
{


    [SerializeField] private int convoyId;
    [SerializeField] private Convoy convoy;

    [SerializeField] private string convoyOwner;
    [SerializeField] private string convoyName;
    [SerializeField] private string nation;
    [SerializeField] private string convoyType;

    [SerializeField] private GameObject convoyLocation;
    [SerializeField] private GameObject convoyGameObject;

    [SerializeField] private List<Ship> convoyShips = new List<Ship>();
    [SerializeField] private List<Item> itemList;



    public ConvoyData(string _convoyOwner, string _convoyName, List<Ship> _convoyShips)
    {
        convoyOwner = _convoyOwner;
        convoyName = _convoyName;
        convoyShips = _convoyShips;       
    }

    public void setConvoy(Convoy c)
    {
        convoy = c;
    }


//----------------------------------------Adders add to convoy -->returns leftover<-- ------------------------------

    //Crew
    public int addCrew(int value)
    {
        int leftover = value;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            leftover -= convoyShips[i].addCurrentCrew(value);

            if (leftover == 0)
            {
                convoy.UpdateClients();
                return 0;
            }

        }

        convoy.UpdateClients();
        return leftover;     
    }

    //Cannons
    public int addCannons(int value)
    {
        int leftover = value;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            leftover -= convoyShips[i].addCurrentCannons(value);

            if (leftover == 0)
            {
                convoy.UpdateClients();
                return 0;
            }
                
        }

        convoy.UpdateClients();
        return leftover;
    }

    //Hold
    public int addHold(int value)
    {
        int leftover = value;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            leftover -= convoyShips[i].addCurrentHold(value);

            if (leftover == 0)
            {
                convoy.UpdateClients();
                return 0;
            }
                
        }

        convoy.UpdateClients();
        return leftover;
    }

    //Health
    public int addHealth(int value)
    {
        int leftover = value;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            leftover -= convoyShips[i].addHealth(value);

            if (leftover == 0)
                return 0;
        }

        return leftover;
    }
//------------------------------------------------------------------------------------------------------------------
    
//--------------------------------------Removers remove from convoy -->returns how much was removed<-- -------------
    //Crew
    public int removeCrew(int value)
    {
        int wasRemoved = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            wasRemoved += convoyShips[i].removeCurrentCrew(value);
            value -= wasRemoved;

            if (wasRemoved == value)
                return value;
        }

        return wasRemoved;
    }

    //Cannons
    public int removeCannons(int value)
    {
        int wasRemoved = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            wasRemoved += convoyShips[i].removeCurrentCannons(value);
            value -= wasRemoved;

            if (wasRemoved == value)
                return value;
        }

        return wasRemoved;
    }

    //Hold
    public int removeHold(int value)
    {
        int wasRemoved = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            wasRemoved += convoyShips[i].removeCurrentHold(value);
            value -= wasRemoved;

            if (wasRemoved == value)
                return value;
        }

        return wasRemoved;
    }
    //------------------------------------------------------------------------------------------------------------------

    //---------------------------------------Getters for max and current data---------------------------------------
    public GameObject getConvoyGameObject()
    {
        return convoyGameObject;
    }
    public int getConvoyMaxCrew()
    {
        int maxCrew = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            maxCrew += convoyShips[i].getShipMaxCrew();
        }

        return maxCrew;
    }

    public int getConvoyCurrentCrew()
    {
        int currentCrew = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            currentCrew += convoyShips[i].getShipCurrentCrew();
        }

        return currentCrew;
    }

    public int getConvoyMaxCannons()
    {
        int maxCannons = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            maxCannons += convoyShips[i].getShipMaxCannons();
        }

        return maxCannons;
    }

    public int getConvoyCurrentCannons()
    {
        int currentCannons = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            currentCannons += convoyShips[i].getShipCurrentCannons();
        }

        return currentCannons;
    }

    public int getConvoyMaxHold()
    {
        int maxHold = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            maxHold += convoyShips[i].getShipMaxHold();
        }

        return maxHold;
    }

    public int getConvoyCurrentHold()
    {
        int currentHold = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            currentHold += convoyShips[i].getShipCurrentHold();
        }

        return currentHold;
    }

    public int getConvoyMaxHealth()
    {
        int maxHealth = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            maxHealth += convoyShips[i].getShipMaxHealth();
        }

        return maxHealth;
    }

    public int getConvoyCurrentHealth()
    {
        int currentHealth = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            currentHealth += convoyShips[i].getShipCurrentHealth();
        }

        return currentHealth;
    }

    public List<Ship> getAllShips()
    {
        return convoyShips;
    }

    public Ship getHighestTierShip()
    {
        int highestTier = 0;
        int index = 0;

        for (int i = 0; i < convoyShips.Count; i++)
        {
            if (convoyShips[i].getShipData().ShipTier > highestTier)
            {
                highestTier = convoyShips[i].getShipData().ShipTier;
                index = i;
            }
                
        }
        return convoyShips[index];
    }                               // 30.04.2020 PM

    public int getConvoyId()
    {
        return convoyId;
    }

    public string getConvoyNation()
    {
        return nation;
    }

    public string getConvoyType()
    {
        return convoyType;
    }

    public string getConvoyName()
    {
        return convoyName;
    }
    public GameObject getConvoyLocation()
    {
        return convoyLocation;
    }

    public string getConvoyOwner()
    {
        return convoyOwner;
    }

    public Convoy getConvoy()
    {
        return convoy;
    }

    public Item getConvoyStorage(int i)
    {
        return itemList[i];
    }

    //-------------------------------------------------------------------------------------------------------------

    //--------------------------------------------------------Setters-----------------------------------------------

    public void setConvoyId(int id)
    {
        convoyId = id;
    }
    public void setConvoyLocation(GameObject value)
    {
        convoyLocation = value;
    }
}
