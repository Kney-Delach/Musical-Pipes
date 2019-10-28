using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScoreSystem;
using GameControllers;

namespace LevelManagement.Data
{
    // manages saved data
    public class DataManager : MonoBehaviour
    {
        private SaveData _saveData;
        private JsonSaver _jsonSaver;

        // public properties to set and get values from the SaveData object
        public float MasterVolume
        {
            get { return _saveData.masterVolume; }
            set { _saveData.masterVolume = value; }
        }

        public float SfxVolume
        {
            get { return _saveData.sfxVolume; }
            set { _saveData.sfxVolume = value; }
        }

        public float MusicVolume
        {
            get { return _saveData.musicVolume; }
            set { _saveData.musicVolume = value; }
        }

        public int FullScreenIndex
        {
            get { return _saveData.fullScreenIndex; }
            set { _saveData.fullScreenIndex = value; }
        }

        public Dictionary<string, List<Score>> Highscores
        {
            get { return _saveData.highscores; }
            set { _saveData.highscores = value; }
        }

        // public void SetScore(string songName, Score newScore)
        // {
        //     _saveData.highscores[songName].Add(newScore); 
        // }

        // public int Highscore 
        // {
        //     get { return _saveData.highscore; }
        //     set { _saveData.highscore = value; }
        // }
         
        // public float HighscoreTime
        // {
        //     get { return _saveData.highscoreTime; }
        //     set { _saveData.highscoreTime = value; }
        // }

        // initializes SaveData and JsonSaver objects
        private void Awake()
        {
            _saveData = new SaveData();
            _jsonSaver = new JsonSaver();
        }

        // saves the data using the JsonSaver
        public void Save()
        {
            //Debug.Log("Saving Data: " + _saveData.highscores.Keys.Count + " dictionary keys");
            _jsonSaver.Save(_saveData);
        }

        // loasd the data using the JsonSaver
        public void Load()
        {
            _jsonSaver.Load(_saveData);
            //Debug.Log("Loading Data: " + _saveData.highscores.Keys.Count + " dictionary keys");
        }

        
        private void Update()
        {
            // TODO: REMOVE BACKDOOR SAVED DATA REMOVER
            if(Input.GetKeyDown(KeyCode.Q) && GameManager.Instance.PlayerName == "ADMIN")
                _jsonSaver.Delete();
        }

	}
}
