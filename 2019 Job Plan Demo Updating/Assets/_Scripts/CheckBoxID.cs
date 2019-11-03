using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//[ExecuteInEditMode]
public class CheckBoxID : MonoBehaviour, IUpdateSelectedHandler {

    // provide a unique id number to reference the checkboxes for saving and loading behaviours.

    public int objID; // actual object id of checkbox
    public int numOfCb; // total number of checkboxes in the project
    public CheckBoxID[] amount; // array of all the checkboxes
    public bool isTaken = false; // check if the number of this objID is already assigned
    public bool dupeExists = false; // check if a duplicate of this objID exists. (same as isTaken)

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
    //void Start()
    //{
    //    amount = Resources.FindObjectsOfTypeAll<CheckBoxID>();
    //    numOfCb = amount.Length;

    //}

    //void Update() {

    //    foreach (CheckBoxID thisCB in amount)
    //    {
    //        if (thisCB.objID == objID && thisCB.GetInstanceID() != this.GetInstanceID())
    //        {
    //            isTaken = true;
    //            Text myText = GetComponentInChildren<Text>();
    //            myText.color = Color.red;


    //            dupeExists = true;
    //        }
    //        if (thisCB.objID == objID && thisCB.GetInstanceID() == this.GetInstanceID())
    //        {
    //            isTaken = false;
    //            Text myText = GetComponentInChildren<Text>();
    //            myText.color = Color.black;


    //        }
    //    }
    //}
}
