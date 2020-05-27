using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace RecipeGraph {
    public class RecipeGraphPlayer : ModPlayer {

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if (RecipeGraph.QuickRecipeHotkey.JustPressed) {
                UIEditor.UIEditor.Instance.UIStateMachine.Activate("RecipeGraph");
                RecipeGraph.Instance.RecipeGraphUI.RecipeGraph.Apply(Main.HoverItem.type);
            }
        }
    }
}
