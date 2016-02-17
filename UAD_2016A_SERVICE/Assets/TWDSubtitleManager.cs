using UnityEngine;
using System.Collections;

public class TWDSubtitleManager : MonoBehaviour
{

    /* The dialogs in the scene, must be arranged in order. */
    public TWDSubtitles[] m_arSceneDialogues;

    public float m_fDialogElapsedTime = 0.0f;
    float m_fActualDialogStartTime = -1.0f;
    float m_fLastDialogStartTime = -1.0f;
    int m_iActualDialogActive = 0;
    bool m_bSceneIsOver = false;

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < m_arSceneDialogues.Length; i++)
        {
            if ( m_arSceneDialogues[i].m_fStartTime > m_fLastDialogStartTime)
            {
                m_fLastDialogStartTime = m_arSceneDialogues[i].m_fStartTime;
            }
            m_arSceneDialogues[i].gameObject.SetActive(false);
        }
        m_arSceneDialogues[m_iActualDialogActive].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bSceneIsOver == true)
        {
            return;
        }

        m_fDialogElapsedTime += Time.deltaTime;
        for (int i = 0; i < m_arSceneDialogues.Length; i++)
        {
            if (m_arSceneDialogues[i].m_fStartTime <= m_fDialogElapsedTime &&
                m_arSceneDialogues[i].m_fStartTime > m_fActualDialogStartTime)
            {
                m_arSceneDialogues[m_iActualDialogActive].gameObject.SetActive(false);
                m_fActualDialogStartTime = m_arSceneDialogues[i].m_fStartTime;
                m_iActualDialogActive = i;
                m_arSceneDialogues[m_iActualDialogActive].gameObject.SetActive(true);
            }
        }
        
        if (m_arSceneDialogues[m_iActualDialogActive].bDialogFinished == true)
        {
            if (m_arSceneDialogues[m_iActualDialogActive].m_fStartTime == m_fLastDialogStartTime)
            {
                m_bSceneIsOver = true;
            }
        //    //Call the function of the actual dialog (which by now must have finished its display time)
        //    //And "darken" its sprite.
        //    //It also disappears the text it had displayed.
        //    m_arSceneDialogues[m_iActualDialogActive].gameObject.SetActive(false);
        //    m_iActualDialogActive++;
        //    if (m_iActualDialogActive >= m_arSceneDialogues.Length)
        //    {
        //        m_bSceneIsOver = true;
        //        m_iActualDialogActive = m_arSceneDialogues.Length - 1;
        //        Debug.Log(" This Scene dialogs are finished. ");
        //    }
        //    m_arSceneDialogues[m_iActualDialogActive].gameObject.SetActive(true);
        }

        m_arSceneDialogues[m_iActualDialogActive].OnUpdate();
    }
}
