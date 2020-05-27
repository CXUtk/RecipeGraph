using RecipeGraph.DataStructures;
using Terraria.ModLoader;
using UIEditor.UILib;
using UIEditor.UILib.Components;

namespace RecipeGraph {
    public class RecipeGraph : Mod {
        internal RecipeGraphState RecipeGraphUI;
        public RGraph Graph;
        public static RecipeGraph Instance;
        public static ModHotKey QuickRecipeHotkey;


        public override void Load() {
            base.Load();
            Instance = this;
            QuickRecipeHotkey = RegisterHotKey("快速查看材料合成", "Q");
        }
        public override void PostAddRecipes() {
            base.PostAddRecipes();
            Graph = new RGraph();
        }

        public override void PostSetupContent() {
            base.PostSetupContent();

            RecipeGraphUI = new RecipeGraphState();
            UIEditor.UIEditor.Instance.AddState(RecipeGraphUI);
            UIImageButton modButton = new UIImageButton() {
                Texture = GetTexture("Images/搜索"),
                WhiteTexture = GetTexture("Images/搜索 白边"),
                Tooltip = "切换合成图面板",
            };
            modButton.OnClick += ModButton_OnClick;
            UIEditor.UIEditor.Instance.AddToolBarButton(modButton);

        }

        private void ModButton_OnClick(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            UIEditor.UIEditor.Instance.UIStateMachine.Toggle("RecipeGraph");
        }

        public override void Unload() {
            base.Unload();
            if (UIEditor.UIEditor.Instance != null) {
                UIEditor.UIEditor.Instance.UIStateMachine.Remove(RecipeGraphUI);
            }
            Instance = null;
            Graph = null;
            QuickRecipeHotkey = null;
            RecipeGraphUI = null;


        }
    }
}
