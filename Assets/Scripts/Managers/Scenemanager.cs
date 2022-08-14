using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tutorialload()
    {
        //Loadscene
    }
    public void quit()
    {
        Application.Quit();
    }
    public void playgamescene()
    {
        SceneManager.LoadScene(1);
    }
}
