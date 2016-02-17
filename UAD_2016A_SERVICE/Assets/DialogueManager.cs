using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour 
{
    /*  Time it will take the sprite of the active interlocutor, to fully appear in the scene.*/
    public float m_fFadeInSpriteTime = 0.5f;
    public float m_fFadeInTextTime = 0.75f;
    private float m_fFadeInElapsedTime = 0.0f;

    /* Time, in seconds, which has to pass until the Active Dialog's sprite and text box fade, respectively. */
    public float m_fFadeOutSpriteTime = 0.2f;
    public float m_fFadeOutTextTime = 0.2f;
    private float m_fFadeOutElapsedTime = 0.0f;

    /* The dialogs in the scene, must be arranged in order. */
    public Dialogue[] m_arSceneDialogues;

    int m_iActualDialogActive = 0;
    bool m_bSceneIsOver = false;



    ////Used to change the background according to the dialog.
    //public GameObject m_pBackgroundRef = null;
    //
    //public Texture[] m_arBackgroundTextures;

	// Use this for initialization
	void Start () 
    {

        for (int i = 0; i < m_arSceneDialogues.Length; i++ )
        {
            m_arSceneDialogues[i].ColorSprite(true, Color.gray);
            m_arSceneDialogues[i].SetSpriteAlpha(0.0f);
            m_arSceneDialogues[i].SetTextBoxAlpha(0.0f);
        }
        m_arSceneDialogues[m_iActualDialogActive].ColorSprite(false, Color.white);
        m_arSceneDialogues[m_iActualDialogActive].SetSpriteAlpha(0.0f);
        m_arSceneDialogues[m_iActualDialogActive].SetTextBoxAlpha(0.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( m_bSceneIsOver == true )
        {
            return;
        }

        if (m_arSceneDialogues[m_iActualDialogActive].bDialogFinished == true)
        {
            if (m_fFadeOutElapsedTime < Mathf.Max(m_fFadeOutSpriteTime, m_fFadeOutTextTime))
            {
                m_fFadeOutElapsedTime += Time.deltaTime;
                //m_fFadeInElapsedTime = Mathf.Min(m_fFadeInElapsedTime, m_fFadeInSpriteTime);
                float fAlphaPercent = Mathf.Max(0.0f, 1.0f - m_fFadeOutElapsedTime / m_fFadeOutSpriteTime);
                m_arSceneDialogues[m_iActualDialogActive].SetSpriteAlpha(fAlphaPercent);
                //Now for the text box 
                fAlphaPercent = Mathf.Max(0.0f, 1.0f - m_fFadeOutElapsedTime / m_fFadeOutTextTime);
                m_arSceneDialogues[m_iActualDialogActive].SetTextBoxAlpha(fAlphaPercent);
            }
            else
            {

                //Call the function of the actual dialog (which by now must have finished its display time)
                //And "darken" its sprite.
                //It also disappears the text it had displayed.
                m_arSceneDialogues[m_iActualDialogActive].ColorSprite(true, Color.gray);
                m_iActualDialogActive++;
                m_fFadeOutElapsedTime = 0.0f;
                m_fFadeInElapsedTime = 0.0f;
                if (m_iActualDialogActive >= m_arSceneDialogues.Length)
                {
                    m_bSceneIsOver = true;
                    m_iActualDialogActive = m_arSceneDialogues.Length - 1;
                    Debug.Log(" This Scene dialogs are finished. ");
                }
                m_arSceneDialogues[m_iActualDialogActive].ColorSprite(false, Color.white);
            }
        }
        
        if( m_fFadeInElapsedTime < Mathf.Max( m_fFadeInSpriteTime, m_fFadeInTextTime ) )
        {
            m_fFadeInElapsedTime += Time.deltaTime;
            //m_fFadeInElapsedTime = Mathf.Min(m_fFadeInElapsedTime, m_fFadeInSpriteTime);
            float fAlphaPercent = Mathf.Min(1.0f, m_fFadeInElapsedTime / m_fFadeInSpriteTime);
            m_arSceneDialogues[m_iActualDialogActive].SetSpriteAlpha(fAlphaPercent);
            //Now for the text box 
            fAlphaPercent = Mathf.Min(1.0f, m_fFadeInElapsedTime / m_fFadeInTextTime);
            m_arSceneDialogues[m_iActualDialogActive].SetTextBoxAlpha(fAlphaPercent);
        }
        else
        {
            m_arSceneDialogues[m_iActualDialogActive].OnUpdate();
        }

        

	}
}
