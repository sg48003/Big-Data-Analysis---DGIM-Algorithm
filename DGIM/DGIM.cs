using System;
using System.Collections.Generic;
using System.Linq;

namespace DGIM
{
    public class DGIM
    {
        public static LinkedList<Bucket> ListOfBuckets = new LinkedList<Bucket>();
        public static int TimeStamp;
        public static int WindowSize;

        public class Bucket
        {
            public int CreationTime;
            public int OneCounter;
        }

        static void Main(string[] args)
        {
            WindowSize = Convert.ToInt32(Console.ReadLine());


            while (true)
            {
                var line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }

                var lineSplit = line.Split();
                if (lineSplit[0] == "q" && lineSplit.Length == 2)
                {
                    var queryWindowSize = Convert.ToInt32(lineSplit[1]);
                    RunQuery(queryWindowSize);
                }
                else
                {
                    foreach (var bit in line)
                    {
                        var bitToInteger = Convert.ToInt32(bit);
                        TimeStamp++;
                        if (bitToInteger != 0)
                        {
                            CreateBucket_Proba();
                        }
                    }
                }
            }
        }

        private static void RunQuery(int queryWindowSize)
        {
            var lastBucket_Size = 0;
            var totalSize = 0;


            foreach (var bucket in ListOfBuckets)
            {
                if (bucket.CreationTime <= TimeStamp - queryWindowSize)
                {
                    break;
                }
                else
                {
                    totalSize += bucket.OneCounter;
                    lastBucket_Size = bucket.OneCounter;
                }
            }

            var result = totalSize - (int)Math.Ceiling(lastBucket_Size / 2.0);
            Console.WriteLine(result);
        }

        public static void CreateBucket()
        {
            var bucket = new Bucket
            {
                OneCounter = 1,
                CreationTime = TimeStamp
            };
            ListOfBuckets.AddFirst(bucket);

            var lastBucket = ListOfBuckets.Last;
            if (lastBucket.Value.CreationTime < TimeStamp - WindowSize)
            {
                ListOfBuckets.RemoveLast();
            }

            if (NeedMerge(ListOfBuckets.GetEnumerator()))
            {
                Merge(ListOfBuckets.GetEnumerator());
            }
        }

        private static bool NeedMerge(LinkedList<Bucket>.Enumerator? enumerator)
        {
            if (enumerator?.MoveNext() == true)
            {
                var current = enumerator?.Current;
                if (enumerator?.MoveNext() == true)
                {
                    var next = enumerator?.Current;

                    if (next?.OneCounter != current?.OneCounter)
                    {
                        return false;
                    }

                    if (enumerator?.MoveNext() == true)
                    {
                        var secondNext = enumerator?.Current;
                        return next?.OneCounter == secondNext?.OneCounter;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

        #region Proba

        public static void CreateBucket_Proba()
        {
            var bucket = new Bucket
            {
                OneCounter = 1,
                CreationTime = TimeStamp
            };
            ListOfBuckets.AddFirst(bucket);

            var lastBucket = ListOfBuckets.Last;
            if (lastBucket.Value.CreationTime < TimeStamp - WindowSize)
            {
                ListOfBuckets.RemoveLast();
            }

            if (NeedMerge_Proba(0))
            {
                Merge_Proba(0);
            }
        }

        private static bool NeedMerge_Proba(int index)
        {
            var current = ListOfBuckets.ElementAtOrDefault(index + 1);
            if (current != null)
            {
                var next = ListOfBuckets.ElementAtOrDefault(index + 2);
                if (next != null)
                {
                    if (next.OneCounter != current.OneCounter)
                    {
                        return false;
                    }
                    var secondNext = ListOfBuckets.ElementAtOrDefault(index + 3);
                    if (secondNext != null)
                    {
                        return next.OneCounter == secondNext.OneCounter;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

        private static void Merge_Proba(int index)
        {
            var next = ListOfBuckets.ElementAtOrDefault(index + 2);
            var nextOneCounter = next.OneCounter;

            var secondNext = ListOfBuckets.ElementAtOrDefault(index + 3);
            next.OneCounter += secondNext.OneCounter;

            ListOfBuckets.Remove(secondNext);

            var enumeratorAt = GetEnumeratorAt_Proba(nextOneCounter);
            if (enumeratorAt != null && NeedMerge_Proba((int)enumeratorAt))
            {
                Merge_Proba(nextOneCounter);
            }
        }

        private static int? GetEnumeratorAt_Proba(int value)
        {
            for (var i = 0; i < ListOfBuckets.Count; i++)
            {
                if (ListOfBuckets.ElementAt(i).OneCounter == value)
                {
                    return i;
                }
            }
            return null;
        }

        #endregion

        private static void Merge(LinkedList<Bucket>.Enumerator? enumerator)
        {
            enumerator?.MoveNext();
            enumerator?.MoveNext();
            var next = enumerator?.Current;
            var nextOneCounter = next.OneCounter;

            enumerator?.MoveNext();
            var secondNext = enumerator?.Current;
            nextOneCounter += secondNext.OneCounter;

            ListOfBuckets.Remove(secondNext);

            var enumeratorAt = GetEnumeratorAt(nextOneCounter);
            if (enumeratorAt != null && NeedMerge(enumeratorAt))
            {
                Merge(enumeratorAt);
            }
        }

        private static LinkedList<Bucket>.Enumerator? GetEnumeratorAt(int value)
        {
            var enumerator = ListOfBuckets.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var bucket = enumerator.Current;
                if (bucket.OneCounter == value)
                {
                    return enumerator;
                }
            }
            return null;
        }
    }
}