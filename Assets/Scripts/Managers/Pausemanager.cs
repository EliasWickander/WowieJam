using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemanager : MonoBehaviour
{
    public GameObject Pausecanvas;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pausecanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
     public void Resumegame()
    {
        Pausecanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Restartgame()
    {
        Pausecanvas.SetActive(false);
        SceneManager.LoadScene(1);
    }
    public void gomenu()
    {
        GameManager.Instance.SetState(GameStateType.GameState_Menu);
    }

}
