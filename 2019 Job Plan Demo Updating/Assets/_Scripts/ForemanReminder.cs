using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForemanReminder : MonoBehaviour {
    public static bool foremanChosen = false;
    DropdownID[] dropdowns;
    PageController pagecontroller;



	// Use this for initialization
	void Start () {
        dropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
        pagecontroller = GameObject.FindObjectOfType<PageController>();
		
	}
    public void SelectName()
    {
        int myddValue = gameObject.GetComponentInChildren<Dropdown>().value;
        foreach (DropdownID thisDropdown in dropdowns) {
            if (!pagecontroller.isEmergency)
            {
                if (thisDropdown.objID == 15)
                {
                    thisDropdown.GetComponent<Dropdown>().value = myddValue;
                    foremanChosen = true;
                }
            }
            if (pagecontroller.isEmergency)
            {
                if (thisDropdown.objID == 1)
                {
                    thisDropdown.GetComponent<Dropdown>().value = myddValue;
                    foremanChosen = true;
                }
            }
        }
        Destroy(gameObject);

    }
	
	
}
