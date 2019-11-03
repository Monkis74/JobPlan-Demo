using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TimeID : MonoBehaviour {

    public int objID;
    public int numOfCb;
    public TimeID[] amount;
    public bool isTaken = false;


    //void Start()
    //{
    //    amount = FindObjectsOfType<TimeID>();
    //    numOfCb = amount.Length;
    //}

    //void Update()
    //{

    //    foreach (TimeID thisTime in amount)
    //    {
    //        if (thisTime.objID == objID && thisTime.GetInstanceID() != this.GetInstanceID())
    //        {
    //            isTaken = true;
    //        }
    //        if (thisTime.objID == objID && thisTime.GetInstanceID() == this.GetInstanceID())
    //        {
    //            isTaken = false;
    //        }
    //    }
    //}
}