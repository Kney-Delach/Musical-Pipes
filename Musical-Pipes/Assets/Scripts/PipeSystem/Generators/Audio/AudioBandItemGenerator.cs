using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioDataSystem; 
using GameControllers;
using TimeSystem; 

namespace PipeSystem {
    public class AudioBandItemGenerator : MonoBehaviour
    {
        [Header("Audio Band Item Components")]

        // reference ot the set of items to generate 
        [SerializeField]
        private PipeItem[] itemPrefabs;

        [SerializeField]
        private PipeItem[] ampSpeedItemPrefabs;

        [SerializeField]
        private PipeItem[] ampLifeItemPrefabs;
        
        // reference to the audio band buffer normalized thresholds
        [Range(0,1.0f)]
        [SerializeField]
        private float[] bandThresholds;

        // reference to audio band spawn interval values
        [SerializeField]
        private float[] bandSpawnIntervals;

        // reference to audio band spawn interval timers
        private float[] bandIntervalTimers;

        [Header("Amplitude Components")]
        // reference to amplitude cutoff threshold
        [SerializeField]
        private float amplitudeThreshold = 0.6f;

        // reference to the item spawn interval duration
        [SerializeField]
        private float ampSpeedSpawnInterval = 10f;
        // reference to local interval timer
        private float ampSpeedIntervalTimer = 0;

        // reference to the item spawn interval duration
        [SerializeField]
        private float ampLifeSpawnInterval = 30f;

        // reference to local interval timer
        private float ampLifeIntervalTimer = 0;
        public float AmpLifeSpawnIntervalTimer { get {return  ampLifeIntervalTimer ; } set { ampLifeIntervalTimer = value ; } }

        // reference to pipe system instance (for shorter loc)
        private PipeSystemManager psInstance;

        // reference to tutorial check 
        [SerializeField]
        private bool isTutorial = false; 
        public bool IsTutorial { set { isTutorial = value; } get { return isTutorial ;} }

        // reference to tutorial index 
        [SerializeField]
        private int tutorialIndex = 0; 
        public int TutorialIndex {get { return tutorialIndex ;} set { tutorialIndex = value; }}

        public bool pauseTutorialObjectSpawn = false; 

        public bool spawnSpeedBoost = false; 

        public bool spawnLifeBoost = false; 

        public bool spawnCollidableObjects = false; 

        public bool endTutorial = false; 
        public float tutorialObjectIntervalTimer = 0f; 
        public float tutorialObjectIntervalTimerReset = 2f; 

        public int tutorialObjectsCollected = 0;

        public DialogueTrigger speedDialogue;  
        public DialogueTrigger lifeDialogue;  

        public DialogueTrigger dangerObjectsDialogue; 
        public DialogueTrigger slowMoDialogue;  

        public DialogueTrigger tutCompleteDialogue;  


        #region Singleton 

        private static AudioBandItemGenerator instance;
        public static AudioBandItemGenerator Instance { get { return instance ; } }


        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else
            {
                instance = this; 
                bandIntervalTimers = new float[bandSpawnIntervals.Length];
                for(int i = 0; i < bandIntervalTimers.Length; i++)
                    bandIntervalTimers[i] = 0;
            }
        }

        #endregion

