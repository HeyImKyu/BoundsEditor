using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoundsEditor : UnityEditor.EditorWindow
{
    public GameObject avatar;
    public GameObject preset;
    public Vector3 additive;
    public bool includeInactive;

    [UnityEditor.MenuItem("Tools/Kyu/BoundsEditor")]
    public static void ShowWindow()
    {
        GetWindow<BoundsEditor>("BoundsEditor");
    }

    private void OnGUI()
    {
        avatar = (GameObject)EditorGUILayout.ObjectField("Avatar", avatar, typeof(GameObject), true);
        preset = (GameObject)EditorGUILayout.ObjectField("Preset", preset, typeof(GameObject), true);
        additive = EditorGUILayout.Vector3Field("Additive", additive);
        includeInactive = EditorGUILayout.ToggleLeft("Include inactive", includeInactive);

        if (GUILayout.Button("Do it!"))
            DoIt();
    }


    private Vector3 RotateAround(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        var dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    private void DoIt()
    {
        Bounds bodyBounds = GetBodyBounds();
        bodyBounds.size += additive;

        var smrs = avatar.transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);

        foreach (var renderer in smrs)
        {
            renderer.localBounds = bodyBounds;
        }
    }

    private Bounds GetBodyBounds()
    {
        return preset.GetComponent<SkinnedMeshRenderer>().localBounds;
    }
}
