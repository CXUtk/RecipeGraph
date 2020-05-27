using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RecipeGraph.DataStructures {
    public class RGraph {
        // 合成图，正向边
        private List<int>[] _G;
        // 合成图，反向边
        private List<Recipe>[] _Grev;
        private int N;
        private bool[] _vis;
        public RGraph() {
            N = Main.itemTexture.Length;
            _G = new List<int>[N];
            _Grev = new List<Recipe>[N];
            _vis = new bool[N];
            _roots = new OffSpringNode[N];
            for (int i = 0; i < N; i++) {
                _vis[i] = false;
                _roots[i] = new OffSpringNode(i);
                _G[i] = new List<int>();
                _Grev[i] = new List<Recipe>();
            }
            for (int i = 0; i < Recipe.numRecipes; i++) {
                var r = Main.recipe[i];
                _Grev[r.createItem.type].Add(r);
                foreach (var req in r.requiredItem) {
                    if (req.type <= 0) continue;
                    _G[req.type].Add(r.createItem.type);
                }
            }
        }
        private OffSpringNode[] _roots;
        internal OffSpringTree FindOffspring(int type) {

            for (int i = 0; i < N; i++) {
                if (!_vis[i]) continue;
                _vis[i] = false;
                _roots[i] = new OffSpringNode(i);
            }
            Queue<int> Q = new Queue<int>();
            Q.Enqueue(type);
            _vis[type] = true;
            while (Q.Count > 0) {
                var cur = Q.Dequeue();
                foreach (var child in _G[cur]) {
                    if (!_vis[child]) {
                        _roots[cur].Children.Add(_roots[child]);
                        Q.Enqueue(child);
                        _vis[child] = true;
                    }
                }
            }
            return new OffSpringTree(_roots[type]);
        }
        public List<Recipe> GetRecipeToThis(int type) {
            return _Grev[type];
        }
    }
}
