using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    public GameObject m_nextObject;
    
    public void ActivateNext()
    {
        if(!m_nextObject.activeSelf)
            m_nextObject.SetActive(true);
    }
}
