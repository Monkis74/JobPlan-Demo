using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using sharpPDF.Enumerators;
using sharpPDF;
using sharpPDF.Elements;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
//using System.Drawing;
using System.Configuration;
//using System.Drawing.Imaging;
using System.Linq;
using UnityEngine.UI;


// take screencaptures to create images
// build a pdf file from the screencaptures of the pages.

public class ScreenCapPDF : MonoBehaviour {

    int captureNum = 0; // counter to give each screenshot a numerical element to order them
    string[] fileName; // the filenames of all the screenshots
    string pdfName;
    List<GameObject> pageList = new List<GameObject>(); // the list of all the pages able to be captured
    //OkToCapture[] pageList;
    OkToCapture[] allpageList; // the array holding all the pages to transfer to the list for sorting.
    DontCapture[] noPage; // the array of all the gameobjects that are not to be screencaptured
    PageController pageController;
    string screenCapDir; // the directory for temporarily saving the screencaptures
    string pdfSaveDir; // the directory to save the created pdf file to.
    public GameObject SubmissionErrorPage;
    public GameObject PdfCreationPanel;
    public GameObject ViewPdfPanel;
    public Text PDFCreationText;
    public GameObject Logo;
    public bool logoshrink = true;

    public XColor BackColor;
    public XColor BackColor2;
    public XColor ShadowColor;
    double BorderWidth;
    public XPen BorderPen;
    public XGraphicsState state;

    // Use this for initialization
    void Start () {
        pageController = FindObjectOfType<PageController>();
        screenCapDir = "C://JobPlanTempFiles";
        string sharepointPath = SaveFile.sharepointPath;
        pdfSaveDir = sharepointPath + "/Job Plans/SubmittedPlansArchive";

        BackColor = XColors.Ivory;
        BackColor2 = XColors.WhiteSmoke;

        BackColor = XColor.FromArgb(212, 224, 240);
        BackColor2 = XColor.FromArgb(253, 254, 254);

        ShadowColor = XColors.Gainsboro;
        BorderWidth = 4.5;
        BorderPen = new XPen(XColor.FromArgb(94, 118, 151), BorderWidth);

    }

    // begin the screen capture process
    public void StarCaptureScreen() {
        ContinuousSaveController.continuousSaveActive = false;
        allpageList = Resources.FindObjectsOfTypeAll<OkToCapture>();
        pageList.Clear();
        foreach (OkToCapture thisPage in allpageList) {
            pageList.Add(thisPage.gameObject);
        }
        pageList = pageList.OrderBy(x => x.GetComponent<OkToCapture>().pageNum).ToList(); // set pageList order for screen capturing.

        //for (int i = 0; i < pageList.Count; i++) {
        //    Debug.Log(pageList[i].name + " is page " + i);
        //}

        noPage = Resources.FindObjectsOfTypeAll<DontCapture>(); // turn off all non relevant objects before screen capture.
        foreach (DontCapture thispage in noPage) {
            thispage.gameObject.SetActive(false);
        }

        StartCoroutine(CaptureScreen());
    }
    public IEnumerator CaptureScreen() { // iterate through the screens and take captures.
        GameObject parent = GameObject.FindGameObjectWithTag("SupervisorSignoff");
        //if (this.transform.parent.gameObject.CompareTag("SupervisorSignoff"))
        //{
        GameObject pdfButton = parent.transform.GetChild(3).gameObject;
        pdfButton.SetActive(false);
       // Debug.Log("PDF Button turned off " + pdfButton.name);
        //}
        if (!Directory.Exists(screenCapDir)){
            Directory.CreateDirectory(screenCapDir);
        }
        foreach (string file in Directory.GetFiles(screenCapDir, "*.png").Where(item => item.EndsWith(".png")))
        {
            File.Delete(file);
        }

        // loop here to go through each page panel and take a screnshot;
        string capName;
        foreach (GameObject page in pageList) {
            page.gameObject.SetActive(false);
        }

        int pageCountModifier = pageList.Count; // the amount of pages to capture based on criteria below.
        int startPageNum = 0; // the page number to start capturing at;
        if (pageController.isEmergency) {
            pageCountModifier = pageList.Count - (pageList.Count - 1);
           // Debug.Log("is emergency pagecount modifier" + pageCountModifier);
            startPageNum = 0;
        }
        if (pageController.isCSEP && !pageController.isCSEP2) {
            pageCountModifier = 7;
            startPageNum = 1;
        }
        if (pageController.isCSEP2) {
            pageCountModifier = pageList.Count;
            startPageNum = 1;
        }
        if (!pageController.isCSEP && !pageController.isEmergency) {
            pageCountModifier = 6;
            startPageNum = 1;
        }

        //if (pageController.isEmergency)
        //{
        //    for (int i = 0; i < 1; i++)
        //    {
        //       // Debug.Log("Using Emergency Cap");
        //        pageList[i].gameObject.SetActive(true);
        //        capName = "screenshot" + captureNum + ".png".ToString();
        //        Application.CaptureScreenshot(screenCapDir + "/" + capName);
        //        yield return new WaitForSeconds(.01f);
        //       // Debug.Log("Screnshot captured" + captureNum);
        //        pageList[i].gameObject.SetActive(false);
        //        captureNum++;

        //    }
        //}

            //if (!pageController.isEmergency) {
            //   // Debug.Log("PagelistCount " + pageList.Count);
                for (int i = startPageNum; i < pageCountModifier; i++)
                {
                   // Debug.Log("Not Emergency Caps");
                    pageList[i].gameObject.SetActive(true);
                    capName = "screenshot" + captureNum + ".png".ToString();
                    ScreenCapture.CaptureScreenshot(screenCapDir + "/" + capName);
            // TODO place while loop to test if file is created yet instead of waitforseconds.
            yield return new WaitForSeconds(.01f);
            FileInfo fileToTest = new FileInfo(screenCapDir + "/" + capName);

                    Debug.Log("Screnshot captured" + captureNum);
                    pageList[i].gameObject.SetActive(false);
                    captureNum++;
            yield return new WaitUntil(() => IsFileLocked(fileToTest) == false);

                }
        //}
        PdfCreationPanel.SetActive(true);
        PDFCreationText.text = "Preparing to create PDF file :" + SaveFile.loadName;
        //yield return new WaitForSeconds(3f);

        // pageController.FinishPage(); // set screen to finished page.
        captureNum = 0;
        
       

        // loop through all screenshots before creating pdf.

        PopulateArray(screenCapDir); // gather all trhe screenshots to array
       // yield return new WaitForSeconds(0.5f);
        if (!Directory.Exists(pdfSaveDir)) {
            Directory.CreateDirectory(pdfSaveDir);
        }
       // StartCoroutine(CreatePDF(pdfSaveDir));  // create the pdf
    }

