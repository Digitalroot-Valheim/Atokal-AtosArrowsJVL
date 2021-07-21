using BepInEx;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.IO;
using UnityEngine;

namespace AtosArrowsJVL
{
  /// <summary>
  /// This is a port of AtosArrows to JVL
  /// PR: https://github.com/Atokal/AtosArrows/pull/3
  /// Assets belong to Atokal and are used with permission because of:
  ///  - Asset use permission You are allowed to use the assets in this file without permission or crediting me. https://www.nexusmods.com/valheim/mods/969 (June 12, 2021)
  /// Original Mod: https://www.nexusmods.com/valheim/mods/969
  /// Code is a complete rewrite.
  /// </summary>
  [BepInPlugin("digitalroot.valheim.mods.atosarrows.jvl", "AtosArrowsJVL", "0.7.2")]
  [BepInDependency(Main.ModGuid)]
  [BepInIncompatibility("com.bepinex.plugins.atosarrows")]
  public class AtosArrows : BaseUnityPlugin
  {
    [UsedImplicitly]
    private void Awake()
    {
      Jotunn.Logger.LogInfo("AtosArrows.Awake()");
      Config.Bind("General", "NexusID", 1301, "Nexus mod ID for updates");
      var assetFile = new FileInfo(Path.Combine(BepInEx.Paths.PluginPath, "AtosArrowsJVL", "atoarrows"));

      if (!assetFile.Exists)
      {
        Jotunn.Logger.LogError($"Unable to find asset file 'atoarrows', please make sure 'atoarrows' and 'AtosArrowsJVL.dll' are in {assetFile.DirectoryName}");
        Jotunn.Logger.LogError($"AtosArrowsJVL is not loaded.");
        return;
      }

      AssetBundle assetBundle = AssetUtils.LoadAssetBundle(assetFile.FullName);

#if DEBUG
      foreach (string assetName in assetBundle.GetAllAssetNames())
      {
				Jotunn.Logger.LogInfo(assetName);
			}
#endif     
			_itemPrefabStoneArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowStone.prefab");
      _itemPrefabCoreArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowCore.prefab");
      _itemPrefabBoneArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowBone.prefab");
      _itemPrefabHeavyCoreArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyCore.prefab");
      _itemPrefabHeavyFlintArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyFlint.prefab");
      _itemPrefabHeavyBoneArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyBone.prefab");
      _itemPrefabHeavyObsidianArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyObsidian.prefab");
      _itemPrefabHeavyNeedleArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyNeedle.prefab");
      _itemPrefabBigFireArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowObsidianFire.prefab");
      _itemPrefabHeavyFireArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyFire.prefab");
      _itemPrefabHeavyIceArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyFrost.prefab");
      _itemPrefabHeavyPoisonArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/Heavy/ArrowHeavyPoison.prefab");
      _itemPrefabFireBomb = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Items/FireBomb.prefab");
      _itemPrefabIceBomb = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Items/IceBomb.prefab");
      _itemPrefabFireAoeArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowFireaoe.prefab");
      _itemPrefabIceAoeArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowIceaoe.prefab");
      _itemPrefabPoisonAoeArrow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Arrows/ArrowPoisonaoe.prefab");
      _itemPrefabXBow = assetBundle.LoadAsset<GameObject>("Assets/AtosArrows/Bows/Xbow.prefab");
      RegisterObjects();
      AddLocalizations();
    }

