using System.Collections.Generic;
using System.Linq;

namespace GKS
{
    public static class Groupping
    {
        private static bool[,] marks;


        public static List<List<int>> Group(List<List<int>> matrixList)
        {
            int[,] matrix = Extensions.ConvertToMatrix(matrixList);
            marks = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            int max = FindPrevMax(matrix, int.MaxValue);

            List<List<int>> groups = new List<List<int>>();
            List<PointInt> indexes = new List<PointInt>();
            List<int> group = new List<int>();

            bool done = false;

            while (max > 0)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < matrix.GetLength(1); y++)
                    {
                        if (matrix[x, y] == max && done == false)
                        {
                            List<PointInt> temp = ThrougCross(x, y, matrix);

                            while (temp.Count != 0)
                            {
                                indexes.AddRange(temp);
                                foreach (var item in indexes)
                                {
                                    marks[item.X, item.Y] = true;
                                }
                                foreach (var item in indexes)
                                {
                                    temp = ThrougCross(item.X, item.Y, matrix);

                                    foreach (var subItem in temp)
                                    {
                                        marks[subItem.X, subItem.Y] = true;
                                    }
                                }
                            }
                            done = true;
                        }
                    }
                }

                group = new List<int>();

                foreach (var item in indexes)
                {
                    if (Extensions.IsInGroup(groups, item.X) == -1 && group.Contains(item.X) == false)
                    {
                        group.Add(item.X);
                    }
                    if (Extensions.IsInGroup(groups, item.Y) == -1 && group.Contains(item.Y) == false)
                    {
                        group.Add(item.Y);
                    }
                }

                if (group.Count > 0)
                {
                    groups.Add(group);
                }

