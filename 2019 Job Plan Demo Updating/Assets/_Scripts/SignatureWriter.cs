using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SignatureWriter : MonoBehaviour {



    GameObject sigPoint; // used for signatures
    GameObject drawingPoint; // used for layout draing (has larger 2dcollider)
    Vector3 screenPoint;
    Vector3 offset;
    Material trailMaterial;
    bool trailAdded = false;
    TrailRenderer myTrail;
    GameObject drawnLines; // only used for drawing a custom traffic layout
    
    

    void Start() {
        trailMaterial = Resources.Load("TrailMaterial") as Material;
        sigPoint = Resources.Load("SigPoint") as GameObject;
        drawingPoint = Resources.Load("DrawingPoint") as GameObject;
        if (GameObject.FindGameObjectWithTag("DrawnLines")) {
            drawnLines = GameObject.FindGameObjectWithTag("DrawnLines");
        }

    }
    //void OnMouseOver() {
    //    float distanceFromCamera = 10f;
    //    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);

    //    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
    //    Debug.Log(curPosition);

    //}

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }
    void OnMouseUp() {
        BoxCollider2D lastCollider = GetComponent<BoxCollider2D>();
        lastCollider.enabled = false;
       
        NewSigPoint();
        if (drawnLines != null) {
            transform.SetParent(drawnLines.transform);
        }

    }
    void NewSigPoint() {
        Transform sigParent = GameObject.FindGameObjectWithTag("SigBackground").transform;
        if (drawnLines == null)
        {
            GameObject thisLine = Instantiate(sigPoint) as GameObject;
            thisLine.transform.SetParent(sigParent);
        }
        if (drawnLines != null) {
            GameObject thisLine = Instantiate(drawingPoint) as GameObject;
            thisLine.transform.SetParent(sigParent);
        }
    }

    IEnumerator OnMouseDrag()
    {
        //Debug.Log("Dragging");
        float distanceFromCamera = 10f;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
        //Debug.Log(curPosition);
        if (trailAdded) {
            
            Vector3[] positionsArray = new Vector3[myTrail.positionCount];
            myTrail.GetPositions(positionsArray);
            for (int i = 0; i < myTrail.positionCount; i++)
            {
                Debug.Log(positionsArray[i]);

            }

            yield break;
        }
        yield return new WaitForSeconds(.01f);
        

        if (myTrail == null)
        {
            gameObject.AddComponent<TrailRenderer>();
            myTrail = GetComponent<TrailRenderer>();
            myTrail.minVertexDistance = 0.1f;
            myTrail.startWidth = 0.1f;
            myTrail.endWidth = 0.1f;
            myTrail.time = Mathf.Infinity;
            myTrail.material = trailMaterial;
            trailAdded = true;
           
        }
    }
   public void ClearSig()
    {



        GameObject[] sigPoints = GameObject.FindGameObjectsWithTag("SigPoint");
        //Debug.Log(sigPoints.Length);
        
        foreach (GameObject thisSigPoint in sigPoints)
        {
            Destroy(thisSigPoint.gameObject);
        }
        NewSigPoint();
            }

    public void HidePanel()
    { // hide a panel using the cancel button.

        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }
}
