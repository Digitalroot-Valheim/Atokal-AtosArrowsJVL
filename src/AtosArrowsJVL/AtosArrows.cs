using BepInEx;
using BepInEx.Configuration;
using JetBrains.Annotations;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
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
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(Jotunn.Main.ModGuid, "2.9.0")]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  [BepInIncompatibility("com.bepinex.plugins.atosarrows")]
  public class AtosArrows : BaseUnityPlugin
  {
    public const string Version = "1.0.0";
    private const string Name = "AtosArrowsJVL";
    public const string Guid = "digitalroot.valheim.mods.atosarrows.jvl";
    public const string Namespace = "AtosArrowsJVL";
    public static AtosArrows Instance;
    private AssetBundle _assetBundle;

    public static ConfigEntry<int> NexusId;

    public AtosArrows()
    {
      Instance = this;
      NexusId = Config.Bind("General", "NexusID", 1301, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Browsable = false, ReadOnly = true }));
    }

    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    public void Awake()
    {
      Jotunn.Logger.LogInfo("AtosArrows.Awake()");
      _assetBundle = AssetUtils.LoadAssetBundleFromResources("atoarrows", typeof(AtosArrows).Assembly);

#if DEBUG
      foreach (var assetName in _assetBundle.GetAllAssetNames())
      {
        Jotunn.Logger.LogInfo(assetName);
      }
#endif

      _itemPrefabStoneArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowstone.prefab");
      _itemPrefabCoreArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowcore.prefab");
      _itemPrefabBoneArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowbone.prefab");
      _itemPrefabHeavyCoreArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavycore.prefab");
      _itemPrefabHeavyFlintArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyflint.prefab");
      _itemPrefabHeavyBoneArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavybone.prefab");
      _itemPrefabHeavyObsidianArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyobsidian.prefab");
      _itemPrefabHeavyNeedleArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyneedle.prefab");
      _itemPrefabBigFireArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowobsidianfire.prefab");
      _itemPrefabHeavyFireArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyfire.prefab");
      _itemPrefabHeavyIceArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyfrost.prefab");
      _itemPrefabHeavyPoisonArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavypoison.prefab");
      _itemPrefabFireBomb = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/items/firebomb.prefab");
      _itemPrefabIceBomb = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/items/icebomb.prefab");
      _itemPrefabFireAoeArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowfireaoe.prefab");
      _itemPrefabIceAoeArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowiceaoe.prefab");
      _itemPrefabPoisonAoeArrow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowpoisonaoe.prefab");
      _itemPrefabXBow = _assetBundle.LoadAsset<GameObject>("assets/atosarrows/bows/xbow.prefab");
      AddRegisterObjects();
    }

    private static void AddRegisterObjects()
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
        Amount = 5, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "FireBomb", Requirements = new[]
        {
          new RequirementConfig {Item = "Coal", Amount = 10}, new RequirementConfig {Item = "Resin", Amount = 8}, new RequirementConfig {Item = "LeatherScraps", Amount = 8}, new RequirementConfig {Item = "Entrails", Amount = 2},
        }
      });
      bombFire.ItemDrop.m_itemData.m_shared.m_name = "$item_ato_firebomb";
      ItemManager.Instance.AddItem(bombFire);

      var bombIce = new CustomItem(_itemPrefabIceBomb, false, new ItemConfig
      {
        Amount = 5, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "IceBomb", Requirements = new[]
        {
          new RequirementConfig {Item = "FreezeGland", Amount = 10}, new RequirementConfig {Item = "Resin", Amount = 8}, new RequirementConfig {Item = "LeatherScraps", Amount = 8}, new RequirementConfig {Item = "Entrails", Amount = 2},
        }
      });
      bombIce.ItemDrop.m_itemData.m_shared.m_name = "$item_ato_icebomb";
      ItemManager.Instance.AddItem(bombIce);
    }

    private static void AddBoneArrows()
    {
      var arrow = new CustomItem(_itemPrefabBoneArrow, false, new ItemConfig
      {
        Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "BoneArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "BoneFragments", Amount = 4}, new RequirementConfig {Item = "Wood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bone";
      arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyBoneArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "Heavy Bone Arrow", Requirements = new[]
        {
          new RequirementConfig {Item = "BoneFragments", Amount = 6}, new RequirementConfig {Item = "RoundLog", Amount = 10}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_bone_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_bone";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_slash = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddBluntedArrows()
    {
      var arrow = new CustomItem(_itemPrefabCoreArrow, false, new ItemConfig
      {
        Amount = 20, CraftingStation = "forge", Enabled = true, MinStationLevel = 2, Name = "CoreArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Bronze", Amount = 1}, new RequirementConfig {Item = "Stone", Amount = 2}, new RequirementConfig {Item = "RoundLog", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_core";
      arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyCoreArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "forge", Enabled = true, MinStationLevel = 3, Name = "HeavyCoreArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Bronze", Amount = 2}, new RequirementConfig {Item = "Stone", Amount = 4}, new RequirementConfig {Item = "RoundLog", Amount = 12}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_core_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_core";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_blunt = 64f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddFireArrows()
    {
      var arrow = new CustomItem(_itemPrefabBigFireArrow, false, new ItemConfig
      {
        Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "BigFireArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 4}, new RequirementConfig {Item = "Resin", Amount = 8}, new RequirementConfig {Item = "Wood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_obsidianfire";
      arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(arrow);

      var heavyArrow = new CustomItem(_itemPrefabHeavyFireArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyFireArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6}, new RequirementConfig {Item = "Resin", Amount = 10}, new RequirementConfig {Item = "FineWood", Amount = 10}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_arrow_heavyfire_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyfire";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_fire = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabFireAoeArrow, false, new ItemConfig
      {
        Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "FireAoeArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4}, new RequirementConfig {Item = "FireBomb", Amount = 1}, new RequirementConfig {Item = "FineWood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bigfire";
      aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddFlintArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyFlintArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "HeavyFlintArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Flint", Amount = 6}, new RequirementConfig {Item = "RoundLog", Amount = 12}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_flint_description";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_flint";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 47f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddIceArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyIceArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyIceArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6}, new RequirementConfig {Item = "FreezeGland", Amount = 10}, new RequirementConfig {Item = "FineWood", Amount = 10}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });

      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavy_frost";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_frost = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabIceAoeArrow, false, new ItemConfig
      {
        Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "IceAoeArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4}, new RequirementConfig {Item = "IceBomb", Amount = 1}, new RequirementConfig {Item = "FineWood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      aoeArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_bigice_description";
      aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bigice";
      aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddNeedleArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyNeedleArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyNeedleArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Needle", Amount = 10}, new RequirementConfig {Item = "FineWood", Amount = 10}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyneedle";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddObsidianArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyObsidianArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyObsidianArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6}, new RequirementConfig {Item = "FineWood", Amount = 12}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyobsidian";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 67f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(heavyArrow);
    }

    private static void AddPoisonArrows()
    {
      var heavyArrow = new CustomItem(_itemPrefabHeavyPoisonArrow, false, new ItemConfig
      {
        Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyPoisonArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Obsidian", Amount = 6}, new RequirementConfig {Item = "Ooze", Amount = 10}, new RequirementConfig {Item = "FineWood", Amount = 10}, new RequirementConfig {Item = "Feathers", Amount = 5},
        }
      });
      heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavy_poison";
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_poison = 72f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
      heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(heavyArrow);

      var aoeArrow = new CustomItem(_itemPrefabPoisonAoeArrow, false, new ItemConfig
      {
        Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "PoisonAoeArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 4}, new RequirementConfig {Item = "BombOoze", Amount = 1}, new RequirementConfig {Item = "FineWood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_aoepoison";
      aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(aoeArrow);
    }

    private static void AddStoneArrows()
    {
      var arrow = new CustomItem(_itemPrefabStoneArrow, false, new ItemConfig
      {
        Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 1, Name = "StoneArrow", Requirements = new[]
        {
          new RequirementConfig {Item = "Stone", Amount = 2}, new RequirementConfig {Item = "Wood", Amount = 8}, new RequirementConfig {Item = "Feathers", Amount = 2},
        }
      });
      arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_stone";
      arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
      ItemManager.Instance.AddItem(arrow);
    }

    private static void AddXbow()
    {
      var xbow = new CustomItem(_itemPrefabXBow, false, new ItemConfig
      {
        Amount = 1, CraftingStation = "forge", RepairStation = "forge", Enabled = true, MinStationLevel = 1, Name = "XBow", Requirements = new[]
        {
          new RequirementConfig {Item = "Crystal", Amount = 10, AmountPerLevel = 2}, new RequirementConfig {Item = "BlackMetal", Amount = 60, AmountPerLevel = 10}, new RequirementConfig {Item = "FineWood", Amount = 8}, new RequirementConfig {Item = "LinenThread", Amount = 20, AmountPerLevel = 2},
        }
      });
      xbow.ItemDrop.m_itemData.m_shared.m_name = "$item_xbow";
      xbow.ItemDrop.m_itemData.m_shared.m_attackForce = 0f;
      xbow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 65f;
      xbow.ItemDrop.m_itemData.m_shared.m_maxDurability = 600f;
      xbow.ItemDrop.m_itemData.m_shared.m_blockPower = 10f;
      xbow.ItemDrop.m_itemData.m_shared.m_maxStackSize = 1;
      xbow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
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
