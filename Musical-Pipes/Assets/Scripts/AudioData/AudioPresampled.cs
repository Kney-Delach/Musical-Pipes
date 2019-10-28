using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem {
    public class AudioPresampled : MonoBehaviour
    {
        // reference to the objects audiosource
        [SerializeField]
        private AudioSource audioSource; 

        private float[] audioSamples; 

        private void Start()
        {

        }
    }
}