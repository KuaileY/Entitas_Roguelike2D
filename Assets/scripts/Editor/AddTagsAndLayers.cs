using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class AddTagsAndLayers
{
    static AddTagsAndLayers()
    {
        SerializedObject tagManager =
                    new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        //增加所有的Tags
        foreach (string str in Enum.GetNames(typeof(GameTags)))
        {
            AddTag(str, tagManager);
        }

        //从10层做起始层
        int startIndex = 8;
        int layerCount = 0;
        //增加所有的Layers
        foreach (var str in Enum.GetValues(typeof(GameLayers)))
        {
            AddLayer(str.ToString(), tagManager, startIndex + layerCount);
            layerCount++;
        }
        //增加所有的SortingLayers
        foreach (GameSortLayers str in Enum.GetValues(typeof(GameSortLayers)))
        {
            addSortingLayer(str, tagManager);
        }
    }


    static void addSortingLayer(GameSortLayers sortLayer, SerializedObject tagManager)
    {
        SerializedProperty sortingLayers = tagManager.FindProperty("m_SortingLayers");
        bool found = false;
        for (int i = 0; i < sortingLayers.arraySize; i++)
        {
            SerializedProperty t = sortingLayers.GetArrayElementAtIndex(i);
            if (t.FindPropertyRelative("name").stringValue.Equals(sortLayer.ToString()))
            {
                found = true;
                t.FindPropertyRelative("uniqueID").longValue = (long)sortLayer;
                break;
            }
        }
        if (!found)
        {
            sortingLayers.InsertArrayElementAtIndex(0);
            SerializedProperty sp = sortingLayers.GetArrayElementAtIndex(0);
            if (sp != null)
            {
                sp.FindPropertyRelative("name").stringValue = sortLayer.ToString();
                sp.FindPropertyRelative("uniqueID").longValue = (long)sortLayer;
            }
        }
        tagManager.ApplyModifiedProperties();
    }

    static void AddTag(string tag, SerializedObject tagManager)
    {
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tag)) { found = true; break; }
        }
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = tag;
        }
        tagManager.ApplyModifiedProperties();
    }

    static void AddLayer(string layer, SerializedObject tagManager, int index)
    {
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        bool found = false;
        for (int i = 0; i < layersProp.arraySize; i++)
        {
            SerializedProperty t = layersProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(layer)) { found = true; break; }
        }
        if (!found)
        {
            //Debug.Log(layersProp.arraySize);
            SerializedProperty sp = layersProp.GetArrayElementAtIndex(index);
            if (sp != null) sp.stringValue = layer;
        }
        tagManager.ApplyModifiedProperties();
    }


}