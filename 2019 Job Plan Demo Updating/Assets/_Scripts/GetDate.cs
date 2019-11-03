using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// give a unique ID to each date field in the project.

public class GetDate : MonoBehaviour {
    public int objID; // ID number of each date 
    DateTimeController myDate; // datetimecintroller script reference.
    Text myText; // text component of the date field


	// Use this for initialization
	void Start () {
        myDate = GameObject.FindObjectOfType<DateTimeController>();
        myText = GetComponent<Text>();
        SetDate();
		
	}

    void SetDate() {
        myText = GetComponent<Text>();
        myText.text = DateTimeController.date.ToString();
      
    }

    // set a new date from the date picker panel.
    public void GetNewDate() {
        StartCoroutine(SetNewDate());
    }

    public IEnumerator SetNewDate() { // wait until date picker is accepted
        yield return new WaitUntil(() => DateTimeController.newDateSet == true);
        if (DateTimeController.dateCancelled)
        {
            DateTimeController.dateCancelled = false;
            DateTimeController.newDateSet = false;
            yield break;
        }
        SetDate();
        DateTimeController.newDateSet = false;
    }

    public void ResetDate() {
        myText = GetComponent<Text>();
        myText.text = System.DateTime.Now.ToString("yyyy-MMM-dd");
    }

    
    
    // Update is called once per frame
	void Update () {
		
	}
}
