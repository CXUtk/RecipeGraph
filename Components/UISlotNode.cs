using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RecipeGraph.Components {

    public class UISlotNode : UIElement {
        public UIItemSlot Slot { get; }
        public List<UISlotNode> SlotChildren { get; }
        public UISlotNode SlotParent { get; }
        public int Type { get { return Slot.ItemType; } }
        public bool InPath { get; set; }
        public UISlotNode(int type, UISlotNode parent) : base() {
            SlotParent = parent;
            InPath = false;
            SlotChildren = new List<UISlotNode>();
            Slot = new UIItemSlot() {
                ItemType = type,
                ItemStack = 1,
                AnchorPoint = new Vector2(0.5f, 0.5f),
            };
            AppendChild(Slot);
        }

        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);

        }
    }
}
