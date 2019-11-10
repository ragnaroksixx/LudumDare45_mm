
public abstract class Module
{
    public bool hasUpdate, hasFixedUpdate;
    public abstract string Name { get; }
    public abstract float Priority { get; }
    public abstract void ActivateModule();
    public abstract void DeactivateModule();
    public virtual string Caterorgy { get => "none"; }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual string GetTip()
    {
        return "";
    }
    public virtual void Default()
    {

    }
}

