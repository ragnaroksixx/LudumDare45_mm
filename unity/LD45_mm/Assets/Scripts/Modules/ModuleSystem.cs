using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ModuleSystem : MonoBehaviour
{
    public List<Module> currentModules;
    public Dictionary<Type, Module> allModules;

    public Transform attachedList, unattachedList;
    public ModuleUIObject uiPrefab;
    public static ModuleSystem instance;

    public PostProcessVolume volume;
    public PostProcessProfile monoChromeProfile;
    public PostProcessProfile normalProfile;

    public int maxComponents = 3;
    // Start is called before the first frame update
    void Awake()
    {
        allModules = new Dictionary<Type, Module>();
        currentModules = new List<Module>();

        allModules.Add(typeof(JumpModule), new JumpModule());
        allModules.Add(typeof(CoreModule), new CoreModule());
        allModules.Add(typeof(WalkModule), new WalkModule());
        allModules.Add(typeof(GunModule), new GunModule());
        allModules.Add(typeof(ChargeGunModule), new ChargeGunModule());
        allModules.Add(typeof(MonochromeModule), new MonochromeModule(volume, monoChromeProfile, normalProfile));
        allModules.Add(typeof(FullSightModule), new FullSightModule(volume, normalProfile));

        instance = this;
    }

    public void AddCollectedModule<T>(bool activate) where T : Module
    {
        ModuleUIObject ui = GameObject.Instantiate(uiPrefab);
        ui.Init(typeof(T), allModules[typeof(T)], this);
        if (activate)
        {
            ui.OnAdd();

        }
        else
        {
            ui.OnRemove();
        }
        ui.AllowEdit(false);
    }
    public void ActivateModule<T>() where T : Module
    {
        ActivateModule(typeof(T));
    }
    public bool CanAddComponent<T>() where T : Module
    {
        return CanAddComponent(typeof(T));
    }
    public bool CanAddComponent(Type t)
    {
        if (currentModules.Count < maxComponents) return true;

        Module m = allModules[t];
        if (!currentModules.Contains(m))
        {
            if (m is TypeModule)
            {
                foreach (Module item in currentModules)
                {
                    if (item is TypeModule)
                    {
                        TypeModule a = m as TypeModule;
                        TypeModule b = item as TypeModule;

                        if (a.Caterorgy == b.Caterorgy)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public void DeactivateModule<T>() where T : Module
    {
        DeactivateModule(typeof(T));
    }
    public void DeactivateModule(Type t)
    {
        Module m = allModules[t];
        DeactivateModule(m);
    }
    public void DeactivateModule(Module m)
    {
        if (currentModules.Contains(m))
        {
            currentModules.Remove(m);
            ModuleUIMenu.instance.UpdateCount(currentModules.Count);
            m.DeactivateModule();
        }
    }
    public void ActivateModule(Type t)
    {
        Module m = allModules[t];
        if (!currentModules.Contains(m))
        {
            if (m is TypeModule)
            {
                foreach (Module item in currentModules)
                {
                    if (item is TypeModule)
                    {
                        TypeModule a = m as TypeModule;
                        TypeModule b = item as TypeModule;

                        if (a.Caterorgy == b.Caterorgy)
                        {
                            ModuleUIObject ui = ModuleUIMenu.instance.FindUI(item.GetType());
                            ui.OnRemove();
                            ui.AllowEdit(ModuleUIMenu.instance.IsOpen);
                            break;
                        }
                    }

                }
            }
            currentModules.Add(m);
            ModuleUIMenu.instance.UpdateCount(currentModules.Count);
            allModules[t].ActivateModule();
        }
    }
    public bool HasModule<T>() where T : Module
    {
        return currentModules.Contains(allModules[typeof(T)]);
    }
    public bool HasModule(Type t)
    {
        return currentModules.Contains(allModules[t]);
    }
}
