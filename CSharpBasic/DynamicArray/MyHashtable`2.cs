using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal struct KeyValuePair<TKey, TValue>
    {
        internal KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        internal TKey Key;
        internal TValue Value;
    }

    internal class MyHashtable<TKey, TValue>
    {
        internal MyHashtable(int capacity)
        {
            _buckets = new int[capacity];

            for (int i = 0; i < _buckets.Length; i++)
            {
                _buckets[i] = EMPTY; // 유효하지않은값으로 초기화
            }

            _entries = new Entry[capacity];
        }

        internal TValue this[TKey key]
        {
            get
            {
                // key가 null인지 체크
                // key로 hashcode 계산, bucketIndex 계산
                // buckets 에 bucketIndex 접근해서 entryIndex 가져옴
                // entries[entryIndex]로 접근해서 Entry 가져옴
                // 해시코드 같은지 비교, Entry 의 key 와 key 비교
                // 같을 시 Entry의 Value 반환
                if (key == null)
                    throw new ArgumentNullException("key is null");

                int hashCode = GetHash(key.ToString());
                int bucketIndex = hashCode % _buckets.Length;
                int entryIndex = -1;

                // 모두 충돌했을 시, 존재하지 않을 때 탐색 시간복잡도는 O(N)
                for (int i = _buckets[bucketIndex]; i >= 0; i = _entries[i].NextIndex)
                {
                    // 해시코드와 키가 전부 같을 시 존재
                    if (_entries[i].HashCode == hashCode && _entries[i].Key.Equals(key))
                    {
                        entryIndex = i;
                        break;
                    }
                }

                if (entryIndex >= 0)
                    return _entries[entryIndex].Value;
                else
                    throw new KeyNotFoundException();
            }
            set
            {

            }
        }

        internal struct Entry
        {
            internal bool isVaild => HashCode >= 0; // HashCode가 유효할 시 true 반환
            internal int HashCode; // 해시코드 저장
            internal TKey Key;
            internal TValue Value;
            internal int NextIndex;
        }

        int[] _buckets; // Entry 의 시작점 인덱스 참조 배열
        Entry[] _entries; // 키-밸류 쌍 데이터 저장하는 배열
        int _count;
        const int EMPTY = -1;
        int _freeFirstEntryIndex; // 생성됫다가 지워진 재사용가능한 Entry 중 첫 진입점
        int _freeCount; // 생성됫다가 지워진 재사용가능한 Entry 의 총 갯수

        internal void Add(TKey key, TValue value)
        {
            // 1. Key 중복검사.
            // 2. Key 가 중복 ? 
            //      시작 entry 를 가져와서 빈자리가 나올때까지 탐색

            int hashCode = GetHash(key.ToString()); // key 에 대한 hashcode 생성
            int bucketIndex = hashCode % _buckets.Length; // hashcode 를 capacity 로 mod 해서 해당 HashCode가 가져야하는 bucketIndex 구함

            // buckets 에서 유효한 값은 양수이므로, 유효하지않은 인덱스값이 나올떄까지 반복
            for (int i = _buckets[bucketIndex]; i >= 0; i = _entries[i].NextIndex)
            {
                // 해시코드와 키가 전부 같을 시 이미 존재
                if (_entries[i].HashCode == hashCode && _entries[i].Key.Equals(key))
                    throw new ArgumentException("The Key already exists.");
            }

            int entryIndex = EMPTY;
            // 재사용가능한 Entry 있는지 체크
            if (_freeCount > 0)
            {
                entryIndex = _freeFirstEntryIndex;
                _freeFirstEntryIndex = _entries[entryIndex].NextIndex; // 첫 Entry를 재사용했으므로 그다음 Entry 를 첫 Entry 로 갱신
                _freeCount--;
            }
            else
            {
                // 범위 초과 시 Resize
                if (_count == _entries.Length)
                {
                    Resize(_count * 2);
                    bucketIndex = hashCode % _buckets.Length; // capacity 조정되었으므로 버킷인덱스 재연산
                }
                
                entryIndex = _count++;
            }


            _entries[entryIndex] = new Entry
            {
                HashCode = hashCode,
                Key = key,
                Value = value,
                NextIndex = _buckets[bucketIndex]
            };
            _buckets[bucketIndex] = entryIndex; // 연산한 버켓인덱스를 가진 버켓에 entryIndex 값 할당
        }

        internal bool Remove(TKey key)
        {
            // 1. key 가 null 이 아닌지 체크
            // 2. key 에 대한 HashCode와 BucketIndex 계산
            // 3. 해당 BucketIndex 를 가진 모든 Entry 순회
            // 4. 찾았을 시 지우려는 것이 첫 진입포인트면 (nextIndex = -1) Bucket의 EntryIndex 갱신 (-1 = EMPTY)
            //                            첫 진입포인트가 아니면 지우려는 Entry의 다음 EntryIndex 를 지우려는 Entry의 이전 Entry의 NextIndex에 대입...
            //    Entry 지움
            //    true 반환
            //    못찾았을 시 false 반환
            if(key == null)
                throw new ArgumentNullException("key is null");

            int hashCode = GetHash(key.ToString());
            int bucketIndex = hashCode % _buckets.Length;
            int prevEntryIndex = EMPTY;

            for (int i = _buckets[bucketIndex]; i >= 0; prevEntryIndex = i, i = _entries[i].NextIndex)
            {
                // 해시코드와 키가 전부 같을 시 이미 존재
                if (_entries[i].HashCode == hashCode && _entries[i].Key.Equals(key))
                {
                    if (prevEntryIndex == EMPTY)                    
                        _buckets[bucketIndex] = _entries[i].NextIndex;
                    else
                        _entries[prevEntryIndex].NextIndex = _entries[i].NextIndex;

                    _entries[i].HashCode = EMPTY;
                    _entries[i].Key = default;
                    _entries[i].Value = default;
                    _entries[i].NextIndex = EMPTY;
                    _freeFirstEntryIndex = i; // _entryies[i]를 재사용해야하기 때문에 기억
                    _freeCount++;

                    return true;
                }
            }

            return false;
        }   

        void Resize(int capacity)
        {
            int[] newBuckets = new int[capacity]; // capacity 의 크기만큼 newBuckets 생성

            for (int i = 0; i < newBuckets.Length; i++)
            {
                newBuckets[i] = EMPTY; // 생성한 newBuckets에 -1 을 할당해 유효하지 않은 인덱스값으로 표현
            }

            Entry[] newEntries = new Entry[capacity]; // capacity 만큼의 크기를 가진 Entry 생성
            Array.Copy(_entries, newEntries, _count); // 초기화한 newEntries에 _entries 할당

            for (int entryIndex = 0; entryIndex < _count; entryIndex++)
            {
                // 유효한 entry만 bucketIndex 갱신
                if (newEntries[entryIndex].isVaild)
                {
                    int bucketIndex = newEntries[entryIndex].HashCode % capacity; // 해당 HashCode가 가져가야하는 bucketIndex 다시 계산해줌
                    newEntries[entryIndex].NextIndex = newBuckets[bucketIndex]; // 버킷인덱스 갱신되었으므로 엔트리의 다음위치도 갱신
                    newBuckets[bucketIndex] = entryIndex; // 해당 HashCode가 가져야하는 값을 할당
                }
            }

            _buckets = newBuckets;
            _entries = newEntries;
        }

        // "GI" -> hash? 144
        // "FJ" -> hash? 144
        int GetHash(string name)
        {
            int hash = 0;

            for (int i = 0; i < name.Length; i++)
            {
                hash += name[i];
            }

            return hash;
        }
    }
}
