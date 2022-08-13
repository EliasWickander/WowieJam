using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activategameobject : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void activateoptions()
    {
        obj.SetActive(true);
    }
}
