using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement;
using LevelManagement.Data;
using TimeSystem;
using ScoreSystem;

namespace GameControllers {
    public class GameManager : MonoBehaviour
    {   
        // reference to active player's name
        private string playerName = "Jeff Kaplan";
        public string PlayerName { get { return playerName ; } set { playerName = value ; } }

        private static GameManager instance;
        public static GameManager Instance { get { return instance ; } }

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else 
                instance = this;
        }
        // private void Start()
        // {
        //     DataManager data = FindObjectOfType<DataManager>();
        //     if (data != null)
        //     {
        //         data.Load();
        //     }
        // }

        public void EndGame(float score, float timerValue)
        {
            Cursor.visible = true;
            TimeController.Instance.Paused = true;
            FindObjectOfType<AudioManager>().StopSelectedSong();
            AssignScores(score*10, timerValue);
            WinScreen.Open();
            //MainMenu.Open();
        }

        public void EndGame()
        {
            Cursor.visible = true;
            TimeController.Instance.Paused = true;
            FindObjectOfType<AudioManager>().StopSelectedSong();
            Time.timeScale = 1;
            MainMenu.Open();
        }

        public void RestartGame()
        {
            FindObjectOfType<AudioManager>().StopSelectedSong();
            StartGame();
        }

        public void StartGame()
        {
            TimeController.Instance.Paused = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            TimeController.Instance.StartGame();
            PlayerController.Instance.StartGame();
            FindObjectOfType<AudioManager>().PlaySelectedSong();
        }
        
        // function assigning score, updating leaderboards and saving json file
        public void AssignScores(float score, float scoreTime)
        {   
            int tempScore = (int) score;
            
            AudioManager audioManager = FindObjectOfType<AudioManager>();

            DataManager data = FindObjectOfType<DataManager>();
            Dictionary<string, List<Score>> highscoresDictionary = data.Highscores;

            if(!highscoresDictionary.ContainsKey(audioManager.ActivateSongName))
            {
                highscoresDictionary.Add(audioManager.ActivateSongName, new List<Score>());
            }

            Score newScore;
            if(highscoresDictionary[audioManager.ActivateSongName].Capacity != 0)
            {
                int curScorePosition = 1;
                foreach(Score prevScores in highscoresDictionary[audioManager.ActivateSongName])
                {
                    if(prevScores.score > tempScore)
                    {
                        curScorePosition ++;
                    }
                    else 
                    {
                        prevScores.scorePosition ++;
                    }
                }

                newScore = new Score(playerName, tempScore, scoreTime, curScorePosition);
            }
            else 
            {
                newScore = new Score(playerName, tempScore, scoreTime, 1);
            }

            highscoresDictionary[audioManager.ActivateSongName].Add(newScore);
            
            // assign new highscore dictionary & save json file
            data.Highscores = highscoresDictionary;
            data.Save();
            
            // display player's score
            WinScreen.Instance.ScoreText.text = newScore.playerID + ": " + newScore.scorePosition + "  /  " + newScore.score + "  /  " + newScore.timeScore.ToString("F2") + " s";

            // if highscore display scene version 1
            if(newScore.scorePosition == 1)
            {
                WinScreen.Instance.HighScoreTitleText.text = "New Highscore!";
                WinScreen.Instance.HighScoreTitleText.fontSize = 120;
                WinScreen.Instance.HighScoreText.text = ""; 
            }
            else 
            {
                // get highscore Score and display scene version 2
                Score highscoreScore = null;
                foreach(Score s in highscoresDictionary[audioManager.ActivateSongName])
                {
                    if(s.scorePosition == 1)
                    {
                        highscoreScore = s;
                        break;
                    }
                }
                WinScreen.Instance.HighScoreTitleText.text = "Current Highscore";
                WinScreen.Instance.HighScoreTitleText.fontSize = 80;
                WinScreen.Instance.HighScoreText.text = highscoreScore.playerID + ": " + highscoreScore.scorePosition + "  /  " + highscoreScore.score + "  /  "+ highscoreScore.timeScore.ToString("F2") + " s";
            }
        }
    }
}