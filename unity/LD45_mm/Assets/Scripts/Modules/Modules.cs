
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

abstract class BasicModule : Module
{
    public override void ActivateModule()
    {
    }

    public override void DeactivateModule()
    {
    }
}
abstract class TypeModule : Module
{
    public abstract string Caterorgy { get; }
}

class JumpModule : BasicModule
{
    public override string Name => "jump";
}
class AudioModule : BasicModule
{
    public override string Name => "audio";
}
class GunModule : BasicModule
{
    public override string Name => "fire";
}
class ChargeGunModule : BasicModule
{
    public override string Name => "charge.exe";
}
class WalkModule : BasicModule
{
    public override string Name => "movement";

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        PlayerMovement.instance.Walk(Vector2.zero);
    }
}
class PlayerHealthModule : BasicModule
{
    public override string Name => "hud";

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        PlayerHealthUI.instance.Hide();
    }
    public override void ActivateModule()
    {
        base.ActivateModule();
        PlayerHealthUI.instance.Show();
    }

    public override void Default()
    {
        base.Default();
        PlayerHealthUI.instance.Hide();
    }
}
class EnemyHealthModule : BasicModule
{
    public override string Name => "enemy scanner";

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        PlayerMovement.instance.Walk(Vector2.zero);
    }
}
class CoreModule : Module
{
    public override string Name => "core32";

    public override void ActivateModule()
    {
    }

    public override void DeactivateModule()
    {
        Debug.LogError("Game Over");
    }
}
abstract class VisionModule : TypeModule
{
    public override string Caterorgy => "SIGHT";
    public PostProcessProfile normal;
    public PostProcessVolume volume;
    static Tweener t;
    
    public VisionModule(PostProcessVolume v, PostProcessProfile n)
    {
        normal = n;
        volume = v;
    }
    public override void DeactivateModule()
    {
        volume.profile = normal;
        DisableSight();
    }
    public static void EnableSight()
    {
        if (t != null && !t.IsComplete())
            t.Kill();

        t = DOTween.To(() => getVignette(), x => setVignette(x), 0, 1).SetAutoKill(false);

    }
    public static void DisableSight()
    {
        if (t != null && !t.IsComplete())
            t.Kill();

        t = DOTween.To(() => getVignette(), x => setVignette(x), 1, 1).SetAutoKill(false);
    }
    public static float getVignette()
    {
        return ModuleUIMenu.instance.blackoutImage.color.a;
    }
    public static void setVignette(float value)
    {
        Color c = ModuleUIMenu.instance.blackoutImage.color;
        c.a = value;
        ModuleUIMenu.instance.blackoutImage.color = c;
    }

}
class MonochromeModule : VisionModule
{
    public override string Name => "bw Sight";

    PostProcessProfile profile;


    public MonochromeModule(PostProcessVolume v, PostProcessProfile p, PostProcessProfile n) : base(v, n)
    {
        profile = p;
    }
    public override void ActivateModule()
    {
        volume.profile = profile;
        EnableSight();
    }

}
class FullSightModule : VisionModule
{
    public override string Name => "Sight";
    public FullSightModule(PostProcessVolume v, PostProcessProfile n) : base(v, n)
    {
    }
    public override void ActivateModule()
    {
        volume.profile = normal;
        EnableSight();
    }
}

