using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

// control the scrolling and snapping of the load file list.
// control the populating of the load file list for the path specified.

public class LoadFileCenterSnap : MonoBehaviour {
    public GameObject loadFileButton;
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform hourList; // the parent object that holds all the hours
   
    public RectTransform center; // the center point to campre to and snap to
    string[] savedFiles;
    LoadFileButton[] loadFileGO;
    float[] distance; // hold the hours objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    int textDistance; // holods the distance between each hours text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myHour;
    public static bool loadPanelShowing = false;
    bool filesDestroyed = false;
   

    // Use this for initialization
    void Start () {

        
		
	}

    // populate the load list from the specified path.
    // create a button object for each loadbale file if any.
    public IEnumerator RepopulateList(string dataPath) {
        loadPanelShowing = false;
        DestroyFileList();
        yield return new WaitUntil(() => filesDestroyed == true);
        filesDestroyed = false;
        GameObject loadParent = GameObject.FindGameObjectWithTag("LoadList");
        GetSaves(dataPath);

        foreach (string thisFile in savedFiles)
        { //loop through all save files and add to file list.
            GameObject thisButton = Instantiate(loadFileButton) as GameObject;
            thisButton.transform.SetParent(loadParent.transform);
            thisButton.transform.localScale = new Vector3(1, 1, 1);
            thisButton.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(thisFile);           
        }
        GetLoadFileButtons();
    }

    //populate save slots with save files.
    public void GetSaves(string dataPath)
    {
        // need if statement for archives vs submitted.
        if (dataPath.Contains("Archive"))
        {
            savedFiles = Directory.GetFiles(dataPath, "*.pdf");
        }
        else
        {
            savedFiles = Directory.GetFiles(dataPath, "*.dat");
        }
       // Debug.Log(savedFiles.Length);
        // add stuff to populate save slots...
    }

    // find the amount of buttons in the list to control snapping and scrolling.
    public void GetLoadFileButtons()
    {
        if (savedFiles.Length > 0)
        {
            loadFileGO = FindObjectsOfType<LoadFileButton>();
           // Debug.Log(loadFileGO.Length);
            distanceReposition = new float[loadFileGO.Length];
            distance = new float[loadFileGO.Length];
            if (loadFileGO.Length > 1)
            {
                textDistance = (int)Mathf.Abs(loadFileGO[1].GetComponent<RectTransform>().anchoredPosition.y - loadFileGO[0].GetComponent<RectTransform>().anchoredPosition.y);
            }
            if (loadFileGO.Length > 1)
            {
                textDistance = 0;
            }

            loadPanelShowing = true;
        }
    }

    // destroy load file buttons when called.
    public void DestroyFileList()
    {
        //Debug.Log(" Destroying delete File List");
        LoadFileButton[] loadButtons = FindObjectsOfType<LoadFileButton>();
        if (loadButtons.Length > 0)
        {
            foreach (LoadFileButton thisButton in loadButtons)
            {
                // Debug.Log("Destroying " + thisButton.name);
                Destroy(thisButton.gameObject);
            }
        }
        filesDestroyed = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (loadPanelShowing && loadFileGO.Length > 0)
        {

            for (int i = 0; i < loadFileGO.Length; i++)
            {
                distanceReposition[i] = center.GetComponent<RectTransform>().position.y - loadFileGO[i].GetComponent<RectTransform>().position.y;
                //distance[i] = Mathf.Abs(center.transform.position.y - loadFileGO[i].transform.position.y);
                distance[i] = Mathf.Abs(distanceReposition[i]);
            }

            float minDistance = Mathf.Min(distance); // get the minimum distance in the array

            for (int a = 0; a < loadFileGO.Length; a++)
            {
                if (minDistance == distance[a])
                {
                    mintextnum = a;                     // sets the number for the game object that is closest to the center.
                }
            }

            if (!dragging)
            {
                //LerpToloadFileGO(mintextnum * textDistance, 10f);
                LerpToloadFileGO(-loadFileGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
            }



        }
    }

    void LerpToloadFileGO(float position, float speed)
    {

        float newY = Mathf.Lerp(hourList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(hourList.anchoredPosition.x, newY);

        hourList.anchoredPosition = newPos;
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
}
