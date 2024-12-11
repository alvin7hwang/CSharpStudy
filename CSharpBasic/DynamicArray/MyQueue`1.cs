using System.Collections;

namespace DynamicArray
{
    /// <summary>
    /// 회전배열로 구현한 Queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MyQueue<T> : IEnumerable<T>
    {
        internal MyQueue()
        {
            _data = new T[DEFAULT_SIZE];
        }

        /// <summary>
        /// Reserve (공간 미리 확보)
        /// 런타임중에 최대 얼마정도까지 아이템이 들어올지 가늠할 수있다면, 그만큼의 공간을 미리 확보해둠
        /// 왜? -> 미리 확보하면 아이템 삽입시 O(N) 알고리즘이 O(1)로 변하는 마법 ~
        /// </summary>
        /// <param name="capacity"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal MyQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _data = new T[capacity];
        }
                

        internal int Capacity
        {
            get => _data.Length;
            set
            {
                // 현재 아이템 수보다 작은 용량으로 바꾸려고하면 예외던질거임
                if (_data.Length > value)
                    throw new Exception("Capacity is less than items count...");

                T[] tmp = new T[value];

                // Before
                // 4 ㅁ ㅁ 2 3
                // head == 3
                // tail == 0
                for (int i = 0; i < _size; i++)
                {
                    tmp[i] = _data[(_head + i) % _data.Length];
                }

                // After
                // 2 3 4 ㅁ ㅁ
                _data = tmp;
                _head = 0;
                _tail = _size - 1;
            }
        }

        internal int Count => _size;

        int _size; // 실제 가지고있는 아이템 수
        T[] _data; // 아이템들이 들어있는 배열
        int _head; // 가장 앞 인덱스
        int _tail; // 가장 뒤 인덱스
        const int DEFAULT_SIZE = 4;

        // 삽입 
        // O(N) - 최악의 경우 : 공간이 부족할때.
        internal void Enqueue(T item)
        {
            // 1. 새 아이템을 추가할 공간이 남아있는지 확인
            // 2. 공간이 없다면, 현재 공간의 두배크기 배열을 생성
            // 3. 새로 생성된 배열에 기존 데이터 복사
            // 4. 가장 마지막 아이템 다음 인덱스위치에 새 아이템 추가
            // 5. 전체아이템수 1 증가

            // 아이템수와 배열길이가 같다면 공간 부족한것임
            if (_size == _data.Length)
            {
                Capacity *= 2;
            }

            _tail = (_tail + 1) % _data.Length;
            _data[_tail] = item;
            _size++;
        }

        // 삭제
        // O(1)
        internal T Dequeue()
        {
            // 1. 삭제할 아이템이 있는지
            // 2. 젤 마지막 아이템 삭제 및 반환
            // 3. 총아이템수 하나 감소

            if (_size == 0)
                throw new IndexOutOfRangeException();

            T item = _data[_head];
            _data[_head] = default;
            _head = (_head + 1) % _data.Length;
            return item;
        }

        internal T Peek()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            return _data[_head];
        }

        /// <summary>
        /// yield 로 Enumerable 객체를 반환할 수 있도록 간편한 함수 작성 (유니티의 코루틴도 이런 방식으로 보통 많이함.)
        /// 그래서 면접볼때 코루틴쓴 내용이 포폴같은데 있으면 
        /// IEnumerable, IEnumerator , yield 에 대해 이해를 정확하게 하고있는지 검증차 물어봄
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            int currentIndex = 0;

            while (currentIndex < _size)
            {
                yield return _data[currentIndex++];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>
        {
            // 생성자를 통한 의존성 주입
            public Enumerator(MyQueue<T> myDynamicArray)
            {
                _myQueue = myDynamicArray;
                _index = -1;
            }

            // 4 ㅁ ㅁ 2 3 
            // head : 3
            // index : 2
            // head + index = 5...  mod Capacity -> 0
            public T Current => _myQueue._data[(_myQueue._head + _index) % _myQueue._data.Length];

            object IEnumerator.Current => _myQueue._data[(_myQueue._head + _index) % _myQueue._data.Length];

            MyQueue<T> _myQueue;
            int _index;

            /// <summary>
            /// 이 객체를 생성하고 실행되는동안 참조하기위해 생성되었던 관리되지 않는 리소스를 해제 
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// 다음 인덱스로 넘어가기 시도
            /// </summary>
            /// <returns> 다음값 존재 여부 </returns>
            public bool MoveNext()
            {
                // 다음으로 넘어갈 인덱스 존재하는지?
                if (_index < _myQueue._size - 1)
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
        }
    }
}
