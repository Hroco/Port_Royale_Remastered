/*
    DESCRIPTION: class handles managment of game


    DATE        USER        ACTION
    02.05.2020  SH          Created
    02.02.2020  PM          Added convoy types icon lists, getters and hit ui element method
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int globalConvoyId = 0;
    [SerializeField] private int globalTownId = 0;
    [SerializeField] private List<Sprite> flags;
    [SerializeField] private List<Material> townStage;
    [SerializeField] private List<Material> townStageHighlighted;
    [SerializeField] private List<ItemDataModel> goods;
    [SerializeField] private List<Sprite> convoyTypes;
    [SerializeField] private List<Sprite> convoyTypeHighlighted;

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    private Camera cam;
    //setup a singleton for GameManager
    private static GameManager _gameManager;
    public static GameManager Instance { get { return _gameManager; } }

    private void Awake()
    {
        if (_gameManager != null && _gameManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _gameManager = this;
        }
    }
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        WhatIsPlayerLookingAt();
    }

    // this method provide unique ID for convoy
    public int askForConvoyId()
    {
        globalConvoyId++;
        return globalConvoyId;
    }

    // this method provide unique ID for town
    public int askForTownId()
    {
        globalTownId++;
        return globalTownId;
    }

    // check what is player locking at and when it is town highlight his icon
    public void WhatIsPlayerLookingAt()
    {
        RaycastHit _hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        if (_hit.collider.gameObject != null)
        {
            switch (_hit.collider.gameObject.tag)
            {
                case "PlayerConvoy":
                    break;
                case "Convoy":
                    break;
                case "Town":
                    _hit.collider.gameObject.GetComponent<TownManager>().setTownHighlighted(true);
                    break;
                case "Map":
                    break;
                default:
                    break;
            }
        }
    }

    //check if player did hit ui element with cursor
    public bool HitUIElement()            // 02.02.2020  PM  
    {
        PointerEventData pointerEvent = new PointerEventData(eventSystem);
        pointerEvent.position = Input.mousePosition;


        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEvent, results);

        raycaster.Raycast(pointerEvent, results);

        if (results.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Setters for max and current data-------------------------------------

    

    //------------------------------------------------------------------------------------------------------------

    //---------------------------------------Getters for max and current data-------------------------------------

    // get town icon normal/highlighted base on town population count 
    public Material getTownIcon(int CurrentCitizens, bool highlighted)
    {
        if (CurrentCitizens >= 0 && CurrentCitizens < 500)
            if (highlighted)
                return townStageHighlighted[0];
            else
                return townStage[0];
        if (CurrentCitizens >= 500 && CurrentCitizens < 1000)
            if (highlighted)
                return townStageHighlighted[1];
            else
                return townStage[1];
        if (CurrentCitizens >= 1000 && CurrentCitizens < 2500)
            if (highlighted)
                return townStageHighlighted[2];
            else
                return townStage[2];
        if (CurrentCitizens >= 2500 && CurrentCitizens < 5000)
            if (highlighted)
                return townStageHighlighted[3];
            else
                return townStage[3];
        if (CurrentCitizens >= 5000)
            if (highlighted)
                return townStageHighlighted[4];
            else
                return townStage[4];

        Debug.LogError("Imported value out of bounds!");
        return null;
    }

    // get flag base on nation name string
    public Sprite getFlag(string nation)
    {
        if (nation == "England")
            return flags[0];
        if (nation == "Spain")
            return flags[1];
        if (nation == "France")
            return flags[2];
        if (nation == "Netherland")
            return flags[3];

        Debug.LogError("Imported value out of bounds!");
        return null;
    }

    // get Goods icon base on item name string
    public ItemDataModel getGoodsIcon(string itemName)
    {
        for (int i = 0; i < goods.Count; i++)
        {
            if (goods[i].ItemName == itemName)
                return goods[i];
        }
        Debug.LogError("Imported value out of bounds!");
        return null;
    }

    // get Convoy type and return Convoy UI image that is on top of the ship when ship is on map
    public Sprite getConvoyType(string type)                        //    02.02.2020  PM 
    {
        if (type == "Regular")
            return convoyTypes[0];
        if (type == "War")
            return convoyTypes[1];
        if (type == "Player")
            return convoyTypes[2];

        Debug.LogError("Imported value out of bounds!");
        return null;
    }

    // get Convoy type and return Highlighted Convoy UI image that is on top of the ship when ship is on map
    public Sprite getConvoyTypeHighlighted(string type)
    {
        if (type == "Regular")
            return convoyTypeHighlighted[0];
        if (type == "War")
            return convoyTypeHighlighted[1];
        if (type == "Player")
            return convoyTypeHighlighted[2];

        Debug.LogError("Imported value out of bounds!");
        return null;
    }

}
