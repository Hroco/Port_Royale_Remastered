using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Waves : MonoBehaviour
{
    [SerializeField] private int dimension = 10;
    [SerializeField] private float uvScale;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Octave[] octaves;

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTries();
        mesh.uv = GenerateUVs();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;
        for (int x = 0; x <= dimension; x++)
        {
            for (int z = 0; z <= dimension; z++)
            {
                var y = 0f;

                for (int i = 0; i < octaves.Length; i++)
                {
                    if (octaves[i].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[i].scale.x) / dimension, (z * octaves[i].scale.y) / dimension) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + octaves[i].speed.magnitude * Time.time) * octaves[i].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[i].scale.x + Time.time * octaves[i].speed.x) / dimension, (z * octaves[i].scale.y + Time.time * octaves[i].speed.y) / dimension) -0.5f;
                        y += perl * octaves[i].height;
                    }
                }

                verts[CalculateIndex(x, z)] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = verts;
        mesh.RecalculateNormals();
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(dimension + 1) * (dimension + 1)];

        //equaly distributed verts
        for (int x = 0; x <= dimension; x++)
        {
            for (int z = 0; z <= dimension; z++)
            {
                verts[CalculateIndex(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    private int CalculateIndex(int x, int z)
    {
        return x * (dimension + 1) + z;
    }

    private int[] GenerateTries()
    {
        var tries = new int[mesh.vertices.Length * 6];

        //two triangles are one tile
        for (int x = 0; x < dimension; x++)
        {
            for (int z = 0; z < dimension; z++)
            {
                tries[CalculateIndex(x, z) * 6 + 0] = CalculateIndex(x, z);
                tries[CalculateIndex(x, z) * 6 + 1] = CalculateIndex(x + 1, z + 1);
                tries[CalculateIndex(x, z) * 6 + 2] = CalculateIndex(x + 1, z);
                tries[CalculateIndex(x, z) * 6 + 3] = CalculateIndex(x, z);
                tries[CalculateIndex(x, z) * 6 + 4] = CalculateIndex(x, z + 1);
                tries[CalculateIndex(x, z) * 6 + 5] = CalculateIndex(x + 1, z + 1);
            }
        }

        return tries;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[mesh.vertices.Length];

        //always set one uv over n tiles than flip the uv and set it again
        for (int x = 0; x <= dimension; x++)
        {
            for (int z = 0; z <= dimension; z++)
            {
                var vec = new Vector2((x / uvScale) % 2, (z / uvScale) % 2);
                uvs[CalculateIndex(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    public float GetHeight(Vector3 position)
    {
        //scale factor and position in local space
        var scale = new Vector3(1 / transform.localScale.x, 0, 1 / transform.localScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside plane
        p1.x = Mathf.Clamp(p1.x, 0, dimension);
        p1.z = Mathf.Clamp(p1.z, 0, dimension);
        p2.x = Mathf.Clamp(p2.x, 0, dimension);
        p2.z = Mathf.Clamp(p2.z, 0, dimension);
        p3.x = Mathf.Clamp(p3.x, 0, dimension);
        p3.z = Mathf.Clamp(p3.z, 0, dimension);
        p4.x = Mathf.Clamp(p4.x, 0, dimension);
        p4.z = Mathf.Clamp(p4.z, 0, dimension);

        //get the max distance to one of the edges and take that to compute max dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        //weighted sum
        var height = mesh.vertices[CalculateIndex((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + mesh.vertices[CalculateIndex((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + mesh.vertices[CalculateIndex((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + mesh.vertices[CalculateIndex((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;
    }


}
