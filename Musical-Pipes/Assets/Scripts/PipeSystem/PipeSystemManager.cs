using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioDataSystem;

namespace PipeSystem {

    public class PipeSystemManager : MonoBehaviour
    {   
        [Header("Pipe component references")]
        // reference to the pipe prefab 
        [SerializeField]
        private Pipe pipePrefab;

        // reference to the pipe count 
        [SerializeField]
        private int pipeCount = 1;

        // reference to the active pipes 
        private Pipe[] pipes; 
        public Pipe[] Pipes { get { return pipes ; } }

        // reference to the intial empty pipe count 
        [SerializeField]
        private int emptyPipeCount = 2;
        public int EmptyPipeCount { get { return emptyPipeCount ; } }

        private int pipeCountRef;
        
        #region Initialization

        // singleton initialization
        private static PipeSystemManager instance = null; 
        public static PipeSystemManager Instance { get { return instance ; } }
        private void Awake () 
        {
            if(instance != null)
                Destroy(gameObject);
            else 
            {
                instance = this; 

                pipes = new Pipe[pipeCount];
                
                for (int i = 0; i < pipes.Length; i++) {
                    Pipe pipe = pipes[i] = Instantiate<Pipe>(pipePrefab);
                    pipe.transform.SetParent(transform, false);
                    
                }

                pipeCountRef = emptyPipeCount;
            }
            
	    }   

        // function resetting the initial empty pipe counter
        public void ResetPipeCounter()
        {
            emptyPipeCount = pipeCountRef;
        }

        // function initializing first pipe
        public Pipe SetupFirstPipe () 
        {
            for(int i = 0; i < pipes.Length; i++)
            {
                Pipe pipe = pipes[i];
                pipe.Generate(i > emptyPipeCount);
                if(emptyPipeCount > 0)
                    emptyPipeCount--;
                if (i > 0) 
			    	pipe.AlignWith(pipes[i - 1]);
            }
            AlignNextPipeWithOrigin();
            
            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
	    }        
        
        #endregion

        #region Pipe Setup Functionality

        // function initializing ongoing pipes in the system
        public Pipe SetupNextPipe () 
        {
            ShiftPipes();
            AlignNextPipeWithOrigin();
            
            // generate new pipes when initializing following pipes
            pipes[pipes.Length - 1].Generate();
		    pipes[pipes.Length - 1].AlignWith(pipes[pipes.Length - 2]);

            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
	    }

        private void ShiftPipes () 
        {
            Pipe temp = pipes[0];
            for (int i = 1; i < pipes.Length; i++) {
                pipes[i - 1] = pipes[i];
            }
            pipes[pipes.Length - 1] = temp;
	    }

        private void AlignNextPipeWithOrigin () 
        {
            Transform transformToAlign = pipes[1].transform;
            for (int i = 0; i < pipes.Length; i++) {
                if (i != 1) 
				    pipes[i].transform.SetParent(transformToAlign);
			}
            
            
            transformToAlign.localPosition = Vector3.zero;
            transformToAlign.localRotation = Quaternion.identity;
            
            for (int i = 0; i < pipes.Length; i++) {
                if (i != 1) 
				    pipes[i].transform.SetParent(transform);
            }
        }
        #endregion 
    }

}