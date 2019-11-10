
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

class DoubleJumpModule : JumpModule
{
    public override string Name => "jump.dbl";
    public override float Priority => 3;
    bool useDoubleJump = false;
    public DoubleJumpModule(SpriteRenderer s, PlayerMovement pm) : base(s, pm)
    {

    }
    public override string GetTip()
    {
        return base.GetTip() + " Press [SPACE] to Jump";
    }
    public override void Update()
    {
        if (!move.wasGrounded && move.isGrounded)
            useDoubleJump = false;

        base.Update();
    }
    public override bool CanJump()
    {
        return (move.isGrounded || Time.time < move.coyoteTimeTrack || !useDoubleJump)
            && Input.GetKeyDown(KeyCode.Space);
    }
    public override void Jump()
    {
        if (!move.isGrounded)
            useDoubleJump = true;
        base.Jump();
    }
}
class JumpModule : AnimationChangeModule
{
    public override string Name => "jump";
    public override float Priority => 3;
    protected PlayerMovement move;
    public JumpModule(SpriteRenderer s, PlayerMovement pm) : base(s)
    {
        move = pm;
        hasUpdate = true;
    }
    public override void Update()
    {
        base.Update();
        if (CanJump())
        {
            Jump();
        }
    }
    public override string GetTip()
    {
        return base.GetTip() + " Press [SPACE] to Jump";
    }
    public virtual bool CanJump()
    {
        return (move.isGrounded || Time.time < move.coyoteTimeTrack) && Input.GetKeyDown(KeyCode.Space);
    }
    public virtual void Jump()
    {
        move.Jump();
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
    public override string GetTip()
    {
        return base.GetTip() + " Press Mouse Click to Shoot";
    }
}
class ChargeGunModule : AnimationChangeModuleGun
{
    public override string Name => "charge.exe";
    public override float Priority => 4;
    public ChargeGunModule(SpriteRenderer s) : base(s)
    {

    }
    public override string GetTip()
    {
        return base.GetTip() + " HOLD Mouse click to charge your shoot";
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
    public override string GetTip()
    {
        return base.GetTip() + " Increases you move speed";
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
    public override string GetTip()
    {
        return base.GetTip() + " Enemy HP is visible";
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
        PlayerHealth.instance.Die();
    }
    public override string GetTip()
    {
        return base.GetTip() + " Remove this Module to restart the level";
    }

}
abstract class VisionModule : Module
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

        t = DOTween.To(() => getVignette(), x => setVignette(x), 0, Time.timeScale).SetAutoKill(false);

    }
    public static void DisableSight()
    {
        if (t != null && !t.IsComplete())
            t.Kill();

        t = DOTween.To(() => getVignette(), x => setVignette(x), 1, Time.timeScale).SetAutoKill(false);
    }
    public static float getVignette()
    {
        return ModuleUIMenu.instance.blackoutImages[0].alpha;
    }
    public static void setVignette(float value)
    {
        foreach (CanvasGroup blackoutImage in ModuleUIMenu.instance.blackoutImages)
        {
            blackoutImage.alpha = value;
        }
    }

    public override void Default()
    {
        base.Default();
        setVignette(1);
    }
}
class MonochromeModule : VisionModule
{
    public override string Name => "sight.bw";

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

    public override string GetTip()
    {
        return base.GetTip() + " See the world without color";
    }
}
class FullSightModule : VisionModule
{
    public override string Name => "Sight.rgb";
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
    public override string GetTip()
    {
        return base.GetTip() + " See the world in full color";
    }
}
class DashModule : Module
{
    public override string Name => "dash.exe";
    public override float Priority => 6;
    protected PlayerMovement move;
    float time = 0.25f;
    float speed = 2.5f;
    public DashModule(PlayerMovement m) : base()
    {
        move = m;
        hasUpdate = true;
    }

    public override void ActivateModule()
    {
    }

    public override void DeactivateModule()
    {
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Dash();
    }
    public void Dash()
    {
        move.Dash(time, speed);
    }


}

class ShieldModule : Module
{
    public override string Name => "shield.blk";
    public override float Priority => 6;
    float cooldown = 5f;
    public ShieldModule() : base()
    {
        hasUpdate = true;
    }

    public override void ActivateModule()
    {
    }

    public override void DeactivateModule()
    {
        Shield(false);
    }
    public override void Default()
    {
        base.Default();
        Shield(false);
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Shield(true);
        else if (Input.GetKeyUp(KeyCode.Mouse1))
            Shield(false);
    }
    public void Shield(bool val)
    {
        playerAnimationController.instance.shield.gameObject.SetActive(val);
        PlayerMovement.instance.canMove = !val;
    }


}

