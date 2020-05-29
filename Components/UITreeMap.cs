using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using UIEditor.UILib.Enums;

namespace RecipeGraph.Components {
    public class UITreeMap : UIParts {
        public UITreeMap(RecipeGraphState main) : base(main) {
            _nodes = new List<UISlotNode>();
        }

        public UISlotNode Root {
            get {
                return _nodes.FirstOrDefault();
            }
        }
        private List<UISlotNode> _nodes;
        private UISlotNode _lastSelected;

        public void AddNodes(List<UISlotNode> nodes) {
            _nodes.Clear();
            foreach (var slot in nodes) {
                _nodes.Add(slot);
                slot.Slot.OnDoubleClick += Slot_OnDoubleClick;
                slot.OnClick += Slot_OnClick;
                slot.OnNextPage += Slot_OnNextPage;
                slot.OnPrevPage += Slot_OnPrevPage;
                slot.Parent = this;
                slot.Slot.PropagationRule |= PropagationFlags.MouseLeftEvents | PropagationFlags.MouseRightEvents;
                slot.Recalculate();
            }
        }

        private void Slot_OnPrevPage(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            UISlotNode slot = (UISlotNode)sender;
            var rect = slot.AdjustedRectangleScreen;
            MainState.RecipeGraph.Canvas.ChangePage(slot.Slot.ItemType, slot.Page - 1, rect.TopLeft() + rect.Size() * slot.Pivot);
        }

        private void Slot_OnNextPage(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            UISlotNode slot = (UISlotNode)sender;
            var rect = slot.AdjustedRectangleScreen;
            MainState.RecipeGraph.Canvas.ChangePage(slot.Slot.ItemType, slot.Page + 1, rect.TopLeft() + rect.Size() * slot.Pivot);
        }

        private void _removeTag(UISlotNode node) {
            while (node != null) {
                node.InPath = false;
                node.Slot.IsSelected = false;
                node = node.SlotParent;
            }
        }

        private void _addTag(UISlotNode node) {
            while (node != null) {
                node.InPath = true;
                node.Slot.IsSelected = true;
                node = node.SlotParent;
            }
        }

        private void Slot_OnClick(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            if (_lastSelected != null) {
                _removeTag(_lastSelected);
            }
            _lastSelected = (UISlotNode)sender;
            _addTag(_lastSelected);
        }

        private void Slot_OnDoubleClick(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            UIItemSlot node = (UIItemSlot)sender;
            MainState.RecipeGraph.Apply(node.ItemType);
        }

        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            if (Root == null) return;
            Children.Clear();
            foreach (var slot in _nodes) {
                slot.Recalculate();
                if (Parent.ScreenHitBox.Intersects(slot.ScreenHitBox)) {
                    AppendChild(slot);
                }
            }
            ShouldRecalculate = true;

        }

        private static Color PathColor = Color.Red;
        private const float LineWidth = 4f;

        private void _dfsDraw(SpriteBatch sb, UISlotNode node) {
            if (node.SlotChildren.Count == 0) return;
            var tex = RecipeGraph.Instance.GetTexture("Images/White");
            var first = node.SlotChildren.FirstOrDefault();
            float Ydis = (first.Position.Y - node.Position.Y) / 2;
            bool flag = node.InPath;

            if (node.SlotChildren.Count > 1) {
                sb.Draw(tex, node.Position,
                    null, Color.White, 0f, new Vector2(0.5f, 0f), new Vector2(LineWidth, Ydis), SpriteEffects.None, 0f);
                var last = node.SlotChildren.LastOrDefault();
                sb.Draw(tex, new Vector2(first.Position.X - LineWidth / 2, first.Position.Y - Ydis),
                    null, Color.White, 0f, new Vector2(0f, 0.5f), new Vector2(last.Position.X - first.Position.X + LineWidth, LineWidth), SpriteEffects.None, 0f);
                foreach (var child in node.SlotChildren) {

                    //Vector2 dif = child.Position - node.Position;
                    sb.Draw(tex, new Vector2(child.Position.X, child.Position.Y - Ydis),
                        null, Color.White, 0f, new Vector2(0.5f, 0f), new Vector2(LineWidth, Ydis), SpriteEffects.None, 0f);
                    // sb.Draw(Main.magicPixel, node.Position, new Rectangle(0, 0, 1, 1), Color.White, dif.ToRotation(), new Vector2(0, 0.5f), new Vector2(dif.Length(), 2f), SpriteEffects.None, 0f);
                    _dfsDraw(sb, child);
                }
                foreach (var child in node.SlotChildren) {
                    if (flag && child.InPath) {
                        int s = child.Position.X < node.Position.X ? -1 : 1;
                        sb.Draw(tex, node.Position,
                            null, PathColor, 0f, new Vector2(0.5f, 0f), new Vector2(LineWidth, Ydis), SpriteEffects.None, 0f);
                        sb.Draw(tex, new Vector2(node.Position.X - LineWidth / 2 * s, first.Position.Y - Ydis),
                            null, PathColor, 0f, new Vector2(0f, 0.5f), new Vector2(child.Position.X - node.Position.X + LineWidth * s, LineWidth), SpriteEffects.None, 0f);
                        sb.Draw(tex, new Vector2(child.Position.X, child.Position.Y - Ydis),
                            null, PathColor, 0f, new Vector2(0.5f, 0f), new Vector2(LineWidth, Ydis), SpriteEffects.None, 0f);
                        break;
                    }
                }
            } else {
                sb.Draw(tex, node.Position,
                    null, (flag && first.InPath) ? PathColor : Color.White, 0f, new Vector2(0.5f, 0f), new Vector2(3, first.Position.Y - node.Position.Y), SpriteEffects.None, 0f);
                _dfsDraw(sb, first);
            }

        }
        public override void DrawSelf(SpriteBatch sb) {
            if (Root != null) {
                _dfsDraw(sb, Root);
            }
            base.DrawSelf(sb);
        }
    }
}
