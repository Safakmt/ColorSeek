using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MeshColorHandler : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Color hidingSpecularColor;
    [SerializeField] private float hidingOutlineWidth;
    [SerializeField] private float colorChangeDuration;
    private float _unhidingOutlineWidth = 2f;
    private Color _unhidingSpecularColor;

    private void Awake()
    {
        meshRenderer.sharedMaterial.SetFloat("_Outline", _unhidingOutlineWidth);
        _unhidingSpecularColor = meshRenderer.sharedMaterial.GetColor("_SpecColor");
    }
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterial.color = color;
    }
    public void HidingValues()
    {
        meshRenderer.sharedMaterial.DOColor(hidingSpecularColor, "_SpecColor", colorChangeDuration);
        meshRenderer.sharedMaterial.DOFloat(hidingOutlineWidth, "_Outline", colorChangeDuration);
    }

    public void UnhidingValues()
    {
        meshRenderer.sharedMaterial.DOColor(_unhidingSpecularColor, "_SpecColor", colorChangeDuration);
        meshRenderer.sharedMaterial.DOFloat(_unhidingOutlineWidth, "_Outline", colorChangeDuration);
    }
    public Color GetColor()
    {
        return meshRenderer.sharedMaterial.color;
    }

    private void OnDisable()
    {
        meshRenderer.sharedMaterial.SetFloat("_Outline", _unhidingOutlineWidth);
        meshRenderer.sharedMaterial.SetColor("_SpecColor",_unhidingSpecularColor);
    }
}
