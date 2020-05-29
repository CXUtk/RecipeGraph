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
        public int Page { get; set; }
        public int MaxPage { get { return (Children.Count + OffSpringTree.MAX_CHILDREN_DISPLAY - 1) / OffSpringTree.MAX_CHILDREN_DISPLAY; } }
        public OffSpringNode(int type) {
            Type = type;
            Children = new List<OffSpringNode>();
            Page = 1;
        }
    }
    internal struct TreeSize {
        public float Width;
        public int size;
    };
    internal class OffSpringTree {
        public const int MAX_CHILDREN_DISPLAY = 10;
        public OffSpringNode Root { get; }
        public float SlotSize { get; set; }
        public float MarginH { get; set; }
        public float VerticalDistance { get; set; }
        private int[] _depth;
        private float _maxY;
        private Vector2[] _nodePosition;
        private OffSpringNode[] _nodes;
        public OffSpringTree(OffSpringNode root) {
            int N = Main.itemTexture.Length;
            Root = root;
            SlotSize = 52f;
            MarginH = 15f;
            VerticalDistance = 30f;
            _depth = new int[N];
            _nodePosition = new Vector2[N];
            _nodes = new OffSpringNode[N];
            for (int i = 0; i < N; i++) _depth[i] = 0;
            _dfs1(Root);
        }

        private void _dfs1(OffSpringNode node) {
            _nodes[node.Type] = node;
            foreach (var child in node.Children) {
                _dfs1(child);
            }
        }

        private float _dfs(OffSpringNode node, float offsetX) {
            if (node.Children.Count == 0)
                return SlotSize;
            float x = offsetX, totWidth = 0;
            for (int i = (node.Page - 1) * 10; i < node.Children.Count && i < node.Page * 10; i++) {
                var child = node.Children[i];
                _depth[child.Type] = _depth[node.Type] + 1;
                _maxY = Math.Max(_maxY, (_depth[child.Type] + 1) * (SlotSize + VerticalDistance));
                var width = _dfs(child, x);
                _nodePosition[child.Type] = new Vector2((2 * x + width) / 2, (_depth[child.Type] + 1) * (SlotSize + VerticalDistance));
                x += MarginH + width;
                totWidth += MarginH + width;
            }
            totWidth -= MarginH;
            return totWidth;
        }
        internal void ChangePage(int type, int page) {
            _nodes[type].Page = page;
            _dfs(Root, 0);
        }

        //private void _calculate(OffSpringNode node, float offsetX) {
        //    float x = offsetX;
        //    foreach (var child in node.Children) {
        //        _nodePosition[child.Type] = new Vector2((2 * x + _treeSizes[child.Type].Width) / 2, (_depth[child.Type] + 1) * (SlotSize + VerticalDistance));
        //        _maxY = Math.Max(_maxY, _nodePosition[child.Type].Y);
        //        _calculate(child, x);
        //        x += MarginH + _treeSizes[child.Type].Width;
        //    }
        //}

        public Vector2 Calculate() {
            var width = _dfs(Root, 0);
            _nodePosition[Root.Type] = new Vector2(width / 2, SlotSize);
            return new Vector2(width + SlotSize, _maxY + SlotSize * 2);
        }

        internal void TryFindPage(int type, int childType) {
            int i = 0;
            foreach (var item in _nodes[type].Children) {
                if (item.Type == childType) {
                    _nodes[type].Page = i / 10 + 1;
                    break;
                }
                i++;
            }
        }

        private List<UISlotNode> _slots;

        private UISlotNode _dfs_slots(OffSpringNode node, UISlotNode parent) {
            var target = new UISlotNode(node.Type, parent, node.Page, node.MaxPage) {
                AnchorPoint = new Vector2(0, 0),
                Pivot = new Vector2(0.5f, 0.5f),
                Size = new Vector2(SlotSize, SlotSize),
                Position = _nodePosition[node.Type],
            };
            _slots.Add(target);
            target.SlotChildren.Clear();
            for (int i = (node.Page - 1) * 10; i < node.Children.Count && i < node.Page * 10; i++) {
                var child = node.Children[i];
                var c = _dfs_slots(child, target);
                target.SlotChildren.Add(c);
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
