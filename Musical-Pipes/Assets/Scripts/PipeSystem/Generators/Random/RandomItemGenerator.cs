using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeSystem {
    public class RandomItemGenerator : PipeItemGenerator
    {
        // reference to the set of random items to generate
        [SerializeField]
        private PipeItem[] itemPrefabs; 

        // function generating items randomly on the pipe
        public override void GenerateItems (Pipe pipe) 
        {
            float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (int i = 0; i < pipe.CurveSegmentCount; i++) {
                PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                float pipeRotation = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f) * 360f / pipe.PipeSegmentCount;
                item.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}