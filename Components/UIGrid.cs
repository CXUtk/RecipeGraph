using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Components.Composite;
using UIEditor.UILib;

namespace RecipeGraph.Components {
    public class UIGrid : UIList {

        public int BlockMargin { get; set; }
        public UIGrid() : base() {
            Name = "网格列表";
            BlockMargin = 5;
        }
        public override void UpdateElementPos(GameTime gameTime) {
            _totHeight = InnerContainerPadding;
            for (int i = 0; i < _elements.Count;) {
                int j = i;
                int startX = 0;
                int maxHeight = 0;
                while (j < _elements.Count && startX + _elements[j].Width <= _viewPort.Width - BlockMargin) {
                    _elements[j].Position = new Vector2(startX, _totHeight);
                    maxHeight = Math.Max(maxHeight, _elements[j].Height);
                    startX += _elements[j].Width;
                    startX += BlockMargin;
                    j++;
                }
                _totHeight += maxHeight + BlockMargin;
                i = j;
            }
            CalculateViewPortScrollRelated();
        }

        public override void AddElement(UIElement element) {
            base.AddElement(element);
            element.OnClick += Element_OnClick;
        }

        private void Element_OnClick(UIEditor.UILib.Events.UIMouseEvent e, UIElement sender) {
            if (SelectedElement != null)
                SelectedElement.IsSelected = false;
            SelectedElement = sender;
            sender.IsSelected = true;
        }
    }
}
