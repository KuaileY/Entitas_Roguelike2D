using System;
using System.Collections.Generic;
using UnityEngine;
public class UIManager
{
    public Dictionary<UIType, GameObject> _UIDict = new Dictionary<UIType, GameObject>();
    private Transform _canvas;

    public UIManager()
    {
        _canvas = GameObject.Find("Canvas").transform;
        foreach (Transform t in _canvas)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    public GameObject GetSingleUI(UIType uiType)
    {
        if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
        {
            GameObject go = null;
            try
            {
                go = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path));
            }
            catch (Exception ex)
            {
                throw new Exception("UIManager Cannot Load.", ex);
            }
            go.transform.SetParent(_canvas, false);
            go.name = uiType.Name;
            _UIDict[uiType] = go;
            return go;
        }
        return _UIDict[uiType];
    }

    public void DestorySingleUI(UIType uiType)
    {
        if (!_UIDict.ContainsKey(uiType))
            return;
        if (_UIDict[uiType] == null)
        {
            _UIDict.Remove(uiType);
            return;
        }
        GameObject.Destroy(_UIDict[uiType]);
        _UIDict.Remove(uiType);
    }
}

