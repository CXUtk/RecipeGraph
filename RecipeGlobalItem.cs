using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RecipeGraph {
    public class RecipeGlobalItem : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            int ItemNameIndex = tooltips.FindIndex(t => t.Name.Equals("ItemName"));
            if (ItemNameIndex != -1) {
                string text = "";
                if (item.modItem == null) {
                    text = "[" + (GameCulture.Chinese.IsActive ? "原版" : "Vanilla") + "]";
                } else {
                    text = "[" + item.modItem.mod.DisplayName + "]";
                }
                var ttl = new TooltipLine(mod, "RecipeGraph : Mod Source", text);
                ttl.overrideColor = Color.Yellow;
                tooltips.Insert(ItemNameIndex + 1, ttl);
            }
        }
    }
}
