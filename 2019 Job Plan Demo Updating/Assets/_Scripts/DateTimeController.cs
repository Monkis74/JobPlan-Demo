using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.EventSystems;
  
// controller for the main date and time of Page1 and the EmergencyPage.

public class DateTimeController : MonoBehaviour {
    public static string date;
    public static string time;
    public static bool newDateSet = false; // check to see if date was changed with the date picker
    public static bool newTimeSet = false; // check to see if time was changed with time picker
    public static bool dateCancelled = false;
    public static bool timeCancelled = false;
    public GameObject datePicker;
    public GameObject timePicker;
    Transform myParent;


	void Start () { // set the date and time to now.
       // datePicker.SetActive(false);
       // timePicker.SetActive(false);
        date = System.DateTime.Now.ToString("yyyy-MMM-dd");
        time = System.DateTime.Now.ToString("HH:mm");
        //Debug.Log(time);
        myParent = GameObject.Find("Canvas").transform;
		
	}
    // used to open the date picker panel
    public void OpenDatePicker() {
        // datePicker.SetActive(true);
        GameObject NewDatePicker = Instantiate(datePicker);
        NewDatePicker.transform.SetParent(myParent, false);
        NewDatePicker.SetActive(true);

    }
    
    // called from datepicker accept date button  to set a newly selected date.
    public void GetNewDate() {
        date = YearPicker.myYear + "-" + MonthPicker.myMonthString + "-" + DayPicker.myDay;
        newDateSet = true;
      //  Debug.Log("New date " + date);
      // datePicker.SetActive(false);
   
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);

   
}
    
    // open the timepicker panel
    public void OpenTimePicker() {
        // timePicker.SetActive(true);
        Debug.Log("Opening Time Picker");
        GameObject NewTimePicker = Instantiate(timePicker);
        NewTimePicker.transform.SetParent(myParent, false);
        NewTimePicker.SetActive(true);
    }

    // called form the timepicker Accept new time button to set newly selected time.
    public void GetNewTime() {
        time = HourPicker.myHour + ":" + MinutePicker.myMinute;
        newTimeSet = true;
        //timePicker.SetActive(false);
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    public void CancelTimePicker() {
        timeCancelled = true;
        newTimeSet = true;
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    public void CancelDatePicker() {
        dateCancelled = true;
        newDateSet = true;
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

}
