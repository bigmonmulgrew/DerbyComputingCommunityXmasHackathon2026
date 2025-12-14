using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Bauble : MonoBehaviour
{
    [SerializeField] int materialIndex = 1;
    [SerializeField] Material colour;

    public bool IsOutOfSync()
    {
        return Slot1Material != colour;
    }  
   

    private Material Slot1Material
    {
        get
        {
            var r = GetComponentInChildren<MeshRenderer>();
            return r && r.sharedMaterials.Length > 1
                ? r.sharedMaterials[materialIndex]
                : null;
        }
    }

    private void OnValidate()
    {
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (!renderer || !colour) return;

        var mats = renderer.sharedMaterials;

        if (mats.Length <= 1) return;

        mats[materialIndex] = colour;
        renderer.sharedMaterials = mats;

        Debug.Log("Validated");
    }

   
}


#if UNITY_EDITOR
[CustomEditor(typeof(Bauble))]
public class BaubleEditor : Editor
{
    Bauble bauble;

    void OnEnable()
    {
        bauble = (Bauble)target;

        // Validate when inspector appears
        if (bauble.IsOutOfSync()) bauble.ApplyMaterial();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (bauble.IsOutOfSync())
        {
            EditorGUILayout.HelpBox(
                "Material out of sync with renderer",
                MessageType.Warning); 

            if (GUILayout.Button("Fix Now"))
            {
                bauble.ApplyMaterial();
                EditorUtility.SetDirty(bauble);
            }
        } 
    }
}
#endif