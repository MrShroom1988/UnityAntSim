﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneVizTile : MonoBehaviour
{
    public GameObject tile;

    Material tileMaterial;

    // Start is called before the first frame update
    void Start()
    {
        tileMaterial = tile.GetComponent<Renderer>().material;
    }

    public void ChangeTransparancy(float newTransparancy)
    {
        Color color = tileMaterial.color;
        color.a = newTransparancy;
        tileMaterial.color = color;
    }
}
