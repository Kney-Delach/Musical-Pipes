using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeSystem{ 
    public class Pipe : MonoBehaviour
    {
        #region Configuration Variables
        // reference to the radius of the curve the pipe is following
        [SerializeField]
        private float curveRadius = 0f;
        public float CurveRadius { get { return curveRadius ; } }

        // reference to the radius of the pipe
        [SerializeField]
        private float pipeRadius = 0f; 
        public float PipeRadius { get { return pipeRadius ; } }

        // reference to the segment count along pipe's curve 
        [SerializeField]
        private int curveSegmentCount = 16; 
        public int CurveSegmentCount { get { return curveSegmentCount ; } }

        // reference to segment count along pipe's surface
        [SerializeField]
        private int pipeSegmentCount = 16; 
        public int PipeSegmentCount { get { return pipeSegmentCount ; } }

        // reference to min and max pipe curve radius
        [SerializeField]
        private float minCurveRadius;
        [SerializeField]
        private float maxCurveRadius; 

        // reference to the min and max curve segmentation counts 
        [SerializeField]
        private int minCurveSegmentCount; 

        [SerializeField]
        private int maxCurveSegmentCount; 

        // reference to the distance between multiple rings 
        [SerializeField]
        private float ringDistance = 0.77f;

        // reference to pipe's relative rotation
        private float relativeRotation;
        public float RelativeRotation { get { return relativeRotation ; } }

        #endregion

        #region Mesh Variables

        // reference to the object mesh
        private Mesh mesh; 
        
        // reference to the mesh vertices 
        private Vector3[] vertices; 

        // reference to the mesh triangles
        private int[] triangles;

        // reference to uv co-ordinates of pipe material
        private Vector2[] uv; 

        #endregion 

        // reference to angle of curve of pipe 
        private float curveAngle;
        public float CurveAngle { get { return curveAngle ; } } 

        #region Item Generation References

        [SerializeField]
        private PipeItemGenerator[] itemGenerators;

        #endregion

        #region Initialization

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Pipe";
        }

        public void Generate (bool withItems = true) 
        {
            curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
            curveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);
            mesh.Clear();
            SetVertices();
            SetUV();
            SetTriangles();
            mesh.RecalculateNormals();

            // destroy all pipe's children before generating new ones
            for (int i = 0; i < transform.childCount; i++) 
		    	Destroy(transform.GetChild(i).gameObject);

            // TODO: Generate Items
            // if(false)
		    //     itemGenerators[Random.Range(0, itemGenerators.Length)].GenerateItems(this); // randomly choose a generator to generate items on this pipe
	    }   

        // function initializing the UV co-ordinates of the pipe's mesh
        private void SetUV () 
        {
            uv = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i+= 4) {
                uv[i] = Vector2.zero;
                uv[i + 1] = Vector2.right;
                uv[i + 2] = Vector2.up;
                uv[i + 3] = Vector2.one;
            }
		    mesh.uv = uv;
	    }
        private void CreateQuadRing (float u, int i) 
        {
		    float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		    int ringOffset = pipeSegmentCount * 4;
		
            Vector3 vertex = GetPointOnTorus(u, 0f);
            for (int v = 1; v <= pipeSegmentCount; v++, i += 4) {
                vertices[i] = vertices[i - ringOffset + 2];
                vertices[i + 1] = vertices[i - ringOffset + 3];
                vertices[i + 2] = vertex;
                vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
            }
	    }
        
        // function initializing the first quad ring 
        private void CreateFirstQuadRing (float u) 
        {
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;

            Vector3 vertexA = GetPointOnTorus(0f, 0f);
            Vector3 vertexB = GetPointOnTorus(u, 0f);
            for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4) {
                vertices[i] = vertexA;
                vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
                vertices[i + 2] = vertexB;
                vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
            }
        }


        // function setting the vertice values for the mesh
        private void SetVertices () 
        {
            vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];
            float uStep = ringDistance / curveRadius; //(2f * Mathf.PI) / curveSegmentCount;
            curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));  // assign curve angle
		    CreateFirstQuadRing(uStep);
            int iDelta = pipeSegmentCount * 4;
            for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
                CreateQuadRing(u * uStep, i);
            }
		    mesh.vertices = vertices;
        }

        // function setting the mesh triangle values for the pipe (initially outside vs eventual inside)
        private void SetTriangles () 
        {
            triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
            for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
                triangles[t] = i;
                // triangles[t + 1] = triangles[t + 4] = i + 1;
                // triangles[t + 2] = triangles[t + 3] = i + 2;
                triangles[t + 1] = triangles[t + 4] = i + 2;
			    triangles[t + 2] = triangles[t + 3] = i + 1;
                triangles[t + 5] = i + 3;
		    }
		    mesh.triangles = triangles;
        }

        #endregion
        
        #region Utilities 

        // utility function aligning this pipe with another pipe
        public void AlignWith(Pipe pipe)
        {
		    relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;
		
            transform.SetParent(pipe.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
            transform.Translate(0f, pipe.curveRadius, 0f);
            transform.Rotate(relativeRotation, 0f, 0f);
            transform.Translate(0f, -curveRadius, 0f);
            transform.SetParent(pipe.transform.parent);
            transform.localScale = Vector3.one;
        }

        // function returning the point on the surface of the pipe "Torus"
        private Vector3 GetPointOnTorus (float u, float v) 
        {
            Vector3 point;
            float r = (curveRadius + pipeRadius * Mathf.Cos(v));
            point.x = r * Mathf.Sin(u);
            point.y = r * Mathf.Cos(u);
            point.z = pipeRadius * Mathf.Sin(v);
            return point;
	    }

        #endregion

        #region Gizmos
        // private void OnDrawGizmos () 
        // {
        //     float uStep = (2f * Mathf.PI) / curveSegmentCount;
        //     float vStep = (2f * Mathf.PI) / pipeSegmentCount;

        //     for (int u = 0; u < curveSegmentCount; u++) {
        //         for (int v = 0; v < pipeSegmentCount; v++) {
        //             Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
        //             Gizmos.color = new Color(
        //                 1f,
        //                 (float)v / pipeSegmentCount,
        //                 (float)u / curveSegmentCount);
        //             Gizmos.DrawSphere(point, 0.1f);
        //         }
        //     }
	    // }


        #endregion
    }
}