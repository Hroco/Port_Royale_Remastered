/*
    DESCRIPTION: class implements graphic output for player information in lobby


    DATE        USER        ACTION
    19.04.2020  PM          Created
    25.04.2020  SH          Added setLocalColor method
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText, playerIdText;
    [SerializeField]
    private Toggle isReadyToggle;

    private PlayerInstance playerInstance;

    private bool scriptedChange = true;

    //set default values
    public void SetDefault(string name, int id, bool ready)
    {
        playerNameText.text = name;
        playerIdText.text = id.ToString();
        isReadyToggle.isOn = ready;
    }

    //returns toggle ready
    public bool getReayStatus()
    {
        return isReadyToggle.isOn;
    }

    //triggered locally by toggle as input
    public void sendReadyInfoToLobby()
    {
        if (!scriptedChange)
            LobbyHandler.Instance.SetReady(isReadyToggle.isOn);
        scriptedChange = false;
    }

    //triggered by server
    public void setReadyStatus(bool status)
    {
        scriptedChange = true;
        isReadyToggle.isOn = status;
    }

    //can be usefull in future now this doesnt implement anything further
    public void setPlayerInstance(PlayerInstance p)
    {
        playerInstance = p;
    }

    //disable toggle button
    public void DisableToggle()
    {
        isReadyToggle.interactable = false;
    }
    public void setLocalColor()                                                         //<----------SH 25.04.2020---------------
    {
        GetComponent<Image>().color = new Color32(100, 100, 100, 100); ;
    }


    //USE ONLY FOR REINDEXING
    public void setNewId(int id)
    {
        playerIdText.text = id.ToString();
    }
}
