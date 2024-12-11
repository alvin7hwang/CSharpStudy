using System.Collections;

namespace Enumeration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerator<int> e = new Enumerator();
            while (e.MoveNext())
            {
                Console.WriteLine(e.Current);
            }

            IEnumerator<int> e2 = GetEnumerator1To10();
            while (e2.MoveNext())
            {
                Console.WriteLine(e2.Current);
            }

            IEnumerable<int> eb3 = GetEnumerable1To10();
            IEnumerator<int> e3 = eb3.GetEnumerator();
            while (e3.MoveNext())
            {
                Console.WriteLine(e3.Current);
            }

            foreach (int i in eb3)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// 1 부터 10까지를 열거하는 객체 형태
        /// </summary>
        struct Enumerator : IEnumerator<int>
        {
            public int Current => _data[_index];

            object IEnumerator.Current => _data[_index];
            private int[] _data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            private int _index = -1;

            public Enumerator() {}

            public bool MoveNext()
            {
                if (_index < _data.Length - 1)
                {
                    _index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                _index = -1;
            }
            public void Dispose() { }
        }

        // yield 는 반복기(열거자) 객체의 다음 열거할 내용을 명시하는 구문
        static IEnumerator<int> GetEnumerator1To10()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield return 6;
            yield return 7;
            yield return 8;
            yield return 9;
            yield return 10;
        }

        static IEnumerable<int> GetEnumerable1To10()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield return 6;
            yield return 7;
            yield return 8;
            yield return 9;
            yield return 10;
        }
    }
}
