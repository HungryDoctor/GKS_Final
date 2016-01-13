using QuickGraph;

namespace GKS
{
    public static class Graph
    {
        public static IBidirectionalGraph<object, IEdge<object>> AdjacentyMatrixToGraph(MatrixWithHeaders matrixWithHeaders)
        {
            var graph = new BidirectionalGraph<object, IEdge<object>>();

            var headers = matrixWithHeaders.Headers;
            var matrix = matrixWithHeaders.Matrix;

            foreach (var item in headers)
            {
                graph.AddVertex(item.Value);
            }

            if (headers.Count > 1)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        if (matrix[x, y] == 1)
                        {
                            graph.AddEdge(new Edge<object>(headers[x], headers[y]));
                        }
                    }
                }
            }
            else
            {
                graph.AddEdge(new Edge<object>(headers[0], headers[0]));
            }

            return graph;
        }

    }
}
