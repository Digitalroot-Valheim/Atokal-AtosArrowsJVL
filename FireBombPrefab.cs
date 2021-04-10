using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JotunnLib.Entities;

namespace AtosArrows.Arrows
{
    public class FireBombPrefab : PrefabConfig
    {
        public FireBombPrefab() : base("FireBomb", "FireBomb_bundle")
        {

        }

        public override void Register()
        {
            ItemDrop item = Prefab.GetComponent<ItemDrop>();
            // info
            item.m_itemData.m_shared.m_name = "Fire Bomb";
            item.m_itemData.m_shared.m_description = "RND request from the Dwarf Hugo.";
            // general
            item.m_itemData.m_dropPrefab = Prefab;
            item.m_itemData.m_shared.m_maxStackSize = 100;
            item.m_itemData.m_shared.m_maxQuality = 1;
            item.m_itemData.m_shared.m_weight = 0.1f;
            item.m_itemData.m_shared.m_maxDurability = 10;
            item.m_itemData.m_shared.m_equipDuration = 0.2f;
            item.m_itemData.m_shared.m_variants = 1;
            // combat
            item.m_itemData.m_shared.m_blockPower = 10;
            item.m_itemData.m_shared.m_timedBlockBonus = 6;
            item.m_itemData.m_shared.m_deflectionForce = 8;
            item.m_itemData.m_shared.m_attackForce = 0;
            // damage
            // attack
            // attack secondary
        }
    }
}
