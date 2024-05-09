using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }

    public List<Unit> allUnits = new List<Unit>();
    public List<Unit> UnitsSelected = new List<Unit>();

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }    
    }
}
