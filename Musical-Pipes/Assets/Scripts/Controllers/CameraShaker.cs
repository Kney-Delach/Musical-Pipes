using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControllers {
    public class CameraShaker : MonoBehaviour
    {   
        [Header("Shake Variables")]
        [SerializeField]
        private float duration = 0.2f;

        [SerializeField]
        private float magnitude = 0.2f;

        #region Singleton 

        private static CameraShaker instance;
        public static CameraShaker Instance { get { return instance ; } }

        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else 
                instance = this; 
        }

        private void OnDestroy()
        {
            if(instance == this)
                instance = null;
        }

        #endregion 

        // shake the camera
        public void StartShake()
        {
            StartCoroutine(Shake());
        }

        public IEnumerator Shake ()
        {
            Vector3 originalPosition = transform.localPosition;

            float timeElapsed = 0.0f;

            while(timeElapsed < duration)
            {
                float xPos = Random.Range(-1f, 1f) * magnitude;
                float zPos = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(xPos, originalPosition.y, zPos);

                timeElapsed += Time.deltaTime;

                yield return null; 
            }

            transform.localPosition = originalPosition;
        }
    }
}