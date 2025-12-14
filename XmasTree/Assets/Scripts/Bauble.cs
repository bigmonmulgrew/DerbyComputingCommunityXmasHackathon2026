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

    }
    public void FetchMaterial()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (!renderer) return;

        var mats = renderer.sharedMaterials;

        if (mats.Length <= 1) return;
        colour = mats[materialIndex];
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

        Undo.undoRedoPerformed += OnUndoRedo;

        AutoSync();
    }
    void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    void OnUndoRedo()
    {
        AutoSync();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())  AutoSync();

        if (bauble.IsOutOfSync())
        {
            EditorGUILayout.HelpBox(
                "Material out of sync with renderer",
                MessageType.Warning); 

            if (GUILayout.Button("Apply Now -->"))
            {
                bauble.ApplyMaterial();
                EditorUtility.SetDirty(bauble);
            }
            if (GUILayout.Button("Fetch Now <--"))
            {
                bauble.FetchMaterial();
                EditorUtility.SetDirty(bauble);
            }
        } 
    }
    void AutoSync()
    {
        if (!bauble) return;

        if (bauble.IsOutOfSync()) bauble.FetchMaterial(); 
    }
}
#endif