using Microsoft.Xna.Framework;
using RecipeGraph.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RecipeGraph.DataStructures {
    internal class OffSpringNode {
        public int Type { get; }
        public List<OffSpringNode> Children { get; }
        public OffSpringNode(int type) {
            Type = type;
            Children = new List<OffSpringNode>();
        }
    }
    internal struct TreeSize {
        public float Width;
        public int size;
    };
    internal class OffSpringTree {
        private int MAXNODES = 10;
        public OffSpringNode Root { get; }
        public float SlotSize { get; set; }
        public float MarginH { get; set; }
        public float VerticalDistance { get; set; }
        private int[] _depth;
        private TreeSize[] _treeSizes;
        private float _maxY;
        private Vector2[] _nodePosition;
        public OffSpringTree(OffSpringNode root) {
            int N = Main.itemTexture.Length;
            Root = root;
            SlotSize = 52f;
            MarginH = 1f;
            VerticalDistance = 30f;
            _depth = new int[N];
            _treeSizes = new TreeSize[N];
            _nodePosition = new Vector2[N];
            for (int i = 0; i < N; i++) _depth[i] = 0;
        }

        private void _dfs(OffSpringNode node) {
            TreeSize treeSize = new TreeSize {
                size = 1,
                Width = 0
            };
            if (node.Children.Count == 0) {
                treeSize.Width = SlotSize;
                _treeSizes[node.Type] = treeSize;
                return;
            }
            int i = 0;
            foreach (var child in node.Children) {
                if (i == MAXNODES) break;
                _depth[child.Type] = _depth[node.Type] + 1;
                _dfs(child);
                var csize = _treeSizes[child.Type];
                treeSize.size += csize.size;
                treeSize.Width += MarginH + csize.Width;
                i++;
            }
            _treeSizes[node.Type] = treeSize;
        }


        private void _calculate(OffSpringNode node, float offsetX) {
            float x = offsetX;
            int i = 0;
            foreach (var child in node.Children) {
                if (i == MAXNODES) break;
                _nodePosition[child.Type] = new Vector2((2 * x + _treeSizes[child.Type].Width) / 2, (_depth[child.Type] + 1) * (SlotSize + VerticalDistance));
                _maxY = Math.Max(_maxY, _nodePosition[child.Type].Y);
                _calculate(child, x);
                x += MarginH + _treeSizes[child.Type].Width;
                i++;
            }
        }

        public Vector2 CalculateSize() {
            _maxY = 0;
            _dfs(Root);
            _calculate(Root, 0);
            _nodePosition[Root.Type] = new Vector2(_treeSizes[Root.Type].Width / 2, SlotSize);
            return new Vector2(_treeSizes[Root.Type].Width + SlotSize, _maxY + SlotSize * 2);
        }

        private List<UISlotNode> _slots;

        private UISlotNode _dfs_slots(OffSpringNode node, UISlotNode parent) {
            var target = new UISlotNode(node.Type, parent) {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(SlotSize, SlotSize),
                Position = _nodePosition[node.Type],
            };
            _slots.Add(target);
            int i = 0;
            foreach (var child in node.Children) {
                if (i == MAXNODES) break;
                var c = _dfs_slots(child, target);
                target.SlotChildren.Add(c);
                i++;
            }
            return target;
        }

        public List<UISlotNode> GetSlots() {
            _slots = new List<UISlotNode>();
            _dfs_slots(Root, null);
            return _slots;
        }


    }
}
