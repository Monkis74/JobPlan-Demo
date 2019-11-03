using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SignatureButton : MonoBehaviour {
    GameObject signaturePanel;
    public RawImage mySigImage;
    public static bool capTaken = false;
    RectTransform mySigBack;
    Vector3[] myLinesArray = new Vector3[5];
    SignatureID mySignatureID;

    //public Texture2D guitext;
    //Texture2D thisTexture;
   

    public float guix;
    public float guiy;
    public float guiw;
    public float guih;

    void Start() {
        mySignatureID = GetComponentInParent<SignatureID>();
    }
    
    

    public void StartSigPan() { // used for button to start the coroutine below.
        StartCoroutine(OpenSignaturePanel());
    }

    void Update()
    {
        if (mySigBack != null)
        {
            
            //Debug.Log(new Rect(guix, guiy, guiw, guih));
        }
    }



    public IEnumerator OpenSignaturePanel () {
        
        Transform myParent = this.transform.root;
        signaturePanel = Resources.Load("SignaturePanel") as GameObject;
        GameObject mySig = Instantiate(signaturePanel) as GameObject;
        mySig.transform.SetParent(myParent,false);
        mySigBack = GameObject.FindGameObjectWithTag("SigBackground").GetComponent<RectTransform>();
        guiw = mySigBack.rect.width ;
        guih = mySigBack.rect.height ;
        guix = Screen.width / 2 + mySigBack.rect.xMin;
        // guiy = Screen.height / 2 + (mySigBack.rect.yMin);
        //Vector3 sigBackRect = new Vector3(mySigBack.rect.xMin, mySigBack.rect.xMin, -10);
        Vector3 rectScreenSpace =  Camera.main.WorldToScreenPoint(mySigBack.position);
        guiy = (rectScreenSpace.y - guih/2);
        
        LineRenderer boxLine = GameObject.FindGameObjectWithTag("BoxLine1").GetComponent<LineRenderer>();

        SetLinePositions();
        boxLine.SetPositions(myLinesArray);
        



       // showBox = true;
        yield return new WaitUntil(() => capTaken == true);
        StartCoroutine(TakeSigCap());
        
    

	
	}

   

    public IEnumerator TakeSigCap()
    {
        LineRenderer boxLine = GameObject.FindGameObjectWithTag("BoxLine1").GetComponent<LineRenderer>();
        boxLine.gameObject.SetActive(false);
        Rect mySigRect = new Rect(guix, guiy, guiw, guih);
        //Debug.Log(mySigRect);
        yield return new WaitForEndOfFrame();
       
        
         Texture2D myTex = new Texture2D((int)guiw, (int)guih, TextureFormat.RGB24, false);
        myTex.ReadPixels(mySigRect, 0, 0);
        myTex.Apply();        
        //mySigImage.texture = myTex;
        capTaken = false;
        mySignatureID.SetImage(myTex);
        mySignatureID.isSigned = true;
        boxLine.gameObject.SetActive(true);
        GameObject parent = GameObject.FindGameObjectWithTag("SupervisorSignoff");
       // Debug.Log(parent.name);
        if (this.transform.parent.parent.gameObject.CompareTag("SupervisorSignoff"))
        {

            GameObject pdfButton = parent.transform.GetChild(3).gameObject;
            pdfButton.SetActive(true);
           // Debug.Log("Supervisor signed off " + pdfButton.name);
        }
        myTex = null;
    

    //this will save the area capture to a file if needed.
    //byte[] bytes = myTex.EncodeToPNG();
    // File.WriteAllBytes("C://JobPlanTempFiles/SavedScreen.png", bytes);


    gameObject.SetActive(false);

    }
    public void SetLinePositions() // set vertice points for signature outline box;
    {
        Vector3 array0 = new Vector3(guix, guiy, 10);
        myLinesArray[0] = Camera.main.ScreenToWorldPoint(array0);
        Vector3 array1 = new Vector3(guix, guiy - mySigBack.rect.yMin * 2, 10);
        myLinesArray[1] = Camera.main.ScreenToWorldPoint(array1);
        Vector3 array2 = new Vector3(guix + guiw, guiy - mySigBack.rect.yMin * 2, 10);
        myLinesArray[2] = Camera.main.ScreenToWorldPoint(array2);
        Vector3 array3 = new Vector3(guix + guiw, guiy, 10);
        myLinesArray[3] = Camera.main.ScreenToWorldPoint(array3);
        Vector3 array4 = new Vector3(guix, guiy, 10);
        myLinesArray[4] = Camera.main.ScreenToWorldPoint(array4);



    }

    public static byte[] ObjectToByteArray(Texture2D myTexture)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, myTexture);
            return ms.ToArray();
        }
    }



    public void QuitApplication() {
        Application.Quit();
    }
    

   
   
}
