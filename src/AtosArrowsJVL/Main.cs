using BepInEx;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Reflection;
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
  [BepInDependency(Jotunn.Main.ModGuid)]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  [BepInIncompatibility("com.bepinex.plugins.atosarrows")]
  public partial class Main : BaseUnityPlugin, ITraceableLogging
  {
    public static Main Instance;
    private AssetBundle _assetBundle;
    public static ConfigEntry<int> NexusId;

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion

    public Main()
    {
      try
      {
        #if DEBUG
        EnableTrace = true;
        #else
        EnableTrace = false;
        #endif
        Instance = this;
        NexusId = Config.Bind("General", "NexusID", 1301, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false, ReadOnly = true }));
        Log.RegisterSource(Instance);
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
      }
      catch (Exception e)
      {
        ZLog.LogError(e);
      }
    }

    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    public void Awake()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");

      PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
    }

    private void AddClonedItems()
    {
      try
      {
        Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name}");

        _assetBundle = AssetUtils.LoadAssetBundleFromResources("atoarrows", typeof(Main).Assembly);

        #if DEBUG
        foreach (var assetName in _assetBundle.GetAllAssetNames())
        {
          Jotunn.Logger.LogInfo(assetName);
        }
        #endif

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

        _assetBundle.Unload(false);

        // You want that to run only once, Jotunn has the item cached for the game session
        PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddBombs()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var bombFire = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/items/firebomb.prefab"), false, new ItemConfig
        {
          Amount = 5, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "FireBomb", Requirements = new[]
          {
            new RequirementConfig { Item = "Coal", Amount = 10 }, new RequirementConfig { Item = "Resin", Amount = 8 }, new RequirementConfig { Item = "LeatherScraps", Amount = 8 }, new RequirementConfig { Item = "Entrails", Amount = 2 },
          }
        });
        bombFire.ItemDrop.m_itemData.m_shared.m_name = "$item_ato_firebomb";
        ItemManager.Instance.AddItem(bombFire);

        var bombIce = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/items/icebomb.prefab"), false, new ItemConfig
        {
          Amount = 5, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "IceBomb", Requirements = new[]
          {
            new RequirementConfig { Item = "FreezeGland", Amount = 10 }, new RequirementConfig { Item = "Resin", Amount = 8 }, new RequirementConfig { Item = "LeatherScraps", Amount = 8 }, new RequirementConfig { Item = "Entrails", Amount = 2 },
          }
        });
        bombIce.ItemDrop.m_itemData.m_shared.m_name = "$item_ato_icebomb";
        ItemManager.Instance.AddItem(bombIce);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddBoneArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var arrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowbone.prefab"), false, new ItemConfig
        {
          Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "BoneArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "BoneFragments", Amount = 4 }, new RequirementConfig { Item = "Wood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bone";
        arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(arrow);

        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavybone.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "Heavy Bone Arrow", Requirements = new[]
          {
            new RequirementConfig { Item = "BoneFragments", Amount = 6 }, new RequirementConfig { Item = "RoundLog", Amount = 10 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
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
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddBluntedArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var arrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowcore.prefab"), false, new ItemConfig
        {
          Amount = 20, CraftingStation = "forge", Enabled = true, MinStationLevel = 2, Name = "CoreArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Bronze", Amount = 1 }, new RequirementConfig { Item = "Stone", Amount = 2 }, new RequirementConfig { Item = "RoundLog", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_core";
        arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(arrow);

        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavycore.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "forge", Enabled = true, MinStationLevel = 3, Name = "HeavyCoreArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Bronze", Amount = 2 }, new RequirementConfig { Item = "Stone", Amount = 4 }, new RequirementConfig { Item = "RoundLog", Amount = 12 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });

        heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_core_description";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_core";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_blunt = 64f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        ItemManager.Instance.AddItem(heavyArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddFireArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var arrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowobsidianfire.prefab"), false, new ItemConfig
        {
          Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "BigFireArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Obsidian", Amount = 4 }, new RequirementConfig { Item = "Resin", Amount = 8 }, new RequirementConfig { Item = "Wood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_obsidianfire";
        arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(arrow);

        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyfire.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyFireArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Obsidian", Amount = 6 }, new RequirementConfig { Item = "Resin", Amount = 10 }, new RequirementConfig { Item = "FineWood", Amount = 10 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });
        heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_arrow_heavyfire_description";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyfire";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_fire = 72f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        ItemManager.Instance.AddItem(heavyArrow);

        var aoeArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowfireaoe.prefab"), false, new ItemConfig
        {
          Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "FireAoeArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Crystal", Amount = 4 }, new RequirementConfig { Item = "FireBomb", Amount = 1 }, new RequirementConfig { Item = "FineWood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bigfire";
        aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(aoeArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddFlintArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyflint.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 3, Name = "HeavyFlintArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Flint", Amount = 6 }, new RequirementConfig { Item = "RoundLog", Amount = 12 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });
        heavyArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_heavy_flint_description";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_heavy_flint";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 47f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        ItemManager.Instance.AddItem(heavyArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddIceArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyfrost.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyIceArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Obsidian", Amount = 6 }, new RequirementConfig { Item = "FreezeGland", Amount = 10 }, new RequirementConfig { Item = "FineWood", Amount = 10 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });

        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavy_frost";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_frost = 72f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        ItemManager.Instance.AddItem(heavyArrow);

        var aoeArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowiceaoe.prefab"), false, new ItemConfig
        {
          Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "IceAoeArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Crystal", Amount = 4 }, new RequirementConfig { Item = "IceBomb", Amount = 1 }, new RequirementConfig { Item = "FineWood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        aoeArrow.ItemDrop.m_itemData.m_shared.m_description = "$item_atoarrow_bigice_description";
        aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_bigice";
        aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(aoeArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddNeedleArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyneedle.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyNeedleArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Needle", Amount = 10 }, new RequirementConfig { Item = "FineWood", Amount = 10 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyneedle";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 72f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        ItemManager.Instance.AddItem(heavyArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddObsidianArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavyobsidian.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyObsidianArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Obsidian", Amount = 6 }, new RequirementConfig { Item = "FineWood", Amount = 12 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavyobsidian";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 67f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(heavyArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddPoisonArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var heavyArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/heavy/arrowheavypoison.prefab"), false, new ItemConfig
        {
          Amount = 10, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 4, Name = "HeavyPoisonArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Obsidian", Amount = 6 }, new RequirementConfig { Item = "Ooze", Amount = 10 }, new RequirementConfig { Item = "FineWood", Amount = 10 }, new RequirementConfig { Item = "Feathers", Amount = 5 },
          }
        });
        heavyArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_arrow_heavy_poison";
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_pierce = 32f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_damages.m_poison = 72f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_attack.m_projectileVel = 10f;
        heavyArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(heavyArrow);

        var aoeArrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowpoisonaoe.prefab"), false, new ItemConfig
        {
          Amount = 5, CraftingStation = "forge", Enabled = true, MinStationLevel = 6, Name = "PoisonAoeArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Crystal", Amount = 4 }, new RequirementConfig { Item = "BombOoze", Amount = 1 }, new RequirementConfig { Item = "FineWood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        aoeArrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_aoepoison";
        aoeArrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(aoeArrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddStoneArrows()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var arrow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/arrows/arrowstone.prefab"), false, new ItemConfig
        {
          Amount = 20, CraftingStation = "piece_workbench", Enabled = true, MinStationLevel = 1, Name = "StoneArrow", Requirements = new[]
          {
            new RequirementConfig { Item = "Stone", Amount = 2 }, new RequirementConfig { Item = "Wood", Amount = 8 }, new RequirementConfig { Item = "Feathers", Amount = 2 },
          }
        });
        arrow.ItemDrop.m_itemData.m_shared.m_name = "$item_atoarrow_stone";
        arrow.ItemDrop.m_itemData.m_shared.m_ammoType = "$ammo_arrows";
        ItemManager.Instance.AddItem(arrow);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddXbow()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        var xbow = new CustomItem(_assetBundle.LoadAsset<GameObject>("assets/atosarrows/bows/xbow.prefab"), false, new ItemConfig
        {
          Amount = 1, CraftingStation = "forge", RepairStation = "forge", Enabled = true, MinStationLevel = 1, Name = "XBow", Requirements = new[]
          {
            new RequirementConfig { Item = "Crystal", Amount = 10, AmountPerLevel = 2 }, new RequirementConfig { Item = "BlackMetal", Amount = 60, AmountPerLevel = 10 }, new RequirementConfig { Item = "FineWood", Amount = 8 }, new RequirementConfig { Item = "LinenThread", Amount = 20, AmountPerLevel = 2 },
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
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }
  }
}
