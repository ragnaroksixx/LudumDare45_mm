
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
abstract class AnimationChangeModule : Module
{
    SpriteRenderer sr;
    public AnimationChangeModule(SpriteRenderer s)
    {
        sr = s;
    }
    public override void ActivateModule()
    {
        playerAnimationController.instance.SetVisibility(true, sr);
    }
    public override void DeactivateModule()
    {
        playerAnimationController.instance.SetVisibility(false, sr);
    }
    public override void Default()
    {
        base.Default();
        DeactivateModule();
    }
}
abstract class AnimationChangeModuleGun : AnimationChangeModule
{
    public AnimationChangeModuleGun(SpriteRenderer s) : base(s)
    {
    }
    public override void ActivateModule()
    {
        base.ActivateModule();
        playerAnimationController.instance.SetVisibility(true, playerAnimationController.instance.playerGun);
        playerAnimationController.instance.SetVisibility(false, playerAnimationController.instance.playerNoGun);
    }
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        if (ModuleSystem.instance.HasModule<GunModule>() || ModuleSystem.instance.HasModule<ChargeGunModule>()) return;

        playerAnimationController.instance.SetVisibility(false, playerAnimationController.instance.playerGun);
        playerAnimationController.instance.SetVisibility(true, playerAnimationController.instance.playerNoGun);
    }

}
abstract class TypeModule : Module
{
    public abstract string Caterorgy { get; }
}

class JumpModule : AnimationChangeModule
{
    public override string Name => "jump";
    public override float Priority => 3;

    public JumpModule(SpriteRenderer s) : base(s)
    {

    }
}
class AudioModule : AnimationChangeModule
{
    public override string Name => "audio";
    public override float Priority => 4;
    public AudioModule(SpriteRenderer s) : base(s)
    {

    }
}
class GunModule : AnimationChangeModuleGun
{
    public override string Name => "fire";
    public override float Priority => 3;
    public GunModule(SpriteRenderer s) : base(s)
    {

    }
}
class ChargeGunModule : AnimationChangeModuleGun
{
    public override string Name => "charge.exe";
    public override float Priority => 4;
    public ChargeGunModule(SpriteRenderer s) : base(s)
    {

    }
}
class WalkModule : BasicModule
{
    public override string Name => "movement";
    public override float Priority => 1;
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        PlayerMovement.instance.anim.SetBool("crawl", true);
    }

    public override void ActivateModule()
    {
        base.ActivateModule();
        PlayerMovement.instance.anim.SetBool("crawl", false);
    }
    public override void Default()
    {
        base.Default();
        PlayerMovement.instance.anim.SetBool("crawl", true);
    }
}
class PlayerHealthModule : BasicModule
{
    public override string Name => "hud";
    public override float Priority => 5;
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
    public override float Priority => 5;
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        EnemyHealthUI[] eHPs = GameObject.FindObjectsOfType<EnemyHealthUI>();
        foreach (EnemyHealthUI eHP in eHPs)
        {
            eHP.Hide();
        }
    }
    public override void ActivateModule()
    {
        base.ActivateModule();
        EnemyHealthUI[] eHPs = GameObject.FindObjectsOfType<EnemyHealthUI>();
        foreach (EnemyHealthUI eHP in eHPs)
        {
            eHP.Show();
        }
    }
    public override void Default()
    {
        base.Default();
        EnemyHealthUI[] eHPs = GameObject.FindObjectsOfType<EnemyHealthUI>();
        foreach (EnemyHealthUI eHP in eHPs)
        {
            eHP.Hide();
        }
    }
}
class CoreModule : Module
{
    public override string Name => "core32";
    public override float Priority => 0;
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
    public override float Priority => .9f;
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

    public override void Default()
    {
        base.Default();
        setVignette(1);
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
        playerAnimationController.instance.SetVisibility(true, playerAnimationController.instance.hat);
    }
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        playerAnimationController.instance.SetVisibility(false, playerAnimationController.instance.hat);
    }
    public override void Default()
    {
        base.Default();
        playerAnimationController.instance.SetVisibility(false, playerAnimationController.instance.hat);
    }
}

