using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    public bool isSelected = false;
    public GameObject canvasImage;
    private Rect rec;

    private void Start()
    {
        canvasImage.SetActive(false);        
    }

    // Update is called once per frame
    void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonUp(0))
            { 
                if (hit.collider.tag == "Units")
                {
                    isSelected = true;
                    canvasImage.transform.position = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
                    Rect rec = new Rect(0, 0, 30, 60);
                    //rec.Contains();
                }
                else
                {
                    isSelected = false;
                }
            }
            
            else if(Input.GetMouseButtonDown(0))
            {
                rec = new Rect(Input.mousePosition.x, Input.mousePosition.y, 0, 0);
            }

            if(rec != null)
            {
                rec.width = Input.mousePosition.x - rec.x;
                rec.height = Input.mousePosition.y - rec.y;
                Debug.Log(rec);
                canvasImage.transform.position = new Vector3(rec.x, rec.y, 0);
                canvasImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rec.width, rec.height);
            }
        }
	}
}
