using System;
using System.Collections.Generic;
using System.Linq;

namespace GKS
{
    static class Operands
    {
        public static List<List<int>> CompareLines(List<String> strings)
        {
            int uniqueItems = CountUnique(strings);
            int arrayLength = strings.Count;
            int[,] comparedArray = new int[arrayLength, arrayLength];

            for (int x = 0; x < arrayLength; x++)
            {
                for (int y = 0; y < arrayLength; y++)
                {
                    if (x != y)
                    {
                        comparedArray[x, y] = uniqueItems - Difference(strings[x], strings[y]);
                    }
                }
            }

            return Extensions.ConvertFromMatrix<int>(comparedArray);
        }

        private static int Difference(string string1, string string2)
        {
            var words1 = Extensions.GetSplittedWords(string1);
            var words2 = Extensions.GetSplittedWords(string2);

            var unique = words1.Except(words2).Union(words2.Except(words1)).ToList();

            return unique.Count();
        }

        public static int CountUnique(List<String> strings)
        {
            List<string> tempList = new List<string>();

            foreach (var item in strings)
            {
                tempList.AddRange(Extensions.GetSplittedWords(item));
            }

            return Extensions.GetSplittedWords(String.Join(" ", tempList.ToArray())).Select(x => x).Distinct().Count();
        }


        public static Dictionary<List<int>, string> GetUniqueInGroups(Dictionary<int, string> strings, List<List<int>> groups)
        {
            Dictionary<List<int>, string> uniqueInGroup = new Dictionary<List<int>, string>();

            foreach (var item in groups)
            {
                string tempString = "";

                foreach (var subitem in item)
                {
                    string str = "";
                    strings.TryGetValue(subitem, out str);

                    tempString += " " + str;
                }
                var words = Extensions.GetSplittedWords(tempString).Distinct();
                uniqueInGroup.Add(item, String.Join(" ", words));
            }

            return uniqueInGroup;
        }

        public static KeyValuePair<List<int>, string> GetUniqueInCurrentGroup(Dictionary<int, string> strings, List<int> group)
        {
            return GetUniqueInGroups(strings, new List<List<int>>() { group }).First();
        }

    }
}