using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColorHandler : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterial.color = color;
    }

    public Color GetColor()
    {
        return meshRenderer.sharedMaterial.color;
    }
}
