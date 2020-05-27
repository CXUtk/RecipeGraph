using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib;

namespace RecipeGraph.Components {
    public abstract class UIParts : UIElement {
        public RecipeGraphState MainState { get; }
        public UIParts(RecipeGraphState main) : base() {
            MainState = main;
        }
    }
}
