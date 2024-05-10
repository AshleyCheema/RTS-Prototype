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

    public void AddSelectedUnit(Unit a_unit)
    {
        if (!UnitsSelected.Contains(a_unit))
        {
            UnitsSelected.Add(a_unit);
        }
    }

    public void RemoveSelectedUnit(Unit a_unit)
    {
        if (UnitsSelected.Contains(a_unit))
        {
            UnitsSelected.Remove(a_unit);
        }
    }

    public void RemoveAllSelected()
    {
        UnitsSelected.Clear();
    }
}
