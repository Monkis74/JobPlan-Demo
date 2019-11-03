using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// the behaviour of the scroll and snapping of the saved file list on the save panel.

public class SaveFileCenterSnap : MonoBehaviour {

    public RectTransform panel; // hold the scrolling panel.
    public RectTransform hourList; // the parent object that holds all the hours
   
    public RectTransform center; // the center point to campre to and snap to
    public GameObject savedFileButton;
    GameObject saveListParent;
    SavedFileButton[] saveFileGO;
    float[] distance; // hold the hours objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    int imageDistance; // holods the distance between each hours text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myHour;
    public bool savePanelShowing = false;
    string[] savedFiles;
    bool filesDestroyed = false;

    // Use this for initialization
    void Start () {

        saveListParent = GameObject.FindGameObjectWithTag("SaveList");
        
		
	}

    public void GetSaves(string dataPath)
    { //populate save slots with save files.
        savedFiles = Directory.GetFiles(dataPath, "*.dat");
        // add stuff to populate save slots...
    }

    public void GetSavedFileButtons() {
       // Debug.Log("Getting saved file buttons");
        saveFileGO = FindObjectsOfType<SavedFileButton>();
        //Debug.Log(saveFileGO.Length);
        distanceReposition = new float[saveFileGO.Length];
        distance = new float[saveFileGO.Length];
        if (saveFileGO.Length > 1)
        {
            imageDistance = (int)Mathf.Abs(saveFileGO[1].GetComponent<RectTransform>().anchoredPosition.y - saveFileGO[0].GetComponent<RectTransform>().anchoredPosition.y);
        }
        if (saveFileGO.Length <= 1)
        {
            imageDistance = 0;
        }


        savePanelShowing = true;
        }

    public void StartRepopulate(string dataPath)
    {
        StartCoroutine(RepopulateFiles(dataPath));
    }

    public IEnumerator RepopulateFiles(string dataPath)
    {
        //Debug.Log("1");
        savePanelShowing = false;
        DestroyFileList();
        yield return new WaitUntil(() => filesDestroyed == true);
        filesDestroyed = false;
        //Debug.Log("Passed file destruction");

        GetSaves(dataPath);

        foreach (string thisFile in savedFiles)
        { //loop through all save files and add to file list.
            GameObject thisButton = Instantiate(savedFileButton) as GameObject;
            thisButton.transform.SetParent(saveListParent.transform);
            thisButton.transform.localScale = new Vector3(1, 1, 1);
            thisButton.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(thisFile);


        }
        GetSavedFileButtons();



    }

    public void DestroyFileList()
    {
        //Debug.Log(" Destroying delete File List");
        DeleteFileButton[] deleteButtons = FindObjectsOfType<DeleteFileButton>();
        if (deleteButtons.Length > 0)
        {
            foreach (DeleteFileButton thisButton in deleteButtons)
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
        if (savePanelShowing && saveFileGO.Length > 0)
        {
           // Debug.Log("saveFileGO Length " + saveFileGO.Length);
            for (int i = 0; i < saveFileGO.Length; i++)
            {
               // Debug.Log(saveFileGO[i].name);
                distanceReposition[i] = center.GetComponent<RectTransform>().position.y - saveFileGO[i].GetComponent<RectTransform>().position.y;
                //distance[i] = Mathf.Abs(center.transform.position.y - saveFileGO[i].transform.position.y);
                distance[i] = Mathf.Abs(distanceReposition[i]);
            }

            float minDistance = Mathf.Min(distance); // get the minimum distance in the array

            for (int a = 0; a < saveFileGO.Length; a++)
            {
                if (minDistance == distance[a])
                {
                    mintextnum = a;                     // sets the number for the game object that is closest to the center.
                }
            }

            if (!dragging)
            {
                //LerpTosaveFileGO(mintextnum * imageDistance, 10f);
                LerpTosaveFileGO(-saveFileGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
            }



        }
    }

    void LerpTosaveFileGO(float position, float speed)
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
