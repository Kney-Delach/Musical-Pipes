using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioDataSystem {
    public class AudioObjectController : MonoBehaviour
    {   
        // reference to object's referenced audio frequency band
        [SerializeField]
        private int band;
        
        // reference to object's initial scale
        [SerializeField]
        private float initialScale; 

        // reference to object's scale multiplier
        [SerializeField]
        private float scaleMultiplier;

        // reference to smoothing boolean
        [SerializeField]
        private bool useBuffer = false;

        // reference to object material 
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
                    transform.localScale = new Vector3(transform.localScale.x, 
                                                        (AudioData.Instance.BandBuffer[band] * scaleMultiplier) + initialScale, 
                                                        transform.localScale.z);
                    
                    // assign freq based emission color value
                    Color tempColor = new Color(AudioData.Instance.NormBandBuffer[band], AudioData.Instance.NormBandBuffer[band], AudioData.Instance.NormBandBuffer[band]);
                    material.SetColor("_EmissionColor", tempColor);

                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, 
                                                        (AudioData.Instance.FrequencyBand[band] * scaleMultiplier) + initialScale, 
                                                        transform.localScale.z);
                    // assign freq based emission color value
                    Color tempColor = new Color(AudioData.Instance.NormBand[band], AudioData.Instance.NormBand[band], AudioData.Instance.NormBand[band]);
                    material.SetColor("_EmissionColor", tempColor);
                } 
            }
            

        }
    }
}