        private void Start()
        {
            psInstance = PipeSystemManager.Instance;
        }

        
        // update pipes with items
        public void FixedUpdate()
        {       
            if(isTutorial)
            {
                if(tutorialIndex == 0)
                {
                    if(spawnSpeedBoost && tutorialObjectsCollected >= 2)
                    {
                        // check if enough speed boosts have been collided with
                        tutorialObjectsCollected = 0;
                        spawnSpeedBoost = false; 
                        tutorialIndex ++;
                    }
                    else if(!DialogueManager.Instance.isActive) 
                    {
                        if(DialogueManager.Instance.spawnObject)
                        {
                            DialogueManager.Instance.spawnObject = false; 
                            spawnSpeedBoost = true; 
                            DialogueManager.Instance.deactivated = false; 
                        }
                        else if(!DialogueManager.Instance.deactivated && !spawnSpeedBoost)
                        {
                            // trigger speed boost dialogue 
                            speedDialogue.TriggerDialogue();
                        }
                    }
                }
                else if( tutorialIndex == 1)
                {
                    if(spawnLifeBoost && tutorialObjectsCollected >=2)
                    {
                        // check if enough lifes have been collided with 
                        tutorialObjectsCollected = 0;
                        spawnLifeBoost = false; 
                        tutorialIndex ++;
                    }
                    else if(!DialogueManager.Instance.isActive) 
                    {
                        if(DialogueManager.Instance.spawnObject)
                        {
                            DialogueManager.Instance.spawnObject = false; 
                            spawnLifeBoost = true; 
                            DialogueManager.Instance.deactivated = false; 
                        }
                        else if(!DialogueManager.Instance.deactivated && !spawnLifeBoost)
                        {
                            // trigger extra life dialogue 
                            lifeDialogue.TriggerDialogue();
                        }
                    }
                }
                else if(tutorialIndex == 2)
                {
                    // check if objects have been collided with
                    if(spawnCollidableObjects && tutorialObjectsCollected >=2)
                    {
                        // check if enough lifes have been collided with 
                        tutorialObjectsCollected = 0;
                        spawnCollidableObjects = false; 
                        tutorialIndex ++;
                    }
                    else if(!DialogueManager.Instance.isActive) 
                    {
                        if(DialogueManager.Instance.spawnObject)
                        {
                            DialogueManager.Instance.spawnObject = false; 
                            spawnCollidableObjects = true; 
                            DialogueManager.Instance.deactivated = false; 
                        }
                        else if(!DialogueManager.Instance.deactivated && !spawnCollidableObjects)
                        {
                            // trigger extra life dialogue 
                            dangerObjectsDialogue.TriggerDialogue();
                        }
                    }
                }
                else if (tutorialIndex == 3)
                {
                     // check if slow motion has been used to dodge objects. 
                    if(spawnCollidableObjects && TimeController.Instance.EnergyValue <= 1)
                    {
                        // check if enough lifes have been collided with 
                        spawnCollidableObjects = false;
                        tutorialIndex ++;
                    }
                    else if(!DialogueManager.Instance.isActive) 
                    {
                        if(DialogueManager.Instance.spawnObject)
                        {
                            DialogueManager.Instance.spawnObject = false; 
                            spawnCollidableObjects = true; 
                            DialogueManager.Instance.deactivated = false; 
                        }
                        else if(!DialogueManager.Instance.deactivated && !spawnCollidableObjects)
                        {
                            // trigger extra life dialogue 
                            slowMoDialogue.TriggerDialogue();
                        }
                    }
                }
                else if (tutorialIndex == 4)
                {   
                    // END Tutorial  
                    // tutorial complete, return to menu. 
                    if(endTutorial)
                    {
                        isTutorial = false; 
                        endTutorial = false; 
                        PlayerController.Instance.StopGame();

                    }
                    else if(!DialogueManager.Instance.isActive) 
                    {
                        if(DialogueManager.Instance.spawnObject)
                        {
                            DialogueManager.Instance.spawnObject = false; 
                            endTutorial = true;
                            DialogueManager.Instance.deactivated = false; 
                        }
                        else if(!DialogueManager.Instance.deactivated && !endTutorial)
                        {
                            // trigger extra life dialogue 
                            tutCompleteDialogue.TriggerDialogue();
                        }
                    }
                }
            }
            ProcessAudioBands();
            //ProcessAmplitudeObjects();
            
            //ProcessAmplitudePipes();
            ProcessAmplitudeBonusObjects();
        }

