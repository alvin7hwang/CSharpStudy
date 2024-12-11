using System.Reflection;

namespace DynamicArray
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // object 타입이 뭔가 ?
            // C# 의 모든 클래스의 기반 클래스. 모든 C# 타입은 object 타입으로 참조해서 쓸수있다.

            // object 타입 자료구조의 장점
            // 형식에 얽메이지않고 원하는 데이터를 모두 관리할 수 있다.

            // object 타입 자료구조의 단점
            // 값타입 데이터를 다뤄야 할때에는, boxing & unboxing 으로 인해 성능저하가 일어난다.

            // boxing 이 뭔가 ? 
            // C# 에서 '값타입' 은 구조체 기반이며 간단하게 object 타입으로 캐스팅이 안됨. 
            // 그래서 '값타입'도 object 타입으로 참조해서 사용할 수 있도록, 
            // 새 object 객체를 동적할당하고 원본값의 타입객체(System.Type)참조와 값을 가지고있게 함.
            
            // unboxing 이 뭔가 ?
            // boxing 된 object 객체에서 원본 값을 읽어오는 과정

            // * Reflection : 런타임중에 메타데이터에 접근하는 기능
            // * System.Type : 런타임중에 자료형에대한 데이터에 접근할 수 있는 객체 타입

            // C# 의 동적배열
            // ArrayList (object 기반)
            // List<T> (제네릭)
            // 다양한형태의 자료를 취급해야되는 상황 자체도 비효율적이며, boxing 으로인한 성능저하도 좋지않기떄문에
            // C# 문서에서는 Generic 타입을 사용할 것을 권장하고있다.
            MyDynamicArray myDynamicArray = new MyDynamicArray();
            myDynamicArray.Add(3);
            myDynamicArray.Add("철수");
            myDynamicArray.Add(4.0f);
            myDynamicArray.Add('A');
            myDynamicArray.Add('B');

            int index = myDynamicArray.FindIndex(3);
            Console.WriteLine(index);

            MyDynamicArray<int> myDynamicArrayOfInt = new MyDynamicArray<int>();
            myDynamicArrayOfInt.Add(3);
            myDynamicArrayOfInt.Add(2);
            myDynamicArrayOfInt.Add(6);

            // foreach 문은 Enumerator 로 Enumeration 을 수행하는 구문
            // 즉, 자료 순회를 위해서는 자료구조 객체를위한 Enumerator 를 구현하고, 그것을 반환받을수있는 함수가 필요함..
            // Enumerator 를 추상화한 인터페이스가 IEnumerator,
            // 그 Enumerator 를 반환받을수있는 함수를 추상화한 인터페이스가 IEnumerable .
            // -> 자료구조를위한 Enumerator 를 IEnumerator 를 상속받아 구현하고,
            // 자료구조타입은 IEnumerable 을 상속받아 GetEnumerator() 를 통해 외부에서 IEnumerator 에 접근할 수 있도록 구현함.
            foreach (int number in myDynamicArrayOfInt)
            {
                Console.WriteLine(number);
            }

            List<object> list = new List<object>();

            foreach (var item in list)
            {
                list.Add(1);
            }

            MyLinkedList<char> myLinkedListOfChar = new MyLinkedList<char>();
            myLinkedListOfChar.AddFirst('a');
            myLinkedListOfChar.AddLast('b');
            MyLinkedListNode<char> node = myLinkedListOfChar.FindLast(x => x == 'a');
            myLinkedListOfChar.AddAfter(node, 'c');

            foreach (var item in myLinkedListOfChar)
            {
                Console.WriteLine(item);
            }
        }
    }
}
