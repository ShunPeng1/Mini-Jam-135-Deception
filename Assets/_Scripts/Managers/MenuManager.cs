using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtilities;

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
    public void StartGame()
    {
        SceneManager.LoadScene("Thuan Testing");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene("How To Play");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
