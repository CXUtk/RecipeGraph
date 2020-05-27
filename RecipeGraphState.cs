using Microsoft.Xna.Framework;
using RecipeGraph.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components.Composite;

namespace RecipeGraph {
    public class RecipeGraphState : UIState {
        public UIWindow Window { get; }
        public UIItemBrowser Browser { get; }
        public UIRecipeGraph RecipeGraph { get; }
        public RecipeGraphState() : base("RecipeGraph") {
            Window = new UIWindow() {
                Name = "RecipeWindow",
                Size = new Vector2(800, 600),
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Position = new Vector2(100, 100)
            };
            Window.OnClose += Window_OnClose;
            AppendChild(Window);
            var body = new UIElement() {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0, 0),
                Position = new Vector2(10, 35),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-20, -40),
            };
            Window.AppendChild(body);
            Browser = new UIItemBrowser(this) {
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                SizeFactor = new Vector2(0.5f, 1f),
            };
            RecipeGraph = new UIRecipeGraph(this) {
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
                SizeFactor = new Vector2(0.5f, 1f),
            };
            body.AppendChild(Browser);
            body.AppendChild(RecipeGraph);
        }

        private void Window_OnClose(UIEditor.UILib.Events.UIActionEvent e, UIElement sender) {
            IsActive = false;
        }
    }
}
