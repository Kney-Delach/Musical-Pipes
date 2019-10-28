using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem {
    public class AudioDataManager : MonoBehaviour
    {   
        [SerializeField]
        private float maxObjectScale = 100000;

        // reference to the distance between objects [Alters Radius]
        [SerializeField]
        private int objectDistance = 25;

        // reference to spawn object prefab 
        [SerializeField]
        private GameObject spawnObjectPrefab;

        private GameObject[] spawnedObjects;

        [SerializeField]
        private AudioData.AudioChannel displayChannel;
        
        private void Start()
        {
            InitializeObjects();
        }

        private void Update()
        {
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            for(int i = 0; i<AudioData.Instance.AudioSampleSize; i++)
            {
                if(spawnedObjects != null)
                {
                    float[] tempSamples = new float[AudioData.Instance.AudioSampleSize];

                    switch(displayChannel)
                    {
                        case AudioData.AudioChannel.Stereo:
                            for(int j = 0; j < AudioData.Instance.AudioSampleSize; j++)
                            {
                                tempSamples[j] = AudioData.Instance.AudioSamplesLeft[j] + AudioData.Instance.AudioSamplesRight[j];
                            }
                            break;
                        
                        case AudioData.AudioChannel.Left:
                            tempSamples = AudioData.Instance.AudioSamplesLeft;
                            break;
                        
                        case AudioData.AudioChannel.Right:
                            tempSamples = AudioData.Instance.AudioSamplesRight;
                            break;

                        default:
                            break;
                    }

                    if(AudioData.Instance.AudioSampleSize > 100 && i < AudioData.Instance.AudioSampleSize/10)
                        spawnedObjects[i].transform.localScale = new Vector3(10 ,(tempSamples[i] * (maxObjectScale/5)) + 2,10);
                    else
                        spawnedObjects[i].transform.localScale = new Vector3(10 ,(tempSamples[i] * maxObjectScale) + 2,10);
                }
            }
        }
        private void InitializeObjects()
        {
            spawnedObjects = new GameObject[AudioData.Instance.AudioSampleSize];    // one object for each sample
            float rotationAngle = 360f / AudioData.Instance.AudioSampleSize;
            for(int i = 0; i < AudioData.Instance.AudioSampleSize; i++)
            {
                GameObject obj = (GameObject) Instantiate(spawnObjectPrefab,this.transform.position,Quaternion.identity,this.transform);
                obj.name = "SampleObject" + i;
                this.transform.eulerAngles = new Vector3(0, -rotationAngle*i, 0);   // rotate parent transform 
                obj.transform.position = Vector3.forward * objectDistance; // set distances between objects
                spawnedObjects[i] = obj; 
            }
        }

    }
}