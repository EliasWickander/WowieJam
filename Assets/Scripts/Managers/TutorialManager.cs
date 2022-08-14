using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUPs;
    [SerializeField] int PopUpIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<popUPs.Length;i++)
        {
            if(i==PopUpIndex)
            {
                popUPs[i].SetActive(true);
            }
            else
            {
                popUPs[i].SetActive(false);
            }
        }
        if(PopUpIndex<=1&&Input.GetKeyDown(KeyCode.Space))
        {
            //pause time,display
            PopUpIndex++;
        }
        if(PopUpIndex==2)//insert condition for hovering
        {
            PopUpIndex++;
        }
        if (PopUpIndex <= 4 && Input.GetKeyDown(KeyCode.Space))
        {
            PopUpIndex++;
        }
        if (PopUpIndex == 5)//slap correct Ai
        {
            PopUpIndex++;
        }
        if (PopUpIndex == 6 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }


    }
}