                done = false;
                max = FindPrevMax(matrix, max);
            }

            group = new List<int>();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                if (Extensions.IsInGroup(groups, x) == -1 & group.Contains(x) == false)
                {
                    group.Add(x);
                }
            }
            if (group.Count > 0)
            {
                groups.Add(group);
            }

            marks = null;
            return groups;
        }

        private static int FindPrevMax(int[,] matrix, int value)
        {
            int max = -1;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] >= value)
                    {
                        continue;
                    }
                    else if (max < matrix[x, y])
                    {
                        max = matrix[x, y];
                    }
                }
            }

            return max;
        }

        private static List<PointInt> ThrougCross(int i, int j, int[,] matrix)
        {
            List<PointInt> elements = new List<PointInt>();

            int item = matrix[i, j];

            if (marks[i, j] == false)
            {
                elements.Add(new PointInt(i, j));
            }

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                if (matrix[x, j] == item && x != i && marks[x, j] == false)
                {
                    elements.Add(new PointInt(x, j));
                }
            }

            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                if (matrix[i, y] == item && y != j && marks[i, y] == false)
                {
                    elements.Add(new PointInt(i, y));
                }
            }

            return elements;
        }


        public static Dictionary<List<int>, string> Simplify(Dictionary<int, string> strings, Dictionary<List<int>, string> groups)
        {
            var returnGroups = new List<KeyValuePair<List<int>, string>>();
            var sortedGroup = new Dictionary<List<int>, string>(groups.OrderByDescending(z => z.Value.Length).ToDictionary(z => z.Key, z => z.Value));
            var sortedList = sortedGroup.ToList();

            while (true)
            {
                sortedGroup = new Dictionary<List<int>, string>(sortedGroup.OrderByDescending(z => z.Value.Length).ToDictionary(z => z.Key, z => z.Value));
                sortedList = sortedGroup.ToList();
                if (sortedGroup.Count > 1)
                {
                    int sameLength = sortedGroup.Values.Count(z => z.Length == sortedGroup.Values.ToList()[0].Length);
                    int max = 0;
                    int foundIndex = 0;

                    for (int y = 0; y < sameLength; y++)
                    {
                        int temp = CheckGroups(strings, sortedGroup, y);
                        if (max < temp)
                        {
                            max = temp;
                            foundIndex = y;
                        }
                    }

                    var tempList = sortedList.Swap(0, foundIndex);
                    sortedGroup = CollapseGroups(strings, tempList.ToDictionary(z => z.Key, z => z.Value));
                    sortedList = sortedGroup.ToList();
                    returnGroups.Add(sortedList[0]);
                    sortedList.RemoveAt(0);
                    sortedGroup = sortedList.ToDictionary(z => z.Key, z => z.Value);
                }
                else if (sortedGroup.Count > 0)
                {
                    returnGroups.Add(sortedList[0]);
                    sortedList.RemoveAt(0);
                    sortedGroup = sortedList.ToDictionary(z => z.Key, z => z.Value);
                }
                else if (sortedGroup.Count == 0)
                {
                    break;
                }
            }

            return returnGroups.ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<List<int>, string> CollapseGroups(Dictionary<int, string> strings, Dictionary<List<int>, string> groups)
        {
            var copyGroups = Extensions.DeepClone<List<KeyValuePair<List<int>, string>>>(groups.ToList());
            var primaryWords = Extensions.GetSplittedWords(copyGroups[0].Value);

            for (int index = 1; index < copyGroups.Count; index++)
            {
                var secondaryWords = Extensions.GetSplittedWords(copyGroups[index].Value);

                switch (primaryWords.ContainsAllItems(secondaryWords))
                {
                    case 0:
                        {
                            copyGroups[0].Key.AddRange(copyGroups[index].Key);
                            copyGroups.RemoveAt(index);
                            break;
                        }
                    case 1:
                        {
                            copyGroups[0].Key.AddRange(copyGroups[index].Key);
                            copyGroups.RemoveAt(index);
                            break;
                        }
                    case 2:
                        {
                            break;
                        }
                    default:
                        {
                            var keys = new List<int>(copyGroups.ToDictionary(x => x.Key, x => x.Value).Keys.ToList()[index]);

                            foreach (var item in keys)
                            {
                                var itemWords = Extensions.GetSplittedWords(strings.Values.ToArray()[item]);

                                if (primaryWords.ContainsAllItems(itemWords) == 0 || primaryWords.ContainsAllItems(itemWords) == 1)
                                {
                                    copyGroups[0].Key.Add(item);
                                    copyGroups[index].Key.Remove(item);

                                    var uniqueInGroup = Operands.GetUniqueInCurrentGroup(strings, copyGroups[index].Key);

                                    KeyValuePair<List<int>, string> updatedItem = new KeyValuePair<List<int>, string>(copyGroups[index].Key, copyGroups[index].Value.Replace(copyGroups[index].Value, uniqueInGroup.Value));
                                    copyGroups[index] = updatedItem;
                                }
                            }
                            break;
                        }
                }
            }

            return copyGroups.ToDictionary(x => x.Key, x => x.Value);
        }

        private static int CheckGroups(Dictionary<int, string> strings, Dictionary<List<int>, string> groups, int searchIndex)
        {
            var copyGroups = Extensions.DeepClone<List<KeyValuePair<List<int>, string>>>(groups.ToList());
            var primaryWords = Extensions.GetSplittedWords(copyGroups[searchIndex].Value);
            int inSearchItem = 0;

            for (int index = 0; index < copyGroups.Count; index++)
            {
                var secondaryWords = Extensions.GetSplittedWords(copyGroups[index].Value);

                if (searchIndex == index)
                {
                    continue;
                }

                switch (primaryWords.ContainsAllItems(secondaryWords))
                {
                    case 0:
                        {
                            copyGroups[0].Key.AddRange(copyGroups[index].Key);
                            inSearchItem += copyGroups[index].Value.Count();
                            copyGroups.RemoveAt(index);
                            break;
                        }
                    case 1:
                        {
                            copyGroups[0].Key.AddRange(copyGroups[index].Key);
                            inSearchItem += copyGroups[index].Value.Count();
                            copyGroups.RemoveAt(index);
                            break;
                        }
                    case 2:
                        {
                            break;
                        }
                    default:
                        {
                            var keys = new List<int>(copyGroups.ToDictionary(x => x.Key, x => x.Value).Keys.ToList()[index]);

                            foreach (var item in keys)
                            {
                                var itemWords = Extensions.GetSplittedWords(strings.Values.ToArray()[item]);

                                if (primaryWords.ContainsAllItems(itemWords) == 0 || primaryWords.ContainsAllItems(itemWords) == 1)
                                {
                                    copyGroups[0].Key.Add(item);
                                    copyGroups[index].Key.Remove(item);

                                    var uniqueInGroup = Operands.GetUniqueInCurrentGroup(strings, copyGroups[index].Key);

                                    inSearchItem += Extensions.GetSplittedWords(uniqueInGroup.Value).Count;

                                    KeyValuePair<List<int>, string> updatedItem = new KeyValuePair<List<int>, string>(copyGroups[index].Key, copyGroups[index].Value.Replace(copyGroups[index].Value, uniqueInGroup.Value));
                                    copyGroups[index] = updatedItem;
                                }
                            }
                            break;
                        }
                }
            }

            return inSearchItem;
        }

    }
}