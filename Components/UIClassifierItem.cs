using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using Microsoft.Xna.Framework;
using Terraria;
using UIEditor.UILib.Components.Advanced;
using UIEditor.UILib.Events;

namespace RecipeGraph.Components {
    public class UIClassifierItem : UITreeNodeDisplay {
        public Texture2D Texture { get { return _image.Texture; } set { _image.Texture = value; } }
        private UISlotImage _image;
        private UILabel _text;
        public UIClassifierItem(string text, Texture2D texture) {
            _image = new UISlotImage() {
                Texture = texture,
                Size = new Vector2(30, 30),
                AnchorPoint = new Vector2(0, 0.5f),
                Pivot = new Vector2(0, 0.5f),
                Position = new Vector2(0, 0),
            };
            AppendChild(_image);
            _text = new UILabel() {
                Text = text,
                Pivot = new Vector2(0, 0.5f),
                AnchorPoint = new Vector2(0, 0.5f),
                Position = new Vector2(5, 0),
            };
            AppendChild(_text);
        }
        public override void MouseDoubleClick(UIMouseEvent e) {
            IsFolded ^= true;
        }
        public override void UpdateSelf(GameTime gameTime) {
            base.UpdateSelf(gameTime);
            _image.Position = new Vector2(LeftOffset + _foldButton.Width + 5f, 0);
            _text.Position = new Vector2(_image.Position.X + _image.Width + 5f, 0);
        }
        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);
            Drawing.DrawAdvBox(sb, 0, 0, Width, Height, Color.White, UIEditor.UIEditor.Instance.SkinManager.GetTexture("Box_Default"), new Vector2(4, 4));
            if (IsSelected) {
                Drawing.DrawAdvBox(sb, 0, 0, Width, Height, new Color(0, 122, 204), Main.magicPixel,
                new Vector2(4, 4));
            }
        }
    }
}
