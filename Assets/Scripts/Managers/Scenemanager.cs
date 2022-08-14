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
        GameManager.Instance.SetState(GameStateType.GameState_Tutorial);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void playgamescene()
    {
        GameManager.Instance.SetState(GameStateType.GameState_Playing);
    }

    public void RestartScene()
    {
        GameManager.Instance.RestartGameScene();
    }
    public void BackToMainMenu()
    {
        GameManager.Instance.SetState(GameStateType.GameState_Menu);
    }
}
