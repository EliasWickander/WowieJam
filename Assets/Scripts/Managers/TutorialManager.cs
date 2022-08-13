using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUPs;
    [SerializeField] int PopUpIndex;
    [SerializeField] GameObject Pspawner;

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
        
    }
}