    private bool IsFileLocked(FileInfo file)
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }

    private void Update()
    {
        if (PdfCreationPanel.activeSelf == true) {
            float speed = 0;           
            
            if (logoshrink) {
                speed = .2f;
                Logo.transform.localScale = new Vector3(Logo.transform.localScale.x - .005f, Logo.transform.localScale.y - .005f, Logo.transform.localScale.z);
                if (Logo.transform.localScale.x <= .8f) {
                    logoshrink = false;
                }
            }
            if (!logoshrink)
            {
                speed = .5f;
                Logo.transform.localScale = new Vector3(Logo.transform.localScale.x + .01f, Logo.transform.localScale.y + .01f, Logo.transform.localScale.z);
                if (Logo.transform.localScale.x >= 1f)
                {
                    logoshrink = true;
                }
            }
            Logo.transform.eulerAngles = new Vector3(Logo.transform.eulerAngles.x, Logo.transform.eulerAngles.y, Logo.transform.eulerAngles.z + speed*2);

        }
    }

    void PopulateArray(string folder) //list of all png files in the folder to arrange into pdf.
    {
        fileName = Directory.GetFiles(folder, "*png");

        //for (int i = 0; i < fileName.Length; i++)
        //{
        //    //Debug.Log(fileName[i]);

        //}
       
        //Debug.Log("length of  saved png array" + fileName.Length);
        StartCoroutine(CreatePDF(pdfSaveDir));  // create the pdf
    }

    public void TestPdf() {
        string folderPath = "C://JobPlanTempFiles";
        PopulateArray(folderPath);
        CreatePDF(folderPath);
    }

    public IEnumerator CreatePDF(string folder) // using SharpPDF library.
    {
        PdfCreationPanel.SetActive(true);
        int pagesCreated = 1;
        SaveFile saveFile = GameObject.FindObjectOfType<SaveFile>();
        pdfName = SaveFile.loadName; // use file name of the file loaded for archiving.
        string savePath = pdfSaveDir;

        PdfDocument doc = new PdfDocument();
        FileInfo fileToTest;
        // old //pdfDocument doc = new pdfDocument(pdfName, "GuelphHydro", false);
        // insert a for loop in here to go through all the screenshots.
        //PdfSharp.Drawing.XImage myImage;
        for (int i = 0; i < fileName.Length; i++)
        {

            // old // pdfPage page = doc.addPage(786 , 1366);
            PdfPage page = doc.AddPage();
            // string imagePath = screenCapDir + "/" + fileName[i];

            //myImage = PdfSharp.Drawing.XImage.FromFile(fileName[i]);
            string screenshot = "Screenshot" + i.ToString();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            //BeginBox(gfx, 1, "Page " + i);
            XImage image = XImage.FromFile(screenCapDir + "/" + screenshot + ".png");
            //page.Width = image.PixelWidth;
            page.Width = 1366;
            //page.Height = image.PixelHeight;
            page.Height = 768;
            // Left position in point
            double x = (250 - image.PixelWidth * 72 / image.HorizontalResolution) / 2;
            gfx.DrawImage(image, 0, 0, 1366, 768);


            //EndBox(gfx);

            //old //  doc.addImageReference(myImage, screenshot );
            //old //  page.addImage(doc.getImageReference(screenshot), 0, 0);

            image = null;
            Debug.Log("PDF Page " + i + " Created");

            Debug.Log("Page Number " + pagesCreated + "/" + (fileName.Length));
            
            PDFCreationText.text = "Page Number " + (pagesCreated).ToString() + "/" + fileName.Length.ToString() + "of PDF file " + SaveFile.loadName + " created";
            pagesCreated++;
           
           
            yield return new WaitForSeconds(.5f);
        }

        if (pagesCreated < fileName.Length - 1)
        {
            PdfCreationPanel.SetActive(false);
            SubmissionErrorPage.SetActive(true);
            yield break;
        }
        doc.Save(pdfSaveDir + "/" + pdfName + ".pdf");
        fileToTest = new FileInfo(pdfSaveDir + "/" + pdfName + ".pdf");
        yield return new WaitUntil(() => IsFileLocked(fileToTest) == false);
        doc.Dispose();

        doc = null;

        //Application.OpenURL(pdfSaveDir + "/" + pdfName + ".pdf");

        // erase screen capture png images here *** Does this too fast and misses pages in pdf creation***
        //foreach (string file in Directory.GetFiles(screenCapDir, "*.png").Where(item => item.EndsWith(".png")))
        //{
        //    File.Delete(file);
        //}

        if (!File.Exists(pdfSaveDir + "/" + pdfName + ".pdf"))
        {
            PdfCreationPanel.SetActive(false);
            SubmissionErrorPage.SetActive(true);
            yield break;
        }
        if (File.Exists(pdfSaveDir + "/" + pdfName + ".pdf"))
        {
            yield return new WaitForSeconds(1f);

            /// ### /// Create a log file to compare old syncs to beyond sharepoints 3 month sync.
            string logPath = SaveFile.sharepointPath + "/Job Plans/Log/";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            if (!File.Exists(logPath + "SignedPlanLog.txt"))
            {
                var newFile = File.Create(logPath + "SignedPlanLog.txt");
                newFile.Close();
            }
            bool _isDuplicate = false;
            string logLine;
            StreamReader logFile = new StreamReader(logPath + "SignedPlanLog.txt");
            while ((logLine = logFile.ReadLine()) != null)
            {
                if (logLine == pdfName + ".dat")
                {
                    Debug.Log("Duplicate entry not created");
                    _isDuplicate = true;
                    break;   
                }
            }
            logFile.Close();
            if (!_isDuplicate)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(logPath + "SignedPlanLog.txt", true))
                {
                    file.WriteLine(pdfName + ".dat");
                }
            }
            /// ### ///
            File.Delete(saveFile.loadedFilePath);
            PdfCreationPanel.SetActive(false);
            ViewPdfPanel.SetActive(true);
            //pageController.FinishPage();
        }
        // Debug.Log("Saved File Deleted" + saveFile.loadedFilePath);
       
      
    }
    public void OpenPDF() {
        Application.OpenURL(pdfSaveDir + "/" + pdfName + ".pdf");
        pageController.FinishPage("The Job Plan " + pdfName + " Has been archived.");
        ViewPdfPanel.SetActive(false);
    }
    public void DontOpenPDF() {
        pageController.FinishPage("The Job Plan " + pdfName + " Has been archived.");
        ViewPdfPanel.SetActive(false);
    }



    public void BeginBox(XGraphics gfx, int number, string title)
    {

        const int dEllipse = 15;
        XRect rect = new XRect(0, 20, 300, 200);
        if (number % 2 == 0)


            rect.X = 300 - 5;
        rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
        rect.Inflate(-10, -10);
        XRect rect2 = rect;
        rect2.Offset(this.BorderWidth, this.BorderWidth);
        gfx.DrawRoundedRectangle(new XSolidBrush(this.ShadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
        XLinearGradientBrush brush = new XLinearGradientBrush(rect, this.BackColor, this.BackColor2, XLinearGradientMode.Vertical);
        gfx.DrawRoundedRectangle(this.BorderPen, brush, rect, new XSize(dEllipse, dEllipse));
        rect.Inflate(-5, -5);

        XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
        gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

        rect.Inflate(-10, -5);
        rect.Y += 20;
        rect.Height -= 20;

        this.state = gfx.Save();
        gfx.TranslateTransform(rect.X, rect.Y);
    }

    public void EndBox(XGraphics gfx)
    {
        gfx.Restore(this.state);
    }

}

