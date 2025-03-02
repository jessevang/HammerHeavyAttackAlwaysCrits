
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;
using StardewValley.Menus;
using HammerHeavyAttackAlwaysCrits;
using StardewValley.Tools;
using Microsoft.Xna.Framework;

/// <summary>Applies prefix Harmony patches to <see cref="MeleeWeapon.triggerClubFunction"/> to add crit damage multiplier to the club special attack.</summary>

internal class PatchName : BasePatcher
{

    public readonly Config _config;
    public static ModEntry Instance;

    public PatchName(Config Config)
    {
        _config = Config;
    }


    public PatchName(Config Config, ModEntry _instance)
    {
        _config = Config;
        Instance = _instance;
    }




    public override void Apply(Harmony harmony, IMonitor monitor)
    {
        //Applies prefix on MeleeWeapon.triggerClubFunction
        harmony.Patch(
            original: this.RequireMethod<MeleeWeapon>(
            nameof(MeleeWeapon.triggerClubFunction)
        ), 
        prefix: this.GetHarmonyMethod(nameof(ModifyTriggerClubFunction)) 
        );


    }




    public static bool ModifyTriggerClubFunction(Game1 __instance, Farmer who)
    {
        if (who.CurrentTool.PlayUseSounds)
        {
            who.playNearbySoundAll("clubSmash", null);
        }

        int minDamage = 0;
        int maxDamage = 0;
        string weaponName = "";
        float critChance = 0f;
        float critMultiplier = 0f;
        if(MeleeWeapon.TryGetData(who.CurrentTool.itemId.ToString(), out var weaponData))
        {
            weaponName = weaponData.Name;
            critMultiplier = weaponData.CritMultiplier + Instance.Config.ApplyAdditionalCritMultiplierDamage;
            critChance = weaponData.CritChance + Instance.Config.ApplyAdditionalCritChance;
            minDamage = (int)(weaponData.MinDamage);
            maxDamage = (int)(weaponData.MaxDamage);

        }
        /*
     
            Console.WriteLine("WeaponName:" + weaponName);
            Console.WriteLine("minDamage:" + minDamage);
            Console.WriteLine("maxDamage:" + maxDamage);
            Console.WriteLine("critChance:" + critChance);
            Console.WriteLine("critMultiplier:" + critMultiplier);
        
        */



        who.currentLocation.damageMonster(new Rectangle((int)who.Position.X - 192, who.GetBoundingBox().Y - 192, 384, 384), minDamage, maxDamage, isBomb: false, 1.5f, 100, critChance, critMultiplier, triggerMonsterInvincibleTimer: false, who);
        Game1.viewport.Y -= 21;
        Game1.viewport.X += Game1.random.Next(-32, 32);
        Vector2 v = who.getUniformPositionAwayFromBox(who.FacingDirection, 64);
        switch (who.FacingDirection)
        {
            case 0:
            case 2:
                v.X -= 32f;
                v.Y -= 32f;
                break;
            case 1:
                v.X -= 42f;
                v.Y -= 32f;
                break;
            case 3:
                v.Y -= 32f;
                break;
        }

        //need to apply a reflection patch to multiplier since it's a protected class
        //Game1.multiplayer.broadcastSprites(who.currentLocation, new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 128, 64, 64), 40f, 4, 0, v, flicker: false, who.FacingDirection == 1));
        who.jitterStrength = 2f;
        return false;

    }
           



}