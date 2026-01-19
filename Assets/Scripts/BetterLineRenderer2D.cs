using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class BetterLineRenderer2D : MonoBehaviour
{
    public Material baseMaterial;
    public Color color;
    public float width;
    Mesh mesh;

    public List<Vector2> points;
    IEnumerable<Vector3> pointsV3
    {
        get
        {
            float depth = transform.position.z;
            return points.Select(point => useWorldSpace ? point.V3(depth) : point.V3());
        }
    }
    public bool useWorldSpace = true;

    void OnEnable()
    {
        if (points == null)
        {
            points = new List<Vector2>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    void Draw()
    {
        Material material = baseMaterial ? new Material(baseMaterial) : new Material(Shader.Find("Particles/Standard Unlit"));
        material.color = color;
        mesh = Render();
        RenderParams rp = new RenderParams(material);
        Graphics.RenderMesh(rp, mesh, 0, useWorldSpace ? Matrix4x4.identity : transform.localToWorldMatrix);
    }

    Mesh Render()
    {
        Mesh newMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector3> positions = pointsV3.ToList();

        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector3 a = positions[i];
            Vector3 b = positions[i + 1];
            float w = Mathf.Min((b - a).magnitude, Math.Abs(width / 2));
            Vector3 r = Vector3.Cross(b - a, Vector3.forward).normalized;
            int offset = vertices.Count;
            vertices.Add(a + r * w); // 0 bottom right
            vertices.Add(a - r * w); // 1 bottom left
            vertices.Add(b - r * w); // 2 top    left
            vertices.Add(b + r * w); // 3 top    right

            indices.Add(0 + offset);
            indices.Add(1 + offset);
            indices.Add(2 + offset);

            indices.Add(2 + offset);
            indices.Add(3 + offset);
            indices.Add(0 + offset);
        }


        for (int i = 0; i < positions.Count - 2; i++)
        {
            Vector3 a = positions[i];
            Vector3 b = positions[i + 1];
            Vector3 c = positions[i + 2];
            float wab = Mathf.Min((b - a).magnitude, Math.Abs(width / 2));
            float wbc = Mathf.Min((c - b).magnitude, Math.Abs(width / 2));
            Vector3 rab = Vector3.Cross(b - a, Vector3.forward).normalized;
            Vector3 rbc = Vector3.Cross(c - b, Vector3.forward).normalized;
            int offset = vertices.Count;

            if (Vector2.SignedAngle(a - b, b - c) < 0)
            {
                vertices.Add(b - rab * wab); // 0 bottom left
                vertices.Add(b - rbc * wbc); // 1 top left
                vertices.Add(b);             // 2 middle                
                indices.Add(0 + offset);
                indices.Add(1 + offset);
                indices.Add(2 + offset);
            }
            else
            {
                vertices.Add(b);             // 0 middle    
                vertices.Add(b + rbc * wbc); // 2 top right   
                vertices.Add(b + rab * wab); // 1 bottom right       

                indices.Add(0 + offset);
                indices.Add(1 + offset);
                indices.Add(2 + offset);
            }


        }

        newMesh.SetVertices(vertices);
        newMesh.SetIndices(indices, MeshTopology.Triangles, 0);
        newMesh.RecalculateBounds();
        newMesh.Optimize();
        return newMesh;
    }
}

static class VectorExtension
{
    public static Vector3 V3(this Vector2 v2, float depth = 0)
    {
        return new Vector3(v2.x, v2.y, depth);
    }
}