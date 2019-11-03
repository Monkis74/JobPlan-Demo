using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// the behaviour of the buttons that are in the load file list.

public class LoadFileButton : MonoBehaviour {

    SaveFile saveFile;    
    GameObject loadPanel;

    // Use this for initialization
    void Start()
    {
        saveFile = GameObject.FindObjectOfType<SaveFile>();
        loadPanel = GameObject.Find("Canvas/LoadPanel");

    }
    public void OnClick()
    {
        string myText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
       // Debug.Log(myText);
        saveFile.SetLoadName(myText);
        LoadFileCenterSnap.loadPanelShowing = false;
       // loadPanel.SetActive(false);
        
        //Debug.Log("Button was clicked");
    }


}