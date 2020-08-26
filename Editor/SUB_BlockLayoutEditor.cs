/// Daniel C Menezes
/// Procedural UI panels with rounded corners - https://danielcmcg.github.io/
/// 
/// Based on CiaccoDavide's Unity-UI-Polygon
/// Sourced from - https://github.com/CiaccoDavide/Unity-UI-Polygon

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProUIRC_Layout))]
[CanEditMultipleObjects]
public class ProUIRC_LayoutEditor : Editor
{
    ProUIRC_Layout blockLayout;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        blockLayout = (ProUIRC_Layout)target;

        EditorGUILayout.LabelField("Corners Radius");
        int i = 0;
        foreach (ProUIRC_LayoutVertice layoutVertice in blockLayout.layoutVerticeArray)
        {
            //float comp;
            //if (blockLayout.rectTransform.rect.width - blockLayout.layoutVerticeArray[layoutVertice.NeighborCornerHV.x].radius < blockLayout.rectTransform.rect.height - blockLayout.layoutVerticeArray[layoutVertice.NeighborCornerHV.y].radius)
            //    comp = blockLayout.rectTransform.rect.width - blockLayout.layoutVerticeArray[layoutVertice.NeighborCornerHV.x].radius;
            //else
            //    comp = blockLayout.rectTransform.rect.height - blockLayout.layoutVerticeArray[layoutVertice.NeighborCornerHV.y].radius;

            SerializedProperty elementRadius = verticeArrayProperty.GetArrayElementAtIndex(i).FindPropertyRelative("radius");
            //elementRadius.floatValue = EditorGUILayout.Slider(elementRadius.floatValue, 0, comp);
            
            EditorGUILayout.PropertyField(elementRadius);
            i++;
        }
        
        EditorGUILayout.PropertyField(fill);
        EditorGUI.BeginDisabledGroup(fill.boolValue);
        EditorGUILayout.PropertyField(thickness);
        EditorGUI.EndDisabledGroup();
        cornerSides.intValue = (int)EditorGUILayout.Slider(cornerSides.intValue, 1, 30);

        serializedObject.ApplyModifiedProperties();
    }

    SerializedProperty verticeArrayProperty;
    SerializedProperty fill;
    SerializedProperty thickness;
    SerializedProperty cornerSides;

    void OnEnable()
    {
        verticeArrayProperty = serializedObject.FindProperty("layoutVerticeArray");
        fill = serializedObject.FindProperty("fill");
        thickness = serializedObject.FindProperty("thickness");
        cornerSides = serializedObject.FindProperty("cornerSides");
    }

    void OnSceneGUI()
    {
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
