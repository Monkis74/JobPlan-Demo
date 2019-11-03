using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// the behaviour of the buttons that are on the saved file list.

public class SavedFileButton : MonoBehaviour {

    SaveFile saveFile; // the savefile script.

	// Use this for initialization
	void Start () {
        saveFile = GameObject.FindObjectOfType<SaveFile>();

	
	}
    public void OnClick() { // save Full File
        string myText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        saveFile.ShowOverwriteSavePanel(myText);
        //Debug.Log("Button was clicked");
    }

 
    // Update is called once per frame
    void Update () {
	
	}
}
