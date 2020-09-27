/*
    DESCRIPTION: class implements lobby functionality like connecting, disconnecting
                 setting ready status, keeping info about players and updating it.                   

    DATE        USER        ACTION
    19.04.2020  PM          Created
    25.04.2020  SH          Added color changing to local player
    25.04.2020  SH          Added statement that allow host start game only when all others players are ready
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//instance of a player in lobby
[System.Serializable]
public class PlayerInstance
{
    [SerializeField]
    private int ID;
    [SerializeField]
    private string playerName;
    [SerializeField]
    private bool isReady;
    [SerializeField]
    private LobbyPlayer lobbyPlayer;

    //constructor
    public PlayerInstance(int _id, string _playerName, bool _isReady, LobbyPlayer _lobbyPlayer)
    {
        ID = _id;
        playerName = _playerName;
        isReady = _isReady;
        lobbyPlayer = _lobbyPlayer;

        lobbyPlayer.SetDefault(playerName, ID, isReady);
    }

    //sets ready status
    public void SetReady(bool status)
    {
        isReady = status;
        lobbyPlayer.setReadyStatus(isReady);
    }
        
    //get ready status
    public bool getReadyStatus()
    {
        isReady = lobbyPlayer.getReayStatus();
        return isReady;
    }

    //USE ONLY FOR REINDEXING AFTER PLAYER DISCONNECT!!
    public void setNewId(int id)
    {
        ID = id;
        lobbyPlayer.setNewId(ID);
    }
}


public class LobbyHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject lobbyPlayerPrefab, lobbyPlayerParent, startGameButton, leaveButton;
    [SerializeField]
    private Text warningText;
    [SerializeField]
    private List<PlayerInstance> players = new List<PlayerInstance>();

    [SerializeField]
    private int localID;
    [SerializeField]
    private bool isHost;

    private PhotonView photonView;


    //setup a singleton for lobbyhandler
    private static LobbyHandler _lobbyHandler;
    public static LobbyHandler Instance { get { return _lobbyHandler; } }

    private void Awake()
    {
        if (_lobbyHandler != null && _lobbyHandler != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _lobbyHandler = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.automaticallySyncScene = true;
    }

    //called if player connects to created room
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(lobbyPlayerPrefab, lobbyPlayerParent.transform);
        PlayerInstance p = new PlayerInstance(PhotonNetwork.playerList.Length - 1, player.NickName, false, obj.GetComponent<LobbyPlayer>());
        obj.GetComponent<LobbyPlayer>().setPlayerInstance(p);
        obj.GetComponent<LobbyPlayer>().DisableToggle();
        players.Add(p);       
    }

    //called after click on create or join room buttons
    public void setHost(bool status)           
    {
        isHost = status;
        leaveButton.SetActive(true);

        if (isHost)
        {
            localID = 0;
            GameObject obj = Instantiate(lobbyPlayerPrefab, lobbyPlayerParent.transform);
            PlayerInstance p = new PlayerInstance(0, PhotonNetwork.player.NickName, false, obj.GetComponent<LobbyPlayer>());
            obj.GetComponent<LobbyPlayer>().setPlayerInstance(p);
            obj.GetComponent<LobbyPlayer>().setLocalColor();                    //<----------SH 25.04.2020---------------
            players.Add(p);
            startGameButton.SetActive(true);
        }
        else
        {
            localID = PhotonNetwork.playerList.Length - 1;
            startGameButton.SetActive(false);
            OnJoinCreatePlayerList();
        }
    }

    //if player joined existing room create list of playerdata already present in the room
    public void OnJoinCreatePlayerList()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab, lobbyPlayerParent.transform);
            PlayerInstance p = new PlayerInstance(i, PhotonNetwork.playerList[i].NickName, false, obj.GetComponent<LobbyPlayer>());
            obj.GetComponent<LobbyPlayer>().setPlayerInstance(p);

            if (i != PhotonNetwork.playerList.Length - 1)
            {
                obj.GetComponent<LobbyPlayer>().DisableToggle();
            }

            if (i == PhotonNetwork.playerList.Length - 1)
            {
                obj.GetComponent<LobbyPlayer>().setLocalColor();
            }

            players.Add(p);
        }
    }

    //call synchronization on ready status
    public void SetReady(bool status)
    {
        players[localID].SetReady(status);
        photonView.RPC("SynchPlayerReady", PhotonTargets.All, localID, status);
    }

    //leave current sroom, synch with others and perform cleanup of data
    public void LeaveRoom()
    {
        if(isHost)
            photonView.RPC("HostLeft", PhotonTargets.All);
        else
            photonView.RPC("SynchLeaveRoom", PhotonTargets.All, localID);

        PhotonNetwork.LeaveRoom();
        startGameButton.SetActive(false);
        leaveButton.SetActive(false);

        PerformCleanup();
    }

    //cleanup
    private void PerformCleanup()
    {
        for (int i = 0; i < lobbyPlayerParent.transform.childCount; i++)
        {
            Destroy(lobbyPlayerParent.transform.GetChild(i).gameObject);
        }

        players.Clear();
    }

    //called via start game button
    public void StartGame()                             //<---------------SH 25.04.2020-----------------
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].getReadyStatus())
                return;
        }
        startGameButton.SetActive(false);
        PhotonNetwork.LoadLevelAsync(1);
    }

    public IEnumerator sendWarningText(string text)
    {
        warningText.text = text;
        yield return new WaitForSeconds(3f);
        warningText.text = " ";
    }

//----------------------------RPCs----------------------------------------------------------------
    

    //synch player ready status
    [PunRPC]
    private void SynchPlayerReady(int id, bool status)
    {
        players[id].SetReady(status);
    }
    
    //synch id and gfx output if player leaves room
    [PunRPC]
    private void SynchLeaveRoom(int id)
    {
        players.RemoveAt(id);

        //reset id after dc
        for (int i = 0; i < players.Count; i++)
        {
            players[i].setNewId(i);
        }

        Destroy(lobbyPlayerParent.transform.GetChild(id).gameObject);
    }

    //host left logic
    [PunRPC]
    private void HostLeft()
    {
        PhotonNetwork.LeaveRoom();
        startGameButton.SetActive(false);
        leaveButton.SetActive(false);

        if (!isHost)
            StartCoroutine(sendWarningText("Host left the room!"));

        PerformCleanup();
    }
}
