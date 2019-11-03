using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;

[ExecuteInEditMode]
public class SignatureID : MonoBehaviour
{
    public int objID;
    GameObject myButton;
    RawImage mySigImage;
    byte[] textureBytes;
    public string bytesString;
    int width;
    int height;
    public bool isSigned = false;
    public bool isSupervisorSigned = false;
    public int numOfSigs;
    SignatureID[] amount;
    public bool isTaken = false;

    private void Awake()
    {
        myButton = GetComponentInChildren<Button>().gameObject;
        mySigImage = GetComponent<RawImage>();
        width = (int)mySigImage.rectTransform.rect.width;
        //Debug.Log("Sig width " + width);
        height = (int)mySigImage.rectTransform.rect.height;
        //Debug.Log("Sig height " + height);
    }
    // Use this for initialization
    void Start()
    {
        amount = FindObjectsOfType<SignatureID>();
        numOfSigs = amount.Length;

    }

    public void DisableButton()
    {
        myButton.SetActive(false);
        
    }



    // Update is called once per frame
    //void Update()
    //{

       
    //    foreach (SignatureID thisSig in amount)
    //    {
    //        if (thisSig.objID == objID && thisSig.GetInstanceID() != this.GetInstanceID())
    //        {
    //            isTaken = true;
    //        }
    //        if (thisSig.objID == objID && thisSig.GetInstanceID() == this.GetInstanceID())
    //        {
    //            isTaken = false;
    //        }
    //    }
    //}
    public void SetImage(Texture2D myTex)
    {
        mySigImage.texture = myTex;
        textureBytes = myTex.EncodeToPNG();
        //Debug.Log(textureBytes.Length);
        //bytesString = Convert.ToBase64String(textureBytes);
        bytesString = GetTextureString(textureBytes);
        //Debug.Log(bytesString);
        myButton.SetActive(false);
        myTex = null;
    }

    public void ConvertStringToTexture(string imageString)
    {
        // Debug.Log(imageString);
        //byte[] imageBytes = Convert.FromBase64String(imageString);
        byte[] imageBytes = GetTextureBytes(imageString);


        // Debug.Log(imageBytes.Length);
        Texture2D thisTex = new Texture2D(width, height);
        thisTex.LoadImage(imageBytes);
        thisTex.Apply();
       // byte[] bytes = thisTex.EncodeToPNG();
        // File.WriteAllBytes("C://JobPlanTempFiles/LoadedSignature.png", bytes);

        SetImage(thisTex);
        isSigned = true;
        imageString = null;
        thisTex = null;
        imageBytes = null;
    }
        

    private string GetTextureString(byte[] bytes)
    {
        //char[] chars = new char[bytes.Length / sizeof(char)];
        //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //return new string(chars);

        //string something = Encoding.ASCII.GetString(bytes);
        //return something;

        string something = Convert.ToBase64String(bytes);
        return something;
    }




    private byte[] GetTextureBytes(string str)
    {
        //byte[] bytes = new byte[str.Length * sizeof(char)];
        //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //return bytes;

        //byte[] toBytes = Encoding.ASCII.GetBytes(str);
        //return toBytes;

        byte[] toBytes = Convert.FromBase64String(str);
        return toBytes;

    }
}