using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class TLPicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform imageList; // the parent object that holds all the images
    public GameObject layoutImage; // the gameobject prefab of each layout object.
    public RectTransform center; // the center point to campre to and snap to
    string[] layouts; // array pf all the layouts in the folder.
    List<string> layoutStrings = new List<string>();
    string folderPath;
    GameObject[] layoutGO;
    float[] distance; // hold the minutes objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    //int textDistance; // holods the distance between each minutes text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myLayout;
    public RawImage imageToReceive;
    public bool layoutChosen = false; // test whether a layout has been selected to use.



    // Use this for initialization
    void Start()
    {
        string sharepointPath = SaveFile.sharepointPath;
        folderPath = sharepointPath + "/Job Plans/FormData/MTOLayouts";
        layouts = Directory.GetFiles(folderPath, "*.jpg");
        foreach (string thisLayout in layouts) {
            layoutStrings.Add(thisLayout);
        }
        layoutStrings.Sort();
           
        layoutGO = new GameObject[layouts.Length];
        //Debug.Log(layoutGO.Length);
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (string layout in layoutStrings) // create an object for each day in the day list transform.
        {
            GameObject thisLayout = Instantiate(layoutImage, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisLayout.transform.position.x + 30, thisLayout.transform.position.y, thisLayout.transform.position.z);
            RectTransform myAnchor = thisLayout.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            
            thisLayout.transform.SetParent(imageList.transform);
            thisLayout.transform.localScale = new Vector3(1, 1, 1);
            
            thisLayout.name = layout.ToString();
            //Text myText = thisLayout.GetComponent<Text>();
            //thisLayout.AddComponent<RawImage>();
            byte[] myJPG = File.ReadAllBytes(layout);
            Texture2D myTex = new Texture2D(450, 700);
            myTex.LoadImage(myJPG);
            myTex.Apply();
            thisLayout.GetComponent<RawImage>().texture = myTex;
            myTex = null;            
            layoutGO[i] = thisLayout.gameObject;
            myJPG = null;
            i++;
        }
        layoutStrings.Clear();
        layouts = null;


        distance = new float[layoutGO.Length];
        distanceReposition = new float[layoutGO.Length];

        // Get distance between day GO's in y axis
        int GOlast = layoutGO.Length;

        //int GOsecondLast = layoutGO.Length - 1;
        // Debug.Log(GOlast + " " + GOsecondLast);
        //textDistance = (int)Mathf.Abs(layoutGO[1].GetComponent<RectTransform>().anchoredPosition.x - layoutGO[0].GetComponent<RectTransform>().anchoredPosition.x);
        
    }


   

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < layoutGO.Length; i++)
        {
            distanceReposition[i] = center.GetComponent<RectTransform>().position.x - layoutGO[i].GetComponent<RectTransform>().position.x;
            //distance[i] = Mathf.Abs(center.transform.position.y - layoutGO[i].transform.position.y);
            distance[i] = Mathf.Abs(distanceReposition[i]);



        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < layoutGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            //LerpTolayoutGO(mintextnum * textDistance, 10f);
            LerpTolayoutGO(-layoutGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.x, 10f);
        }
        myLayout = layoutGO[mintextnum].name.ToString();


    }

    void LerpTolayoutGO(float position, float speed)
    {

        float newX = Mathf.Lerp(imageList.anchoredPosition.x, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(newX, imageList.anchoredPosition.y);

        imageList.anchoredPosition = newPos;
        // Debug.Log("selected Day " + mintextnum);
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void SetPanelToPopulate(RawImage myImage) { // call with button to select a layout from traffic control page.
        imageToReceive = myImage;
       // Debug.Log(myImage.name);
    }

    public void ConfirmChosenLayout() { // call from TLpicker page button to choose the centered layout to bring to traffic control page.
       
        byte[] chosenPNG = File.ReadAllBytes(myLayout);
        Texture2D chosenTex = new Texture2D(450, 700);
        chosenTex.LoadImage(chosenPNG);
        chosenTex.Apply();
        imageToReceive.texture = chosenTex;
       
        ChosenLayout chosenLayout = GameObject.FindObjectOfType<ChosenLayout>();
        chosenLayout.SetImage(chosenTex);
        //layoutGO = null;
        //chosenPNG = null;
        //distance = null;
        //distanceReposition = null;
        Destroy(imageList.gameObject);
        layoutChosen = true;

    }


}
