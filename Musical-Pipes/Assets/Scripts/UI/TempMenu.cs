using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers;
using UnityEngine.UI;
using PipeSystem; 

namespace GameMenus {
    public class TempMenu : MonoBehaviour
    {
        [Header("Game Controllers")]
        // reference to the player 
        [SerializeField]
        private PlayerController playerController;

        [Header("UI Components")]

        //reference to player score label 
        [SerializeField]
        private Text scoreText;

        [Header("Audio Components")]
        [SerializeField]
        private AudioSource levelMusic; 

        // reference to singleton instance 
        private static TempMenu instance = null;
        public static TempMenu Instance { get { return instance ; } }

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else
            {
                instance = this; 
                
                //Application.targetFrameRate = 1000; // assign application framerate 
            }
        }
        // function starting the game
        public void StartGame()
        {
            playerController.StartGame();
            gameObject.SetActive(false);
            Cursor.visible = false;
            levelMusic.Play();
        }
        
        // function performing the endgame protocol 
        public void EndGame(float distanceTravelled, float timerValue)
        {
            Debug.Log("Timer Value: " + timerValue.ToString("F2"));

            scoreText.text = "Score: " + ((int)(distanceTravelled * 10f)).ToString();
            gameObject.SetActive(true);
            Cursor.visible = true;
            levelMusic.Stop();
            PipeSystemManager.Instance.ResetPipeCounter();
        }

        // public void UpdateSpeed(string newSpeed)
        // {   
        //     float tempFloat = 0;
        //     bool isFloat = float.TryParse(newSpeed, out tempFloat);
        //     if(isFloat)
        //         playerController.UpdateSpeed(tempFloat);
        // }
    }
}