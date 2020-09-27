/*
    DESCRIPTION: handles individual ship data. Ships are located in convoyData in List.


    DATE        USER        ACTION
    27.04.2020  PM          Created
    30.04.2020  PM          Added getter for ship data
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship
{
    [SerializeField]
    private ShipDataModel shipData;

    [SerializeField] private int currentHold;
    [SerializeField] private int currentCannons;
    [SerializeField] private int currentCrew;
    [SerializeField] private int currentHealth;

    public void UpdateClient(int crew, int cannons, int hold)
    {
        currentCrew = crew;
        currentCannons = cannons;
        currentHold = hold;
    }





//------------------------------------------------Removers for current values -->returns how much was removed<-- ------------------

    //Crew
    public int removeCurrentCrew(int value)
    {
        if (currentCrew - value >= 0)
        {
            currentCrew -= value;
            return value;
        }
        else
        {
            int difference = currentCrew;

            currentCrew = 0;

            return difference;
        }
    }

    //Cannons
    public int removeCurrentCannons(int value)
    {
        if (currentCannons - value >= 0)
        {
            currentCannons -= value;
            return value;
        }
        else
        {
            int difference = currentCannons;

            currentCannons = 0;

            return difference;
        }
    }

    //Hold
    public int removeCurrentHold(int value)
    {
        if (currentHold - value >= 0)
        {
            currentHold -= value;
            return value;
        }
        else
        {
            int difference = currentHold;

            currentHold = 0;

            return difference;
        }
    }
//---------------------------------------------------------------------------------------------------------------------------------

//---------------------------------------------------Adders for current values -->returns leftover<-- -----------------------------

    //Adding crew
    public int addCurrentCrew(int value)
    {
        if (currentCrew + value <= shipData.ShipMaxCrew)
        {
            currentCrew += value;
            return 0;
        }
        else
        {
            int difference = shipData.ShipMaxCrew - currentCrew;

            currentCrew = shipData.ShipMaxCrew;
            return value - difference;
        }

        
    }

    //Adding Cannons
    public int addCurrentCannons(int value)
    {
        if (currentCannons + value <= shipData.ShipMaxCannons)
        {
            currentCannons += value;
            return 0;
        }
        else
        {
            int difference = shipData.ShipMaxCannons - currentCannons;

            currentCannons = shipData.ShipMaxCannons;
            return value - difference;
        }
    }

    //Adding Hold
    public int addCurrentHold(int value)
    {
        if (currentHold + value <= shipData.ShipMaxHold)
        {
            currentHold += value;
            return 0;
        }
        else
        {
            int difference = shipData.ShipMaxHold - currentHold;

            currentHold = shipData.ShipMaxHold;
            return value - difference;
        }
    }

    //Adding Health
    public int addHealth(int value)
    {
        if (currentHealth + value <= shipData.ShipMaxHealth)
        {
            currentHealth += value;
            return 0;
        }
        else
        {
            int difference = shipData.ShipMaxHealth - currentHealth;

            currentHealth = shipData.ShipMaxHealth;
            return value - difference;
        }
    }
//--------------------------------------------------------------------------------------------------------------------------------


//------------------------------------------------------Getters for max and current values----------------------------
    public int getShipMaxCrew()
    {
        return shipData.ShipMaxCrew;
    }

    public int getShipCurrentCrew()
    {
        return currentCrew;

    }

    public int getShipMaxCannons()
    {
        return shipData.ShipMaxCannons;
    }

    public int getShipCurrentCannons()
    {
        return currentCannons;
    }

    public int getShipMaxHold()
    {
        return shipData.ShipMaxHold;
    }

    public int getShipCurrentHold()
    {
        return currentHold;
    }

    public int getShipMaxHealth()
    {
        return shipData.ShipMaxHealth;
    }

    public int getShipCurrentHealth()
    {
        return currentHealth;
    }

    public ShipDataModel getShipData()
    {
        return shipData;
    }           // 30.04.2020 PM
//-------------------------------------------------------------------------------------------------------------------
}
