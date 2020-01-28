using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

// populate the dropdown lists for each type of tagged dropdown outlined below.

public class GetDropdownNames : MonoBehaviour {
    List<string> namesList = new List<string>();
    List<string> truckList = new List<string>();
    List<string> superList = new List<string>();
    List<string> feederList = new List<string>();    
    string sharepointPath;
    string path;
    Dropdown myDropdown;

	// Use this for initialization
	 void Start()
    {
        sharepointPath = SaveFile.sharepointPath;
        myDropdown = this.gameObject.GetComponent<Dropdown>();
        if (!File.Exists(sharepointPath + "/Job Plans/FormData/CrewNames.txt")) {
            return;
        }        
        if (gameObject.CompareTag("Names"))
        {
            //path = "C://JobPlanTempFiles/CrewNames.txt";
            path = sharepointPath + "/Job Plans/FormData/CrewNames.txt";
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                namesList.Add(s);
            }
            //myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(namesList);
            readText = null;
        }
        if (gameObject.CompareTag("Trucks")) {
            // path = "C://JobPlanTempFiles/TruckNumbers.txt";
            path = sharepointPath + "/Job Plans/FormData/TruckNumbers.txt";
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                truckList.Add(s);
            }
            //myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(truckList);
            readText = null;
        }
        if (gameObject.CompareTag("Supervisor"))
        {
            //path = "C://JobPlanTempFiles/Supervisors.txt";
            path = sharepointPath + "/Job Plans/FormData/Supervisors.txt";
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                superList.Add(s);
            }
            //myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(superList);
            readText = null;
        }
        if (gameObject.CompareTag("Feeder"))
        {
            //path = "C://JobPlanTempFiles/Feeders.txt";
            path = sharepointPath + "/Job Plans/FormData/Feeders.txt";
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                feederList.Add(s);
            }
            //myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(feederList);
            readText = null;

        }
        if (gameObject.CompareTag("SWP")) {
            // path = sharepointPath + "/Job Plans/FormData/OP-SWP.txt";
            path = sharepointPath + "/_Safe Work Practices";
            // string[] readText = File.ReadAllLines(path);
            string[] readText = Directory.GetFiles(path);
            for (int i = 0; i < readText.Length; i++)
            {
                readText[i] = Path.GetFileNameWithoutExtension(readText[i]);
            }
            foreach (string s in readText)
            {                
                feederList.Add(s);
            }
           // myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(feederList);
            readText = null;
        }
        if (gameObject.CompareTag("Duties"))
        {
            path = sharepointPath + "/Job Plans/FormData/ShortDuties.txt";
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                feederList.Add(s);
            }
           // myDropdown = this.gameObject.GetComponent<Dropdown>();
            myDropdown.AddOptions(feederList);
            readText = null;
        }
        
        System.GC.Collect();


    }

    // Update is called once per frame
    void Update () {
		
	}
}
