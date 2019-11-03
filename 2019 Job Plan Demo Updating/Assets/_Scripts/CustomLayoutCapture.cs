using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// take a screen capture of the custom layout page traffic layout built by the user.

public class CustomLayoutCapture : MonoBehaviour
{

    public RectTransform myPanelRect; // the rect of the layout panel to capture.
                                      // SelectLayoutController layoutController; // the controller which applies the chosen layout
    public float guix; // x pos of mypanelrect
    public float guiy; // y pos of mypanelrect
    public float guiw; // width of mypanelrect
    public float guih; // height of mypanelrect
    public bool layoutChosen = false; // test wheher a lyout has been applied (chosen) yet or not
    public RawImage imageToReceive; // the layout placeholder on the page2 traffic plan page
    LineRenderer boxLine;
    Vector3[] myLinesArray = new Vector3[5];

    // Use this for initialization
    void Start()
    {
        guiw = myPanelRect.rect.width;
        guih = myPanelRect.rect.height;
        Debug.Log(myPanelRect.rect);
        Vector3 rectScreenSpace = Camera.main.WorldToScreenPoint(myPanelRect.position);
        //guix = Screen.width / 2 + myPanelRect.rect.xMin;
        guix = (rectScreenSpace.x + myPanelRect.rect.xMin);
        guiy = (rectScreenSpace.y - guih / 2);
        boxLine = GameObject.FindGameObjectWithTag("BoxLine2").GetComponent<LineRenderer>();
        //SetLinePositions();
        //boxLine.SetPositions(myLinesArray);

        // layoutController = FindObjectOfType<SelectLayoutController>();
    }

    public void SetPanelToPopulate(RawImage myImage)
    { // call with button to select a layout from traffic control page.
        imageToReceive = myImage;
        //  Debug.Log(myImage.name);
    }
    public void StartlayoutCapture()
    { // used to start the coroutine below easily.
        StartCoroutine(TakeLayoutCap());
    }

    public IEnumerator TakeLayoutCap()
    { // take a capture of the created custom traffic layout on myPanelRect
        PartManipulator.itemToMove = null;
        Rect myLayoutRect = new Rect(guix, guiy, guiw, guih);
        yield return new WaitForEndOfFrame();
        Texture2D myTex = new Texture2D((int)guiw, (int)guih);
        myTex.ReadPixels(myLayoutRect, 0, 0);
        myTex.Apply();
        imageToReceive.texture = myTex;
        layoutChosen = true;
        ChosenLayout chosenLayout = GameObject.FindObjectOfType<ChosenLayout>();
        chosenLayout.SetImage(myTex);

    }
    public void SetLinePositions() // set vertice points for signature outline box;
    {
        Vector3 array0 = new Vector3(guix, guiy, 10);
        myLinesArray[0] = Camera.main.ScreenToWorldPoint(array0);
        Vector3 array1 = new Vector3(guix, guiy + guih, 10);
        myLinesArray[1] = Camera.main.ScreenToWorldPoint(array1);
        Vector3 array2 = new Vector3(guix + guiw, guiy + guih, 10);
        myLinesArray[2] = Camera.main.ScreenToWorldPoint(array2);
        Vector3 array3 = new Vector3(guix + guiw, guiy, 10);
        myLinesArray[3] = Camera.main.ScreenToWorldPoint(array3);
        Vector3 array4 = new Vector3(guix, guiy, 10);
        myLinesArray[4] = Camera.main.ScreenToWorldPoint(array4);
    }
}
