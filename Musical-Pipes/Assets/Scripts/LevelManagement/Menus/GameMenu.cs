using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeSystem;

namespace LevelManagement
{
    // shown when in game 
    public class GameMenu : Menu<GameMenu>
    {

        public static float openedNow = 0f;

        private void Update()
        {
            // pause if escape or two finger touch
            if (Input.touchCount == 3 || Input.GetKeyDown(KeyCode.Escape) && openedNow <= 0)
            {
                OnPausePressed();
            }

            if (openedNow > 0)
            {
                openedNow -= Time.deltaTime * 1 * (1 / Time.timeScale);
            }
        }

        // pauses the game and opens the pause menu
        public void OnPausePressed()
        {
            Cursor.visible = true;
            TimeController.Instance.Paused = true;
            Time.timeScale = 0;
            AudioManager audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.PauseAudio();
            PauseMenu.Open();
            PauseMenu.openedNow = 1f;

        }

    }
}
