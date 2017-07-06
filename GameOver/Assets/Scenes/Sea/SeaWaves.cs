using UnityEngine;

public class SeaWaves : MonoBehaviour
{
    public float scale = 10.0f;
    public float speed = 1.0f;
    public float freq;
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;
    private Vector3[] baseHeight;
    MeshFilter mf;
    MeshCollider mc;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
    }

    void Update()
    {
        Mesh mesh = mf.mesh;

        if (baseHeight == null)
        {
            baseHeight = mesh.vertices;
        }

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y = CalculateY(baseHeight[i].x, baseHeight[i].z, 0);
            //vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mc.sharedMesh = mesh;
    }

    public float CalculateY(float x, float z, float timeOffset)
    {
        return Mathf.Sin((Time.time + timeOffset) * speed + x * freq /*+ baseHeight[i].y/* + baseHeight[i].z*/) * scale * (z + 0.5f);
    }
}
