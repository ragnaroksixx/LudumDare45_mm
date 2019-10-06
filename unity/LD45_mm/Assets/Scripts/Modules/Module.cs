﻿
public abstract class Module
{
    public abstract string Name { get; }
    public abstract int Priority { get; }
    public abstract void ActivateModule();
    public abstract void DeactivateModule();
    public virtual void Default()
    {

    }
}

