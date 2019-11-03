using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// control the behaviour of the pencil and eraser on the custom layout panel

public class PencilController : MonoBehaviour {

    GameObject drawingFrame; // the game object that hold the trail renderer and boxcollider2d
    GameObject drawnLines; // the game object that holds all the drwn lines as children
    Image pencil; // the sprite for the pencil.
        
    
  
	void Awake () {
        drawingFrame = GameObject.FindGameObjectWithTag("DrawingFrame");
       // Debug.Log(drawingFrame);
        drawnLines = GameObject.FindGameObjectWithTag("DrawnLines");
        
        pencil = GameObject.FindGameObjectWithTag("PencilImage").GetComponent<Image>();
		
	}

    void Start() {
        drawingFrame.SetActive(false);
    }

    // used when the pecil button is pressed to start or stop drawing.
    public void ActivateDrawing() {
        if (drawingFrame.activeSelf == true) {
            
            int thisline = drawnLines.transform.childCount; // get the line created during the button click and delete it 
            Destroy(drawnLines.transform.GetChild(thisline - 1).gameObject);
          //   Debug.Log("DeletingLastLine from pencil deactivation");

           }
        drawingFrame.SetActive(!drawingFrame.activeSelf);
        
      //  Debug.Log("Drawing frame is on " + drawingFrame.activeSelf);
    }


    // called form the eraser button to start the coroutine below.
    public void StartLastLineDlelete() {
        StartCoroutine(DeleteLastLine());
    }

    public IEnumerator DeleteLastLine() {
        if (drawingFrame.activeSelf == true) {
            //drawingFrame.SetActive(false);
            int thisline = drawnLines.transform.childCount; // get the line created during the button click 
            Destroy(drawnLines.transform.GetChild(thisline - 1).gameObject);
          //  Debug.Log("DeletingLastLine from eraser clicking" + thisline);
            yield return new WaitForSeconds(.1f);
        }
        
        int lastline = drawnLines.transform.childCount; // get the line alrready created from drawing
        if (lastline > 0)
        {
            Destroy(drawnLines.transform.GetChild(lastline - 1).gameObject);
        }
       // Debug.Log("DeletingLastLine already made" + lastline);

    }

    
	
	// Update is called once per frame
	void Update () {
        if (drawingFrame.activeSelf == true) {
            pencil.color = Color.red;
        }
        else { pencil.color = Color.white; }
		
	}
}
