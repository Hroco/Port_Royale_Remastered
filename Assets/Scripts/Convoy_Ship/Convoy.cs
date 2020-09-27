/*
    DESCRIPTION: handles convoy object. How is displayed on map HUD etc.


    DATE        USER        ACTION
    27.04.2020  PM          Created
    30.04.2020  PM          Added calculation for selecting coresponding rotation image
    01.05.2020  PM          Added logic for entering town
    02.05.2020  PM          Reworked logic for changing rotation pictures and added highlighting functionality
    03.05.2020  PM          Added getter for convoy data
    05.04.2020  PM          Added server synch on convoy entering town
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Convoy : MonoBehaviour
{
    [SerializeField] private int convoyId = 0;

    [SerializeField] private GameObject target;
    [SerializeField] private Image convoyFlag, convoyTypeImg;
    [SerializeField] private TMP_Text shipCountTxt;
    [SerializeField] private Image shipIcon;

    [SerializeField] private List<Behaviour> componentsToDisableOrEnable = new List<Behaviour>();
    
    [SerializeField] ConvoyData convoyData;
    [SerializeField] Vector3 movingDirection;

    private PhotonView photonView;
    private Vector3 prevLoc;

    private void Start()
    {
        convoyId = GameManager.Instance.askForConvoyId();
        convoyData.setConvoyId(convoyId);
        photonView = GetComponent<PhotonView>();
        convoyData.setConvoy(this);
        prevLoc = transform.position;

        //testing, will be removed later
        convoyFlag.sprite = GameManager.Instance.getFlag(convoyData.getConvoyNation());
        shipCountTxt.text = convoyData.getAllShips().Count.ToString();
        convoyTypeImg.sprite = GameManager.Instance.getConvoyType(convoyData.getConvoyType());

        if (convoyData.getConvoyOwner() == PhotonNetwork.player.NickName)
        {
            GetComponent<PhotonView>().ownerId = PhotonNetwork.player.ID;
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
        }
            
    }

    private void Update()
    {
        CalculateDirection();
        if (target != null)
            CheckDestination();
    }

    //calculates direction and selects coresponding image
    private void CalculateDirection()                               // 30.04.2020 PM
    {
        movingDirection = transform.position - prevLoc;
        movingDirection.Normalize();
        if (movingDirection == Vector3.zero)
            return;

        float angle = Vector3.Angle(Vector3.forward, movingDirection);
        angle = angle / 11.25f;
        int index = (int)angle;
        Mathf.Clamp(index, 0, 16);

        if (movingDirection.x < 0)
            index = 31 - (int)angle;
        //Converting ships locationi index to sprite image index DO NOT OPEN
        switch (index) 
        {
            case 0:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(14);
                break;
            case 1:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(15);
                break;
            case 2:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(15);
                break;
            case 3:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(0);
                break;
            case 4:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(0);
                break;
            case 5:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(1);
                break;
            case 6:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(1);
                break;
            case 7:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(2);
                break;
            case 8:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(2);
                break;
            case 9:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(3);
                break;
            case 10:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(3);
                break;
            case 11:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(4);
                break;
            case 12:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(4);
                break;
            case 13:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(5);
                break;
            case 14:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(5);
                break;
            case 15:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(6);
                break;
            case 16:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(6);
                break;
            case 17:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(7);
                break;
            case 18:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(7);
                break;
            case 19:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(8);
                break;
            case 20:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(8);
                break;
            case 21:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(9);
                break;
            case 22:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(9);
                break;
            case 23:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(10);
                break;
            case 24:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(10);
                break;
            case 25:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(11);
                break;
            case 26:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(11);
                break;
            case 27:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(12);
                break;
            case 28:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(12);
                break;
            case 29:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(13);
                break;
            case 30:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(13);
                break;
            case 31:
                shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(14);
                break;
        }
        //shipIcon.sprite = convoyData.getHighestTierShip().getShipData().getShipRotation(index);
        //GetComponent<MeshRenderer>().material = convoyData.getHighestTierShip().getShipData().getShipRotation(index);
        prevLoc = transform.position;
    }

    //testing 
    public void addRandom()
    {
        int i = Random.Range(1, 100);
        convoyData.addCrew(i);
        i = Random.Range(1, 100);
        convoyData.addCannons(i);
        i = Random.Range(1, 100);
        convoyData.addHold(i);
    }

    //synch convoy data with all clients
    public void UpdateClients()
    {
        List<Ship> convoyShips = convoyData.getAllShips();

        for (int i = 0; i < convoyShips.Count; i++)
        {
            photonView.RPC("UpdateConvoyShips", PhotonTargets.All, i, convoyShips[i].getShipCurrentCrew(), convoyShips[i].getShipCurrentCannons(), convoyShips[i].getShipCurrentHold());
        }
    }

    //sets target destination
    public void setCovnoyTarget(GameObject tgt)
    {
        target = tgt;
    }

    //behaviours for different destinations
    private void CheckDestination()                     // 01.05.2020 PM
    {
        switch (target.tag)
        {
            case "Town":
                if ((Vector3.Distance(transform.position, target.GetComponent<TownManager>().getSpawnerPos().position) <= 2f))
                {
                    int townId = target.GetComponent<TownManager>().getTownId();
                    
                    PlayerController.Instance.resetSelectedConvoy(getConvoyData().getConvoyGameObject());
                    target.GetComponent<TownManager>().addConvoy(convoyData);
                    target = null;
                    this.gameObject.SetActive(false);
                    setComponents(false);
                    if(PlayerController.Instance.getDevTesting())
                        photonView.RPC("EnterTown", PhotonTargets.Others, townId);
                }
                break;           
        }
    }                              

    //disables or enables components
    public void setComponents(bool status)
    {
        for (int i = 0; i < componentsToDisableOrEnable.Count; i++)
        {
            componentsToDisableOrEnable[i].enabled = status;
        }
    }

    //reset convoy data;
    public void setNewConvoyData(ConvoyData data)
    {
        convoyData = data;
    }

    //get global id of convoy
    public int getConvoyId()
    {
        return convoyId;
    }

    //get convoy data
    public ConvoyData getConvoyData()                   //    03.05.2020  PM  
    {   
        return convoyData;
    }

    //highlights convoy if selected
    public void HighlightConvoy(bool status)
    {
        if (status)
        {
            convoyTypeImg.sprite = GameManager.Instance.getConvoyTypeHighlighted(convoyData.getConvoyType().ToString());
        }
        else
        {
            convoyTypeImg.sprite = GameManager.Instance.getConvoyType(convoyData.getConvoyType().ToString());
        }
    }

    [PunRPC]
    public void UpdateConvoyShips(int index, int crew, int cannons, int hold)
    {
        List<Ship> convoyShips = convoyData.getAllShips();

        convoyShips[index].UpdateClient(crew, cannons, hold);
    }

    [PunRPC]
    public void EnterTown(int townId)                                       //    05.04.2020  PM  
    {
        GameObject[] towns = GameObject.FindGameObjectsWithTag("Town");
        for (int i = 0; i < towns.Length; i++)
        {
            if (towns[i].GetComponent<TownManager>().getTownId() == townId)
                target = towns[i];
        }

        PlayerController.Instance.resetSelectedConvoy(getConvoyData().getConvoyGameObject());
        target.GetComponent<TownManager>().addConvoy(convoyData);
        target = null;
        this.gameObject.SetActive(false);
        setComponents(false);
    }
}
