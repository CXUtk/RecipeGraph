using Microsoft.Xna.Framework;
using RecipeGraph.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Components.Composite;

namespace RecipeGraph.Components {
    public class UIRecipeGraph : UIParts {
        public UICanvas Canvas { get; }

        private UIList _craftList;
        private UIPanel _canvasPanel;
        public UIRecipeGraph(RecipeGraphState main) : base(main) {
            _canvasPanel = new UIPanel() {
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0),
                Position = new Vector2(0, 5f),
                SizeFactor = new Vector2(1, 0.6f),
                Size = new Vector2(-10, -10),
                PropagationRule = UIEditor.UILib.Enums.PropagationFlags.FocusEvents,
            };
            Canvas = new UICanvas(MainState) {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-4, -4),

            };

            AppendChild(_canvasPanel);
            _canvasPanel.AppendChild(Canvas);

            var panel = new UIPanel() {
                Pivot = new Vector2(0.5f, 1),
                AnchorPoint = new Vector2(0.5f, 1f),
                Position = new Vector2(0, -5),
                SizeFactor = new Vector2(1, 0.4f),
                Size = new Vector2(-10, -10),
            };
            AppendChild(panel);
            _craftList = new UIList() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                SizeFactor = new Vector2(1f, 1f),
                Size = new Vector2(-10, -10),
            };
            var scrollV = new UIScrollBarV() {
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _craftList.SetScrollBarV(scrollV);
            panel.AppendChild(_craftList);

        }


        public void Apply(int type) {
            Canvas.ResetTree(type);
            ReAdd(Main.recipe.Where(r => r.createItem.type == type));
        }

        private void ReAdd(IEnumerable<Recipe> receipes) {
            _craftList.Clear();
            foreach (var recipe in receipes) {
                var uiRecipe = new UICraftItem(recipe) {
                    SizeFactor = new Vector2(1f, 0f),
                };
                _craftList.AddElement(uiRecipe);
            }
        }
    }
}
