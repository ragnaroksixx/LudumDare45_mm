
using System;
using System.Collections.Generic;
using UnityEngine;

public static class SavedData
{
    public static List<Type> allTypes;
    static SavedData()
    {
        Load();
    }

    public static void Save()
    {
        string types="";
        foreach (Type type in allTypes)
        {
            types += type.ToString() + ",";
        }
        types = types.Substring(0, types.Length - 1);

        PlayerPrefs.SetString("mods", types);
    }

    public static void Load()
    {
        allTypes = new List<Type>();
        string s = PlayerPrefs.GetString("mods");
        string[] types = s.Split(',');

        foreach (string type in types)
        {
            if (string.IsNullOrEmpty(type)) continue;
            allTypes.Add(Type.GetType(type));
        }
    }

    public static void AddModule(Type t)
    {
        if(!HasCollectedModule(t))
        {
            allTypes.Add(t);
        }
        Save();
    }

    public static bool HasCollectedModule(Type t)
    {
        return allTypes.Contains(t);
    }
}

