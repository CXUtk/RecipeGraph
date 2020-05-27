using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RecipeGraph.DataStructures {
    public class Filter {
        public string Name { get; }
        public Predicate<Item> FilterFunction { get; }
        public Texture2D Texture { get; }
        public Filter(string name, Predicate<Item> filter, Texture2D texture) {
            Name = name;
            FilterFunction = filter;
            Texture = texture;
        }
    }
}
