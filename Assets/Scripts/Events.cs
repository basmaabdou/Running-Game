using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{
   public void ReplayGame()
   {
       SceneManager.LoadScene("Level");
   }
   public void QuitGame()
   {
       Application.Quit();
   }
    public void PlayLevelTwo()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

}