        // function processing instantiation of object per audio band
        private void ProcessAudioBands()
        {
            float playerFactor = PlayerController.Instance.avatarRotation/360;

            if (playerFactor < 0f) 
            {
                playerFactor += 360f;
            }
            else if (playerFactor >= 360f) 
            {
                playerFactor -= 360f;
            }
            if(playerFactor == 0)
            {
                playerFactor = 1;
            }
            if(psInstance.EmptyPipeCount <= 0 && psInstance.Pipes.Length >5 && !isTutorial)
            {
                for(int i = 0; i < AudioData.Instance.FrequencyBandSize; i++)
                {
                    int tempIndex = psInstance.Pipes.Length-4;  // TODO: Alter to further away pipe - display color on groun ? 

                    if((bandIntervalTimers[i] <= 0) && (AudioData.Instance.NormBandBuffer[i] >= bandThresholds[i]))
                    {
                        float start = (Random.Range(0, psInstance.Pipes[tempIndex].PipeSegmentCount) + 0.5f);
                        //float direction = Random.value < 0.5f ? 1f : -1f;
                        
                        float angleStep = psInstance.Pipes[tempIndex].CurveAngle / psInstance.Pipes[tempIndex].CurveSegmentCount;
                        
                        PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                        // float rng = Random.Range(0,1);
                        // if(i == 0 && rng >= 0.5)
                        //     item = Instantiate<PipeItem>(ampItemPrefabs[Random.Range(0, ampItemPrefabs.Length)]);
                        // else 
                        //     item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                        
                        float pipeRotation = start * 360f / psInstance.Pipes[tempIndex].PipeSegmentCount;
                        item.Position(psInstance.Pipes[tempIndex], i * angleStep * playerFactor, pipeRotation);
                        item.MaterialSettings(i,AudioData.Instance.NormBandBuffer[i]);

                        bandIntervalTimers[i] = bandSpawnIntervals[i];
                    }

                    if(bandIntervalTimers[i] > 0f)
                        bandIntervalTimers[i] -= Time.deltaTime;

                }

                
            }

            else if(psInstance.EmptyPipeCount <= 0 && psInstance.Pipes.Length >5 && isTutorial && tutorialIndex == 2 && spawnCollidableObjects && tutorialObjectIntervalTimer <= 0) 
            {

                PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);

                int tempIndex = psInstance.Pipes.Length-4;
                float start = (Random.Range(0, psInstance.Pipes[tempIndex].PipeSegmentCount) + 0.5f);
                float angleStep = psInstance.Pipes[tempIndex].CurveAngle / psInstance.Pipes[tempIndex].CurveSegmentCount;
                float pipeRotation = start * 360f / psInstance.Pipes[tempIndex].PipeSegmentCount;
                item.Position(psInstance.Pipes[tempIndex], angleStep, pipeRotation);
                item.MaterialSettings(0,AudioData.Instance.NormBandBuffer[0]);

                //spawnCollidableObjects = false; ASSIGN FROM DIALOGUE SYSTEM
                tutorialObjectIntervalTimer = 3; 

            }
           
        }

        private void ProcessAmplitudePipes()
        {
            if(psInstance.Pipes != null && psInstance.EmptyPipeCount <= 0)
            {
                // int thresh = psInstance.Pipes.Length/2 + 1;
                for(int i = 0; i < psInstance.Pipes.Length; i++)
                {   
                    //psInstance.Pipes[i].GetComponent<Renderer>().materials[0].SetFloat("_Transparency", AudioData.Instance.AmplitudeBuffer);
                    // if(i < thresh)
                    //     psInstance.Pipes[i].GetComponent<Renderer>().materials[0].SetFloat("_Transparency", AudioData.Instance.AmplitudeBuffer);
                    // else 
                    //     psInstance.Pipes[i].GetComponent<Renderer>().materials[0].SetFloat("_Transparency", 1.0f);
                    // Color tintColor = new Color(AudioData.Instance.AmplitudeBuffer,AudioData.Instance.AmplitudeBuffer,AudioData.Instance.AmplitudeBuffer);
                    // psInstance.Pipes[i].GetComponent<Renderer>().materials[0].SetColor("_TintColor", tintColor); 
                    //Camera.main.backgroundColor = tintColor;
                }
                
                
            }

        }
        
        // TODO: Implement this as a colour changer for active objects
        // function instantiating objects on the nearest pipe per amplitude existence over some threshold
        // private void ProcessAmplitudeObjects()
        // {
        //     if((AudioData.Instance.AmplitudeBuffer >= amplitudeThreshold) && (ampIntervalTimer <= 0) && (psInstance.Pipes[psInstance.Pipes.Length-1] != null) && psInstance.EmptyPipeCount <=0)
        //     {
        //             int tempIndex = psInstance.Pipes.Length-3;


        //             float start = (Random.Range(0, psInstance.Pipes[tempIndex].PipeSegmentCount) + 0.5f);

        //             float angleStep = psInstance.Pipes[tempIndex].CurveAngle / psInstance.Pipes[tempIndex].CurveSegmentCount;

        //             PipeItem item = Instantiate<PipeItem>(ampItemPrefabs[Random.Range(0, ampItemPrefabs.Length)]);
                    
        //             float pipeRotation = start * 360f / psInstance.Pipes[tempIndex].PipeSegmentCount;
        //             item.Position(psInstance.Pipes[tempIndex], ((int) (AudioData.Instance.AmplitudeBuffer*10))* angleStep, pipeRotation);
        //             item.MaterialSettings(8,AudioData.Instance.AmplitudeBuffer);

        //             ampIntervalTimer = ampSpawnInterval;
        //     }

        //     if (ampIntervalTimer > 0f) 
        //         ampIntervalTimer -= Time.deltaTime; // substracting time from the interval if it's above zero, making sure on this line, no balls are being spawn, while this is counting down
            
        // }

        
        // function processing bonus object creation 
        private void ProcessAmplitudeBonusObjects()
        {
            int tempIndex2 = psInstance.Pipes.Length-3;
            float start = (Random.Range(psInstance.Pipes[tempIndex2].PipeSegmentCount/2, psInstance.Pipes[tempIndex2].PipeSegmentCount) + 0.5f);
            float startLife = (Random.Range(0, psInstance.Pipes[tempIndex2].PipeSegmentCount/2) + 0.5f);
            float pipeRotation = start * 360f / psInstance.Pipes[tempIndex2].PipeSegmentCount;
            float pipeRotationLife = startLife * 360f / psInstance.Pipes[tempIndex2].PipeSegmentCount;

           if(((AudioData.Instance.AmplitudeBuffer >= amplitudeThreshold) && (psInstance.Pipes[psInstance.Pipes.Length-1] != null) && psInstance.EmptyPipeCount <=0 ) || (isTutorial && tutorialIndex < 2 ))
            {   
                // process speed boost objects
                if(isTutorial)
                {
                        if(tutorialIndex == 0 && spawnSpeedBoost && (tutorialObjectIntervalTimer <= 0))
                        {
                            // TODO: TUTORIALIZE SPEED BOOST IF TUTORIAL
                            float angleStep = psInstance.Pipes[tempIndex2].CurveAngle / psInstance.Pipes[tempIndex2].CurveSegmentCount;

                            PipeItem item = Instantiate<PipeItem>(ampSpeedItemPrefabs[Random.Range(0, ampSpeedItemPrefabs.Length)]);
                            
                            
                            // float rng = Random.Range(0,7);
                            item.Position(psInstance.Pipes[tempIndex2], angleStep, pipeRotation);
                            item.MaterialSettings(0,AudioData.Instance.AmplitudeBuffer);

                            ampSpeedIntervalTimer = ampSpeedSpawnInterval;
                            //spawnSpeedBoost = false; set by dialogue system
                            tutorialObjectIntervalTimer = tutorialObjectIntervalTimerReset; 
                            //tutorialIndex++; 
                        }
                        else if(tutorialIndex == 1 && spawnLifeBoost && tutorialObjectIntervalTimer <= 0)
                        {
                            float angleStep = psInstance.Pipes[tempIndex2].CurveAngle / psInstance.Pipes[tempIndex2].CurveSegmentCount;
                            PipeItem item = Instantiate<PipeItem>(ampLifeItemPrefabs[Random.Range(0, ampLifeItemPrefabs.Length)]);                                        
                            float rng = Random.Range(2,5);
                            item.Position(psInstance.Pipes[tempIndex2], rng * angleStep, pipeRotationLife);
                            item.MaterialSettings(0,AudioData.Instance.AmplitudeBuffer);

                            ampLifeIntervalTimer = ampLifeSpawnInterval;
                            //spawnLifeBoost = false;  set by dialogue system
                            tutorialObjectIntervalTimer = 4; 

                            //tutorialIndex++; 
                        }
                }
                else 
                {
                    if((PlayerController.Instance.Velocity < 12 && (ampSpeedIntervalTimer <= 0)))
                    {
                        float angleStep = psInstance.Pipes[tempIndex2].CurveAngle / psInstance.Pipes[tempIndex2].CurveSegmentCount;
                        PipeItem item = Instantiate<PipeItem>(ampSpeedItemPrefabs[Random.Range(0, ampSpeedItemPrefabs.Length)]);
                        // float rng = Random.Range(0,7);
                        item.Position(psInstance.Pipes[tempIndex2], angleStep, pipeRotation);
                        item.MaterialSettings(0,AudioData.Instance.AmplitudeBuffer);
                        ampSpeedIntervalTimer = ampSpeedSpawnInterval;
                    }
                    // process extra life objects 
                    else if((PlayerController.Instance.LivesLeft < PlayerController.Instance.InitialLives && (ampLifeIntervalTimer <= 0)))
                    {
                        float angleStep = psInstance.Pipes[tempIndex2].CurveAngle / psInstance.Pipes[tempIndex2].CurveSegmentCount; 
                        PipeItem item = Instantiate<PipeItem>(ampLifeItemPrefabs[Random.Range(0, ampLifeItemPrefabs.Length)]);              
                        float rng = Random.Range(2,5);
                        item.Position(psInstance.Pipes[tempIndex2], rng * angleStep, pipeRotationLife);
                        item.MaterialSettings(0,AudioData.Instance.AmplitudeBuffer);
                        ampLifeIntervalTimer = ampLifeSpawnInterval;
                    }
                }



                    
            }

            if(isTutorial && tutorialObjectIntervalTimer > 0f)
                tutorialObjectIntervalTimer -= Time.deltaTime;
            
            if (ampSpeedIntervalTimer > 0f) 
                ampSpeedIntervalTimer -= Time.deltaTime; // substracting time from the interval if it's above zero, making sure on this line, no balls are being spawn, while this is counting down
            
            if (ampLifeIntervalTimer > 0f) 
                ampLifeIntervalTimer -= Time.deltaTime; // substracting time from the interval if it's above zero, making sure on this line, no balls are being spawn, while this is counting down
          
        }


    }
}