using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSaveController : MonoBehaviour {
    SaveFile saveFile;
    float saveInterval = 5f;
    float timePassed = 0;
    public static bool continuousSaveActive = false;
    public static bool isSaving = false;

    // Use this for initialization
    void Start () {
        saveFile = FindObjectOfType<SaveFile>();
        
		
	}
	
	// Update is called once per frame
	void Update () {
        timePassed = timePassed + Time.deltaTime;
        //Debug.Log(timePassed);

        if (timePassed >= saveInterval && continuousSaveActive) {
            SaveContinuousSave();
            timePassed = 0;
        }

		
	}

    public void SaveContinuousSave() {
        if (isSaving || !continuousSaveActive)
        {
            return;
        }
        else
        {
            isSaving = true;
          //  Debug.Log("Saving...");
            saveFile.SetDataPath("C://JobPlanContinuousSave");
            SaveFile.saveName = "ContinuousSave";
            saveFile.Save();
        }
    }

    public void LoadContuousSave() {
        saveFile.SetDataPath("C://JobPlanContinuousSave");
        SaveFile.loadName = "ContinuousSave";
        saveFile.LoadFile();
    }
}
