using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TimeSystem;
using PipeSystem;
using GameControllers;

namespace LevelManagement
{
    // controls the pause menu screen
    public class PauseMenu : Menu<PauseMenu>
    {
        // delay before switching scenes
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader levelTransitionPrefab;

        // reference to initial selected component
        [SerializeField]
        private Button _selectedComponent;

        public static bool isMobile = false; 

        public static float openedNow = 0f; 

        // reference to active status of menu
        private bool _active = false;

        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }

            // checks mobile input two fingers to unpause
            //if(isMobile && Input.touchCount == 3)
            //{
            //   OnResumePressed();
            //}
            //else if(Input.GetKeyDown(KeyCode.Escape))
            //{
            //    OnResumePressed();
            //}

            if(Input.touchCount == 3 || Input.GetKeyDown(KeyCode.Escape) && openedNow <= 0)
            {
                OnResumePressed();
            }

            if (openedNow > 0)
            {
                openedNow -= 0.01f;
            }

        }
        // resumes the game and closes the pause menu
        public void OnResumePressed()
        {
            TimeController.Instance.Paused = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            AudioManager audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.UnPauseAudio();
            GameMenu.openedNow = 2f;
            base.OnBackPressed();
            _active = false;
        }

        // unpauses and restarts the current level
        public void OnRestartPressed()
        {
            if(AudioBandItemGenerator.Instance.IsTutorial)
            {
                AudioBandItemGenerator.Instance.TutorialIndex = 0;
                AudioBandItemGenerator.Instance.spawnLifeBoost = false; 
                AudioBandItemGenerator.Instance.spawnSpeedBoost = false;
                AudioBandItemGenerator.Instance.spawnCollidableObjects = false; 
                AudioBandItemGenerator.Instance.endTutorial = false; 
                PlayerController.Instance.LivesLeft = 1;
            }
            GameManager.Instance.RestartGame();
            base.OnBackPressed();
            _active = false;
        }

        // opens the settings menu
        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
            _active = false;
        }

        // unpauses and loads the MainMenu level
        public void OnMainMenuPressed()
        {
            PlayerController.Instance.StopGame();
            _active = false;
        }

        // quits the application (does not work in Editor, build only)
        public void OnQuitPressed()
        {
            Application.Quit();
        }
    }
}
