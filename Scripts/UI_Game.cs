using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UoBStealthGame.Highscores;

namespace UoBStealthGame.UI
{



    public class UI_Game : MonoBehaviour
    {

        [SerializeField] private int PlayerScore = 0;
        [SerializeField] private TMP_Text ScoreTextWin;
        [SerializeField] private TMP_Text ScoreTextLose;
        [SerializeField] private TMP_Text ScoreTextNewScore;


        public GameObject UI_Lose;
        public GameObject UI_Win;
        public GameObject UI_HighScore;



        public HighScoreTable HST;
        public NewHighScoreEntry_UI entry_UI;

        // Start is called before the first frame update
        public void Start()
        {
            GuardClass.OnSpottedPlayer += GameLost;

            FindObjectOfType<PlayerClass>().OnFinishedLevel += GameWon;

            HST.OnNewHighScore += NewHighScore;

   
        }

     
        void GameWon()
        {
            if(!HST.CheckScores(PlayerScore))
            {
                OnGameOver(UI_Win);
            }

         
        }

        void GameLost()
        {
            if (!HST.CheckScores(PlayerScore))
            {
                OnGameOver(UI_Lose);
            }
        }

        public void NewHighScore()
        {
          OnNewHighScore(UI_HighScore);           
        }



        void OnGameOver(GameObject UI)
        {
            UI.SetActive(true);


            GuardClass.OnSpottedPlayer -= GameLost;
            FindObjectOfType<PlayerClass>().OnFinishedLevel -= GameWon;

        }

        void OnNewHighScore(GameObject UI)
        {
            UI.SetActive(true);

            
        }

        public void UpdateScreenScore(int Score)
        {
            PlayerScore = Score;
            ScoreTextWin.text = PlayerScore.ToString();
            ScoreTextLose.text = PlayerScore.ToString();
            ScoreTextNewScore.text = PlayerScore.ToString();
            entry_UI.HST.SetPlayerScoreTracker(Score);

        }

     

    }
}