    private void AddLocalizations()
    {
      LocalizationManager.Instance.AddLocalization(new LocalizationConfig("English")
      {
        Translations = {
          {"item_atoarrow_stone", "Stone Arrow"}, {"item_atoarrow_stone_description", "A crude arrow with a blunted stone tip. Good for bonking something from a distance."},
          {"item_atoarrow_core", "Blunted Arrow"}, {"item_atoarrow_core_description", "An Arrow fixed with a blunt Bronze tip. Great for bashing in a skull at a distance."},
          {"item_atoarrow_heavy_core", "Heavy Blunted Arrow"}, {"item_atoarrow_heavy_core_description", "A sturdy and well made arrow. Much harder to make but does much more damage."},
          {"item_atoarrow_bone", "Bone Arrow"}, {"item_atoarrow_bone_description", "An arrow made of several bone fragments. This unique arrow slashes and tears the flesh of your enemies."},
          {"item_atoarrow_heavy_bone", "Heavy Bone Arrow"}, {"item_atoarrow_heavy_bone_description", "An arrow made of several bone fragments. This unique arrow slashes and tears the flesh of your enemies."},
          {"item_atoarrow_heavy_flint", "Heavy Flint Arrow"}, {"item_atoarrow_heavy_flint_description", "A sturdy and well made arrow. Much harder to make but does much more damage."},
          {"item_arrow_heavyobsidian", "Heavy Obsidian Arrow"}, {"item_arrow_obsidian_heavy_description", "An heavy arrow made with a hard Obsidian tip."},
          {"item_arrow_heavyneedle", "Heavy Needle Arrow"}, {"item_arrow_heavyneedle_description", "An heavier and harder hitting Needle Arrow."},
          {"item_arrow_obsidianfire", "Obsidian Fire Arrow"}, {"item_arrow_obsidianfire_description", "An powerful arrow that will ignite your foes."},
          {"item_arrow_heavyfire", "Heavy Fire Arrow"}, {"item_arrow_heavyfire_description", "A sturdy and well made arrow. Much harder to make but does much more damage."},
          {"item_atoarrow_bigfire", "Exploding Fire Arrow"}, {"item_atoarrow_bigfire_description", "An arrow loaded to the brim with explosvies. This rare arrow is great for setting everything on fire!"},
          {"item_arrow_heavy_frost", "Heavy Frost Arrow"}, {"item_arrow_frost_heav_description", "A sturdy and well made arrow. Much harder to make but does much more damage."},
          {"item_atoarrow_bigice", "Exploding Ice Arrow"}, {"item_atoarrow_bigice_description", "An arrow loaded to the brim with all sorts of Magical cold objects found from the mountains. This rare arrow is great for freezing multiple foes!"},
          {"item_arrow_heavy_poison", "Heavy Poison Arrow"}, {"item_arrow_poison_heavy_description", "A sturdy and well made arrow. Much harder to make but does much more damage."},
          {"item_atoarrow_aoepoison", "Exploding Poison Arrow"}, {"item_atoarrow_aoepoison_description", "An arrow loaded to the brim with Poison and Oozes. This rare arrow is great for poisoning a large group of enemies."},
          {"item_ato_firebomb", "Fire Bomb"}, {"item_ato_firebomb_description", "RND request from the Dwarf Hugo."},
          {"item_ato_icebomb", "Ice Bomb"}, {"item_ato_icebomb_description", "RND request from the Dwarf Hugo."},
          {"item_xbow", "Cross Bow"}, {"item_xbow_description", "Ugly XBow"},
        }
      });
    }

    private static void RegisterObjects()
		{
      Jotunn.Logger.LogInfo("AtosArrows.RegisterObjects()");

      AddStoneArrows();
      AddBluntedArrows();
      AddBoneArrows();
      AddFlintArrows();
      AddObsidianArrows();
      AddNeedleArrows();
      AddFireArrows();
      AddIceArrows();
      AddPoisonArrows();
      AddBombs();
      AddXbow();
    }

