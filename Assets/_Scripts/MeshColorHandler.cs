using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColorHandler : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float hidingOutlineWidth;
    private float _unhidingOutlineWidth;
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterial.color = color;
    }
    public void HidingValues()
    {
        _unhidingOutlineWidth = meshRenderer.sharedMaterial.GetFloat("_Outline");
        meshRenderer.sharedMaterial.SetFloat("_Outline", hidingOutlineWidth);
    }

    public void UnhidingValues()
    {
        meshRenderer.sharedMaterial.SetFloat("_Outline", _unhidingOutlineWidth);
    }
    public Color GetColor()
    {
        return meshRenderer.sharedMaterial.color;
    }
}
