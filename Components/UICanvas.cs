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

        internal void ResetTree(OffSpringTree tree) {
            var size = tree.CalculateSize();
            _treeMap.SizeFactor = new Vector2(0, 0);
            Vector2 realSize = new Vector2(Math.Max(Width, size.X), Math.Max(Height, size.Y));
            _treeMap.Size = realSize;
            _treeMap.AnchorPoint = new Vector2(0.5f, 0f);
            _treeMap.Pivot = new Vector2(0.5f, 0f);
            _treeMap.Position = new Vector2(0, 0);
            _treeMap.Children.Clear();
            _treeMap.AddNodes(tree.GetSlots());
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
