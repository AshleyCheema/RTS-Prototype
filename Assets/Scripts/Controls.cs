using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    public bool isDragging = false;
    public GameObject canvasImage;
    public GameObject target;

    private Rect rec;
    private float FOV;
    private Vector2 recPos;
    private SelectionManager selectionManager;

    private void Start()
    {
        selectionManager = SelectionManager.instance;
        canvasImage.SetActive(false);
        target.SetActive(false);
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
                
                if (hit.collider.TryGetComponent(out Unit unit))
                {
                    selectionManager.AddSelectedUnit(unit);
                    unit.UnitSelected(true);
                }
            }
        }

        else if (Input.GetMouseButtonDown(0))
        {
            recPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            rec = new Rect(recPos.x, recPos.y, 0, 0);
            canvasImage.SetActive(true);
            isDragging = true;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (selectionManager.UnitsSelected.Count > 0)
            {
                for (int i = 0; i < selectionManager.UnitsSelected.Count; i++)
                {
                    selectionManager.UnitsSelected[i].GetComponent<Unit>().StartPath();
                }
                ShowPositionPointer(true);
                MoveTarget();
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            ShowPositionPointer(false);
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
                if(rec.Contains(Camera.main.WorldToScreenPoint(selectionManager.allUnits[i].transform.position)))
                {
                    selectionManager.AddSelectedUnit(selectionManager.allUnits[i]);
                    selectionManager.UnitsSelected[i].UnitSelected(true);
                }
                else
                {
                    selectionManager.RemoveSelectedUnit(selectionManager.allUnits[i]);
                    if (selectionManager.UnitsSelected.Count < 0)
                    {
                        selectionManager.UnitsSelected[i].UnitSelected(false);
                    }
                }
            }   
        }
    }

    public void MoveTarget()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            target.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            target.transform.position = hit.point;
        }


    }

    //Temporary fix maybe... Would like to use an animation like in AoE
    private void ShowPositionPointer(bool showPointer)
    {
        target.SetActive(showPointer);
    }
}
