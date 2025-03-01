using HarmonyLib;
using Spacechase.Shared.Patching;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buffs;

namespace HammerHeavyAttackAlwaysCrits
{
    public class Config
    {
        public string Note { get; set; } = "additional crit multiplier set below will only apply to the special attack of hammer/clubs";
        public float ApplyAdditionalCritMultiplierDamage { get; set; } = 0.0f;

    }

    internal sealed class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }
        public Config Config { get; private set; }

        
        public override void Entry(IModHelper helper)
        {
            Instance = this; 
            Config = helper.ReadConfig<Config>() ?? new Config(); 

            //S
            HarmonyPatcher.Apply(this,
            new PatchName(Config, Instance)
            );

        }



    }






}