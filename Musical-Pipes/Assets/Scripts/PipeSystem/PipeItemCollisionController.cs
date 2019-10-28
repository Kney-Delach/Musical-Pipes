using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeSystem {
    public class PipeItemCollisionController : MonoBehaviour
    {
        private float timeInitialized;
        public float TimeInitialized { get { return  timeInitialized; } }
        private void Awake()
        {
            timeInitialized = Time.deltaTime;
        }
        
        private void Update()
        {
            timeInitialized += Time.deltaTime;
        }
        public void OnTriggerEnter(Collider collider)
        {
            if(collider.tag != "Player")
            {
                if(timeInitialized < collider.GetComponent<PipeItemCollisionController>().TimeInitialized)
                {
                    //Debug.Log("Destroying Object, time: " + timeInitialized.ToString("F2"));
                    Destroy(gameObject);
                }
                else
                {
                    //Debug.Log("NOT Destroying Object, time: " + timeInitialized.ToString("F2"));
                }

            }
        }
    }
}