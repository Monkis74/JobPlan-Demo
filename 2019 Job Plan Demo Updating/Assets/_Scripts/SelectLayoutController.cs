using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class SelectLayoutController : MonoBehaviour {

    public RawImage myImage; // the rawimage on this page to set a layout to.
    public GameObject tlpickerPanel; // the parent gameobject to control the enabled disabled of the panel.
    TLPicker tlpicker; //thegameobject that has the tlpicker script attached.
    CustomLayoutCapture customLayoutPanel;
    public GameObject customPanel;
    public Transform myParent;



	// Use this for initialization
	void Start () {
       // tlpicker = Resources.FindObjectsOfTypeAll<TLPicker>(); // there is only one, using this format to find it if it's disabled.
       // tlpickerPanel = tlpicker[0].transform.parent.parent.gameObject;
       // customLayoutPanel = Resources.FindObjectsOfTypeAll<CustomLayoutCapture>();
        //customPanel = customLayoutPanel[0].gameObject;
        
		
	}
    public void StartLayoutChoice() {      // start the coroutine listed by a button press.
        StartCoroutine(ChooseMTOlayout());
    }

    public IEnumerator ChooseMTOlayout() {  // start the mto layout panel picker and wait until a layout is picked.
                                            // Debug.Log(tlpicker[0].name);
                                            // tlpickerPanel.gameObject.SetActive(true);
        GameObject NewTLPickerPanel = Instantiate(tlpickerPanel) as GameObject;
        NewTLPickerPanel.transform.SetParent(myParent, false);
        tlpicker = GameObject.FindObjectOfType<TLPicker>(); // there is only one, using this format to find it if it's disabled.

        tlpicker.SetPanelToPopulate(myImage);
        yield return new WaitUntil(() => tlpicker.layoutChosen == true);
        tlpicker.layoutChosen = false;
        // tlpickerPanel.gameObject.SetActive(false);
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Resources.UnloadUnusedAssets();

    }
    public void StartCustomLayout() {
        StartCoroutine(ChooseCustomLayout());
    }


    public IEnumerator ChooseCustomLayout() {
        // customPanel.SetActive(true);
        GameObject NewCustomPanel = Instantiate(customPanel);
        NewCustomPanel.name = "CustomLayoutPanel";
        NewCustomPanel.transform.SetParent(myParent, false);
        customLayoutPanel = GameObject.FindObjectOfType<CustomLayoutCapture>();

        customLayoutPanel.SetPanelToPopulate(myImage);
        yield return new WaitUntil(() => customLayoutPanel.layoutChosen == true);
        customLayoutPanel.layoutChosen = false;
        Destroy(NewCustomPanel.gameObject);

        //customPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
