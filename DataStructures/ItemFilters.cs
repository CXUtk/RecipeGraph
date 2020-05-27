using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RecipeGraph.DataStructures {
    public enum FilterEnum {
        All,
        Weapons,
        Armors,
        Accessories,
        Potions,
        Tiles,
        Pets,
        Ammo,
        Mounts,
        Summons,
        Dyes,
    }
    public class ItemFilters {
        public Dictionary<string, Filter> ModFilters { get; }
        public Dictionary<string, Filter> Filters { get; }

        public static Filter CombineFilters(Filter A, Filter B) {
            return new Filter(A.Name + " " + B.Name, item => A.FilterFunction(item) && B.FilterFunction(item), Main.magicPixel);
        }

        public ItemFilters() {
            Filters = new Dictionary<string, Filter>() {
                {"Weapons", new Filter("武器", item => item.damage > 0 && !item.accessory && item.ammo == 0, Main.itemTexture[ItemID.IronBroadsword]) },
                {"Armors", new Filter("装备", item => item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1, Main.itemTexture[ItemID.HallowedPlateMail]) },
                {"Accessories", new Filter("饰品", item => item.accessory, Main.itemTexture[ItemID.StarVeil]) },
                {"Ammos", new Filter("弹药", item => item.ammo != 0, Main.itemTexture[ItemID.WoodenArrow]) },
                {"Potions", new Filter("药剂", item => item.potion || (item.UseSound != null && item.UseSound.Style == 3), Main.itemTexture[ItemID.HealingPotion]) },
                {"Tiles", new Filter("物块", item => item.createTile != -1, Main.itemTexture[ItemID.DirtBlock]) },
                {"Pets", new Filter("宠物", item => Main.vanityPet[item.buffType] || Main.lightPet[item.buffType], Main.itemTexture[ItemID.AlphabetStatueA]) },
                {"Mounts", new Filter("坐骑", item => item.mountType != -1, Main.itemTexture[ItemID.SlimySaddle]) },
                {"Summons", new Filter("召唤物", item => ItemID.Sets.SortingPriorityBossSpawns[item.type] != -1, Main.itemTexture[ItemID.SuspiciousLookingEye]) },
                {"Dyes", new Filter("染料", item => item.dye > 0 || item.hairDye != -1, Main.itemTexture[ItemID.AcidDye]) },
            };
            ModFilters = new Dictionary<string, Filter>();
            ModFilters.Add("All", new Filter("全局", item => true, Main.itemTexture[ItemID.AlphabetStatueA]));
            ModFilters.Add("Vanilla",
                    new Filter("原版", item => item.modItem == null, RecipeGraph.Instance.GetTexture("Images/Icon")));
            foreach (var mod in ModLoader.Mods) {
                Texture2D tex = null;
                try {
                    tex = mod.GetTexture("icon");
                } catch {
                    tex = Main.itemTexture[ItemID.AlphabetStatueM];
                }
                ModFilters.Add(mod.Name,
                    new Filter(mod.DisplayName, item => item.modItem != null && item.modItem.mod.Name == mod.Name, tex));
            }
        }

    }
}
