using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    public bool isSelected = false;
    public bool isDragging = false;
    public GameObject canvasImage;
    public GameObject target;

    private Rect rec;
    private List<GameObject> _selectedUnits;
    private float FOV;
    private Vector2 recPos;
    private SelectionManager selectionManager;

    private void Start()
    {
        selectionManager = SelectionManager.instance;
        canvasImage.SetActive(false);
        _selectedUnits = new List<GameObject>();
        FOV = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.fieldOfView = FOV;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            FOV--;
            FOV = Mathf.Clamp(FOV, 25, 100);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            FOV++;
            FOV = Mathf.Clamp(FOV, 25, 100);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                canvasImage.SetActive(false);

                if (hit.collider.tag == "Units")
                {
                    AddSelectedUnit(hit.collider.GetComponent<Unit>());
                    hit.collider.gameObject.GetComponent<Unit>().UnitSelected(true);
                    canvasImage.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
                    Rect rec = new Rect(0, 0, 30, 60);
                }
                else
                {
                    isSelected = false;
                }
            }
        }

        else if (Input.GetMouseButtonDown(0))
        {
            rec = new Rect(Input.mousePosition.x, Input.mousePosition.y, 0, 0);
            canvasImage.SetActive(true);
            isDragging = true;

            recPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (_selectedUnits.Count > 0)
            {
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    _selectedUnits[i].GetComponent<Unit>().StartPath();
                }
                MoveTarget();
            }
        }
        else
        {
            target.SetActive(false);
        }

        if (isDragging)
        {
            if (Input.mousePosition.y < recPos.y && Input.mousePosition.x > recPos.x)
            {
                rec.width = Input.mousePosition.x - recPos.x;

                rec.height = recPos.y - Input.mousePosition.y;
                rec.y = Input.mousePosition.y;
                //Debug.Log(rec.height);
            }
            else if(Input.mousePosition.y < recPos.y && Input.mousePosition.x < recPos.x)
            {
                rec.width = recPos.x - Input.mousePosition.x;
                rec.x = Input.mousePosition.x;

                rec.height = recPos.y - Input.mousePosition.y;
                rec.y = Input.mousePosition.y;
            }
            else if(Input.mousePosition.y > recPos.y && Input.mousePosition.x < recPos.x)
            {
                rec.width = recPos.x - Input.mousePosition.x;
                rec.x = Input.mousePosition.x;

                rec.height = Input.mousePosition.y - recPos.y;
            }
            else
            {
                rec.width = Input.mousePosition.x - rec.x;
                rec.height = Input.mousePosition.y - rec.y;
               // Debug.Log(rec);
            }

            canvasImage.transform.position = new Vector3(rec.x, rec.y, 0);
            canvasImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rec.width, rec.height);
        }

        if(isDragging)
        {
            for(int i = 0; i < selectionManager.allUnits.Count; ++i)
            {
                if( rec.Contains(Camera.main.WorldToScreenPoint(selectionManager.allUnits[i].transform.position)))
                {
                    isSelected = true;
                    AddSelectedUnit(selectionManager.allUnits[i]);
                    selectionManager.allUnits[i].UnitSelected(true);
                }
                else
                {
                    RemoveSelectedUnit(selectionManager.allUnits[i]);
                    selectionManager.allUnits[i].UnitSelected(false);
                }
            }   
        }
    }

    public void MoveTarget()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        target.SetActive(true);

        if (Physics.Raycast(ray, out hit))
        {
            target.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            target.transform.position = hit.point;
        }
    }

    public void AddSelectedUnit(Unit a_unit)
    {
        if(!selectionManager.allUnits.Contains(a_unit))
        {
            selectionManager.UnitsSelected.Add(a_unit);
        }
    }

    public void RemoveSelectedUnit(Unit a_unit)
    {
        if(selectionManager.allUnits.Contains(a_unit))
        {
            selectionManager.UnitsSelected.Remove(a_unit);
        }
    }

    public void RemoveAllSelected()
    {
        selectionManager.UnitsSelected.Clear();
    }
}
