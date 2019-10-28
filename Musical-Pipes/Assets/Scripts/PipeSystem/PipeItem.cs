using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers;

namespace PipeSystem {
    public class PipeItem : MonoBehaviour
    {   
        // reference to obstacle rotation transform
        private Transform rotater;

        [SerializeField]
        private MeshRenderer renderer; 
        // reference to the object material 
        private Material material; 

        [Header("Reference to band textures")]
        [SerializeField]    
        private Texture[] bandTextures;

        [Header("Functional Values")]
        // reference to functional item values (speedbost etc...)
        [SerializeField]
        private float amount = 0;
        public float Amount { get { return amount ; } }

        private void Awake () {
            rotater = transform.GetChild(0);
            if(renderer != null)
                material = renderer.materials[0];
        }

        // function iniailizing the position of the obstacle along a pipe
        public void Position (Pipe pipe, float curveRotation, float ringRotation) {
            transform.SetParent(pipe.transform, false);
            transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
            rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
            rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
        }
        
        public void MaterialSettings(int band, float value)
        {
            if(renderer != null)
            {
                //material.SetTexture("TextureName",bandTextures[band]);
                
                material.mainTexture = bandTextures[band];
                // Color albedo;

                // switch(band)
                // {   
                //     case 0:
                //         //albedo = new Color(value, value, 0);
                //         break;
                //     case 1:
                //         //albedo = new Color(value, 0, value);
                //         break;
                //     case 2:
                //         //albedo = new Color(0, value, value);
                //         break;
                //     case 3:
                //         //albedo = new Color(0, value, value/2);
                //         break;
                //     case 4:
                //         //albedo = new Color(0, value/2, value);
                //         break;
                //     case 5:
                //         //albedo = new Color(value/2, 0, value);
                //         break;
                //     case 6:
                //         //albedo = new Color(value, 0, value/2);
                //         break;
                //     case 7:
                //         //albedo = new Color(value, value/2, 0);
                //         break;
                //     default:
                //         //albedo = new Color(value/2, value, 0);
                //         break;
                // }

                // material.SetColor("_Albedo", albedo);

                // Color tempColor;
                // if(value >= 0.8)
                //     tempColor = new Color(0.8f, 0.8f, 0.8f);
                // else if(value <= 0.4f)                 
                //     tempColor = new Color(0.4f, 0.4f, 0.4f);
                // else 
                //     tempColor = new Color(value, value, value);

                // material.SetColor("_EmissionColor", tempColor);
            }
        }
    }
}