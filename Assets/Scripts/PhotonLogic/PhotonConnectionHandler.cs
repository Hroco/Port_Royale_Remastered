/*
    DESCRIPTION: class implements connection to photon services and multiplayer handling
                 not lobby handling.

    DATE        USER        ACTION
    19.04.2020  PM          Created
    25.04.2020  SH          Merged input fields to create and join lobby
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonConnectionHandler : MonoBehaviour
{
    public string gameVersion = "0.1";

    [SerializeField]
    private GameObject photonDisconnected, photonConnecting, photonCnonnected, connecting, buttonsAndInputs, multiplayerPlaceHolder, lobbyPlaceHolder, mainMenu, lobbyBG;
    [SerializeField]
    private Text inputPlayerName, inputJoinCreateText;
    [SerializeField]
    private Button createButton, joinButton;
    [SerializeField]
    private bool startup;

    private bool isHost = false;

    //setup a singleton for photonConnectionHandler
    private static PhotonConnectionHandler _photonConnectionHandler;
    public static PhotonConnectionHandler Instance { get { return _photonConnectionHandler; } }

    private void Awake()
    {
        if (_photonConnectionHandler != null && _photonConnectionHandler != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _photonConnectionHandler = this;
        }
    }

    private void Start()
    {
        if (startup)
        {
            mainMenu.SetActive(true);
            multiplayerPlaceHolder.SetActive(false);
            lobbyPlaceHolder.SetActive(false);
            lobbyBG.SetActive(false);
            photonDisconnected.SetActive(true);
            photonConnecting.SetActive(false);
            photonCnonnected.SetActive(false);
        }
    }

    private void Update()
    {
        //set buttons interactable based on filled text in the input                                <------------SH 25.04.2020--------------
        if (inputJoinCreateText.text != "" && inputPlayerName.text != "")
        {
            createButton.interactable = true;
            joinButton.interactable = true;
        }else
        {
            createButton.interactable = false;
            joinButton.interactable = false;
        }
           

            
    }

    //connects to photon services
    public void ConnectToPhoton()
    {
        photonDisconnected.SetActive(false);
        photonConnecting.SetActive(true);
        photonCnonnected.SetActive(false);

        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    //callback on ConnectToPhoton -> success
    private void OnConnectedToMaster()
    {
        photonDisconnected.SetActive(false);
        photonConnecting.SetActive(false);
        photonCnonnected.SetActive(true);

        buttonsAndInputs.SetActive(true);
    }

    //join room with room name
    public void JoinRoom()
    {
        PhotonNetwork.playerName = inputPlayerName.text;
        isHost = false;
        PhotonNetwork.JoinRoom(inputJoinCreateText.text);

        buttonsAndInputs.SetActive(false);
        connecting.SetActive(true);
    }

    //create room with room name
    public void CreateRoom()
    {
        PhotonNetwork.playerName = inputPlayerName.text;
        isHost = true;
        PhotonNetwork.CreateRoom(inputJoinCreateText.text);

        buttonsAndInputs.SetActive(false);
        connecting.SetActive(true);
    }

    //callback on join room -> failed
    private void OnPhotonJoinRoomFailed()
    {
        buttonsAndInputs.SetActive(true);
        connecting.SetActive(false);
        StartCoroutine(LobbyHandler.Instance.sendWarningText("Failed to join a room!"));
    }

    //callback on create room -> failed
    private void OnPhotonCreateRoomFailed()
    {
        buttonsAndInputs.SetActive(true);
        connecting.SetActive(false);
        isHost = false;
        StartCoroutine(LobbyHandler.Instance.sendWarningText("Failed to create a room!"));
    }

    //callback on join room and create room -> success
    private void OnJoinedRoom()
    {
        connecting.SetActive(false);
        multiplayerPlaceHolder.SetActive(false);
        lobbyPlaceHolder.SetActive(true);       
        LobbyHandler.Instance.setHost(isHost);
    }

    private void OnLeftRoom()
    {
        lobbyPlaceHolder.SetActive(false);
        multiplayerPlaceHolder.SetActive(true);
    }

    
}
