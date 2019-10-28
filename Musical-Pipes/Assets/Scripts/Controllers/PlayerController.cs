using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PipeSystem; 
using GameMenus;
using TimeSystem;

namespace GameControllers {
    public class PlayerController : MonoBehaviour
    {
        #region Pipe System Variables 

        [Header("Pipe System Variables")]
        // refernece to the activate pipe system manager
        [SerializeField]
        private PipeSystemManager pipeSystem ;

        // reference to movement velocity 
        [SerializeField]
        private float velocity = 5f;
        public float Velocity { get { return velocity ; } }
        // reference to initial velocity
        private float initialVelocity;

        // reference to currently active pipe
        private Pipe currentPipe;  

        #endregion 

        #region Player Performance Variables

        // reference to distance travelled by player
        private float distanceTravelled = 0;

        private float timerValue = 0;

        // reference to players lives 
        [SerializeField]
        private int livesLeft = 3;
        public int LivesLeft { get { return livesLeft ; } set { livesLeft = value ;} }
        
        private int initialLives;
        public int InitialLives { get { return initialLives ; } }

        // reference to wait interval between crashes
        [SerializeField]
        private float crashInterval = 1;

        // reference to status whether or not player can crash
        private bool canCrash = true;

        // interval to wait between increasing speeds
        [SerializeField]
        private float speedIncreaseInterval = 10f;

        // status whether can increase speed 
        private bool increaseSpeedNow = true;

        #endregion

        #region Movement Control Variables 

        // reference to the width of the mobile screen
        private float screenWidth;

        // reference to whether or not game is being played on mobile 
        private bool isMobile = false; 
        public bool IsMobile { set { isMobile = value ;} get { return isMobile; } }

        // reference to the player's rotation velocity - for movement 
        [SerializeField]
    	private float rotationVelocity = 180;

        // reference to the player's avatar rotation value 
        public float avatarRotation; 

        // reference to current world rotation 
        private float worldRotation;

        // reference to pipe system parent object's transform (compensates for default orientation)
        private Transform world;

        // reference to the player's rotator
        [SerializeField]
        private Transform rotator; 

        #endregion

        #region Utility Variables

        // reference to rotation components for translating angular distance into straight line distance 
        private float deltaToRotation;
    	private float systemRotation;

        #endregion 
        
        #region Singleton 
        
        private static PlayerController instance; 

        public static PlayerController Instance { get { return instance ; } }


        #endregion 
        // TODO: Implement singleton pattern

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else 
            {
                instance = this;
                world = pipeSystem.transform.parent;
                gameObject.SetActive(false);
                initialVelocity = velocity;
                initialLives = livesLeft;
            }

        }

        // function starting the game and initializing the pipe system
        public void StartGame () 
        {
            screenWidth = Screen.width;

            StopAllCoroutines();
            //TimeController.Instance.StartGame();
            velocity = initialVelocity;
            livesLeft = initialLives;
            timerValue = 0f;
            distanceTravelled = 0f;
            avatarRotation = 0f;
            systemRotation = 0f;
            worldRotation = 0f;
            currentPipe = pipeSystem.SetupFirstPipe();
            SetupCurrentPipe();
            gameObject.SetActive(true);
            HUD.Instance.SetValues(distanceTravelled, 0, velocity, livesLeft);
            increaseSpeedNow = true;
            canCrash = true;
            StartCoroutine(SpeedIncreaseRoutine());
	    }

        private void Update () 
        {
            SimulateMovement(); 
            ProcessPlayerRotation();  
            timerValue += Time.deltaTime;
            if(increaseSpeedNow && velocity < 11)
                StartCoroutine(SpeedIncreaseRoutine());
            HUD.Instance.SetValues(distanceTravelled, timerValue, velocity, livesLeft);    // TODO: Implement self containing controller to update UI components
	    }

        #region Movement Speed
        private IEnumerator SpeedIncreaseRoutine()
        {
            increaseSpeedNow = false;
            yield return new WaitForSeconds(speedIncreaseInterval);
            ProcessSpeedIncrease();
            increaseSpeedNow = true;
        }
        private void ProcessSpeedIncrease()
        {   
           float amount = livesLeft * 0.01f;  // update speed by lives left multiplier
           UpdateSpeed(amount);
        }
        
        // function updating velocity value 
        public void UpdateSpeed(float amount)
        {
            velocity += amount;      
            if(velocity < 8)
                velocity = 8;    

            if(velocity > 11)
                velocity = 11;  
        }

        #endregion 

        // function processing the player's rotation movement
        private void ProcessPlayerRotation () 
        {
            float moveInput = 0;
            if (!isMobile)
            {
                moveInput = Input.GetAxis("Horizontal");
                avatarRotation += rotationVelocity * Time.deltaTime * moveInput;

            }
            else
            {
                if(Input.touchCount > 0 && Input.touchCount < 3)
                {
                    if (Input.GetTouch(0).position.x > screenWidth / 2)
                    {
                        //move right
                        avatarRotation += rotationVelocity * Time.deltaTime * 1f;

                    }
                    else
                    {
                        // move left
                        avatarRotation += rotationVelocity * Time.deltaTime * -1f;
                    }                    
                }

            }
            if (avatarRotation < 0f) {
                avatarRotation += 360f;
            }
            else if (avatarRotation >= 360f) {
                avatarRotation -= 360f;
            }
            rotator.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
	    }

        // function simulating pipe system movement 
        private void SimulateMovement()
        {
            // calculates score (distance -> speed x time)
            float delta = velocity * Time.deltaTime;
            distanceTravelled += delta;

            if (systemRotation >= currentPipe.CurveAngle) {
                delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
                currentPipe = pipeSystem.SetupNextPipe();
                SetupCurrentPipe();
                systemRotation = delta * deltaToRotation;
            }
            systemRotation += delta * deltaToRotation;  
            pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);  // update pipesystem rotation position (Simulates movement)
        }

        // function setting up current parent pipe
        private void SetupCurrentPipe () 
        {
            deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            worldRotation += currentPipe.RelativeRotation;
            if (worldRotation < 0f) {
                worldRotation += 360f;
            }
            else if (worldRotation >= 360f) {
                worldRotation -= 360f;
            }
            world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
        }

        // TODO: Add sound effect
        // function processing loss of a life 
        public bool ProcessLifeLost()
        {
            if(canCrash)
            {

                livesLeft --;
                AudioBandItemGenerator.Instance.AmpLifeSpawnIntervalTimer = 8f;

                CameraShaker.Instance.StartShake();

                StartCoroutine(CrashCoroutine());

                if(livesLeft == 0)
                {
                    ProcessDeath();
                }
                else
                {
                    UpdateSpeed(-0.5f);
                }

                return true;

            }
            else 
            {
                return false;
            }
            
        }

        // TODO: Fix unchecked casting
        public void UpdateLives(float amount)
        {
            livesLeft += (int) amount;
        }

        public IEnumerator CrashCoroutine()
        {
            canCrash = false; 
            yield return new WaitForSeconds(crashInterval);
            canCrash = true;
        }

        // function disabling gameobject 
        public void ProcessDeath()
        {
            StopAllCoroutines();
            GameManager.Instance.EndGame(distanceTravelled, timerValue);
            gameObject.SetActive(false);
        }
        
        // stop the game
        public void StopGame()
        {
            StopAllCoroutines();
            GameManager.Instance.EndGame();
            gameObject.SetActive(false);

        }

    }
}