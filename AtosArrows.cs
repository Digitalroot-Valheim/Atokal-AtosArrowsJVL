using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using JotunnLib.Managers;
using JotunnLib.Entities;
using UnityEngine;
using System;
using System.IO;
using AtosArrows.Arrows;

namespace AtosArrows
{
    [BepInPlugin("com.bepinex.plugins.atosarrows", "AtosArrows", "0.1.0")]
    [BepInDependency("com.bepinex.plugins.jotunnlib")]
    public class AtosArrows : BaseUnityPlugin
    {
        // Add Items here and below at %%%
        private static GameObject itemPrefabStoneArrow;
        private static GameObject itemPrefabCoreArrow;
        private static GameObject itemPrefabFireBomb;

        private void Awake()
        {
            ObjectManager.Instance.ObjectRegister += registerObjects;
            PrefabManager.Instance.PrefabRegister += registerPrefabs;

            //ASSET BUNDLES
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "AtosArrows/atoarrows"));

            // %%% add aditional items here
            // STONE ARROW
            itemPrefabStoneArrow = (GameObject)bundle.LoadAsset("Assets/AtosArrows/ArrowStone.prefab");
            itemPrefabCoreArrow = (GameObject)bundle.LoadAsset("Assets/AtosArrows/ArrowCore.prefab");
            itemPrefabFireBomb = (GameObject)bundle.LoadAsset("Assets/AtosArrows/FireBomb.prefab");




        }
        private void registerPrefabs(object sender, EventArgs e)
        {
            // STONE ARROW
            PrefabManager.Instance.RegisterPrefab(itemPrefabStoneArrow, "StoneArrow_bundle");
            PrefabManager.Instance.RegisterPrefab(new StoneArrowPrefab());

            PrefabManager.Instance.RegisterPrefab(itemPrefabCoreArrow, "CoreArrow_bundle");
            PrefabManager.Instance.RegisterPrefab(new CoreArrowPrefab());

            PrefabManager.Instance.RegisterPrefab(itemPrefabFireBomb, "FireBomb_bundle");
            PrefabManager.Instance.RegisterPrefab(new FireBombPrefab());

        }


        //  REGISTER ITEMS
        private void registerObjects(object sender, EventArgs e)
        {
            // STONE ARROW
            ObjectManager.Instance.RegisterItem("StoneArrow");
            ObjectManager.Instance.RegisterItem("CoreArrow");
            ObjectManager.Instance.RegisterItem("FireBomb");


            // REGISTER RECIPIES
            // Stone Arrow

            ObjectManager.Instance.RegisterRecipe(new RecipeConfig()
            {
                Name = "Recipe_StoneArrow",
                Item = "StoneArrow",
                Amount = 20,
                CraftingStation = "piece_workbench",
                MinStationLevel = 1,

                Requirements = new PieceRequirementConfig[]
                {
                    new PieceRequirementConfig()
                    {
                        Item = "Stone",
                        Amount = 2
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Wood",
                        Amount = 8
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Feathers",
                        Amount = 2
                    }
                }               
            });

            //Core Arrow
            ObjectManager.Instance.RegisterRecipe(new RecipeConfig()
            {
                Name = "Recipe_CoreArrow",
                Item = "CoreArrow",
                Amount = 20,
                CraftingStation = "forge",
                MinStationLevel = 1,

                Requirements = new PieceRequirementConfig[]
                {
                    new PieceRequirementConfig()
                    {
                        Item = "Bronze",
                        Amount = 1
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Stone",
                        Amount = 2
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "RoundLog",
                        Amount = 8
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Feathers",
                        Amount = 2
                    }
                }
            });
            //Fire Bomb
            ObjectManager.Instance.RegisterRecipe(new RecipeConfig()
            {
                Name = "Recipe_CoreArrow",
                Item = "FireBomb",
                Amount = 2,
                CraftingStation = "piece_workbench",
                MinStationLevel = 1,

                Requirements = new PieceRequirementConfig[]
                {
                    new PieceRequirementConfig()
                    {
                        Item = "Resin",
                        Amount = 10
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Coal",
                        Amount = 10
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "LeatherScraps",
                        Amount = 8
                    },
                    new PieceRequirementConfig()
                    {
                        Item = "Entrails",
                        Amount = 2
                    }
                }
            });
        }        
    }
}
