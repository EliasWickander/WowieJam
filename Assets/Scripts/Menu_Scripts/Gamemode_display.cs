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
   [SerializeField] string modename, modedescription;
    // Start is called before the first frame update
    void Start()
    {
        //Get Gameobject
        InformationPanel = GameObject.Find("InfoPanel");
       
        titleholder = InformationPanel.transform.GetChild(0).gameObject;
        Descriptionholder=InformationPanel.transform.GetChild(1).gameObject;
        //Get TMP
       title = titleholder.GetComponent<TMP_Text>();
       description = Descriptionholder.GetComponent<TMP_Text>();

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
    }
    private void OnMouseOver()
    {
      
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InformationPanel.SetActive(false);
    }
    private void OnMouseExit()
    {
        
    }
}
