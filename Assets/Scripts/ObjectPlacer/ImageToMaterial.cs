using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageToMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] materials;


    // Start is called before the first frame update
    void Start()
    {
        var textureIndex = Random.Range(0, materials.Length);
        meshRenderer.material = materials[textureIndex];
    }
}
