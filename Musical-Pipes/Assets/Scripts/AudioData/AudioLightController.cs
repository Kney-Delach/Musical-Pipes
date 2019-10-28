using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem {
    
    [RequireComponent(typeof(Light))]
    public class AudioLightController : MonoBehaviour
    {
        // reference to band number
        [SerializeField]
        private int band; 

        // reference to minimum light intensity
        [SerializeField]
        private float minIntensity = 0;

        // reference to maximum light intensity
        [SerializeField]
        private float maxIntensity = 1;

        // reference to light component
        private Light light;
        
        private void Awake()
        {
            light = GetComponent<Light>();
        }

        private void Update()
        {
            light.intensity = (AudioData.Instance.NormBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;     // calculate light intensity
        }
    }
}