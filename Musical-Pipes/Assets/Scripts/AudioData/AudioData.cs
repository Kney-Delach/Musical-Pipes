using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem{

    public class AudioData : MonoBehaviour
    {
        // reference to the objects audiosource
        private AudioSource audioSource; 
        public AudioSource AudioSource { get { return audioSource ; } }

        // reference to audio samples array size
        [SerializeField]
        private int audioSampleSize = 512;
        public int AudioSampleSize { get { return audioSampleSize ; } }

        // reference to audio source samples 
        private float[] audioSamplesLeft;
        public float[] AudioSamplesLeft { get { return audioSamplesLeft ; } }
        private float[] audioSamplesRight;
        public float[] AudioSamplesRight { get { return audioSamplesRight ; } }

        [SerializeField]
        private int frequencyBandSize = 8;
        public int FrequencyBandSize { get { return frequencyBandSize ; } }
        
        // reference to frequency band
        private float[] frequencyBand; 
        public float[] FrequencyBand { get { return frequencyBand ; } }

        // reference to freq band buffer array, used for smoothing out visualization
        private float[] bandBuffer; 
        public float[] BandBuffer { get { return bandBuffer ; } }

        // reference to buffer decrease value per frame usage
        private float[] bufferDecrease; 
        public float[] BufferDecrease { get { return bufferDecrease ; } }

        // reference to max frequency value in each audio band
        private float[] bandMaxVal;

        // reference to the initial band values 
        [SerializeField]
        private float initialBandValue = 0;

        // refernece to normalized freq band values
        private float[]  normBand;
        public float[] NormBand { get { return normBand ; } }

        // refernece to normalized freq band values
        private float[]  normBandBuffer;
        public float[] NormBandBuffer { get { return normBandBuffer ; } }

        // reference to the amplitude 
        private float amplitude; 
        public float Amplitude { get { return amplitude ; } }

        // reference to the amplitude buffer - used for smoothing
        [SerializeField]
        private float amplitudeBuffer; 
        public float AmplitudeBuffer { get { return amplitudeBuffer ; } }

        // reference to max amplitude value (initially set to 5 for use in initial iterations)
        private float amplitudeMaxVal = 5;


        public enum AudioChannel { Stereo, Left, Right};
        
        [SerializeField]
        private AudioChannel channel;
        public AudioChannel Channel { get { return channel ; } }

        #region SINGELTON

        // reference to the instance 
        private static AudioData instance;
        public static AudioData Instance { get { return instance; } }

        // initialize instance
        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;

                //audioSource = GetComponent<AudioSource>();  // assign current audio source

                audioSamplesLeft = new float[audioSampleSize];  
                audioSamplesRight = new float[audioSampleSize];  
                frequencyBand = new float[frequencyBandSize];
                bandBuffer = new float[frequencyBandSize];
                bufferDecrease = new float[frequencyBandSize];
                bandMaxVal = new float[frequencyBandSize];
                normBand = new float[frequencyBandSize];
                normBandBuffer = new float[frequencyBandSize];
            } 
        }

        private void Start()
        {
            if(audioSource != null)
                InitializeMaxValues();
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        #endregion

        private void Update()
        {
            if(audioSource != null)
            {
                GetAudioSourceSpectrum();
                CalculateFrequencyBands();
                ProcessBandBuffer();
                CalculateNormalizedBands();
                CalculateAmplitude();
            }

        }

        public void SetAudioSource(AudioSource source)
        {
            audioSource = source; 
            InitializeMaxValues();
        }

        private void InitializeMaxValues()
        {
            for(int i = 0; i < frequencyBandSize; i++)
            {
                bandMaxVal[i] = initialBandValue;
            }
        }

        // function calculating the amlitude tally of the all the audio frequencies 
        private void CalculateAmplitude()
        {
            float tempAmp = 0;
            float tempAmpBuffer = 0;

            // calculate amplitudes for all bands
            for(int i = 0; i < frequencyBandSize; i++)
            {
                tempAmp += frequencyBand[i];
                tempAmpBuffer += bandBuffer[i];
            }

            // assign new max val if necessary
            if(tempAmp > amplitudeMaxVal)
            {
                amplitudeMaxVal = tempAmp;
            }
            
            amplitude = tempAmp / amplitudeMaxVal;
            amplitudeBuffer = tempAmpBuffer / amplitudeMaxVal;

        }   

        // function calculating normalized band values (produces intensity within range 0-1)
        private void CalculateNormalizedBands()
        {
            for(int i = 0; i< frequencyBandSize; i++)
            {
                if(frequencyBand[i] > bandMaxVal[i])
                {
                    bandMaxVal[i] = frequencyBand[i];
                }

                normBand[i] = (frequencyBand[i] / bandMaxVal[i]);
                normBandBuffer[i] = (bandBuffer[i] / bandMaxVal[i]);
            }
        }
        
        // function processing the smoothing buffer for the audio bands
        private void ProcessBandBuffer()
        {
            for(int i = 0; i < frequencyBandSize; ++i)
            {
                if(frequencyBand[i] > bandBuffer[i])
                {
                    bandBuffer[i] = frequencyBand[i];
                    bufferDecrease[i] = 0.005f;
                }

                if( frequencyBand[i] < bandBuffer[i])
                {
                    bandBuffer[i] -= bufferDecrease[i];
                    bufferDecrease[i] *= 1.2f;
                }
            }
        }
        // function calculating the frequency band values
        private void CalculateFrequencyBands()
        {
            /*
            22050 / audioSampleSize = N hertz per sample
            22050 / 8 = 43

            A: 20-60 Hz
            B: 60-250 Hz
            C: 250-500 Hz
            D: 500-2000 Hz
            E: 2000-4000 Hz
            F: 4000-6000 Hz
            G: 6000-20000 Hz
            

            B0: 2 samples - 2N [0-86]
            B1: 4 samples - 4N [87-258]
            B2: 8 samples - 8N [259-602]
            B3: 16 samples - 16N [603-1290]
            B4: 32 samples - 32N [1291-2666]
            B5: 64 samples - 64N [2667-5418]
            B6: 128 samples - 128N [5419-10922]
            B7: 256 samples - 256N [10923-21930]
            */

            int count = 0;
            for(int i = 0; i < frequencyBandSize; i++)
            {
                int sampleCount = (int)Mathf.Pow(2,i) * 2;  // calculate number of samples in current iteration 

                float bandAverage = 0;

                if(i == (frequencyBandSize-1))
                    sampleCount += 2;
                
                for(int j = 0; j < sampleCount; j++)
                {
                    if(channel == AudioChannel.Stereo)
                        bandAverage += audioSamplesLeft[count] + audioSamplesRight[count] * (count + 1);
                    else if(channel == AudioChannel.Left)
                        bandAverage += audioSamplesLeft[count] * (count + 1);
                    else if(channel == AudioChannel.Right)
                        bandAverage += audioSamplesRight[count] * (count + 1);
                    
                    count++;
                }

                bandAverage /= count;

                frequencyBand[i] = bandAverage * 10;
            }
        }

        // function getting the spectrum data for the audio sample using the Blackman algorithm FFT window        
        private void GetAudioSourceSpectrum()
        {
            audioSource.GetSpectrumData(audioSamplesLeft, 0, FFTWindow.Blackman);
            audioSource.GetSpectrumData(audioSamplesRight, 1, FFTWindow.Blackman);
        }
    }
}