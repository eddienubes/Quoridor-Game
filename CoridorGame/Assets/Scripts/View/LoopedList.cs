using System;
using System.Collections.Generic;

namespace LoopedList
{
    /// <summary>
    ///  The type of list, that is looped - <para> after last element it gives link to first
    /// </summary>
    /// <typeparam name="T"> T means type of the data,stored in list</typeparam>
    public class LoopedList<T> : List<T>
    {
        /// <value> Toggle this token to true to break the infinite loop in foreach cycle</value>
        /// <example>
        /// <code>
        /// LoopedList<int> myList = new LoopedList<int>{1,2,3,4};
        /// int counter = 0;
        /// foreach (int x int myList)'
        /// {
        ///     Mylist.CancellationToken = counter++ == 100;
        ///     Console.WriteLine(x);       
        /// }
        /// </code>
        /// </example>
        public bool CancellationToken { get; set; }
        /// <value> means current position in list</value>
        int counter;
        public LoopedList()
        {
            Reset();
        }
        public LoopedList(IEnumerable<T> y) : base(y)
        {
            Reset();
        }
        public LoopedList(int size) : base(size)
        {
            Reset();
        }
        /// <summary>
        /// Resets counter and Token, used in
        /// <list type="uses">
        /// <item>
        /// <term>Constrictors</term>
        /// <description>in constructors for default initialization</description>
        /// </item>
        /// <item>
        /// <term>Enumerators</term>
        /// <description>in enumerator after exiting from cycle</description>
        /// </item>
        /// </list>
        /// </summary>
        void Reset()
        {
            counter = 0;
            CancellationToken = false;
        }
        /// <summary>
        /// An infinite indexator, uses for "for" cycles
        /// </summary>
        /// <param name="index">index of list, which value are seeked</param>
        /// <returns> value that cycled list contain on this index</returns>
        /// <exception cref="IndexOutOfRangeException"> Causes when index is negative or CancellationToken is enabled</exception>
        public new T this[int index]
        {
            get { return base[GetLoopedIndex(index)]; }
            set { base[GetLoopedIndex(index)] = value; }
        }
        /// <summary>
        /// Get the real index, which displays real index of list in RepeatedList
        /// <para>actually just trims it down to the desired boundaries
        /// </summary>
        /// <param name="index"> index, that need to be "trimmed"</param>
        /// <returns>trimmed index</returns>
        int GetLoopedIndex(int index)
        {
            if (CancellationToken)
            {
                Reset();
                return -1;
            }
            else
            {
                return index % Count;
            }
        }
        /// <summary>
        /// Returns enumerator, used in foreach cycle
        /// </summary>
        /// <returns>looped enumerator</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            while (counter != Capacity)
            {
                if (CancellationToken)
                {
                    Reset();
                    yield break;
                }
                if (counter == Count)
                {
                    counter = 0;
                }
                yield return base[GetLoopedIndex(counter++)];
            }
        }
    }
}