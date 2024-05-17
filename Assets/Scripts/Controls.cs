using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Controls : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    public bool isDragging = false;
    public RectTransform canvasImage;
    public RectTransform testImage;
    public GameObject target;
    public int draggingOffset = 1; 

    private bool isMouseDown;
    private Rect rec;
    private float FOV;
    private Vector3 recPos;
    private SelectionManager selectionManager;

    private void Start()
    {
        selectionManager = SelectionManager.instance;
        canvasImage.gameObject.SetActive(false);
        target.SetActive(false);
        FOV = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustFOV();

        if (Input.GetMouseButtonUp(0))
        {
            //Update this to check what has been hit if adding building construction
            if (!isDragging)
            {
                SelectUnit();
            }

            canvasImage.gameObject.SetActive(false);
            isDragging = false;
            isMouseDown = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            recPos = Input.mousePosition;
        }

        if(Input.GetMouseButtonDown(1))
        {
            Transform targetPos = TargetPosition();
            ShowPositionPointer(true);

            if (selectionManager.UnitsSelected.Count > 0)
            {
                for (int i = 0; i < selectionManager.UnitsSelected.Count; i++)
                {
                    selectionManager.UnitsSelected[i].GetComponent<Unit>().MoveUnit(targetPos.position);
                }
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            ShowPositionPointer(false);
        }

        if (isMouseDown)
        {
            if (Vector3.Distance(Input.mousePosition, recPos) > draggingOffset && !isDragging)
            {
                isDragging = true;
            }
            if (isDragging)
            {
                CreateRectSelection();
                SelectOnDrag();
            }
        }
    }

    private void CreateRectSelection()
    {
        canvasImage.gameObject.SetActive(true);

        float boxWidth = Input.mousePosition.x - recPos.x;
        float boxHeight = Input.mousePosition.y - recPos.y;
        
        rec = new Rect(Mathf.Min(recPos.x, Input.mousePosition.x), Mathf.Min(recPos.y, Input.mousePosition.y), Mathf.Abs(boxWidth), Mathf.Abs(boxHeight));
        canvasImage.sizeDelta = rec.size;
        canvasImage.anchoredPosition = (recPos + Input.mousePosition) / 2;
    }

    private void SelectUnit()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out Unit unit))
            {
                if (selectionManager.UnitsSelected.Count > 0)
                {
                    DeslectionAllUnits();
                }

                selectionManager.AddSelectedUnit(unit);
                unit.UnitSelected(true);
            }
            else
            {
                DeslectionAllUnits();
            }
        }
    }

    private void DeslectionAllUnits()
    {
        if(selectionManager.UnitsSelected.Count > 0)
        {
            selectionManager.RemoveAllSelected();
        }
    }

    private void SelectOnDrag()
    {
        for (int i = 0; i < selectionManager.allUnits.Count; ++i)
        {
            if (rec.Contains(Camera.main.WorldToScreenPoint(selectionManager.allUnits[i].transform.position)))
            {
                selectionManager.AddSelectedUnit(selectionManager.allUnits[i]);
                selectionManager.allUnits[i].UnitSelected(true);
            }
            else
            {
                if (selectionManager.UnitsSelected.Count > 0)
                {
                    selectionManager.allUnits[i].UnitSelected(false);
                    selectionManager.RemoveSelectedUnit(selectionManager.allUnits[i]);
                }
            }
        }
    }

    public Transform TargetPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            target.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            target.transform.position = hit.point;
        }

        return target.transform;
    }

    //Temporary fix maybe... Would like to use an animation like in AoE
    private void ShowPositionPointer(bool showPointer)
    {
        target.SetActive(showPointer);
    }

    private void AdjustFOV()
    {
        Camera.main.fieldOfView = FOV;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            FOV--;
            FOV = Mathf.Clamp(FOV, 25, 100);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            FOV++;
            FOV = Mathf.Clamp(FOV, 25, 100);
        }
    }
}
