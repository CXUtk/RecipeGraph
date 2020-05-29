using Microsoft.Xna.Framework;
using RecipeGraph.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib.Events;

namespace RecipeGraph.Components {
    public class UICanvas : UIParts {
        private UITreeMap _treeMap;
        private bool _isLeftDragging;
        private Vector2 _startPos;
        private Vector2 _startMousePos;

        public override void MouseLeftDown(UIMouseEvent e) {
            base.MouseLeftDown(e);
            _isLeftDragging = true;
            _startPos = _treeMap.Position;
            _startMousePos = e.MouseScreen;
        }

        public override void MouseLeftUp(UIMouseEvent e) {
            base.MouseLeftUp(e);
            _isLeftDragging = false;
        }

        private float _scale;
        public override void ScrollWheel(UIScrollWheelEvent e) {
            _scale += e.ScrollValue / 120f * 0.1f;
            _scale = MathHelper.Clamp(_scale, -2f, 1f);
            float s = (float)Math.Exp(_scale);
            _treeMap.Scale = new Vector2(s, s);
            var newPivot = new Vector2((Main.MouseScreen.X - _treeMap.AdjustedRectangleScreen.X) / _treeMap.AdjustedRectangleScreen.Width, (Main.MouseScreen.Y - _treeMap.AdjustedRectangleScreen.Y) / _treeMap.AdjustedRectangleScreen.Height);
            var change = newPivot - _treeMap.Pivot;
            _treeMap.Pivot = newPivot;
            _treeMap.Position += new Vector2(_treeMap.Width, _treeMap.Height) * change * s;
            base.ScrollWheel(e);
        }
        public UICanvas(RecipeGraphState main) : base(main) {
            Overflow = UIEditor.UILib.OverflowType.Hidden;
            _treeMap = new UITreeMap(main) {
                AnchorPoint = new Vector2(0.5f, 0.5f),
                SizeFactor = new Vector2(1, 1),
            };
            AppendChild(_treeMap);
        }
        private OffSpringTree _offspringTree;


        internal void ChangePage(int type, int page, Vector2 posScreen) {
            _offspringTree.ChangePage(type, page);
            var size = _offspringTree.Calculate();
            _treeMap.SizeFactor = new Vector2(0, 0);
            Vector2 realSize = new Vector2(Math.Max(Width, size.X), Math.Max(Height, size.Y));
            _treeMap.Size = realSize;
            _treeMap.AnchorPoint = new Vector2(0.5f, 0f);
            _treeMap.Pivot = new Vector2(0.5f, 0f);

            _treeMap.Children.Clear();
            var slots = _offspringTree.GetSlots();
            foreach (var slot in slots) {
                if (slot.Slot.ItemType == type) {
                    _treeMap.Pivot = new Vector2(slot.Position.X / realSize.X, slot.Position.Y / realSize.Y);
                    break;
                }
            }
            _treeMap.AddNodes(slots);
            _treeMap.Position = _treeMap.ScreenPositionToParentAR(posScreen);

        }

        internal void ResetTree(int type, int childtype = 0) {
            _offspringTree = RecipeGraph.Instance.Graph.FindOffspring(type);
            Main.NewText(childtype);
            if (childtype != 0)
                _offspringTree.TryFindPage(type, childtype);
            var size = _offspringTree.Calculate();
            _treeMap.SizeFactor = new Vector2(0, 0);
            Vector2 realSize = new Vector2(Math.Max(Width, size.X), Math.Max(Height, size.Y));
            _treeMap.Size = realSize;
            _treeMap.AnchorPoint = new Vector2(0.5f, 0f);
            _treeMap.Pivot = new Vector2(0.5f, 0f);
            _treeMap.Position = new Vector2(0, 0);
            _treeMap.Children.Clear();
            var slots = _offspringTree.GetSlots();
            UISlotNode targetnode = null;
            if (childtype != 0) {
                foreach (var slot in slots) {
                    if (slot.Slot.ItemType == childtype) {
                        targetnode = slot;
                        break;
                    }
                }
            }
            _treeMap.AddNodes(slots);
            if (targetnode != null) {
                targetnode.MouseLeftClick(new UIMouseEvent(this, new TimeSpan(), new Vector2()));
            }
            _scale = 0;
            float s = (float)Math.Exp(_scale);
            _treeMap.Scale = new Vector2(s, s);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);

            if (_isLeftDragging) {
                Vector2 offset = Main.MouseScreen - _startMousePos;
                Vector2 pos = _startPos + offset;
                _treeMap.Position = pos;
            }
        }
    }
}
