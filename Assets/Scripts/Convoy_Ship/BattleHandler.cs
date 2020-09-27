using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{

    [SerializeField] private GameObject selectedShip;

    //setup a singleton for PlayerController
    private static BattleHandler _battleHandler;
    public static BattleHandler Instance { get { return _battleHandler; } }

    private void Awake()
    {
        if (_battleHandler != null && _battleHandler != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _battleHandler = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
