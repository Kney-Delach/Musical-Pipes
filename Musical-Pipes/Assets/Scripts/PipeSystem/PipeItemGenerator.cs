using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeSystem {
    public abstract class PipeItemGenerator : MonoBehaviour
    {
        public abstract void GenerateItems(Pipe pipe);
    }
}