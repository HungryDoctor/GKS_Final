using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Collections;

namespace GKS
{
    public static class Modules
    {
        public static List<string> GetModulesUnique(Dictionary<List<int>, string> modulesDict)
        {
            List<string> modules = new List<string>();
            foreach (var item in modulesDict.Values)
            {
                foreach (var subitem in item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    modules.Add(subitem.Trim());
                }
            }
            modules = modules.OrderBy(x => x.Length).ToList();

            for (int x = 0; x < modules.Count; x++)
            {
                var primaryWords = Extensions.GetSplittedWords(modules[x]);
                for (int y = x + 1; y < modules.Count; y++)
                {
                    var itemWords = Extensions.GetSplittedWords(modules[y]);
                    if (itemWords.ContainsAllItems(primaryWords) == 0 || itemWords.ContainsAllItems(primaryWords) == 1)
                    {
                        modules.RemoveAt(x);
                        x--;
                        break;
                    }
                }
            }

            for (int x = modules.Count - 1; x >= 0; x--)
            {
                for (int y = x - 1; y >= 0; y--)
                {
                    var itemWords = Extensions.GetSplittedWords(modules[y]);
                    var primaryWords = Extensions.GetSplittedWords(modules[x]);

                    for (int z = 0; z < primaryWords.Count; z++)
                    {
                        for (int k = 0; k < itemWords.Count; k++)
                        {
                            if (primaryWords[z] == itemWords[k])
                            {
                                if (primaryWords.Count > itemWords.Count)
                                {
                                    primaryWords.RemoveAt(z);
                                    z--;
                                    break;
                                }
                                else
                                {
                                    itemWords.RemoveAt(k);
                                    k--;
                                    break;
                                }
                            }
                        }
                    }

                    modules[x] = String.Join(" ", primaryWords);
                    modules[y] = String.Join(" ", itemWords);
                }
            }

            return modules.OrderBy(x => x.Length).ToList();
        }

        public static MatrixWithHeaders GetModulesMatrix(Dictionary<int, string> strings, KeyValuePair<List<int>, string> group)
        {
            var matrixWithHeaders = CreateRelationMatrix(strings, group);

            if (matrixWithHeaders.Headers.Count() > 1)
            {
                while (true)
                {
                    var matrix = matrixWithHeaders.Matrix;

                    var roots = GetRoots(matrixWithHeaders);
                    matrixWithHeaders = ConnectElements(matrixWithHeaders, roots);

                    var sinks = GetSinks(matrixWithHeaders);
                    matrixWithHeaders = ConnectElements(matrixWithHeaders, sinks);

                    var cycles = GetCycle(matrixWithHeaders);
                    matrixWithHeaders = ConnectElements(matrixWithHeaders, cycles);

                    var straight = GetStraight(matrixWithHeaders);
                    matrixWithHeaders = ConnectElements(matrixWithHeaders, straight);

                    if (matrixWithHeaders.Matrix == matrix)
                    {
                        break;
                    }
                }
            }

            return matrixWithHeaders;
        }

        private static List<List<string>> GetRoots(MatrixWithHeaders matrixWithHeaders)
        {
            IVertexListGraph<object, IEdge<object>> graph = Graph.AdjacentyMatrixToGraph(matrixWithHeaders);
            List<List<string>> roots = new List<List<string>>();

            foreach (var item in graph.Sinks())
            {
                roots.Add(new List<string>() { item.ToString() });
            }

            return roots;
        }

        private static List<List<string>> GetSinks(MatrixWithHeaders matrixWithHeaders)
        {
            IVertexListGraph<object, IEdge<object>> graph = Graph.AdjacentyMatrixToGraph(matrixWithHeaders);
            List<List<string>> sinks = new List<List<string>>();

            foreach (var item in graph.Sinks())
            {
                sinks.Add(new List<string>() { item.ToString() });
            }

            return sinks;
        }

        private static List<List<string>> GetCycle(MatrixWithHeaders matrixWithHeaders)
        {
            IVertexListGraph<object, IEdge<object>> graph = Graph.AdjacentyMatrixToGraph(matrixWithHeaders);
            List<List<object>> candidates = new List<List<object>>();
            List<List<string>> outlines = new List<List<string>>();

            var stronglyConnectedDict = (IDictionary<object, int>)(new Dictionary<object, int>());
            var z = graph.StronglyConnectedComponents(out stronglyConnectedDict);

            for (int x = 0; x < z; x++)
            {
                candidates.Add(new List<object>());
                outlines.Add(new List<string>());
            }

            foreach (var item in stronglyConnectedDict)
            {
                candidates[item.Value].Add(item.Key);
            }

            int counter = 0;
            foreach (var item in candidates)
            {
                if (item.Count > 1)
                {
                    foreach (var subitem in item)
                    {
                        outlines[counter].Add(subitem.ToString());
                    }
                    counter++;
                }
            }

            return outlines;
        }

