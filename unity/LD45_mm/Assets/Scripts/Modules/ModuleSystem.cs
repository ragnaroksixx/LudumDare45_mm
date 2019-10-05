using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSystem : MonoBehaviour
{
    public List<Module> currentModules;
    public Dictionary<Type, Module> allModules;
    // Start is called before the first frame update
    void Start()
    {
        allModules = new Dictionary<Type, Module>();
        currentModules = new List<Module>();

        allModules.Add(typeof(JumpModule), new JumpModule());
        allModules.Add(typeof(WalkModule), new WalkModule());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateModule<T>() where T : Module
    {
        currentModules.Add(allModules[typeof(T)]);
        allModules[typeof(T)].ActivateModule();

    }

    public void DeactivateModule<T>() where T : Module
    {
        Module m = allModules[typeof(T)];
        currentModules.Remove(m);
        m.DeactivateModule();
    }

    public bool HasModule<T>() where T : Module
    {
        return currentModules.Contains(allModules[typeof(T)]);
    }
}
