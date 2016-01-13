using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GKS.GKS.Additions
{
    public class MainData
    {
        #region Props

        private bool initialized;


        private Dictionary<int, string> initialStrings;
        private List<List<string>> initialList;
        private List<List<int>> comparationList;
        private Dictionary<List<int>, string> gropsFull;
        private Dictionary<List<int>, string> groupsSimplified;
        private Dictionary<List<int>, string> modulesFull;
        private List<string> modulesUnique;
        private List<IBidirectionalGraph<object, IEdge<object>>> graphsListInitial;
        private List<IBidirectionalGraph<object, IEdge<object>>> graphsListModuled;
        private IBidirectionalGraph<object, IEdge<object>> graphFlows;


        public Dictionary<int, string> InitialStrings { get { return initialStrings; } }
        public bool IsInitialized { get { return initialized; } }
        public List<List<string>> InitialList { get { return initialList; } }
        public List<List<int>> ComparationList { get { return comparationList; } }
        public Dictionary<List<int>, string> GropsFull { get { return gropsFull; } }
        public Dictionary<List<int>, string> GroupsSimplified { get { return groupsSimplified; } }
        public Dictionary<List<int>, string> ModulesFull { get { return modulesFull; } }
        public List<string> ModulesUnique { get { return modulesUnique; } }
        public List<IBidirectionalGraph<object, IEdge<object>>> GraphsListInitial { get { return graphsListInitial; } }
        public List<IBidirectionalGraph<object, IEdge<object>>> GraphsListModuled { get { return graphsListModuled; } }
        public IBidirectionalGraph<object, IEdge<object>> GraphFlows { get { return graphFlows; } }

        #endregion

        public MainData(string filePath)
        {
            this.initialized = false;
            this.initialStrings = IO.ReadStringsFromFile(filePath);

            Calculate();
        }

        public MainData(Dictionary<int, string> initialStrings)
        {
            this.initialized = false;
            this.initialStrings = initialStrings;

            Calculate();
        }

        private void Calculate()
        {
            initialList = new List<List<string>>();

            foreach (var item in initialStrings)
            {
                initialList.Add(Extensions.GetSplittedWords(item.Value));
            }

            comparationList = Operands.CompareLines(initialStrings.Values.ToList());

            List<List<int>> groupsList = Groupping.Group(comparationList);
            gropsFull = Operands.GetUniqueInGroups(initialStrings, groupsList);
            groupsSimplified = Groupping.Simplify(initialStrings, gropsFull);

            modulesFull = new Dictionary<List<int>, string>();
            graphsListInitial = new List<IBidirectionalGraph<object, IEdge<object>>>();
            graphsListModuled = new List<IBidirectionalGraph<object, IEdge<object>>>();

            int counter = 0;

            foreach (var item in groupsSimplified)
            {
                graphsListInitial.Add(Graph.AdjacentyMatrixToGraph(Modules.CreateRelationMatrix(initialStrings, item)));

                var modulesMatrix = Modules.GetModulesMatrix(initialStrings, item);
                graphsListModuled.Add(Graph.AdjacentyMatrixToGraph(modulesMatrix));

                modulesFull.Add(item.Key, String.Join("  ", modulesMatrix.Headers.Values.ToList()));
                counter++;
            }

            modulesUnique = Modules.GetModulesUnique(modulesFull);

            graphFlows = Modules.GetFlows(initialStrings.Values.ToList(),groupsSimplified.Values.ToList(), modulesUnique);

            initialized = true;
        }

    }
}
