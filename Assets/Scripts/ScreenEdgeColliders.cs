// adds EdgeCollider2D colliders to screen edges
// only works with orthographic camera

using UnityEngine;
using System.Collections;


    public class ScreenEdgeColliders : MonoBehaviour
    {
        void Awake()
        {
            AddCollider();
        }

        void AddCollider()
        {
            if (Camera.main == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

            var cam = Camera.main;
            if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

            var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
            var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
            var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

            // add or use existing EdgeCollider2D
            var edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();
            edge.edgeRadius = 3.0f;

            var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        
            for (int i = 0; i < edgePoints.Length; i++)
            {
                if(edgePoints[i].x < 0)
                {
                    edgePoints[i].x -= edge.edgeRadius;
                }
                else
                {
                    edgePoints[i].x += edge.edgeRadius;

                }
                if (edgePoints[i].y < 0)
                {
                    edgePoints[i].y -= edge.edgeRadius;
                }
                else
                {
                    edgePoints[i].y += edge.edgeRadius;
                }
        }
            edge.points = edgePoints;
        }
    }
