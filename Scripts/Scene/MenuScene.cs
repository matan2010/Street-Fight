using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public GameObject actionButtons;
    bool actionButtonsEnabled = false;
    private void Start()
    {
        actionButtons.SetActive(actionButtonsEnabled);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ActionButtons()
    {
        actionButtons.SetActive(!actionButtonsEnabled);
        actionButtonsEnabled = !actionButtonsEnabled;

    }
    public void quit()
    {
        Application.Quit();
    }
}
