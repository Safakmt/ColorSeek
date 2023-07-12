using UnityEngine;
using UnityEditor;

public class RaycastEditorScript : EditorWindow
{
    private Transform startPoint;
    private Transform endPoint;
    private HidingSpot hidingSpot;

    [MenuItem("Custom/Raycast Editor")]
    private static void OpenWindow()
    {
        RaycastEditorScript window = GetWindow<RaycastEditorScript>();
        window.titleContent = new GUIContent("Raycast Editor");
        window.Show();
    }

    private void OnGUI()
    {

        if (Selection.activeGameObject.TryGetComponent<HidingSpot>(out hidingSpot))
        {
            startPoint = hidingSpot.GetHidingTransform();
            endPoint = Selection.activeGameObject.transform;
        }
        else
        {
            Debug.Log("Object has not a hiding spot");
        }

        if (GUILayout.Button("Perform Raycast"))
        {
            PerformRaycast();
        }
    }



    private void PerformRaycast()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("Start Point or End Point is not assigned!");
            return;
        }

        Collider col = Selection.activeGameObject.GetComponent<Collider>();
        col.enabled = false;

        MeshCollider meshColl = Selection.activeGameObject.AddComponent<MeshCollider>();
        meshColl.enabled = true;

        Ray ray = new Ray(startPoint.position, (endPoint.position - startPoint.position).normalized);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hideable"))
            {
                Debug.Log(hit.collider.gameObject.name);
                Texture2D texture = (Texture2D) hit.transform.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

                Debug.Log(hit.textureCoord);
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;
               
                Color pixelColor = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);


                hidingSpot.SetHidingColor(pixelColor);
            }
        }

        col.enabled = true;
        DestroyImmediate(meshColl);
    }
}