using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UoBStealthGame.Highscores;
using UoBStealthGame.UI;

public class MainMenu : MonoBehaviour
{
  
    public HighScoreTable HST;
    public UI_Game gameUI;
    public void PlayGame()
    {
        int rand = Random.Range(1, 7); //exclusive max range

        if (SceneManager.GetActiveScene().buildIndex <= 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + rand);
             
        }
        else
        {
             SceneManager.LoadScene(0);
        }
    }



    public void QuitGame()
    {
        Debug.Log("Game has quit");
        Application.Quit();
    }


}
