using System;
using System.Collections.Generic;

namespace DGIM_2
{
    class DGIM
    {
        public enum EnumBinaryNumber
        {
            Zero = '0',
            One = '1'
        }

        private const char Query = 'q';
        static readonly List<int> buckets = new List<int>();
        static readonly List<int> timestamps = new List<int>();

        static int windowSize;
        static int[] bucketSizes;

        static void Main(string[] args)
        {
            windowSize = Convert.ToInt32(Console.ReadLine());
            bucketSizes = new int[(int)(Math.Log(windowSize, 2) + 1)];
            for (var i = 0; i < (int)(Math.Log(windowSize, 2) + 1); i++)
            {
                bucketSizes[i] = (int) Math.Pow(2, i);
            }

            while (true)
            {
                var line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }

                var lineSplit = line.Trim().Split();
                if (lineSplit[0] == Query.ToString() && lineSplit.Length == 2)
                {
                    var queryWindowSize = Convert.ToInt32(lineSplit[1]);
                    RunQuery(queryWindowSize);
                }
                else
                {
                    AddBucket(line);
                }
            }
        }

        private static void AddBucket(string line)
        {
            foreach (var bit in line)
            {
                var deleteList = new List<int>();
                for (var i = 0; i < timestamps.Count; i++)
                {
                    timestamps[i]++;
                    if (timestamps[i] >= windowSize)
                    {
                        deleteList.Add(i);
                    }
                }

                foreach (var index in deleteList)
                {
                    buckets.RemoveAt(index);
                    timestamps.RemoveAt(index);
                }

                if (bit == (char)EnumBinaryNumber.One)
                {
                    buckets.Add(1);
                    timestamps.Add(0);

                    foreach (var size in bucketSizes)
                    {
                        var indexList = new List<int>();
                        for (var i = 0; i < buckets.Count; i++)
                        {
                            if (buckets[i] == size)
                            {
                                indexList.Add(i);
                            }
                        }

                        if (indexList.Count == 3)
                        {
                            buckets[indexList[1]] = size * 2;

                            buckets.RemoveAt(indexList[0]);
                            timestamps.RemoveAt(indexList[0]);
                        }
                    }
                }
            }
        }

        private static void RunQuery(int queryWindowSize)
        {
            var first = true;
            var sum = 0;

            for (var index = 0; index < timestamps.Count; index++)
            {
                if (timestamps[index] < queryWindowSize)
                {
                    if (first)
                    {
                        sum += buckets[index] / 2;
                        first = false;
                    }
                    else
                    {
                        sum += buckets[index];
                    }
                }
            }

            Console.WriteLine(sum);
        }
    }
}
