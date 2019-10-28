using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PipeSystem; 

namespace GameControllers {
    public class PlayerColliderController : MonoBehaviour
    {   
        [Header("Player Components")]
        // reference to the player controller 
        [SerializeField]
        private PlayerController playerController;

        [Header("Death Effect Variables")]
        // utilize if adding time trial functionality
        [SerializeField]
        private float deathTimer = -1f;
        public float DeathTimer { get { return deathTimer ; } }

        [Header("Audio SFX Components")]
        [SerializeField]
        private AudioController _audioCrash;
        [SerializeField]
        private AudioController _audioLife;
        [SerializeField]
        private AudioController _audioBoost;

        // reference to alive status of player 
        private bool isDead = false;

        private void OnTriggerEnter(Collider collider)
        {   
            if(collider.tag == "Obstacle")
            {
                if(AudioBandItemGenerator.Instance.IsTutorial && AudioBandItemGenerator.Instance.TutorialIndex == 2)
                {
                    AudioBandItemGenerator.Instance.tutorialObjectsCollected++;
                }
                if(playerController.ProcessLifeLost())
                    _audioCrash.PlaySfx();
            }
            if(collider.tag == "SpeedBoost")
            {
                if(AudioBandItemGenerator.Instance.IsTutorial && AudioBandItemGenerator.Instance.TutorialIndex == 0)
                {
                    AudioBandItemGenerator.Instance.tutorialObjectsCollected++;
                }
                _audioBoost.PlaySfx();
                playerController.UpdateSpeed(collider.GetComponentInParent<PipeItem>().Amount);
                //Debug.Log("Boost Aquired!; Speed Increased by: " + collider.GetComponentInParent<PipeItem>().Amount);
            }
            if(collider.tag == "ExtraLife")
            {
                if(AudioBandItemGenerator.Instance.IsTutorial && AudioBandItemGenerator.Instance.TutorialIndex == 1)
                {
                    AudioBandItemGenerator.Instance.tutorialObjectsCollected++;
                }
                _audioLife.PlaySfx();
                playerController.UpdateLives(collider.GetComponentInParent<PipeItem>().Amount);
            }
        }
    }
}