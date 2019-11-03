using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOffID : MonoBehaviour {

    public int objID;
    GameObject child;

	// Use this for initialization
	void Start () {
        child = gameObject.transform.GetChild(1).gameObject;
        child.SetActive(false);
       // Debug.Log(child.name);
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SaveFile.loadedForSignoff) {
            child.SetActive(true);
        }
        if (!SaveFile.loadedForSignoff) {
            child.SetActive(false);
        }
		
	}
}
