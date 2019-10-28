using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem {

    [RequireComponent(typeof(MeshRenderer))]
    public class AudioAmplitudeController : MonoBehaviour
    {
        // reference to object's initial scale 
        [SerializeField]
        private float initialScale = 1;

        // reference to object's max scale
        [SerializeField]
        private float maxScale = 8;

        // reference to bool whether using buffer or not
        [SerializeField]
        private bool useBuffer = false;

        // reference to object's rgb multipliers
        [SerializeField]
        private float red = 0;
        
        [SerializeField]
        private float green = 0;
        
        [SerializeField]
        private float blue = 0;

        // reference to object's material 
        private Material material; 

        private void Start()
        {
            if(GetComponent<MeshRenderer>().materials.Length > 0)
                material = GetComponent<MeshRenderer>().materials[0];
        }

        private void Update()
        {
            if(material != null)
            {
                if(useBuffer)
                {
                    float scale = (AudioData.Instance.AmplitudeBuffer * maxScale) + initialScale;
                    transform.localScale = new Vector3(scale, 
                                                        scale, 
                                                        scale);
                    
                    // assign freq based emission color value
                    Color tempColor = new Color(AudioData.Instance.AmplitudeBuffer * red , AudioData.Instance.AmplitudeBuffer * green, AudioData.Instance.AmplitudeBuffer * blue);
                    material.SetColor("_EmissionColor", tempColor);

                }
                else
                {
                    float scale = (AudioData.Instance.Amplitude * maxScale) + initialScale;
                    transform.localScale = new Vector3(scale, 
                                                        scale, 
                                                        scale);

                    // assign freq based emission color value
                    Color tempColor = new Color(AudioData.Instance.Amplitude * red , AudioData.Instance.Amplitude * green, AudioData.Instance.Amplitude * blue);
                    material.SetColor("_EmissionColor", tempColor);
                } 
            }
            
        }
    }
}