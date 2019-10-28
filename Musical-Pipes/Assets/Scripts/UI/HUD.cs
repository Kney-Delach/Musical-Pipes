using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenus {
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private Text distanceText;

        [SerializeField]
        private Text timerText; 
        
        [SerializeField]
        private Text velocityText;

        [SerializeField]
        private Text sloMoText;

        [SerializeField]
        private Text livesText;

        // reference to singleton instances 
        private static HUD instance = null; 

        public static HUD Instance { get { return instance ; } }

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else 
                instance = this;
        }
        // function to update UI values 
        public void SetValues (float distanceTravelled,float timerValue, float velocity, int lives) {
            timerText.text = "Time: " + timerValue.ToString("F2") + " s";
            distanceText.text = "Score: " + ((int)(distanceTravelled * 10f)).ToString();
            velocityText.text = "Speed: " + velocity.ToString("F2") + " m/s";
            livesText.text = "Lives: " + lives.ToString();
        }

        public void SetSlowMoValue(float sloMoValue)
        {
            sloMoText.text = "SlowMo: " +((int)sloMoValue).ToString();
        }
    }
}