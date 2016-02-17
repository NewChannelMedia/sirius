using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
    NGUIResizeManager.cs
    Created by: 	Adrian Ulises Gonzalez Casillas
    Email: 			mute_e6@hotmail.com
    Date created:	2/01/2016
    Date Modified: 	23/01/2015 

    Purpose: 		class NGUIResizeManager is used in an "Actor-Component" engine arquitecture 
                    to adjust the elements emparented to the Actor holding this script to the dimensions of the display device.
 */
public class NGUIResizeManager : MonoBehaviour
{
    protected float m_fAspectRatio; /*!< m_fAspectRatio is a float value which represents the relation between the width over the height of the screen. */
    private float m_fScreenWidth; /*!< m_fScreenWidth is a float value for the width of the screen in pixels. */
    private float m_fScreenHeight; /*!< m_fScreenHeight is a float value for the height of the screen in pixels. */
    private float m_fOrtoCamSize;/*!< m_fOrtoCamSize is a float value which indicates the size of an orthographic camera. */
    public Camera m_ActiveCameraRef;/*!< A reference to the Camera object which will be rendering the objects in scene. Must be orthographic. */


    public float fScreenWidth
    {
        get
        {
            return m_fScreenWidth;
        }
    }


    public float fScreenHeight
    {
        get
        {
            return m_fScreenHeight;
        }
    }

    /**
        @Description: CalculateAspectRatio is an inner script function to retrieve some screen-related values in order to re-adjust the size of this gameobject later.
     */
    void CalculateAspectRatio()
    {
        m_fScreenWidth = (float)UnityEngine.Screen.width;
        m_fScreenHeight = (float)UnityEngine.Screen.height;
        m_fAspectRatio = (float)UnityEngine.Screen.width / (float)UnityEngine.Screen.height;
    }

    /**
        @Description: SetScaleBasedOnAspectRatio() is an inner script function to set the corresponding values to the scale of the gameObject holding this script, readjusting the scale of all the children of this one.
     */
    void SetScaleBasedOnAspectRatio()
    {
        m_fOrtoCamSize = m_ActiveCameraRef.orthographicSize;
        transform.localScale = new Vector3(m_fOrtoCamSize * m_fAspectRatio, m_fOrtoCamSize, 1.0f);
    }

    /**
        @Description: Awake is a Unity standard function which is called when this script, attached to a GameObject is First initialized or created on a scene. Is called before start of others scripts.
     */
    void Awake()
    {
        CalculateAspectRatio();/* Get the new dimensions of the device so the scale is set correctly. */
    }

    /**
        @Description: Start is a Unity standard function which is called when this script, attached to a GameObject is initialized on a scene.
     */
    void Start()
    {
        CalculateAspectRatio();		/* Get the new dimensions of the device so the scale is set correctly. */
        SetScaleBasedOnAspectRatio();	/* Set the values for the scale of the world based on the aspect ratio. */
    }/* End of Start() function. */

    /**
        @Description: Update is a Unity standard function which is called every frame.
     */
    void Update()
    {
        /* Check if the viewport size/device screen has changed, if so, then continue, else, exit the function. */
        if (m_fScreenWidth == (float)UnityEngine.Screen.width && m_fScreenHeight == (float)UnityEngine.Screen.height &&
            m_fOrtoCamSize == m_ActiveCameraRef.orthographicSize)
            return;

        /* Get the new dimensions of the device so the scale is set correctly. */
        CalculateAspectRatio();

        /* Set the values for the scale of the world based on the aspect ratio. */
        SetScaleBasedOnAspectRatio();
    }/* End of Update() function */
}
