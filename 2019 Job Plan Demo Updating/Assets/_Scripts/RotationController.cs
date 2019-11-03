using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RotationController : MonoBehaviour {
    int width;
    int height;
    Transform myCamera;
    // Transform myPanels;
    public Canvas myCanvas;
    public CanvasScaler myCanvasScale;
    Resolution BaseRes;
    bool isPortrait = false;
    bool isLandscape = false;
    public Text usedResolution;
   
    

	// Use this for initialization
	void Start () {
        width = Screen.width;
        height = Screen.height;
        Screen.SetResolution(1366, 768, true);
        //Screen.SetResolution(width, height, true);
        isLandscape = true;
        BaseRes = Screen.currentResolution;
       // Debug.Log("BAse Resolution " + BaseRes);
        myCamera = Camera.main.GetComponent<Transform>();
      //  myPanels = GameObject.FindGameObjectWithTag("PanelsHolder").GetComponent<Transform>();
        myCanvasScale = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        usedResolution.text = width.ToString() + " x " + height.ToString();
        //myCanvasScale.referenceResolution = new Vector2(width, height);
        //Debug.Log("Canvas Scale Reference = " + myCanvasScale.referenceResolution);
      
		
	}

   // Update is called once per frame

    void Update()
    {
        width = Screen.width;
        height = Screen.height;
        if (width <= height && !isPortrait)
        {
            //Screen.SetResolution(height, width, true);
            Screen.SetResolution(768, 1366, true);
            myCamera.rotation = Quaternion.Euler(0, 0, 90);
            myCanvasScale.referenceResolution = new Vector2(768, 1366);
           // myCanvasScale.referenceResolution = new Vector2(height, width);
            isPortrait = true;
            isLandscape = false;
        }

        if (height <= width && !isLandscape)
        {
            //Screen.SetResolution(width, height, true);
            Screen.SetResolution(1366, 768, true);
            myCamera.rotation = Quaternion.Euler(0, 0, 0);
            myCanvasScale.referenceResolution = new Vector2(1366, 768);
            //myCanvasScale.referenceResolution = new Vector2(width, height);
            isLandscape = true;
            isPortrait = false;
          

        }

    }
}
