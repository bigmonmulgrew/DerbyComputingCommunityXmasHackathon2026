using UnityEditor;
using UnityEngine;

public class XmasLight : Bauble
{

    [SerializeField] Color lightColour;

    Light lightSource;

    Light LightSource
    {
        get 
        {
            if (lightSource == null) lightSource = GetComponent<Light>();
            if (lightSource == null) lightSource = GetComponentInChildren<Light>();
            return lightSource; 
        }
        set { lightSource = value; }
    }

    public override bool IsOutOfSync()
    {
        if (LightSource == null) return true;     // Cannot be in sync if not valid

        return base.IsOutOfSync() || IsLightOutOfSync(lightColour, lightSource.color);
    }

    private bool IsLightOutOfSync(Color lightColour, Color expected)
    {
        
        return lightColour != expected;
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (!Application.isPlaying) ApplyLights();
    }

    public void ApplyLights()
    {
        base.ApplyMaterial();

        if (!LightSource) return;

        LightSource.color = lightColour;

    }

    public void FetchLights()
    {
        base.FetchMaterial();

        if (!LightSource) return;

        lightColour = LightSource.color;

    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(XmasLight))]
public class XmasLightEditor : Editor
{
    XmasLight xmasLight;

    void OnEnable()
    {
        xmasLight = (XmasLight)target;
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

        if (xmasLight.IsOutOfSync())
        {
            EditorGUILayout.HelpBox(
                "One or more materials are out of sync with the renderer,\n" +
                "The light souce is out of sync with the colour,\n" +
                "Or the light source does not exist.",
                MessageType.Warning);

            if (GUILayout.Button("Apply Now -->"))
            {
                xmasLight.ApplyMaterial();
                EditorUtility.SetDirty(xmasLight);
            }

            if (GUILayout.Button("Fetch Now <--"))
            {
                xmasLight.FetchMaterial();
                EditorUtility.SetDirty(xmasLight);
            }
        }
    }

    void AutoSync()
    {
        if (!xmasLight) return;
        if (xmasLight.IsOutOfSync()) xmasLight.FetchLights();
    }
}
#endif