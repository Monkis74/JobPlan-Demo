using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteFileButton : MonoBehaviour {

    //GameObject deleteConfirmPanel; // the panel holding the confirm delete (the selected file) dialogue.
    SaveFile saveFile; // the savefile script
    DeleteFileCenterSnap mycentersnap; // the center snap for the list to scroll and snap to.

    // Use this for initialization
    void Start () {
       // deleteConfirmPanel = GameObject.Find("Canvas/DeletePanel/DeleteConfirmPanel");
        saveFile = FindObjectOfType<SaveFile>();
        mycentersnap = FindObjectOfType<DeleteFileCenterSnap>();


    }

    // the behaviour when a deletable save file button is clicked.
    public void OnClick()
    {
        string myText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        saveFile.ShowDeleteFileConfirmation(myText);
        mycentersnap.deleteListActive = false;

    }

 
}
