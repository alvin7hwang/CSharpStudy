using System.Collections;

namespace DynamicArray
{
    /*
     * 동적배열에 대한 의사코드작성
     * 현재 int 타입 데이터에 대한 아래 동적배열을 object 타입에 대한 동적배열로 바꾸시오..
     */
    internal class MyDynamicArray
    {
        public MyDynamicArray()
        {
            _data = new object[DEFAULT_SIZE];
        }


        // 인덱스 검색
        // O(1)
        internal object this[int index]
        {
            get
            {
                // 1. index 가 유효한지 검사 (현재 아이템 갯수를 초과한다면 인덱스 초과 예외)
                // 2. 데이터배열에서 index 접근하여 값을 반환.

                // 인덱스가 음수이거나 현재 아이템수를 넘어간다면
                if (index < 0 || index >= _size)
                    throw new IndexOutOfRangeException();

                return _data[index];
            }
            set
            {
                // 1. index 가 유효한지 검사 (현재 아이템 갯수를 초과한다면 인덱스 초과 예외)
                // 2. 데이터배열에서 index 접근하여 값을 대입.

                if (index < 0 || index >= _size)
                    throw new IndexOutOfRangeException();

                _data[index] = value;
            }
        }

        int _size; // 실제 가지고있는 아이템 수
        object[] _data; // 아이템들이 들어있는 배열
        const int DEFAULT_SIZE = 4;

        // 삽입 
        // O(N) - 최악의 경우 : 공간이 부족할때.
        internal void Add(object item)
        {
            // 1. 새 아이템을 추가할 공간이 남아있는지 확인
            // 2. 공간이 없다면, 현재 공간의 두배크기 배열을 생성
            // 3. 새로 생성된 배열에 기존 데이터 복사
            // 4. 가장 마지막 아이템 다음 인덱스위치에 새 아이템 추가
            // 5. 전체아이템수 1 증가

            // 아이템수와 배열길이가 같다면 공간 부족한것임
            if (_size == _data.Length)
            {
                object[] tmp = new object[_size * 2];

                Array.Copy(_data, tmp, _size);
                //for (int i = 0; i < _data.Length; i++)
                //{
                //    tmp[i] = _data[i];
                //}

                _data = tmp;
            }

            _data[_size] = item;
            _size++;
        }

        // 순회 검색
        // O(N) - 최악의 경우 : 못찾았을때
        internal int FindIndex(object item)
        {
            // 1. 전체 순회하면서 찾으려는 아이템과 동일한 아이템이 있는지 확인
            // 2. 있으면 현재 인덱스 반환, 없으면 에러코드 반환.

            for (int i = 0; i < _size; i++)
            {
                if (_data[i].Equals(item))
                    return i;
            }

            return -1;
        }

        // 삭제
        // O(N) - 최악의 경우 : 젤 앞에꺼 지우는거
        internal void RemoveAt(int index)
        {
            // 1. 삭제하려는 인덱스가 유효한지 검사
            // 2. 삭제하려는 인덱스 뒤부터 마지막까지를 순회하면서 한칸씩 앞으로 당김
            // 3. 총아이템수 하나 감소

            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException();

            for (int i = index; i < _size - 1; i++)
            {
                _data[i] = _data[i + 1];
            }

            _size--;
        }
    }
}
