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
    
    Vector2 recPos;
    GameObject[] units;

    private void Start()
    {
        canvasImage.SetActive(false);
        units = GameObject.FindGameObjectsWithTag("Units");
        _selectedUnits = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
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
                    hit.collider.gameObject.GetComponent<Unit>().isSelected = true;
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
                Debug.Log(rec.height);
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
            for(int i = 0; i < units.Length; ++i)
            {
                if( rec.Contains( Camera.main.WorldToScreenPoint(units[i].transform.position)))
                {
                    units[i].GetComponent<Unit>().isSelected = true;
                    isSelected = true;
                    AddSelectedUnit(units[i].GetComponent<Unit>());
                }
                else
                {
                    units[i].GetComponent<Unit>().isSelected = false;
                    RemoveSelectedUnit(units[i].GetComponent<Unit>());
                }
            }
        }
        Debug.Log(_selectedUnits.Count);
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
        if(!_selectedUnits.Contains(a_unit.gameObject))
        {
            _selectedUnits.Add(a_unit.gameObject);
        }
    }

    public void RemoveSelectedUnit(Unit a_unit)
    {
        if(_selectedUnits.Contains(a_unit.gameObject))
        {
            _selectedUnits.Remove(a_unit.gameObject);
        }
    }

    public void RemoveAllSelected()
    {
        _selectedUnits.Clear();
    }
}
