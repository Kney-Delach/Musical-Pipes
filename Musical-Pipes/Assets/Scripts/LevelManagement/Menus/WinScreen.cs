using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControllers;

namespace LevelManagement
{
    // shown when player completes the level
    public class WinScreen : Menu<WinScreen>
    {
        // delay before we play the game
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader levelTransitionPrefab;

        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;

        [Header("Score References")]
        [SerializeField]
        private Text scoreText;
        public Text ScoreText { get { return scoreText ; } }

        [SerializeField]
        private Text highscoreText; 
        public Text HighScoreText { get { return highscoreText ; } }

        [SerializeField]
        private Text highscoreTitleText; 
        public Text HighScoreTitleText { get { return highscoreTitleText ; } }

        // reference to whether or not this menu has been made active or not 
        private bool _active = false;

        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }
        }



        // restart the current level
        public void OnRestartPressed()
        {
            GameManager.Instance.StartGame();
            base.OnBackPressed();           
            _active = false;
        }

        // return to MainMenu scene
        public void OnMainMenuPressed()
        {
            MainMenu.Open();
            _active = false;
        }

    }
}
