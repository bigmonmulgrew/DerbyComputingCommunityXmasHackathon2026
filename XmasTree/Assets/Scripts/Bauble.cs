using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Bauble : MonoBehaviour
{
    [Header("Material Slots")]
    [SerializeField] int materialIndex = 1;
    [SerializeField] int insideMaterialIndex = 2;

    [Header("Materials")]
    [SerializeField] Material colour;
    [SerializeField] Material insideColour;

    public virtual bool IsOutOfSync()
    {
        return IsSlotOutOfSync(materialIndex, colour)
            || IsSlotOutOfSync(insideMaterialIndex, insideColour);
    }

    private bool IsSlotOutOfSync(int index, Material expected)
    {
        if (!expected) return false;

        var r = GetComponentInChildren<MeshRenderer>();
        if (!r) return false;

        var mats = r.sharedMaterials;
        if (index < 0 || index >= mats.Length) return false;

        return mats[index] != expected;
    }

    protected virtual void OnValidate()
    {
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (!renderer) return;

        var mats = renderer.sharedMaterials;
        bool changed = false;

        if (colour && materialIndex >= 0 && materialIndex < mats.Length)
        {
            mats[materialIndex] = colour;
            changed = true;
        }

        if (insideColour && insideMaterialIndex >= 0 && insideMaterialIndex < mats.Length)
        {
            mats[insideMaterialIndex] = insideColour;
            changed = true;
        }

        if (changed) renderer.sharedMaterials = mats;
    }

    public void FetchMaterial()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (!renderer) return;

        var mats = renderer.sharedMaterials;

        if (materialIndex >= 0 && materialIndex < mats.Length)
            colour = mats[materialIndex];

        if (insideMaterialIndex >= 0 && insideMaterialIndex < mats.Length)
            insideColour = mats[insideMaterialIndex];
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

        if (EditorGUI.EndChangeCheck()) AutoSync();

        if (bauble.IsOutOfSync())
        {
            EditorGUILayout.HelpBox(
                "One or more materials are out of sync with the renderer",
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
        if (bauble.IsOutOfSync())  bauble.FetchMaterial();
    }
}
#endif
