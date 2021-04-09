using BepInEx;
using BepInEx.Configuration;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using ValheimLib;
using ValheimLib.ODB;



namespace AtosArrows
{
    //BepInPlugin - GUID/Name/Version
    //BepinDependency - what mod is dependant. for importing assets from unity use valheim.lib.valheim.lib.moguid
    [BepInPlugin("AtoArrows", "Atos Arrows", "0.1")]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]

    //Makes it so its an exntension of base unity plugin. 
    public class AtoSword : BaseUnityPlugin
    {
        // private = only Harmony can access _harmoney, making it a private variable. 
        private Harmony _harmony;
        //internal pointer for my class to get and set it to private
        internal static AtoSword Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            AssetHelper.Init("atoarrows", "Atos_Arrows.json");
            this._harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }
    }
}