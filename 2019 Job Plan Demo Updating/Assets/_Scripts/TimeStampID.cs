using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//[ExecuteInEditMode]   // enable this to increment the objID number as gameobject is placed in scene. disable when done placing objects.
public class TimeStampID : MonoBehaviour, IUpdateSelectedHandler {
    // Provide a unique number for each time stamp reference on the plan.

    public int objID; // actual ID of this time stamp
    public int numOfTs; // total number of time stamps in the project.
    public TimeStampID[] amount; // Array of all the time stamps in the project.
    public bool isTaken = false; // check if the number of this objID is already assigned 
    public bool dupeExists = false; // check if a duplicate of this objID exists. (same as isTaken)
    public bool isStamped = false;
    Text myStamp; // the text object to timestamp.

    void Start()
    {
        amount = Resources.FindObjectsOfTypeAll<TimeStampID>();
        numOfTs = amount.Length;
        myStamp = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // increment the objects uniwue ID as it is created in the editor. 
        // prefab object has objID of 0.
        //Enable for number incrementing with execute in edit mode above. disable whwn done placing objects.
        //amount = Resources.FindObjectsOfTypeAll<TimeStampID>();
        //foreach (TimeStampID thisID in amount)
        //{
        //    for (int i = 0; i < amount.Length; i++)
        //    {
        //        if (thisID.objID == amount[i].objID)
        //        {
        //            thisID.objID = amount.Length - 1;
        //        }
        //    }

        //}
    }

    public void OnUpdateSelected(BaseEventData data)
    {

        if (SaveFile.loadedForSignoff)
        {
            GetComponentInChildren<Text>().color = Color.red;
            // Debug.Log("Using red font");
            return;
        }
        else
        {
            GetComponentInChildren<Text>().color = Color.black;
            //  Debug.Log("Using black font");
        }
    }

    public void SetStamp()
    {
        //double hour = System.DateTime.Now.Hour;
        //double minute = System.DateTime.;
        string time = System.DateTime.Now.ToString("HH:mm");
        string date = System.DateTime.Today.ToShortDateString();
        myStamp.text = time + "\n" + date;
        isStamped = true;
    }
    
}
