using UnityEngine;
using UnityEngine.SceneManagement;
 
public class MenuController : MonoBehaviour

{

    [Header("Options Panel (optional)")]

    public GameObject optionsPanel;
 

    public void OnNewGame()

    {

        SceneManager.LoadScene("Level1");

    }
 

    public void OnLoadGame()

    {

        SceneManager.LoadScene("Select_Lvl");

    }


    public void OnOptions()

    {

         SceneManager.LoadScene("Options");

    }

    public void OnExit()

    {

        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif

    }
 

    public void OnBackFromOptions()

    {

        if (optionsPanel != null)

            optionsPanel.SetActive(false);

    }

}
 