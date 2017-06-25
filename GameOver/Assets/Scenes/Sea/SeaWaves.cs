﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWaves : MonoBehaviour
{
    public float scale = 10.0f;
    public float speed = 1.0f;
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
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y/* + baseHeight[i].z*/) * scale;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mc.sharedMesh = mesh;
    }
}