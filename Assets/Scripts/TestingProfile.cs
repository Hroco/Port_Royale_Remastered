using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingProfile : MonoBehaviour
{
    [SerializeField] private bool connect;
    [SerializeField] private string playerName;


    //setup a singleton for GameManager
    private static TestingProfile _testingProfile;
    public static TestingProfile Instance { get { return _testingProfile; } }

    private void Awake()
    {
        if (_testingProfile != null && _testingProfile != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _testingProfile = this;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.player.NickName != null)
            playerName = PhotonNetwork.player.NickName;
    }

    private void Update()
    {
        if (connect)
        {
            ConnectToPhoton();
            connect = false;
        }

    }

    public string getPlayerName()
    {
        return playerName;
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    //callback on ConnectToPhoton -> success
    private void OnConnectedToMaster()
    {
        PhotonNetwork.player.NickName = playerName;

    }
}
