using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class OBJIDController : MonoBehaviour {

    public int maxDD;
    public int maxCB;
    public int maxIF;
    public int maxTimes;
    public int maxDates;
    public Text ddTExt;
    public Text cbText;
    public Text ifText;
    public Text timeText;
    public Text dateText;
    DropdownID[] alldropdowns;
    CheckBoxID[] allcbs;
    InputFieldID[] allifs;
    TimeID[] alltimes;
    GetDate[] alldates;
    // Use this for initialization
    void Start () {

        
    }

    int? MaxDropdownID() {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
        //int index = -1;
       
        alldropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();

        for (int i = 0; i < alldropdowns.Length; i++)
        {
            int thisNum = alldropdowns[i].objID;
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
               // Selection.activeGameObject = alldropdowns[i].gameObject;
              //  index = i;
            }
        }
        return maxVal;
    }

    int? MaxCBID()
    {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
                            //int index = -1;

        allcbs = Resources.FindObjectsOfTypeAll<CheckBoxID>();
        for (int i = 0; i < allcbs.Length; i++)
        {
            int thisNum = allcbs[i].objID;
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                // Selection.activeGameObject = alldropdowns[i].gameObject;
                //  index = i;
            }
        }
        return maxVal;
    }
    int? MaxIFID()
    {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
                            //int index = -1;

        allifs = Resources.FindObjectsOfTypeAll<InputFieldID>();
        for (int i = 0; i < allifs.Length; i++)
        {
            int thisNum = allifs[i].objID;
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                // Selection.activeGameObject = alldropdowns[i].gameObject;
                //  index = i;
            }
        }
        return maxVal;
    }

    int? MaxTimeID()
    {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
                            //int index = -1;

        alltimes = Resources.FindObjectsOfTypeAll<TimeID>();
        for (int i = 0; i < alltimes.Length; i++)
        {
            int thisNum = alltimes[i].objID;
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                // Selection.activeGameObject = alldropdowns[i].gameObject;
                //  index = i;
            }
        }
        return maxVal;
    }
    int? MaxDateID()
    {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
                            //int index = -1;

        alldates = Resources.FindObjectsOfTypeAll<GetDate>();
        for (int i = 0; i < alldates.Length; i++)
        {
            int thisNum = alldates[i].objID;
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                // Selection.activeGameObject = alldropdowns[i].gameObject;
                //  index = i;
            }
        }
        return maxVal;
    }

    // Update is called once per frame
    //void Update () {
    //    maxDD = (int)MaxDropdownID();
    //    ddTExt.text = "Highest DD:" + (maxDD.ToString()) + "/" + alldropdowns.Length.ToString() ;
        
    //    maxCB = (int)MaxCBID();
    //    cbText.text = "Highest CB:" + (maxCB.ToString()) + "/" + allcbs.Length.ToString();
    //    maxIF = (int)MaxIFID();
    //    ifText.text = "Highest IF:" + (maxIF.ToString()) + "/" + allifs.Length.ToString();
    //    maxTimes = (int)MaxTimeID();
    //    timeText.text = "Higest TimeID:" + (maxTimes.ToString()) + "/" + alltimes.Length.ToString();
    //    maxDates = (int)MaxDateID();
    //    dateText.text = "Highest DateID:" + (maxDates.ToString()) + "/" + alldates.Length.ToString();

    //}
}
