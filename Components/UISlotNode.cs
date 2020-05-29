using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;
using UIEditor.UILib.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UIEditor.UILib.Events;

namespace RecipeGraph.Components {

    public class UISlotNode : UIElement {
        public event MouseEvent OnNextPage;
        public event MouseEvent OnPrevPage;
        public UIItemSlot Slot { get; }
        public List<UISlotNode> SlotChildren { get; }
        public UISlotNode SlotParent { get; }
        public int Type { get { return Slot.ItemType; } }
        public bool InPath { get; set; }
        public int Page { get; }
        public int MaxPage { get; }
        private UILabel _label;
        public UISlotNode(int type, UISlotNode parent, int page, int maxPage) : base() {
            SlotParent = parent;
            InPath = false;
            MaxPage = maxPage;
            Page = page;
            SlotChildren = new List<UISlotNode>();
            Slot = new UIItemSlot() {
                ItemType = type,
                ItemStack = 1,
                AnchorPoint = new Vector2(0.5f, 0.5f),
            };

            _label = new UILabel() {
                Text = $"{Page}/{MaxPage}",
                Pivot = new Vector2(0.5f, 1f),
                AnchorPoint = new Vector2(0.5f, 1f),
                IsActive = MaxPage > 0,
                NoEvent = true,
                Scale = new Vector2(0.9f, 0.9f),
            };
            AppendChild(Slot);
            AppendChild(_label);
            if (maxPage > 1) {
                var buttonL = new UIImageButton() {
                    Texture = RecipeGraph.Instance.GetTexture("Images/ArrowSmallLeft"),
                    Pivot = new Vector2(0, 0.5f),
                    AnchorPoint = new Vector2(0, 0.5f),
                    Scale = new Vector2(0.5f, 1f),
                };
                buttonL.OnClick += ButtonL_OnClick;
                AppendChild(buttonL);
                var buttonR = new UIImageButton() {
                    Texture = RecipeGraph.Instance.GetTexture("Images/ArrowSmallRight"),
                    Pivot = new Vector2(1, 0.5f),
                    AnchorPoint = new Vector2(1, 0.5f),
                    Scale = new Vector2(0.5f, 1f),
                };
                buttonR.OnClick += ButtonR_OnClick;
                AppendChild(buttonR);
            }
        }

        private void ButtonR_OnClick(UIMouseEvent e, UIElement sender) {
            if (Page != MaxPage)
                OnNextPage?.Invoke(e, this);
        }

        private void ButtonL_OnClick(UIMouseEvent e, UIElement sender) {
            if (Page != 1)
                OnPrevPage?.Invoke(e, this);
        }


        public override void DrawSelf(SpriteBatch sb) {
            base.DrawSelf(sb);

        }
    }
}
