﻿
public abstract class Module
{
    public abstract string Name { get; }
    public abstract float Priority { get; }
    public abstract void ActivateModule();
    public abstract void DeactivateModule();
    public virtual string GetTip()
    {
        return "";
    }
    public virtual void Default()
    {

    }
}

