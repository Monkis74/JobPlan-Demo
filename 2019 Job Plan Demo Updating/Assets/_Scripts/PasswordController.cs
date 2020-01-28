using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordController : MonoBehaviour
{

    public Button setUpSharepointButton;
    public Button checkSyncFilesButton;
    public Button loadSubmittedPlan;
    public Button loadArchivedPDF;
    public InputField adminPass;
    public InputField supPass;
    List<string> passCharacters = new List<string>();
    InputField focusedField;
    InputField previousFocused;
    string adminPassword = "Sm0k3r";
    string supervisorPassword = "@l3ctr@";



    // Start is called before the first frame update
    void Start()
    {
        loadSubmittedPlan.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        loadArchivedPDF.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        setUpSharepointButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        checkSyncFilesButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableSupButtons()
    {
        if (supPass.text == supervisorPassword)
        {
            loadSubmittedPlan.interactable = true;
            loadArchivedPDF.interactable = true;
            loadSubmittedPlan.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            loadArchivedPDF.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            supPass.text = "";
        }
    }

    public void EnableAdminButtons()
    {
        if (adminPass.text == adminPassword)
        {
            setUpSharepointButton.interactable = true;
            checkSyncFilesButton.interactable = true;
            setUpSharepointButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            checkSyncFilesButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            adminPass.text = "";
        }
    }
   

   



}
