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
    public ModulePickUp moduleObjectPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        allModules = new Dictionary<Type, Module>();
        currentModules = new List<Module>();

        Add(typeof(JumpModule), new JumpModule());
        Add(typeof(CoreModule), new CoreModule());
        Add(typeof(WalkModule), new WalkModule());
        Add(typeof(GunModule), new GunModule());
        Add(typeof(ChargeGunModule), new ChargeGunModule());
        Add(typeof(MonochromeModule), new MonochromeModule(volume, monoChromeProfile, normalProfile));
        Add(typeof(FullSightModule), new FullSightModule(volume, normalProfile));
        Add(typeof(AudioModule), new AudioModule());
        Add(typeof(EnemyHealthModule), new EnemyHealthModule());
        Add(typeof(PlayerHealthModule), new PlayerHealthModule());
        instance = this;
    }
    private void Start()
    {
        foreach (Module module in allModules.Values)
        {
            module.Default();
        }
    }
    void Add(Type t, Module m)
    {
        allModules.Add(t, m);
    }
    public void Spawn(Type t, Pose p)
    {
        ModulePickUp.Spawn(t, allModules[t], p);
    }
    public void AddCollectedModule<T>(bool activate = true) where T : Module
    {
        AddCollectedModule(typeof(T), activate);
    }
    public void AddCollectedModule(Type t, bool activate = true)
    {
        ModuleUIObject ui = GameObject.Instantiate(uiPrefab);
        ui.Init(t, allModules[t], this);
        if (activate && CanAddComponent(t))
        {
            ui.OnAdd();

        }
        else
        {
            ui.OnRemove();
        }
        ui.AllowEdit(ModuleUIMenu.instance.IsOpen);
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
    public Module Pop()
    {
        if (currentModules.Count <= 0) return null;
        Module m = currentModules[currentModules.Count - 1];
        currentModules.RemoveAt(currentModules.Count - 1);

        return m;
    }
    public int ActivateModule(Type t)
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
            int index = -1;
            for (int i = 0; i < currentModules.Count; i++)
            {
                if (m.Priority < currentModules[i].Priority)
                {
                    currentModules.Insert(i, m);
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = currentModules.Count;
                currentModules.Add(m);
            }
            ModuleUIMenu.instance.UpdateCount(currentModules.Count);
            allModules[t].ActivateModule();
            return index;
        }
        return -1;
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
