using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UIEditor.UILib.Components;

namespace RecipeGraph.Components {
    public class UISlotImage : UIImage {
        public Rectangle? Frame { get; set; }
        public UISlotImage() : base() {
            SizeStyle = UIEditor.UILib.SizeStyle.Block;
            Frame = null;
        }

        public override void DrawSelf(SpriteBatch sb) {
            var frame = Frame.HasValue ? Frame.Value : Texture.Frame();
            float scale = 1f;
            if (frame.Width > 32 || frame.Height > 32)
                scale = (frame.Width > frame.Height ? (32f / frame.Width) : (32f / frame.Height));
            sb.Draw(Texture, 0.5f * new Vector2(Width, Height), frame, Color.White, 0, 0.5f * frame.Size(),
                     scale * 0.8f, SpriteEffects.None, 0f);
        }
    }
}