        private static List<List<string>> GetStraight(MatrixWithHeaders matrixWithHeaders)
        {
            Dictionary<object, IEnumerable<IEdge<object>>> vertexEdges = new Dictionary<object, IEnumerable<IEdge<object>>>();
            BidirectionalGraph<object, IEdge<object>> graph = Graph.AdjacentyMatrixToGraph(matrixWithHeaders) as BidirectionalGraph<object, IEdge<object>>;
            List<List<string>> straight = new List<List<string>>();
            EdgeList<string, Edge<string>> candidates = new EdgeList<string, Edge<string>>();

            var matrix = matrixWithHeaders.Matrix;
            var headers = matrixWithHeaders.Headers;
            var vertexes = graph.Vertices;

            foreach (var item in vertexes)
            {
                var inEdges = graph.InEdges(item);
                var outEdges = graph.OutEdges(item);
                if (inEdges.Count() == 1 && outEdges.Count() == 1)
                {
                    candidates.Add(new Edge<string>(inEdges.ElementAt(0).Source.ToString(), inEdges.ElementAt(0).Target.ToString()));
                    candidates.Add(new Edge<string>(outEdges.ElementAt(0).Source.ToString(), outEdges.ElementAt(0).Target.ToString()));
                }
            }

            for (int x = candidates.Count() - 1; x > 0; x--)
            {
                if (candidates[x - 1].Source == candidates[x].Source && candidates[x - 1].Target == candidates[x].Target)
                {
                    candidates.RemoveAt(x);
                }
            }

            for (int x = 0; x < candidates.Count; x++)
            {
                for (int y = x + 1; y < candidates.Count; y++)
                {
                    IEdge<object> edge = null;
                    graph.TryGetEdge(candidates[x].Source, candidates[y].Target, out edge);

                    if (edge != null)
                    {
                        var existItems = candidates.Select(z => z.Source == edge.Source.ToString() && z.Target == edge.Target.ToString()).ToList();
                        bool exist = false;

                        foreach (var item in existItems)
                        {
                            exist = exist || item;
                        }

                        if (exist == false)
                        {
                            List<string> tempList = new List<string>();
                            for (int z = x; z <= y; z++)
                            {
                                if (tempList.Contains(candidates[z].Source) == false)
                                {
                                    tempList.Add(candidates[z].Source);
                                }
                                if (tempList.Contains(candidates[z].Target) == false)
                                {
                                    tempList.Add(candidates[z].Target);
                                }
                            }
                            straight.Add(tempList);
                        }
                    }
                }
            }


            return straight;
        }


        private static MatrixWithHeaders ConnectElements(MatrixWithHeaders matrixWithHeaders, List<List<string>> elements)
        {
            var matrix = matrixWithHeaders.Matrix;
            var headers = matrixWithHeaders.Headers;

            for (int x = elements.Count - 1; x >= 0; x--)
            {
                if (elements[x].Count > 0)
                {
                    List<int> list = new List<int>();
                    foreach (var item in elements[x])
                    {
                        foreach (var subitem in headers.Values)
                        {
                            if (subitem.Contains(item) == true)
                            {
                                list.Add(headers.GetFirstKeyByValue(subitem));
                            }
                        }
                    }

                    matrixWithHeaders = MergeMatrixWithHeaders(matrixWithHeaders, list);
                }
            }

            return matrixWithHeaders;
        }

        private static MatrixWithHeaders MergeMatrixWithHeaders(MatrixWithHeaders matrixWithHeaders, List<int> elements)
        {
            var headers = matrixWithHeaders.Headers;

            int priamryElement = elements[0];
            elements.Sort();
            elements.RemoveAt(0);

            int counter = 0;
            foreach (var item in elements)
            {
                var matrix = matrixWithHeaders.Matrix;
                int index = item - counter;

                for (int y = matrix.GetLength(1) - 1; y >= 0; y--)
                {
                    if (matrix[index, y] == 1)
                    {
                        matrix[priamryElement, y] = 1;
                    }
                }

                for (int x = matrix.GetLength(0) - 1; x >= 0; x--)
                {
                    if (matrix[x, index] == 1)
                    {
                        matrix[x, priamryElement] = 1;
                    }
                }

                headers[priamryElement] += " " + headers[index];
                matrixWithHeaders = matrixWithHeaders.TrimMatrixWithHeaders(index);

                counter++;
            }

            for (int x = 0; x < matrixWithHeaders.Matrix.GetLength(0); x++)
            {
                matrixWithHeaders.Matrix[x, x] = 0;
            }

            return matrixWithHeaders;
        }


