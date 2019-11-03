using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SignatureCapture : MonoBehaviour {
    GameObject myParent;
   

    void Start() {
        myParent = transform.parent.gameObject;
    }
    

    public void TakeCap() {
        
        SignatureButton.capTaken = true;
        StartCoroutine(WaitforCap());
        
    }

    public IEnumerator WaitforCap() {
        yield return new WaitUntil(() => SignatureButton.capTaken == false);
        Destroy(myParent.gameObject);
    }


    

   

    

}
