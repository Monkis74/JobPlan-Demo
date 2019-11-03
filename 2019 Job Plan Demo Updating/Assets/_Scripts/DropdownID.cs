using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// apply a unique id used for saving and loading the state of dropdowns in the project.
[ExecuteInEditMode]
public class DropdownID : MonoBehaviour, IUpdateSelectedHandler {

    public int objID; // actual ID of a given dropdown.
    public int numOfDd; // total number of dropdowns in project
    public DropdownID[] amount; // array of all the dropdowns
    public bool isTaken = false; // test whether the Id number is already taken.

    public void OnUpdateSelected(BaseEventData data) {
        //Debug.Log("Dropdown updated");
        if (SaveFile.loadedForSignoff)
        {
            GetComponentInChildren<Text>().color = Color.red;
           // Debug.Log("Using red font");
            return;
        }
        else { GetComponentInChildren<Text>().color = Color.black;
           // Debug.Log("Using black font");
        }
    }

    //void Start()
    //{
    //    amount = FindObjectsOfType<DropdownID>();
    //    numOfDd = amount.Length;
    //    //foreach (DropdownID thisDropdown in amount)
    //    //{
    //    //    //Debug.Log(thisDropdown.name + " " + objID);
    //    //}



    //    }

    //    void Update() {

    //    foreach (DropdownID thisDropdown in amount)
    //        {
    //        //Debug.Log(thisDropdown.name + " " + objID);
    //        if (thisDropdown.objID == objID && thisDropdown.GetInstanceID() != this.GetInstanceID())
    //        {
    //            isTaken = true;
    //            GetComponent<Image>().color = Color.red;
                
    //        }
    //        if (thisDropdown.objID == objID && thisDropdown.GetInstanceID() == this.GetInstanceID()) {
    //            isTaken = false;
    //            GetComponent<Image>().color = Color.white;
    //        }
    //    }
    //}
}
