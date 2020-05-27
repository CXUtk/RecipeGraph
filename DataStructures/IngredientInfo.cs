using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeGraph.DataStructures {
    public struct IngredientInfo {
        public int ItemType { get; set; }
        public int ItemStack { get; set; }
        public bool IsReceipeGroup { get; set; }
    }
}
