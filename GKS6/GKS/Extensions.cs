using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using QuickGraph.Collections;

namespace GKS
{
    public static class Extensions
    {
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static int ContainsAllItems<T>(this IList<T> listA, IList<T> listB)
        {
            var bool1 = listA.Except(listB).Any();
            var bool2 = listB.Except(listA).Any();

            int difference = listA.Except(listB).Union(listB.Except(listA)).Count();

            if (bool1 == false && bool2 == false)
            {
                return 0; //A == B
            }
            else if (bool1 == true && bool2 == false)
            {
                return 1; //A+ contains B
            }
            else if (bool1 == false && bool2 == true)
            {
                return 2; //B+ contains A
            }
            else
            {
                return -1; //Some shit
            }
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in properties)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }

        public static List<List<T>> ConvertFromMatrix<T>(T[,] matrix)
        {
            List<List<T>> returnList = new List<List<T>>();

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                List<T> tempList = new List<T>();
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    tempList.Add(matrix[x, y]);
                }
                returnList.Add(tempList);
            }

            return returnList;
        }

        public static T[,] ConvertToMatrix<T>(List<List<T>> list)
        {
            int maxLength = list.Max(x => x.Count);
            T[,] matrix = new T[maxLength, maxLength];

            int xCounter = 0;
            int yCounter = 0;
            foreach (var item in list)
            {
                foreach (var subitem in item)
                {
                    matrix[xCounter, yCounter] = subitem;
                    yCounter++;
                }
                yCounter = 0;
                xCounter++;
            }

            return matrix;
        }

        public static List<string> GetSplittedWords(string str)
        {
            return str.Split(new char[] { ' ', '\t', '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static T GetFirstKeyByValue<T, V>(this IDictionary<T, V> dictionary, V value)
        {
            return dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;
        }

        public static MatrixWithHeaders TrimMatrixWithHeaders(this MatrixWithHeaders matrixWithHeaders, int index)
        {
            matrixWithHeaders.Matrix = matrixWithHeaders.Matrix.TrimArray(index, index);

            var tempDictionary = new Dictionary<int, string>();

            for (int x = index + 1; x < matrixWithHeaders.Headers.Count; x++)
            {
                tempDictionary.Add(x - 1, matrixWithHeaders.Headers[x]);
            }

            for (int x = matrixWithHeaders.Headers.Count - 1; x >= index; x--)
            {
                matrixWithHeaders.Headers.Remove(x);
            }

            for (int x = 0; x < tempDictionary.Count; x++)
            {
                matrixWithHeaders.Headers.Add(matrixWithHeaders.Headers.Count, tempDictionary[matrixWithHeaders.Headers.Count]);
            }


            return matrixWithHeaders;
        }

        private static int[,] TrimArray(this int[,] originalArray, int rowToRemove, int columnToRemove)
        {
            int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }


        public static int IsInGroup(List<List<int>> list, int value)
        {
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    foreach (var subitem in item)
                    {
                        if (subitem == value)
                        {
                            return list.IndexOf(item);
                        }
                    }
                }
            }

            return -1;
        }

        public static bool IsInVertices(IEnumerable<object> list, string find)
        {
            foreach (var item in list)
            {
                if (item.ToString().Contains(find))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
