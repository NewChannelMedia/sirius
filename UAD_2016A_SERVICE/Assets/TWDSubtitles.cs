using UnityEngine;
using System.Collections;

public class TWDSubtitles : MonoBehaviour 
{
    //public GameObject m_pArrowForTextRef = null;
    //public AudioSource m_pTextSoundEffect = null;
    //public float m_fPitchIncrease = 0.01f;
    //public GameObject m_pTextBoxObjReference = null;
    //public GameObject m_pSpriteObjReference = null;
    //public GameObject m_pDarkFilterObjReference = null;

    public float m_fStartTime = -1.0f;

    /*< The whole Dialog Text, before changing to any other "actor"'s dialog */

    [MultilineAttribute(5)]
    [TextAreaAttribute(1, 5)]
    public string m_szDialogueText = "";
    private string m_szRemaningText = "";
    private string m_szShownText = "";
    private string m_szFillText = "\n\n\n\n\n";

    private float m_fElapsedTime = 0.0f;
    private float m_fNextTextElapsedTime = 0.0f;
    private int m_iActualShownTextIndex = 0;
    private int m_iTotalCharactersDisplayed = 0;

    private UILabel m_pTextAreaRef = null;

    /*< The total time it will take to show all the characters in a line*/
    public float m_fTimeToShowLine = 0.050f;
    public bool m_bPlayAutomatically = false;
    public float m_fTimePauseBeforeChange = 2.0f;

    private bool m_bAllTextShown = false;
    private bool m_bDialogFinished = false;

    [MultilineAttribute(5)]
    [TextAreaAttribute(1, 5)]
    private string m_szProcessedText = "";

    /*< Get access variable to provide Read-Only sharing with other scripts. */
    public bool bAllTextShown
    {
        get
        {
            return m_bAllTextShown;
        }
    }

    public bool bDialogFinished
    {
        get
        {
            return m_bDialogFinished;
        }
    }

    /**
        Updates the real text that is being sent to the rendering panel, according to the 
     * elapsed time and the Time to show a line.
     */
    void ShowText()
    {


        m_fElapsedTime += Time.deltaTime;
        while (m_fElapsedTime >= m_fTimeToShowLine)
        {
            //Debug.Log("Changing text");
            m_fElapsedTime -= m_fTimeToShowLine;
            if (m_iActualShownTextIndex < m_szProcessedText.Length)
            {
                m_szShownText += m_szProcessedText[m_iActualShownTextIndex];
                m_pTextAreaRef.text = m_szShownText + m_szFillText;
                //m_szProcessedText = m_pTextAreaRef.processedText;
                m_iActualShownTextIndex++;
                //m_pTextSoundEffect.Stop();
                //m_pTextSoundEffect.pitch += m_fPitchIncrease;
                //m_pTextSoundEffect.pitch = Mathf.Clamp(m_pTextSoundEffect.pitch, 0.0f, 1.50f);
                //m_pTextSoundEffect.Play();
                // if( !m_pTextSoundEffect.isPlaying )
                //     m_pTextSoundEffect.Play();

            }
            else
            {
                m_bAllTextShown = true;
                //m_pArrowForTextRef.SetActive(true);
                m_fElapsedTime = 0.0f;
                return;
            }


        }
    }

    //public void ColorSprite(bool in_bActivateFilter, Color in_NewSpriteTint)
    //{
    //    m_pSpriteObjReference.GetComponent<UITexture>().color = in_NewSpriteTint;
    //    m_pDarkFilterObjReference.SetActive(in_bActivateFilter);
    //}
    //public void SetSpriteAlpha(float in_fAlpha)
    //{
    //    m_pSpriteObjReference.GetComponent<UITexture>().alpha = in_fAlpha;
    //}
    //public void SetTextBoxAlpha(float in_fAlpha)
    //{
    //    m_pTextBoxObjReference.GetComponent<UITexture>().alpha = in_fAlpha;
    //}

    void GetNextParagraph()
    {
        //m_fElapsedTime = 0.0f;
        //m_pTextSoundEffect.pitch = 1.0f;//Re initialize the pitch of the Typing effect.
        m_szShownText = "";
        m_iTotalCharactersDisplayed += m_szProcessedText.Length;
        if (m_iTotalCharactersDisplayed >= m_szDialogueText.Length)
        {
            Debug.Log("This whole dialog has been processed");
            m_pTextAreaRef.text = "";
            m_szProcessedText = "";
            m_bDialogFinished = true;
            return;
        }
        m_iActualShownTextIndex = 0;

        m_szRemaningText = m_szDialogueText.Substring(m_iTotalCharactersDisplayed);//Get the remaining text of the dialog
        m_pTextAreaRef.text = m_szRemaningText + m_szFillText;
        m_pTextAreaRef.ProcessText();
        m_szProcessedText = m_pTextAreaRef.processedText + '\n';
        m_pTextAreaRef.text = "";

    }

    void NextText()
    {
        if (m_bAllTextShown == true)
        {
            if (m_bPlayAutomatically == true)
            {
                Debug.Log("Waiting to change automatically.");
                m_fNextTextElapsedTime += Time.deltaTime;
                if (m_fNextTextElapsedTime >= m_fTimePauseBeforeChange)
                {
                    m_fNextTextElapsedTime = 0.0f;
                    m_bAllTextShown = false;
                    //m_pArrowForTextRef.SetActive(false);
                    //Pass to the next part of the dialog

                    GetNextParagraph();
                }
            }
            else
            {
                m_bAllTextShown = false;
                //m_pArrowForTextRef.SetActive(false);
                //Pass to the next part of the dialog
                GetNextParagraph();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        m_pTextAreaRef = GetComponent<UILabel>();
        GetNextParagraph();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Does exactly the same as Update, but is called from another Script to ensure a correct execution order.
    public void OnUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    if (m_bAllTextShown == false)
        //    {
        //        //m_szShownText = m_szDialogueText;
        //        m_pTextAreaRef.text = m_szProcessedText + m_szFillText;//ADD THE fill just in case.
        //        m_pArrowForTextRef.SetActive(true);
        //        m_bAllTextShown = true;
        //    }
        //    else
        //    {
        //        //Show the arrow which indicates there's more text to display.
        //        //Show the next part of the dialog
        //        NextText();
        //        //Put the Index of the current character to the last that was displayed before.            
        //    }
        //}

        if (m_bAllTextShown == false)
        {
            ShowText();

        }
        else if (m_bPlayAutomatically == true)
        {
            //Show the arrow which indicates there's more text to display.
            //Show the next part of the dialog
            NextText();
            //Put the Index of the current character to the last that was displayed before.            
        }
    }
}
