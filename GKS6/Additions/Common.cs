using GraphSharp.Algorithms.OverlapRemoval;
using GraphSharp.Controls;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace GKS.Additions
{
    public static class Common
    {
        public static void AddListOfListsToDataGrid<T>(DataGrid grid, List<List<T>> list)
        {
            var maxLength = list.Max(c => c.Count);

            for (int i = 0; i < maxLength; i++)
            {
                grid.Columns.Add(new DataGridTextColumn() { Header = String.Format("{0,-2}", i), Binding = new Binding("Row" + i) });
            }

            foreach (var listItem in list)
            {
                var properties = new Dictionary<string, object>();
                for (int i = 0; i < listItem.Count; i++)
                {
                    properties.Add("Row" + i, listItem[i].ToString());
                }
                var myObject = Extensions.GetDynamicObject(properties);

                grid.Items.Add(myObject);
            }

            grid.Items.Refresh();
            grid.UpdateLayout();
        }

        public static void SetDictionaryToListView(ListView listView, Dictionary<List<int>, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                listView.Items.Add(new { Keys = String.Join(" ", item.Key), Values = item.Value });
            }

            listView.Items.Refresh();
            listView.UpdateLayout();
        }

        public static void SetStringToListView(ListView listView, List<string> list)
        {
            foreach (var item in list)
            {
                listView.Items.Add(new { Keys = "", Values = item });
            }

            listView.Items.Refresh();
            listView.UpdateLayout();
        }

        public static void ShowGraphCircular(GraphLayout layout, IBidirectionalGraph<object, IEdge<object>> graph, int horizontalGap = 25, int verticalGap = 10)
        {
            var overlapParameters = new OverlapRemovalParameters();
            overlapParameters.HorizontalGap = horizontalGap;
            overlapParameters.VerticalGap = verticalGap;
            layout.OverlapRemovalParameters = overlapParameters;
            layout.OverlapRemovalAlgorithmType = "FSA";
            layout.OverlapRemovalConstraint = AlgorithmConstraints.Must;

            layout.LayoutMode = LayoutMode.Simple;
            layout.LayoutAlgorithmType = "Circular";

            layout.Graph = graph;
        }

    }
}
