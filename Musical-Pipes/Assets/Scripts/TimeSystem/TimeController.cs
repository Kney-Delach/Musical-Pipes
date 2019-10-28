using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMenus; 
using GameControllers;
// TODO: Implement pausing of generators

namespace TimeSystem {
    public class TimeController : MonoBehaviour
    {
        [Header("Audio References")]

        // reference to audio source to alter pitch
        [SerializeField]
        private AudioSource audioSource; 

        // reference to player energy value 
        [SerializeField]
        private float energyValue = 5f;
        public float EnergyValue { get {return energyValue ; }}
        private float initialEnergy;

        // reference to time slow mo energy cost
        [SerializeField]
        private float energyCost = 1f;

        // reference to slow mo pressed status 
        private bool wasPressed = false;

        // reference to using slow mo status 
        private bool slowMoActive = false;
        
        private bool paused = false; 
        public bool Paused { set { paused = value ; } }
        
        #region Singleton 

        private static TimeController instance = null; 

        public static TimeController Instance { get { return instance ; } }

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else 
            {
                instance = this; 
                initialEnergy = energyValue;
            }
        }


        private void OnDestroy()
        {
            instance = null; 
        }

        #endregion

        private void Start()
        {
            HUD.Instance.SetSlowMoValue(energyValue);
        }

        // function to reset energy value
        public void StartGame()
        {
            energyValue = initialEnergy;   
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }
        
        private void Update()
        {
            if(audioSource != null &&!paused)
            {

            
                if((Input.GetKey(KeyCode.Space) || Input.touchCount == 2) && energyValue > 0 && !wasPressed)
                {
                    energyValue -= Time.deltaTime * energyCost * (1/Time.timeScale);
                    energyValue = Mathf.Clamp (energyValue, 0, initialEnergy);
                    HUD.Instance.SetSlowMoValue(energyValue);
                    slowMoActive = true;
                }
                else 
                {
                    energyValue += Time.deltaTime * energyCost * .1f;   // rate at which slow mo juice comes back
                    energyValue = Mathf.Clamp (energyValue, 0, initialEnergy);
                    HUD.Instance.SetSlowMoValue(energyValue);
                    slowMoActive = false;
                }

                // TODO: Insert UI Update

                if (slowMoActive) {
                    Time.timeScale = .35f;
                    audioSource.pitch = .5f;
                    //PlayerController.Instance.UpdateSpeed(-0.0005f);
                } 
                else 
                {
                    Time.timeScale = 1f;
                    audioSource.pitch = 1f;
                }

                if(!slowMoActive)
                {
                    wasPressed = (Input.GetKey(KeyCode.Space) || Input.touchCount == 2);
                }

            }
        }

    }
}