using Microsoft.Xna.Framework;
using RecipeGraph.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using UIEditor.UILib;
using UIEditor.UILib.Components;

namespace RecipeGraph.Components {
    public class UIItemBrowser : UIParts {
        public UITextBox SearchText { get; }
        public UIGrid ItemGrid { get; }
        private UIImage _image;
        private UIPanel _gridPanel;
        private UIPanel _textPanel;
        private UIClassifiers _classifiers;

        public UIItemBrowser(RecipeGraphState main) : base(main) {
            var panel = new UIPanel() {
                Pivot = new Vector2(0, 0),
                Position = new Vector2(5, 5),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -10),
            };
            AppendChild(panel);
            _textPanel = new UIPanel() {
                Position = new Vector2(5, 5),
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1, 0),
                Size = new Vector2(-10, 35f),
            };
            panel.AppendChild(_textPanel);
            _image = new UIImage() {
                Pivot = new Vector2(1, 0.5f),
                AnchorPoint = new Vector2(1, 0.5f),
                Texture = ModContent.GetTexture("RecipeGraph/Images/搜索"),
            };
            SearchText = new UITextBox() {
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                DrawPanel = false,
            };
            SearchText.OnTextChange += SearchText_OnTextChange;
            _gridPanel = new UIPanel() {
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(-10, 0),
            };
            ItemGrid = new UIGrid() {
                Pivot = new Vector2(0, 0),
                SizeFactor = new Vector2(1f, 1f),
                Position = new Vector2(5, 5),
                Size = new Vector2(-10, -10),
            };
            var scrollV = new UIScrollBarV() {
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            ItemGrid.SetScrollBarV(scrollV);
            _textPanel.AppendChild(_image);
            _textPanel.AppendChild(SearchText);

            _classifiers = new UIClassifiers(MainState) {
                Pivot = new Vector2(0.5f, 0),
                AnchorPoint = new Vector2(0.5f, 0f),
                SizeFactor = new Vector2(1f, 0f),
                Size = new Vector2(-10, 200),
            };
            panel.AppendChild(_classifiers);
            panel.AppendChild(_gridPanel);
            _gridPanel.AppendChild(ItemGrid);

            for (int i = 0; i < Main.itemTexture.Length; i++) {
                var p = new UIItemSlot() {
                    ItemType = i,
                    Pivot = new Vector2(0, 0),
                    Size = new Vector2(52, 52),
                    Enabled = true,
                };
                p.OnClick += P_OnClick;
                ItemGrid.AddElement(p);
            }
        }


        private Filter _currentFilter;
        public void ApplyFilter(Filter filter) {
            _currentFilter = filter;
            ApplyFilters(SearchText.Text);
        }

        private List<int> getItems() {
            List<int> result = new List<int>();
            if (_currentFilter == null) {
                for (int i = 0; i < Main.itemTexture.Length; i++) {
                    result.Add(i);
                }
                return result;
            } else {
                for (int i = 0; i < Main.itemTexture.Length; i++) {
                    Item item = new Item();
                    item.SetDefaults(i);
                    if (_currentFilter.FilterFunction(item))
                        result.Add(i);
                }
                return result;
            }
        }


        private void ApplyFilters(string text) {
            ItemGrid.Clear();
            bool[] vis = new bool[Main.itemTexture.Length];
            for (int i = 0; i < Main.itemTexture.Length; i++) vis[i] = false;
            var items = getItems();
            foreach (var i in items) {
                var S = Lang.GetItemNameValue(i).ToLower();
                if (S.Contains(text.ToLower())) {
                    var p = new UIItemSlot() {
                        ItemType = i,
                        Pivot = new Vector2(0, 0),
                        Size = new Vector2(52, 52),
                        Enabled = true,
                    };
                    p.OnClick += P_OnClick;
                    ItemGrid.AddElement(p);
                    vis[i] = true;
                }
            }
            foreach (var i in items) {
                var S = Lang.GetItemNameValue(i).ToLower();
                if (!vis[i] && ContainsSubsequence(S, text.ToLower())) {
                    var p = new UIItemSlot() {
                        ItemType = i,
                        Pivot = new Vector2(0, 0),
                        Size = new Vector2(52, 52),
                        Enabled = true,
                    };
                    p.OnClick += P_OnClick;
                    ItemGrid.AddElement(p);
                    vis[i] = true;
                }
            }
        }

        private void SearchText_OnTextChange(UIEditor.UILib.Events.UITextChangeEvent e, UIElement sender) {
            ApplyFilters(e.NewString);
        }

        private bool ContainsSubsequence(string S, string c) {
            if (S.Length < c.Length) return false;
            int j = 0;
            for (int i = 0; i < S.Length && j < c.Length; i++) {
                if (S[i] == c[j]) j++;
            }
            return j == c.Length;
        }

        private void P_OnClick(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            var item = (UIItemSlot)sender;
            MainState.RecipeGraph.Apply(item.ItemType);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            SearchText.Size = new Vector2(-10 - _image.Width - 10, 0);
            _classifiers.Position = new Vector2(0, SearchText.Position.Y + SearchText.Height + 10f);
            _gridPanel.Position = new Vector2(0, _classifiers.Position.Y + _classifiers.Height + 5f);
            _gridPanel.Size = new Vector2(-10, _gridPanel.Parent.Height - _classifiers.Height - 5f - SearchText.Height - 10f);
        }
    }
}