    private static void AddBombs()
    {
      var bombFire = new CustomItem(_itemPrefabFireBomb, false, new ItemConfig
      {
        Amount = 5,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 3,
        Name = "FireBomb",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Coal", Amount = 10},
          new RequirementConfig {Item = "Resin", Amount = 8},
          new RequirementConfig {Item = "LeatherScraps", Amount = 8},
          new RequirementConfig {Item = "Entrails", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(bombFire);

      var bombIce = new CustomItem(_itemPrefabIceBomb, false, new ItemConfig
      {
        Amount = 5,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 3,
        Name = "IceBomb",
        Requirements = new[]
        {
          new RequirementConfig {Item = "FreezeGland", Amount = 10},
          new RequirementConfig {Item = "Resin", Amount = 8},
          new RequirementConfig {Item = "LeatherScraps", Amount = 8},
          new RequirementConfig {Item = "Entrails", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(bombIce);
    }

    private static void AddBoneArrows()
    {
      var arrow = new CustomItem(_itemPrefabBoneArrow, false, new ItemConfig
      {
        Amount = 20,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 3,
        Name = "BoneArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "BoneFragments", Amount = 4},
          new RequirementConfig {Item = "Wood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyBoneArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 3,
        Name = "Heavy Bone Arrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "BoneFragments", Amount = 6},
          new RequirementConfig {Item = "RoundLog", Amount = 10},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_bone_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_bone";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_slash = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddBluntedArrows()
    {
      var arrow = new CustomItem(_itemPrefabCoreArrow, false, new ItemConfig
      {
        Amount = 20,
        CraftingStation = "forge",
        Enabled = true,
        MinStationLevel = 2,
        Name = "CoreArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Bronze", Amount = 1},
          new RequirementConfig {Item = "Stone", Amount = 2},
          new RequirementConfig {Item = "RoundLog", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyCoreArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "forge",
        Enabled = true,
        MinStationLevel = 3,
        Name = "HeavyCoreArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Bronze", Amount = 2},
          new RequirementConfig {Item = "Stone", Amount = 4},
          new RequirementConfig {Item = "RoundLog", Amount = 12},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_core_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_core";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_blunt = 64f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddFireArrows()
    {
      var arrow = new CustomItem(_itemPrefabBigFireArrow, false, new ItemConfig
      {
        Amount = 20,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "BigFireArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 4},
          new RequirementConfig {Item = "Resin", Amount = 8},
          new RequirementConfig {Item = "Wood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyFireArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "HeavyFireArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6},
          new RequirementConfig {Item = "Resin", Amount = 10},
          new RequirementConfig {Item = "FineWood", Amount = 10},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_arrow_heavyfire_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyfire";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_fire = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabFireAoeArrow, false, new ItemConfig
      {
        Amount = 5,
        CraftingStation = "forge",
        Enabled = true,
        MinStationLevel = 6,
        Name = "FireAoeArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4},
          new RequirementConfig {Item = "FireBomb", Amount = 1},
          new RequirementConfig {Item = "FineWood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddFlintArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyFlintArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 3,
        Name = "HeavyFlintArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Flint", Amount = 6},
          new RequirementConfig {Item = "RoundLog", Amount = 12},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_flint_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_flint";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 47f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddIceArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyIceArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "HeavyIceArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6},
          new RequirementConfig {Item = "FreezeGland", Amount = 10},
          new RequirementConfig {Item = "FineWood", Amount = 10},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });

      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_frost = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabIceAoeArrow, false, new ItemConfig
      {
        Amount = 5,
        CraftingStation = "forge",
        Enabled = true,
        MinStationLevel = 6,
        Name = "IceAoeArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4},
          new RequirementConfig {Item = "IceBomb", Amount = 1},
          new RequirementConfig {Item = "FineWood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      aoeArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_bigice_description";
      aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bigice";
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddNeedleArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyNeedleArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "HeavyNeedleArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Needle", Amount = 10},
          new RequirementConfig {Item = "FineWood", Amount = 10},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddObsidianArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyObsidianArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "HeavyObsidianArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6},
          new RequirementConfig {Item = "FineWood", Amount = 12},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 67f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddPoisonArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyPoisonArrow, false, new ItemConfig
      {
        Amount = 10,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 4,
        Name = "HeavyPoisonArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6},
          new RequirementConfig {Item = "Ooze", Amount = 10},
          new RequirementConfig {Item = "FineWood", Amount = 10},
          new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_poison = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabPoisonAoeArrow, false, new ItemConfig
      {
        Amount = 5,
        CraftingStation = "forge",
        Enabled = true,
        MinStationLevel = 6,
        Name = "PoisonAoeArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4},
          new RequirementConfig {Item = "BombOoze", Amount = 1},
          new RequirementConfig {Item = "FineWood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddStoneArrows()
    {
      var arrow = new CustomItem(_itemPrefabStoneArrow, false, new ItemConfig
      {
        Amount = 20,
        CraftingStation = "piece_workbench",
        Enabled = true,
        MinStationLevel = 1,
        Name = "StoneArrow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Stone", Amount = 2},
          new RequirementConfig {Item = "Wood", Amount = 8},
          new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      ItemManager.Instance.AddItem(arrow);
    }

    private static void AddXbow()
    {
      var xbow = new CustomItem(_itemPrefabXBow, false, new ItemConfig
      {
        Amount = 1,
        CraftingStation = "forge",
        RepairStation = "forge",
        Enabled = true,
        MinStationLevel = 1,
        Name = "XBow",
        Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 10, AmountPerLevel = 2},
          new RequirementConfig {Item = "BlackMetal", Amount = 60, AmountPerLevel = 10},
          new RequirementConfig {Item = "FineWood", Amount = 8},
          new RequirementConfig {Item = "LinenThread", Amount = 20, AmountPerLevel = 2},
        }
      });
      xbow.ItemDrop.m_itemData.m_shared.m_attackForce = 0f;
      xbow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 65f;
      xbow.ItemDrop.m_itemData.m_shared.m_maxDurability = 600f;
      xbow.ItemDrop.m_itemData.m_shared.m_blockPower = 10f;
      xbow.ItemDrop.m_itemData.m_shared.m_maxStackSize = 1;
      ItemManager.Instance.AddItem(xbow);
    }

    private static GameObject _itemPrefabStoneArrow;
    private static GameObject _itemPrefabCoreArrow;
    private static GameObject _itemPrefabBoneArrow;
    private static GameObject _itemPrefabHeavyCoreArrow;
    private static GameObject _itemPrefabHeavyFlintArrow;
    private static GameObject _itemPrefabHeavyBoneArrow;
    private static GameObject _itemPrefabHeavyObsidianArrow;
    private static GameObject _itemPrefabHeavyNeedleArrow;
    private static GameObject _itemPrefabBigFireArrow;
    private static GameObject _itemPrefabHeavyFireArrow;
    private static GameObject _itemPrefabHeavyIceArrow;
    private static GameObject _itemPrefabHeavyPoisonArrow;
    private static GameObject _itemPrefabFireBomb;
    private static GameObject _itemPrefabIceBomb;
    private static GameObject _itemPrefabFireAoeArrow;
    private static GameObject _itemPrefabIceAoeArrow;
    private static GameObject _itemPrefabPoisonAoeArrow;
    private static GameObject _itemPrefabXBow;
	}
}
