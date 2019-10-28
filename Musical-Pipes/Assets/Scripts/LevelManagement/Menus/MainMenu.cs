using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelManagement.Data;
using GameControllers;

namespace LevelManagement
{
    // controls the main menu screen
    public class MainMenu : Menu<MainMenu>
    {
        // delay before game begins
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader startTransitionPrefab;

        // reference to DataManager
        private DataManager _dataManager;

        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;
        
        // #region DEBUGGING
        // [SerializeField]
        // private Text _debugString;

        // public void SetDebugString(string text)
        // {
        //     _debugString.text = text;
        // }

        // #endregion 

        // reference to whether or not this menu has been made active or not 
        private bool _active = false;

		protected override void Awake()
		{
            base.Awake();
        }
        private void Start()
        {
            _dataManager = Object.FindObjectOfType<DataManager>();
            SettingsMenu.Instance.LoadStuff();
        }

        private void Update()
        {
            if(!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
                Cursor.visible = true;
            }
        }

        // launch the first game level
        public void OnPlayPressed()
        {
            GameManager.Instance.StartGame();
            GameMenu.Open();
            _active = false;
        }

        public void OnLeaderboardsPressed()
        {
            LeaderboardMenu.Instance.SetDropdown();
            LeaderboardMenu.Instance.SetupCurrentLeaderboard();
            LeaderboardMenu.Open();
            _active = false;
        }

        // function updating current player's name 
        public void OnPlayerNameUpdate(string name)
        {
            //Debug.Log("Player Name Update: " + name);
            LeaderboardMenu.Instance.ActivePlayerName = name;
            GameManager.Instance.PlayerName = name;
        }

        // set selected song 
        public void OnSongSelect(string songName)
        {
            FindObjectOfType<AudioManager>().SelectSong(songName);
        }

        // open the settings menu
        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
            _active = false;
        }

        // quit the application
        public override void OnBackPressed()
        {
            Application.Quit();
        }

    }
}