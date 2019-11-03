using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingScript : MonoBehaviour {

    DropdownID[] alldropdowns;
    CheckBoxID[] allcbs;
    InputFieldID[] allifs;
   // TimeID[] alltimes;
    //GetDate[] alldates;
    bool runtest = false;

    // Use this for initialization
    void Start () {

        alldropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
        allcbs = Resources.FindObjectsOfTypeAll<CheckBoxID>();
        allifs = Resources.FindObjectsOfTypeAll<InputFieldID>();
        
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.T) && !runtest) {
            RunTest();
        }
		
	}

    void RunTest() {
        runtest = true;
        for (int i = 0; i < allifs.Length; i++) {
            foreach (InputFieldID thisID in allifs) {
                if (thisID.objID == i) {
                    thisID.GetComponent<InputField>().text = i.ToString();
                   // i++;
                }
            }
        }
    }
}
