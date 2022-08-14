using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Gamemode_display : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    //Data
    GameObject InformationPanel,titleholder,Descriptionholder;
    TMP_Text title, description;
    AudioSource au;
   [SerializeField] string modename, modedescription;


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
    // Start is called before the first frame update
    void Start()
    {
       


       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Yaover");
        InformationPanel.SetActive(true);
        title.text = modename;
        description.text = modedescription;
        au.Play();

    }
    private void OnMouseOver()
    {
      
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DisablePanel();
    }

    public void DisablePanel()
    {
        if(InformationPanel.activeSelf)
            InformationPanel.SetActive(false);
    }
    
    private void OnMouseExit()
    {
        
    }
}
