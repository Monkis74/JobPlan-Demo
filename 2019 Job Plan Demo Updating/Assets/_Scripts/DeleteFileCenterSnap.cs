using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class DeleteFileCenterSnap : MonoBehaviour {
    string[] savedFiles; // array of the saved files in question to select for deletion.
    public GameObject deleteFileButton; // the button that the file will be tied to, when clicked see the deletefilebutton script.
    GameObject deleteParent; // the parent of the file list 
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform deleteList; // the parent object that holds all the hours
    public RectTransform center; // the center point to campre to and snap to
    DeleteFileButton[] deleteFileGO;
    float[] distance; // hold the hours objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
   // int imageDistance; // holods the distance between each hours text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public bool deleteListActive = false;
    bool filesDestroyed = false;

    // Use this for initialization
    void Start () {
        
         deleteParent = GameObject.FindGameObjectWithTag("DeleteList");
        
    }

    // find the saves in the path specified. 
    public void GetSaves(string dataPath)
    { //populate save slots with save files.
        savedFiles = Directory.GetFiles(dataPath, "*.dat");
        // add stuff to populate save slots...
    }

    // find the delete file buttons to have the list scroll and snap properly.
    public void GetDeleteFileButtons()
    {

        deleteFileGO = FindObjectsOfType<DeleteFileButton>();
       // Debug.Log(deleteFileGO.Length);
        distanceReposition = new float[deleteFileGO.Length];
        distance = new float[deleteFileGO.Length];
        //if (deleteFileGO.Length > 1)
        //{
        //    imageDistance = (int)Mathf.Abs(deleteFileGO[1].GetComponent<RectTransform>().anchoredPosition.y - deleteFileGO[0].GetComponent<RectTransform>().anchoredPosition.y);
        //}
        //if (deleteFileGO.Length <= 1)
        //{
        //    imageDistance = 0;
        //}
        deleteListActive = true;
        
    }


    // start the list repopulation when any changes are made to it, eg: a file was deleted.
    public void StartRepopulate(string dataPath) {
        StartCoroutine(RepopulateFiles(dataPath));
    }

    public IEnumerator RepopulateFiles(string dataPath) {
        //Debug.Log("1");
        deleteListActive = false;
        DestroyFileList();
        yield return new WaitUntil(() => filesDestroyed == true);
        filesDestroyed = false;
        //Debug.Log("Passed file destruction");
        
        GetSaves(dataPath);

        foreach (string thisFile in savedFiles)
        { //loop through all save files and add to file list.
            GameObject thisButton = Instantiate(deleteFileButton) as GameObject;
            thisButton.transform.SetParent(deleteParent.transform);
            thisButton.transform.localScale = new Vector3(1, 1, 1);
            thisButton.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(thisFile);


        }
        GetDeleteFileButtons();



    }

    // destroy the list to enable repopulation to new list after changes were made.
    public void DestroyFileList()
    {
         DeleteFileButton[] deleteButtons = FindObjectsOfType<DeleteFileButton>();
        if (deleteButtons.Length > 0)
        {
            foreach (DeleteFileButton thisButton in deleteButtons)
            {
                Destroy(thisButton.gameObject);
            }
        }
        filesDestroyed = true;

    }


    // Update is called once per frame
    void Update() {
        if (deleteListActive && deleteFileGO.Length > 0) {
            for (int i = 0; i < deleteFileGO.Length; i++)
            {
                distanceReposition[i] = center.GetComponent<RectTransform>().position.y - deleteFileGO[i].GetComponent<RectTransform>().position.y;
                //distance[i] = Mathf.Abs(center.transform.position.y - deleteFileGO[i].transform.position.y);
                distance[i] = Mathf.Abs(distanceReposition[i]);
            }

            float minDistance = Mathf.Min(distance); // get the minimum distance in the array

            for (int a = 0; a < deleteFileGO.Length; a++)
            {
                if (minDistance == distance[a])
                {
                    mintextnum = a;                     // sets the number for the game object that is closest to the center.
                }
            }

            if (!dragging)
            {
                //LerpTodeleteFileGO(mintextnum * imageDistance, 10f);
                if (deleteFileGO.Length > 0) {
                    LerpTodeleteFileGO(-deleteFileGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
                }
            }



        }
    }


void LerpTodeleteFileGO(float position, float speed)
{

    float newY = Mathf.Lerp(deleteList.anchoredPosition.y, position, Time.deltaTime * speed);
    Vector2 newPos = new Vector2(deleteList.anchoredPosition.x, newY);

    deleteList.anchoredPosition = newPos;
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

