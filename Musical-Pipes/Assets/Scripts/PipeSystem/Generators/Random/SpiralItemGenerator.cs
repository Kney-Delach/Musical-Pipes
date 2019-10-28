using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeSystem {
    public class SpiralItemGenerator : PipeItemGenerator
    {
        // reference to the set of random items to generate
        [SerializeField]
        private PipeItem[] itemPrefabs; 

        // function generating items in a spiral (clockwise or counterclockwise)
        public override void GenerateItems (Pipe pipe) 
        {
            float start = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f);
            float direction = Random.value < 0.5f ? 1f : -1f;

            float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (int i = 0; i < pipe.CurveSegmentCount; i++) {
                PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                float pipeRotation = (start + i * direction) * 360f / pipe.PipeSegmentCount;
                item.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}