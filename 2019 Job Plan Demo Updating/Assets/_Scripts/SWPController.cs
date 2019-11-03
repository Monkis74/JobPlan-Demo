using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SWPController : MonoBehaviour {
    public Button myButton;
    string pdfFilename;
    string dataPath;

	// Use this for initialization
	void Start () {
        // myButton.gameObject.SetActive(false);
        CheckDDSelection();
        dataPath = SaveFile.sharepointPath;
       // Debug.Log(dataPath);
		
	}

   public void CheckDDSelection() {
       // Debug.Log("Check Dropdown Value Ran");
        if (GetComponent<Dropdown>().value != 0) {
            myButton.gameObject.SetActive(true);
        }
        else { myButton.gameObject.SetActive(false); }
       // Debug.Log(myButton.gameObject.activeSelf);
    }

    public void ViewPDF() {
        pdfFilename = GetComponent<Dropdown>().captionText.text;
        string pdftoView = dataPath + "/_Safe Work Practices/" + pdfFilename + ".pdf";
        //File.Open(pdftoView, FileMode.Open);
        Application.OpenURL(pdftoView);
    }

    private void OnEnable()
    {
        CheckDDSelection();
    }

    // Update is called once per frame
    void Update () {

		
	}
}
