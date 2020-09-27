/*
    DESCRIPTION: class handles convoy mooving, line rendering and convoy selection via tag


    DATE        USER        ACTION
    19.04.2020  SH          Created
    24.04.2020  SH          Logic reworking, script is now on handlers game object instead of every ship game object, added selecting convoys.
    25.04.2020  PM          Set linerenderer to clear line before redrawing
    29.04.2020  PM          Set line renderer to use world space. This has to be off in editor because it caused some visual bugs however it needs to be on at runtime.
    02.05.2020  PM          Added logic for highlighting selected convoy
    03.05.2020  SH          Added statement that handle removing convoy from cities
    03.05.2020  PM          Fixed highlighting on selected convoy
    05.04.2020  PM          Added server synch on removing convoy from town

    TODO:   -selected convoy and what clicked could be in same method
            -rename method with bad names
            -move update statements into methods
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedConvoy;        //selected convoy on map

    private NavMeshAgent agent;
    private Camera cam;
    private LineRenderer line;
    private PhotonView photonView;
    [SerializeField] private int removedConvoyID = 0;

    [SerializeField] bool devTesting;

    //setup a singleton for PlayerController
    private static PlayerController _playerController;
    public static PlayerController Instance { get { return _playerController; } }

    private void Awake()
    {
        if (_playerController != null && _playerController != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _playerController = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //reset  line renderer and set destination
            if (selectedConvoy != null || HUDManager.Instance.GetConvoyData() != null)                  //<------------------25.04.2020 PM----------------------------  
            {
                if (selectedConvoy == null && HUDManager.Instance.GetConvoyData() != null)              //<------------------03.05.2020 SH----------------------------
                {
                    RaycastHit _hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out _hit);
                    if (_hit.collider.gameObject != null)
                        if (_hit.collider.gameObject == HUDManager.Instance.GetConvoyData().getConvoyLocation())
                        {
                            HUDManager.Instance.activateTownWarehouseHUD(true);
                            HUDManager.Instance.setActiveCity(HUDManager.Instance.GetConvoyData().getConvoyLocation());
                            return;
                        }
                        else 
                        {
                            HUDManager.Instance.activateTownWarehouseHUD(false);
                            selectedConvoy = HUDManager.Instance.GetConvoyData().getConvoyGameObject();
                            selectedConvoy.SetActive(true);
                            selectedConvoy.GetComponent<NavMeshAgent>().enabled = true;
                            selectedConvoy.GetComponent<Convoy>().enabled = true;

                            int convoyId = selectedConvoy.GetComponent<Convoy>().getConvoyId();                             //  05.04.2020  PM 
                            int townId = selectedConvoy.GetComponent<Convoy>().getConvoyData().getConvoyLocation().GetComponent<TownManager>().getTownId();
                            photonView.RPC("RemoveConvoyClient", PhotonTargets.Others, convoyId, townId);

                            if (line != null)
                                line.enabled = false;
                            agent = selectedConvoy.GetComponent<NavMeshAgent>();
                            line = selectedConvoy.GetComponent<LineRenderer>();
                            line.enabled = true;
                            line.useWorldSpace = true;
                            selectedConvoy.GetComponent<Convoy>().HighlightConvoy(true);
                            HUDManager.Instance.setConvoyData(selectedConvoy.GetComponent<Convoy>().getConvoyData());
                            selectedConvoy.GetComponent<Convoy>().setNewConvoyData(selectedConvoy.GetComponent<Convoy>().getConvoyData().getConvoyLocation().GetComponent<TownManager>().getConvoyData(selectedConvoy.GetComponent<Convoy>().getConvoyId()));
                            selectedConvoy.GetComponent<Convoy>().getConvoyData().getConvoyLocation().GetComponent<TownManager>().removeConvoyFromTownList(selectedConvoy.GetComponent<Convoy>().getConvoyId());
                            selectedConvoy.GetComponent<Convoy>().getConvoyData().setConvoyLocation(null);
                        }
   
                }
                for (int i = 0; i < line.positionCount; i++)
                {
                    line.SetPosition(i, selectedConvoy.transform.position);
                }
                SetDestinationTarget();
            }
        }

        if ((agent != null) && (agent.enabled) && (agent.remainingDistance > 1))
        {
            line.enabled = true;
        }
        else
        {
            if ((agent != null) && (agent.enabled) && (agent.remainingDistance <= 1))
                line.enabled = false;
        }
         

        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.HitUIElement() && !checkIfUIElementIsHit())
        {
            SelectConvoy();      
        }

    }

    // fire raycast from mouse position and r
    private void SelectConvoy()
    {
        RaycastHit _hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        WhatClicked(_hit);
    }

    //Method that select game object based on tag
    private void WhatClicked(RaycastHit _hit)
    {
        switch (_hit.collider.gameObject.tag)
        {
            case "PlayerConvoy":
                HUDManager.Instance.activateTownWarehouseHUD(false);
                if (line != null)
                    line.enabled = false;

                if (selectedConvoy != null)                                                 //03.05.2020  PM 
                    selectedConvoy.GetComponent<Convoy>().HighlightConvoy(false);

                if (_hit.collider.gameObject.GetComponent<Convoy>().getConvoyData().getConvoyOwner() != PhotonNetwork.player.NickName && !devTesting)
                    break;

                selectedConvoy = _hit.collider.gameObject;
                agent = selectedConvoy.GetComponent<NavMeshAgent>();
                line = selectedConvoy.GetComponent<LineRenderer>();
                line.enabled = true;
                line.useWorldSpace = true;                                  // 29.04.2020 PM
                selectedConvoy.GetComponent<Convoy>().HighlightConvoy(true); //02.05.2020  PM
                HUDManager.Instance.setConvoyData(selectedConvoy.GetComponent<Convoy>().getConvoyData());
                break;
            case "Convoy":
                HUDManager.Instance.activateTownWarehouseHUD(false);
                break;
            case "TownConvoyNumber":
                HUDManager.Instance.activateTownWarehouseHUD(false);
                break;
            case "Town":
                break;
            case "Map":
                HUDManager.Instance.activateTownWarehouseHUD(false);
                if (line != null)
                    line.enabled = false;
                HUDManager.Instance.removeSelectedConvoyHUD();
                if (selectedConvoy != null)
                {
                    selectedConvoy.GetComponent<Convoy>().HighlightConvoy(false);
                    HUDManager.Instance.setConvoyData(null);
                }

                selectedConvoy = null;
                agent = null;
                line = null;
                break;
        }
    }

    // set path origin and call method draw path
    private IEnumerator GetPath()
    {
        line.SetPosition(0, selectedConvoy.transform.position); //set the line's origin

        yield return new WaitForSeconds(0.1f);
        DrawPath(agent.path);
    }

    // drwa path of ship from corner to corner 
    private void DrawPath(NavMeshPath _path)
    {

        line.positionCount = _path.corners.Length; //set the array of positions to the amount of corners

        for (var i = 1; i < _path.corners.Length; i++)
        {
            line.SetPosition(i, _path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }

    // reset selected convoy base on game object value that you want to reset
    public void resetSelectedConvoy(GameObject convoy)
    {
        if (convoy == selectedConvoy)
        {
            if (selectedConvoy == null)
                return;

            selectedConvoy.GetComponent<Convoy>().HighlightConvoy(false);
            selectedConvoy = null;
        }
    }

    // this method return true/fale if UI element is hited
    public bool checkIfUIElementIsHit()
    {
        RaycastHit _hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        if (_hit.collider.gameObject.tag == "UIElement")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //something with photon network that will find convoy in town base on ID of town and convoy and remove that convoy from town and enable his game object
    [PunRPC]
    private void RemoveConvoyClient(int convoyId, int townId)                               //    05.04.2020  PM 
    {
        GameObject[] towns = GameObject.FindGameObjectsWithTag("Town");
        TownManager tmanager = null;
        Convoy convoy = null;

        for (int i = 0; i < towns.Length; i++)
        {
            if (towns[i].GetComponent<TownManager>().getTownId() == townId)
            {
                tmanager = towns[i].GetComponent<TownManager>();
                break;
            }

        }
        convoy = tmanager.getConvoyData(convoyId).getConvoy();
        convoy.enabled = true;
        convoy.setNewConvoyData(tmanager.getConvoyData(convoy.getConvoyId()));
        tmanager.removeConvoyFromTownList(convoyId);
        convoy.gameObject.SetActive(true);
    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Setters for max and current data-------------------------------------

    // set destination target Town/Map if destiantion is town go to spawner else go to hit point
    private void SetDestinationTarget()
    {
        agent.velocity = Vector3.zero;
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider.gameObject.tag == "Town")
        {
            agent.SetDestination(hit.collider.gameObject.GetComponent<TownManager>().getSpawnerPos().position);
        }
        else
        {
            agent.SetDestination(hit.point);
        }

        StartCoroutine(GetPath());
        if (hit.collider.gameObject.tag != "Map")
        {
            selectedConvoy.GetComponent<Convoy>().setCovnoyTarget(hit.collider.gameObject);
        }


    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Getters for max and current data-------------------------------------

    // get selected convoy <------------------24.04.2020 SH----------------------------
    public GameObject getSelectedConvoy()
    {
        return selectedConvoy;
    }

    // get ID of removed convoy
    public int getRemovedConvoyID()
    {
        return removedConvoyID;
    }

    // get bool value of DevTesting Variable
    public bool getDevTesting()
    {
        return devTesting;
    }

}
