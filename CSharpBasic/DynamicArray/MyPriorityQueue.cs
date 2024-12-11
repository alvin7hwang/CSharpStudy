using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DynamicArray
{
    internal class MyPriorityQueue<T>
        where T : IComparable<T>
    {
        internal MyPriorityQueue()
        {
            _heap = new List<T>();
        }

        internal MyPriorityQueue(int capacity)
        {
            _heap = new List<T>(capacity);
        }

        List<T> _heap;

        internal T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty");

            return _heap[0];
        }

        internal void Enqueue(T value)
        {
            // 1. 일단 heap 제일 마지막에 집어넣는다
            // 2. SIFT Up 으로 올리면서 정렬한다.

            _heap.Add(value);
            SIFTUp(_heap.Count - 1);
        }

        internal T Dequeue()
        {
            // 1. 루트 값 기억
            // 2. 루트 위치에 마지막 값 대입
            // 3. 마지막 아이템 삭제
            // 4. 기억했던 최우선순위 값 반환

            if (_heap.Count == 0)
                throw new InvalidOperationException("Is empty...");

            T rootValue = _heap[0];
            int end = _heap.Count - 1;
            _heap[0] = _heap[end];
            _heap.RemoveAt(end);
            SIFTDown(0);
            return rootValue;
        }

        void HeapifyTopDown()
        {
            int current = 1;

            while (current < _heap.Count)
            {
                SIFTUp(current++);
            }
        }

        void HeapifyBottonUp()
        {
            int current = _heap.Count - 1;

            while (current >= 0)
            {
                SIFTDown(current--);
            }
        }

        void SIFTUp(int current)
        {
            int parent = (current - 1) / 2; // 부모노드 인덱스 계산

            // 정렬하려는 노드의 인덱스가 루트보다 크다면 반복
            while (current > 0)
            {
                if (_heap[current].CompareTo(_heap[parent]) > 0)
                {
                    SwapByIndex(ref current, ref parent);
                    parent = (current - 1) / 2; // <- 추가해주삼 
                }
                else
                {
                    break;
                }
            }
        }

        void SIFTDown(int current)
        {
            int leftChild = (current * 2) + 1;

            while (leftChild < _heap.Count)
            {
                int priorityChild = leftChild;
                int rightChild = leftChild + 1;

                // 오른쪽 자식이 존재하면서 왼쪽자식보다 더 우세하다면 오른쪽자식을 비교대상으로 삼음
                if (rightChild < _heap.Count &&
                    _heap[rightChild].CompareTo(_heap[leftChild]) > 0)
                {
                    priorityChild = rightChild;
                }

                // current 값과 child 값을 비교해서 current 가 더 작으면(우선순위가 낮으면) 스왑.
                if (_heap[current].CompareTo(_heap[priorityChild]) < 0)
                {
                    SwapByIndex(ref current, ref priorityChild);
                    leftChild = (current * 2) + 1;
                }
                else
                {
                    break;
                }
            }
        }

        void SwapByIndex(ref int index1, ref int index2)
        {
            T tmp = _heap[index1];
            _heap[index1] = _heap[index2];
            _heap[index2] = tmp;

            int tmpIdx = index1;
            index1 = index2;
            index2 = tmpIdx;
        }
    }
}
