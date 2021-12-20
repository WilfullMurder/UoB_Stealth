using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace UoBStealthGame.Highscores
{
    public class HighScoreTable : MonoBehaviour
    {
        public event System.Action OnNewHighScore;

        public NewHighScoreEntry_UI scoreEntry_UI;

        public int PlayerScoreTracker = 0;
        public string PlayerNameTracker = "";

        private Transform EntryContainer;
        public Transform EntryTemplate;
        private List<HighScoreEntry> HighScoreEntries;
        private List<Transform> HighScoretransforms;
        private HighScores CurrentHighScores;
        private string ScoresToJSONString;
        private string Scorespath;
        [SerializeField] TextMeshProUGUI PositionText;
        [SerializeField] TextMeshProUGUI ScoreText;
        [SerializeField] public TextMeshProUGUI NameText;
        public static bool bNewScore = false;

        public float TemplateHeight = 50.0f;

        private void Awake()
        {
            //instantiate container and template
            EntryContainer = GameObject.Find("HighScoreEntryContainer").transform;
            //EntryTemplate = EntryContainer.Find("HighScoreEntryTemplate");
            //deactivate Placeholder template
            EntryTemplate.gameObject.SetActive(false);
            //instantiate new HighScore list
            HighScoreEntries = new List<HighScoreEntry>();
          

            Scorespath = Application.streamingAssetsPath + "/ScoresTable.txt"; //Get the pathfor ScoresTable.txt
            ScoresToJSONString = File.ReadAllText(Scorespath); //Convert Json to string
            PlayerPrefs.SetString("HighScoreTablepath", Scorespath);
            PlayerPrefs.Save();

            if (ScoresToJSONString.Length == 0)
            {
                //ScoresTable.txt is empty, fill table from blank table, Overwrite ScoresTable
                Scorespath = Application.streamingAssetsPath + "/BlankTable.txt";
                ScoresToJSONString = File.ReadAllText(Scorespath);


                File.WriteAllText(Application.streamingAssetsPath + "/ScoresTable.txt",ScoresToJSONString); //Write new table to ScoresTable.txt


                Scorespath = Application.streamingAssetsPath + "/ScoresTable.txt"; //Get the pathfor ScoresTable.txt
                ScoresToJSONString = File.ReadAllText(Scorespath); //Convert Json to string
                //save the filepath to Player prefs
                PlayerPrefs.SetString("HighScoreTablepath", Scorespath); 
                PlayerPrefs.Save();
            }

            //Load saved HighScores
              
                CurrentHighScores = JsonUtility.FromJson<HighScores>(ScoresToJSONString);
                HighScoreEntries = CurrentHighScores.HighScoreEntryList;

            //@TODO::
            //Call AddHighScoreEntry at win/lose if HighScore is achieved

            //CurrentHighScores is null when HST.CheckScores(PlayerScore) is called by UI_Game.GameLost() (Assume also .GameWon())
            //Table needs initialising BEFORE this call is made. Start of level load or start of player maybe.

            //Sort entry list by score
            for (int i = 0; i < CurrentHighScores.HighScoreEntryList.Count; i++)
            {
                for (int j = 0; j < CurrentHighScores.HighScoreEntryList.Count; j++)
                {
                    if (CurrentHighScores.HighScoreEntryList[j].Score < CurrentHighScores.HighScoreEntryList[i].Score)
                    {
                        HighScoreEntry TempEntry = CurrentHighScores.HighScoreEntryList[i];
                        CurrentHighScores.HighScoreEntryList[i] = CurrentHighScores.HighScoreEntryList[j];
                        CurrentHighScores.HighScoreEntryList[j] = TempEntry;
                    }
                }
            }
            //instantiate new Transform list
            HighScoretransforms = new List<Transform>();
            foreach (HighScoreEntry CurrentHighScoreEntry in HighScoreEntries)
            {
                //Save the transform of each new Entry in HighScores
                CreateHighScoreEntryTransform(CurrentHighScoreEntry, EntryContainer, HighScoretransforms);
            }

        }

        private void CreateHighScoreEntryTransform(HighScoreEntry ScoreToEnter, Transform Container, List<Transform> TransformList)
        {
            //Spawn new entry template
            Transform EntryTransform = Instantiate(EntryTemplate, Container);

            //Set Template position and activate
            RectTransform EntryRectTransform = EntryTransform.GetComponent<RectTransform>();
            EntryRectTransform.anchoredPosition = new Vector2(-52, -TemplateHeight * TransformList.Count + 1);
            EntryTransform.gameObject.SetActive(true);

            //Get rank
            int Rank = TransformList.Count + 1;
            string RankString = "TH"; //Default rank string

            switch (Rank)
            {
                //Ammends rankstring if score places above third
                case 1:
                    RankString = "ST";
                    break;
                case 2:
                    RankString = "ND";
                    break;
                case 3:
                    RankString = "RD";
                    break;
            }
            //Concatenate Rank and Rank string, set Position Text to NewRank
            string NewRank = Rank + RankString;
            PositionText.text = NewRank;

            //Convert Score to string
            int Score = ScoreToEnter.Score;
            string prefix = "00";

            if(Score < 10)
            {
                ScoreText.text = prefix + Score.ToString();
            }

            else if (Score >= 10 && Score < 100)
            {
               prefix = "0";
                ScoreText.text = prefix + Score.ToString();
            }
          

            //Set NameText to score name
            string Name = ScoreToEnter.Name;
            NameText.text = Name;

            //Add transform to list
            TransformList.Add(EntryTransform);
        }

       private void AddHighScoreEntry(int ScoreToAdd, string NameToAdd)
       {           

            //Create new HighScoreEntry
            HighScoreEntry entry = new HighScoreEntry { Name = NameToAdd, Score = ScoreToAdd };

            //Load saved HighScores
            string jsonString = PlayerPrefs.GetString("HighScoreTablepath");
            string ScoresToJSONString = File.ReadAllText(jsonString);
            HighScores CurrentHighScores = JsonUtility.FromJson<HighScores>(ScoresToJSONString);


            if (CheckScores(CurrentHighScores, entry))
            {
                ScoresToJSONString = JsonUtility.ToJson(CurrentHighScores);
                //Save updated HighScores
                string scoresPath = Application.streamingAssetsPath + "/ScoresTable.txt"; ;
                File.WriteAllText(scoresPath, ScoresToJSONString); //Write new table to ScoresTable.txt
                PlayerPrefs.SetString("HighScoreTablepath", scoresPath); //update path string
                PlayerPrefs.Save();
            }
          
                       
       }

        public void SaveNewHighScoreEntry(int Score, string Name)
        {
           AddHighScoreEntry(Score, Name);
        }

        public bool CheckScores(int ScoreTracker)
        {
            Scorespath = Application.streamingAssetsPath + "/ScoresTable.txt"; //Get the pathfor ScoresTable.txt
            ScoresToJSONString = File.ReadAllText(Scorespath); //Convert Json to string
            PlayerPrefs.SetString("HighScoreTablepath", Scorespath);
            PlayerPrefs.Save();

            if (ScoresToJSONString.Length == 0)
            {
                //ScoresTable.txt is empty, fill table from blank table, Overwrite ScoresTable
                Scorespath = Application.streamingAssetsPath + "/BlankTable.txt";
                ScoresToJSONString = File.ReadAllText(Scorespath);


                File.WriteAllText(Application.streamingAssetsPath + "/ScoresTable.txt", ScoresToJSONString); //Write new table to ScoresTable.txt


                Scorespath = Application.streamingAssetsPath + "/ScoresTable.txt"; //Get the pathfor ScoresTable.txt
                ScoresToJSONString = File.ReadAllText(Scorespath); //Convert Json to string
                //save the filepath to Player prefs
                PlayerPrefs.SetString("HighScoreTablepath", Scorespath);
                PlayerPrefs.Save();
            }

            //Load saved HighScores

            CurrentHighScores = JsonUtility.FromJson<HighScores>(ScoresToJSONString);
            HighScoreEntries = CurrentHighScores.HighScoreEntryList;

            if (CurrentHighScores.HighScoreEntryList.Count > 0)
            {
                for (int i = 0; i < CurrentHighScores.HighScoreEntryList.Count; i++)
                {
                    if (ScoreTracker >= CurrentHighScores.HighScoreEntryList[i].Score)
                    {
                        if (OnNewHighScore != null)
                        {
                            OnNewHighScore();
                           
                        }
                        return true;
                    }
                }
            }
                return false;
        }


        public bool CheckScores(HighScores CurrentHighScores, HighScoreEntry Entry)
        {
            for (int i = 0; i < CurrentHighScores.HighScoreEntryList.Count; i++) //Loop CurrentHighScores
            {
                if (Entry.Score >= CurrentHighScores.HighScoreEntryList[i].Score) //entry is greater than current score
                {
                    CurrentHighScores.HighScoreEntryList.Insert(i, Entry); //Insert entry (above) current score
                    CurrentHighScores.HighScoreEntryList.RemoveAt(CurrentHighScores.HighScoreEntryList.Count - 1); //Remove lowest score

                    if(OnNewHighScore != null)
                    {
                        OnNewHighScore();
                    }
                  
                    return true; //break the loop so we don't overwrite all lower scores
                }
            }
            return false;
        }

        public int GetPlayerTrackedScore()
        {
            return PlayerScoreTracker;
        }
        


        [System.Serializable]
        public class HighScores
        {
            public List<HighScoreEntry> HighScoreEntryList;
        }

        [System.Serializable]
        public struct HighScoreEntry
        {
            public int Score;
            public string Name;
        }

        public void SetPlayerScoreTracker(int Score)
        {
            PlayerScoreTracker = Score;
        }






    }
}