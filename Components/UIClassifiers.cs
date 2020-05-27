using RecipeGraph.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components;
using Microsoft.Xna.Framework;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib.Components.Advanced;
using Terraria;

namespace RecipeGraph.Components {
    public class UIClassifiers : UIParts {
        private ItemFilters _filters;
        private UITreeList _list;
        public UIClassifiers(RecipeGraphState main) : base(main) {
            _filters = new ItemFilters();
            var panel = new UIPanel() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                SizeFactor = new Vector2(1, 1),
            };
            _list = new UITreeList() {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                SizeFactor = new Vector2(1, 1),
                Size = new Vector2(-10, -10),
            };
            var scroll = new UIScrollBarV() {
                AnchorPoint = new Vector2(1, 0.5f),
                Pivot = new Vector2(1, 0.5f),
            };
            _list.SetScrollBarV(scroll);
            panel.AppendChild(_list);
            foreach (var pair in _filters.ModFilters) {
                List<UITreeNode> itemcls = new List<UITreeNode>();
                foreach (var pair2 in _filters.Filters) {
                    var citem = new UIClassifierItem(pair2.Value.Name, pair2.Value.Texture) {
                        Name = pair.Key + " " + pair2.Key,
                        SizeFactor = new Vector2(1f, 0f),
                        Size = new Vector2(0, 36f),
                    };
                    citem.OnClick += Button_OnClick;
                    itemcls.Add(new UITreeNode(citem, new List<UITreeNode>()));
                }
                var moditem = new UIClassifierItem(pair.Value.Name, pair.Value.Texture) {
                    Name = pair.Key,
                    SizeFactor = new Vector2(1f, 0f),
                    Size = new Vector2(0, 36f),
                };
                var node = new UITreeNode(moditem, itemcls);
                moditem.OnClick += Button_OnClick;
                _list.AddElement(node);
                if (pair.Key == "All") {
                    _list.SelectedElement = node.DisplayElement;
                }
            }
            AppendChild(panel);
        }

        private void Button_OnClick(UIEditor.UILib.Events.UIMouseEvent e, UIEditor.UILib.UIElement sender) {
            var split = sender.Name.Split(' ');
            if (split.Length == 1) {
                MainState.Browser.ApplyFilter(_filters.ModFilters[sender.Name]);
            } else {
                MainState.Browser.ApplyFilter(ItemFilters.CombineFilters(_filters.ModFilters[split[0]], _filters.Filters[split[1]]));
            }

        }
    }
}
