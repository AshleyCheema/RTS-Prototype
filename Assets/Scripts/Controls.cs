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
    public float panSpeed = 20f;

    private Rect rec;
    private List<GameObject> _selectedUnits;
    private float FOV;
    private Vector2 recPos;
    private GameObject[] units;
    private MoveTo moveTo;

    private void Start()
    {
        Application.runInBackground = true;

        canvasImage.SetActive(false);
        units = GameObject.FindGameObjectsWithTag("Units");
        _selectedUnits = new List<GameObject>();
        FOV = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.fieldOfView = FOV;

        //Using the scroll wheel we can zoom in and zoom out of the map but only between the range of 25 and 100
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

        //If the button is up then ray trace and if the object is hit with the tag "Units"
        //Then select that unit 
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // shoot a ray into the scene from the camera
            // if an object with a collider is hit return true
            if (Physics.Raycast(ray, out hit))
            {
                canvasImage.SetActive(false);

                if (hit.collider.tag == "Units")
                {
                    AddSelectedUnit(hit.collider.GetComponent<UnitS>());
                    hit.collider.gameObject.GetComponent<UnitS>().IsSelected = true;
                    canvasImage.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
                    Rect rec = new Rect(0, 0, 30, 60);
                    //moveTo = hit.collider.GetComponent<MoveTo>();

                }
                else
                {
                    //if selected elsewhere then select is turned to false
                    isSelected = false;
                }
            }
            // If the raycast hits nothing with a collider on it 
            // then disable the canvasImage and reset the rec object
            else
            {
                canvasImage.SetActive(false);
                Rect rec = new Rect(0, 0, 30, 60);

            }
        }

        //If left mouse button is down then dragging is true, set canvas image is also true. Then create a new rect at current mouse position
        //This allows for many units to be selected at once
        else if (Input.GetMouseButtonDown(0))
        {
            rec = new Rect(Input.mousePosition.x, Input.mousePosition.y, 0, 0);
            canvasImage.SetActive(true);
            isDragging = true;

            recPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        //If right mouse button is down check if units have been selected
        //Then do function, else disable target so it is not active.
        if(Input.GetMouseButtonDown(1))
        {
            if (_selectedUnits.Count > 0)
            {
                MoveTarget();
            }
        }
        else
        {
            target.SetActive(false);
        }

        //If is dragging is true, then depending which way the user is dragging the image
        //depends how it will be drawn. This is because the image if flipped has no back face
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

        //This check whether there are units underneath the image can makes them selected units
        if(isDragging)
        {
            for(int i = 0; i < units.Length; ++i)
            {
                if( rec.Contains( Camera.main.WorldToScreenPoint(units[i].transform.position)))
                {
                    UnitS unitS = units[i].GetComponent<UnitS>();
                    unitS.IsSelected = true;
                    AddSelectedUnit(unitS);
                    //units[i].GetComponent<UnitS>().IsSelected = true;
                    isSelected = true;
                    //AddSelectedUnit(units[i].GetComponent<UnitS>());
                    //moveTo = units[i].GetComponent<MoveTo>();
                }
                else
                {
                    if (units[i].GetComponent<UnitS>())
                    {
                        UnitS unitS = units[i].GetComponent<UnitS>();
                        unitS.IsSelected = false;
                        RemoveSelectedUnit(unitS);

                        //units[i].GetComponent<UnitS>().IsSelected = false;
                        //RemoveSelectedUnit(units[i].GetComponent<UnitS>());
                    }
                }
            }
        }
        Debug.Log(_selectedUnits.Count);
    }
    /// <summary>
    /// Moves target position. 
    /// </summary>
    public void MoveTarget()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        target.SetActive(true);

        if (Physics.Raycast(ray, out hit))
        {
            target.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            target.transform.position = hit.point;
            //moveTo.MoveToTarget();
            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                _selectedUnits[i].GetComponent<UnitS>().MoveToTarget();
            }
        }
    }

    /// <summary>
    /// Add a selected unit to a selected unit list <see cref="UnitS"/>
    /// </summary>
    /// <param name="a_unit"></param>
    public void AddSelectedUnit(UnitS a_unit)
    {
        if(!_selectedUnits.Contains(a_unit.gameObject))
        {
            _selectedUnits.Add(a_unit.gameObject);
        }
    }

    public void RemoveSelectedUnit(UnitS a_unit)
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