        public static MatrixWithHeaders CreateRelationMatrix(Dictionary<int, string> strings, KeyValuePair<List<int>, string> group)
        {
            var words = Extensions.GetSplittedWords(group.Value);
            int uniqueCount = words.Distinct().Count();

            MatrixWithHeaders matrixWithHeaders = new MatrixWithHeaders(new int[uniqueCount, uniqueCount], new Dictionary<int, string>());
            var headers = matrixWithHeaders.Headers;
            var matrix = matrixWithHeaders.Matrix;

            foreach (var item in words)
            {
                headers.Add(headers.Count, words[headers.Count]);
            }

            if (words.Count > 1)
            {
                foreach (var item in group.Key)
                {
                    var strWords = Extensions.GetSplittedWords(strings[item]);
                    for (int x = 1; x < strWords.Count; x++)
                    {
                        matrix[headers.GetFirstKeyByValue(strWords[x - 1]), headers.GetFirstKeyByValue(strWords[x])] = 1;
                    }
                }
            }
            else
            {
                matrix[headers.GetFirstKeyByValue(words[0]), headers.GetFirstKeyByValue(words[0])] = 1;
            }

            return new MatrixWithHeaders(matrix, headers);
        }


        public static IBidirectionalGraph<object, IEdge<object>> GetFlows(List<string> initialStrings, List<string> groups, List<string> modules)
        {
            BidirectionalGraph<object, IEdge<object>> graph = new BidirectionalGraph<object, IEdge<object>>();
            foreach (var module in modules)
            {
                graph.AddVertex(module);
            }

            foreach (var group in groups)
            {
                var words = Extensions.GetSplittedWords(group);

                if (words.Count > 1)
                {
                    string previousVertex = "";
                    for (int x = 0; x < words.Count; x++)
                    {
                        for (int y = 0; y < modules.Count; y++)
                        {
                            if (x == 0 && modules[y].Contains(words[x]))
                            {
                                previousVertex = modules[y];
                            }
                            else if (x > 0 && modules[y].Contains(words[x]))
                            {
                                IEdge<object> tempEdge;
                                if (graph.TryGetEdge(previousVertex, modules[y], out tempEdge) == false)
                                {
                                    graph.AddEdge(new Edge<object>(previousVertex, modules[y]));
                                }
                                previousVertex = modules[y];
                            }
                        }
                    }
                }
            }

            IDictionary<object, int> weaklyConnectedComp = new Dictionary<object, int>();
            graph.WeaklyConnectedComponents(weaklyConnectedComp);

            int uniqueStructs = weaklyConnectedComp.Values.GroupBy(x => x).Count();
            for (int x = 0; x < uniqueStructs; x++)
            {
                var allKeysByValues = weaklyConnectedComp.Where(pair => pair.Value == x).Select(pair => pair.Key);

                List<string> roots = new List<string>();
                List<string> sinks = new List<string>();
                foreach (var item in initialStrings)
                {
                    roots.Add(item[0].ToString() + item[1].ToString());
                    sinks.Add(item[item.ToString().Length - 2].ToString() + item[item.ToString().Length - 1].ToString());
                }

                var entrance = roots.GroupBy(gr => gr).Select(gr => new { Name = gr.Key, Count = gr.Count() }).OrderByDescending(gr => gr.Count).FirstOrDefault();
                var exit = sinks.GroupBy(gr => gr).Select(gr => new { Name = gr.Key, Count = gr.Count() }).OrderByDescending(gr => gr.Count).FirstOrDefault();

                graph.AddVertex("Вход");
                graph.AddVertex("Выход");

                foreach (var module in modules)
                {
                    if (module.Contains(entrance.Name))
                    {
                        graph.AddEdge(new Edge<object>("Вход", module));
                    }

                    if (module.Contains(exit.Name))
                    {
                        graph.AddEdge(new Edge<object>(module, "Выход"));
                    }
                }
            }

            return graph;
        }

    }
}
