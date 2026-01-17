using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] GameObject optionPanel;
    public void GoToMain()
    {
        mainPanel.SetActive(true);
        instructionPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void GoToInstruct()
    {
        mainPanel.SetActive(false);
        instructionPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
    public void GoToOption()
    {
        mainPanel.SetActive(false);
        instructionPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
