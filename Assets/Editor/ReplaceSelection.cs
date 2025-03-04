using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class ReplaceSelection : ScriptableWizard
{
    static GameObject replacement = null;
    static bool keep = false;
 
    public GameObject ReplacementObject = null;
    public bool KeepOriginals = false;
 
    [MenuItem("GameObject/-Replace Selection...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard(
            "Replace Selection", typeof(ReplaceSelection), "Replace");
    }
 
    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keep;
    }
 
    void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keep = KeepOriginals;
    }
 
    void OnWizardCreate()
    {
        if (replacement == null)
            return;
 
        Undo.RegisterSceneUndo("Replace Selection");
 
        Transform[] transforms = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 
        foreach (Transform t in transforms)
        {
            GameObject g;
            PrefabType pref = EditorUtility.GetPrefabType(replacement);
 
            if (pref == PrefabType.Prefab || pref == PrefabType.ModelPrefab)
            {
                g = (GameObject)EditorUtility.InstantiatePrefab(replacement);
            }
            else
            {
                g = (GameObject)Editor.Instantiate(replacement);
            }
 
            Transform gTransform = g.transform;
            gTransform.parent = t.parent;
            g.name = replacement.name;
            gTransform.localPosition = t.localPosition;
            gTransform.localScale = t.localScale;
            gTransform.localRotation = t.localRotation;
        }
 
        if (!keep)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                GameObject.DestroyImmediate(g);
            }
        }
    }
}