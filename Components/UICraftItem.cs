using RecipeGraph.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using UIEditor.UILib.Components;
using Terraria.Map;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;

namespace RecipeGraph.Components {
    public class UICraftItem : UIPanel {
        public Recipe Recipe { get; }
        private List<UIItemSlot> _reqSlot;
        private UIItemSlot _targetSlot;
        private UILabel _label;
        private const float MARGIN = 5f;
        private const float SLOT_SIZE = 45f;
        private List<int> _lineSnippetNumber = new List<int>();
        private List<TextSnippet> _text2 = new List<TextSnippet>();
        public UICraftItem(Recipe recipe) : base() {
            Recipe = recipe;
            _reqSlot = new List<UIItemSlot>();
            foreach (var item in recipe.requiredItem) {
                if (item.IsAir) continue;
                var slot = new UIItemSlot() {
                    ItemType = item.type,
                    ItemStack = item.stack,
                    Pivot = new Vector2(0, 0f),
                    Size = new Vector2(SLOT_SIZE, SLOT_SIZE),
                    ItemScale = 0.9f,
                };
                slot.OnDoubleClick += _targetSlot_OnDoubleClick;
                _reqSlot.Add(slot);
            }
            _targetSlot = new UIItemSlot() {
                ItemType = recipe.createItem.type,
                ItemStack = recipe.createItem.stack,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                Size = new Vector2(52, 52),
                Position = new Vector2(5f, 0f),
            };
            _label = new UILabel() {
                Pivot = new Vector2(0, 0f),
                Text = "",
            };
            _targetSlot.OnDoubleClick += _targetSlot_OnDoubleClick;
            AppendChild(_targetSlot);
            foreach (var slot in _reqSlot) {
                AppendChild(slot);
            }
            AppendChild(_label);
            foreach (var tile in Recipe.requiredTile) {
                if (tile < 0) continue;
                _text2.Add(new TextSnippet(Lang.GetMapObjectName(MapHelper.TileToLookup(tile, 0)), Color.White));
            }
            if (Recipe.needWater) {
                _text2.Add(new TextSnippet(Lang.inter[53].Value, Color.Blue));
            }
            if (Recipe.needHoney) {
                _text2.Add(new TextSnippet(Lang.inter[58].Value, Color.Yellow));
            }
            if (Recipe.needLava) {
                _text2.Add(new TextSnippet(Lang.inter[56].Value, Color.OrangeRed));
            }
            if (Recipe.needSnowBiome) {
                _text2.Add(new TextSnippet(Lang.inter[123].Value, Color.Cyan));
            }
        }

        private void _targetSlot_OnDoubleClick(UIEditor.UILib.Events.UIMouseEvent e, UIEditor.UILib.UIElement sender) {
            var slot = (UIItemSlot)sender;
            RecipeGraph.Instance.RecipeGraphUI.RecipeGraph.Apply(slot.ItemType);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            int start = 0;
            for (int i = 0; i < _lineSnippetNumber.Count; i++) {
                StringBuilder sbb = new StringBuilder();
                for (int j = start; j < _text2.Count && j < start + _lineSnippetNumber[i]; j++) {

                    TextSnippet[] snippet = new TextSnippet[2];
                    snippet[0] = _text2[j];
                    snippet[1] = new TextSnippet(((j != _text2.Count - 1) ? ", " : ""));
                    int h;
                    ChatManager.DrawColorCodedStringWithShadow(sb, Main.fontMouseText, snippet, _label.Position + new Vector2(_label.MeasureSize(sbb.ToString()).X, i * 26f), 0f, Vector2.Zero, Scale, out h);
                    sbb.Append(_text2[j].TextOriginal + ((j != _text2.Count - 1) ? ", " : ""));
                }
                start += _lineSnippetNumber[i];
            }

        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            float startX = _targetSlot.Position.X + _targetSlot.Width + 10f;
            float startY = 8f;
            int i = 0;
            while (i < _reqSlot.Count) {
                int j = i;
                for (; j < _reqSlot.Count && startX + _reqSlot[j].Width <= Width - 5; j++) {
                    _reqSlot[j].Position = new Vector2(startX, startY);
                    startX += _reqSlot[j].Width + MARGIN;
                }
                i = j;
                startX = _targetSlot.Position.X + _targetSlot.Width + 10f;
                startY += SLOT_SIZE + MARGIN;
            }
            startY += 3;
            _lineSnippetNumber.Clear();
            _label.Position = new Vector2(startX, startY);
            StringBuilder sb = new StringBuilder();
            int last = 0;
            for (int j = 0; j < _text2.Count; j++) {
                string str = _text2[j].TextOriginal + ((j != _text2.Count - 1) ? ", " : "");
                sb.Append(str);
                if (startX + _label.MeasureSize(sb.ToString()).X > Width - 5) {
                    _lineSnippetNumber.Add(j - last);
                    last = j;
                    startY += 26f;
                    sb.Clear();
                    sb.Append(str);
                }
            }
            _lineSnippetNumber.Add(_text2.Count - last);
            startY += 26f;
            Size = new Vector2(Size.X, Math.Max(60, startY));
        }
    }
}
