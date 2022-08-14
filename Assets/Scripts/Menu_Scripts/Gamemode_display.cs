using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Gamemode_display : MonoBehaviour
{
    //Data
    GameObject InformationPanel,titleholder,Descriptionholder;
    TMP_Text title, description;
    AudioSource au;
   [SerializeField] string modename, modedescription;

   public CustomButton m_bindedButton;
   
    private void Awake()
    {
        //Get Gameobject
        InformationPanel = GameObject.Find("InfoPanel");

        titleholder = InformationPanel.transform.GetChild(0).gameObject;
        Descriptionholder = InformationPanel.transform.GetChild(1).gameObject;
        au = GetComponent<AudioSource>();
        //Get TMP
        title = titleholder.GetComponent<TMP_Text>();
        description = Descriptionholder.GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        m_bindedButton.OnHoverEnterCB += OnHoverEnter;
        m_bindedButton.OnHoverExitCB += OnHoverExit;
    }
    
    private void OnDisable()
    {
        m_bindedButton.OnHoverEnterCB += OnHoverEnter;
        m_bindedButton.OnHoverExitCB -= OnHoverExit;
    }

    
    private void OnHoverEnter()
    {
        InformationPanel.SetActive(true);
        title.text = modename;
        description.text = modedescription;
        au.Play();
    }

    private void OnHoverExit()
    {
        DisablePanel();
    }
    
    public void DisablePanel()
    {
        if(InformationPanel.activeSelf)
            InformationPanel.SetActive(false);
    }
}
