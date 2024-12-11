using System.Xml.Linq;

namespace DynamicArray
{
    internal class Node<T>
        where T : IComparable<T>
    {
        internal T Value;
        internal Node<T> Right;
        internal Node<T> Left;
        internal int Height;

        internal void RefreshHeight()
        {
            int leftHeight = 0;

            if (Left != null)
                leftHeight = Left.Height;

            int rightHeight = 0;

            if (Right != null)
                rightHeight = Right.Height;

            Height = Math.Max(leftHeight, rightHeight) + 1;
        }
    }

    internal class MyAVLTree<T>
        where T : IComparable<T>
    {
        Node<T> _root;

        public bool Contains(T value)
        {
            // 1. root 부터 시작
            // 2. 현재 탐색중인 노드의 값이 value 와 같은지 확인
            //      같으면 true
            //      작으면 오른쪽 노드로 탐색
            //      크면 왼쪽 노드로 탐색
            // 끝날때까지 2번 반복

            Node<T> tmp = _root;

            while (tmp != null)
            {
                if (value.Equals(tmp.Value))
                    return true;
                else if (value.CompareTo(tmp.Value) > 0)
                    tmp = tmp.Right;
                else
                    tmp = tmp.Left;
            }

            return false;
        }

        public void Add(T value)
        {
            _root = Add(_root, value);
        }

        /// <summary>
        /// 특정 노드에 자식을 추가하고 추가된 자식노드 반환 (재귀용 함수)
        /// </summary>
        /// <param name="node"> 부모가 될 노드 </param>
        /// <param name="value"> 자식으로 붙일 노드의 값</param>
        /// <returns> 자식으로 붙여진 노드 </returns>
        private Node<T> Add(Node<T> node, T value)
        {
            // 1. node 가 null 이면 빈자리 이므로 새 노드만들고 value 초기화 후 상위로 리턴
            // 2. node 의 값과 value 를 비교
            //      value 가 더 크면 node의 오른쪽자식에 대해 Add 재귀호출, 새로 추가된 노드를 node 의 오른쪽자식으로 대입.
            //      value 가 더작으면 node 의 왼쪽자식에 대해 Add 재귀호출, 새로 추가된 노드를 node 의 왼쪽자식으로 대입.
            // 3. node 의 height 계산. (왼쪽, 오른쪽 자식들의 height 중에서 가장 큰값에 + 1)
            // 4. 높이의 밸런스가 무너졌다면 리밸런싱을 수행

            if (node == null)
                return new Node<T> { Value = value, Height = 1 };

            int compare = value.CompareTo(node.Value);

            if (compare < 0)
                node.Left = Add(node.Left, value);
            else if (compare > 0)
                node.Right = Add(node.Right, value);

            //node.Height = 1 + Math.Max(node.Left?.Height ?? 0, node.Right?.Height ?? 0);
            node.RefreshHeight();
            int balance = CalcBalance(node);

            // 왼쪽으로 무너짐
            if (balance > 1)
            {
                // LR Case
                if (value.CompareTo(node.Left.Value) > 0)
                    node.Left = RotateLeft(node.Left);

                // LL Case
                return RotateRight(node);
            }
            // 오른쪽으로 무너짐
            else if (balance < -1)
            {
                // RL case
                if (value.CompareTo(node.Right.Value) < 0)
                    node.Right = RotateRight(node.Right);

                // RR Case
                return RotateLeft(node);
            }
            // 밸런스 맞음
            else
            {
                return node;
            }
        }

        public void Remove(T value)
        {
            _root = Remove(_root, value);
        }

        private Node<T> Remove(Node<T> node, T value)
        {
            // 지우려는 노드 못찾음
            if (node == null)
                return null;

            int compare = value.CompareTo(node.Value);

            // 삭제하려는 대상이 왼쪽에 있을거같다
            if (compare < 0)
            {
                node.Left = Remove(node.Left, value);
            }
            // 삭제하려는 대상이 오른쪽에 있을거같다
            else if (compare > 0)
            {
                node.Right = Remove(node.Right, value);
            }
            // 삭제하려는 대상을 찾았다.
            else
            {
                // 왼쪽없으면 오른쪽 그대로 올리면됨
                if (node.Left == null)
                {
                    return node.Right;
                }
                // 오른쪽 없으면 왼쪽 그대로 올리면됨
                else if (node.Right == null)
                {
                    return node.Left;
                }
                // 둘다있으면 오른쪽자식의 왼쪽자식 의 왼쪽자식의 왼쪽자식의... Leaf 노드 를 올리면됨
                else
                {
                    Node<T> tmp = node.Right;
                    Node<T> tmpParent = tmp;

                    while (tmp.Left != null)
                    {
                        tmpParent = tmp;
                        tmp = tmp.Left;
                    }

                    node.Value = tmp.Value;
                    node.Right = Remove(node.Right, tmp.Value); // 삭제한 Leaf 에 대한 리밸런싱
                }
            }

            node.RefreshHeight();
            int balance = CalcBalance(node);

            // 왼쪽으로 무너짐
            if (balance > 1)
            {
                // LR Case
                if (value.CompareTo(node.Left.Value) > 0)
                    node.Left = RotateLeft(node.Left);

                // LL Case
                return RotateRight(node);
            }
            // 오른쪽으로 무너짐
            else if (balance < -1)
            {
                // RL case
                if (value.CompareTo(node.Right.Value) < 0)
                    node.Right = RotateRight(node.Right);

                // RR Case
                return RotateLeft(node);
            }
            // 밸런스 맞음
            else
            {
                return node;
            }
        }

        /// <summary>
        /// 현재 노드의 자식들의 밸런스 계산
        /// </summary>
        /// <param name="node"></param>
        /// <returns> 음수 : 왼쪽으로 치우쳐짐, 양수 : 오른쪽으로 치우쳐짐 </returns>
        private int CalcBalance(Node<T> node)
        {
            int leftHeight = 0;

            if (node.Left != null)
                leftHeight = node.Left.Height;

            int rightHeight = 0;

            if (node.Right != null)
                rightHeight = node.Right.Height;

            return leftHeight - rightHeight;
        }

        /// <summary>
        /// 오른쪽으로 치우쳐진 브랜치를 왼쪽으로 회전하여 밸런스 맞춤
        /// </summary>
        /// <param name="node"> Branch 의 Root </param>
        /// <returns> 회전후 갱신된 Branch Root </returns>
        private Node<T> RotateLeft(Node<T> node)
        {
            if (node == null || node.Right == null)
                return node;

            Node<T> newRoot = node.Right;
            node.Right = newRoot.Left;
            newRoot.Left = node;
            node.RefreshHeight();
            newRoot.RefreshHeight();
            return newRoot;
        }

        /// <summary>
        /// 왼쪽으로 치우쳐진 브랜치를 오르쪽으로 회전하여 밸런스 맞춤
        /// </summary>
        /// <param name="node"> Branch 의 Root </param>
        /// <returns> 회전후 갱신된 Branch Root </returns>
        private Node<T> RotateRight(Node<T> node)
        {
            if (node == null || node.Left == null)
                return node;

            Node<T> newRoot = node.Left;
            node.Left = newRoot.Right;
            newRoot.Right = node;
            node.RefreshHeight();
            newRoot.RefreshHeight();
            return newRoot;
        }
    }

}
