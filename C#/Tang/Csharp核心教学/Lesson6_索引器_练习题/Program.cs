namespace Lesson6_索引器_练习题
{
    class IntArray
    {
        private int[] array;
        //房间容量
        private int capacity;
        //当前用了多少容量
        private int length;

        public int Lenght
        {
            get
            {
                return length;
            }
        }

        public IntArray()
        {
            capacity = 5;
            length = 0;
            array = new int[capacity];
        }

        public void Add(int value)
        {
            //放最后一个东西的时候就扩容
            if (length < capacity)
            {
                array[length] = value;
                ++length;
            }
            //扩容搬家
            else
            {
                capacity *= 2;
                //临时的新房间
                int[] tempArray = new int[capacity];
                //东西临时放到新房间
                for (int i = 0; i < array.Length; i++)
                {
                    tempArray[i] = array[i];
                }
                array = tempArray;
            }
        }

        public void Remove(int value)
        {
            //先找到传入值在哪个位置
            for (int i = 0; i < length; i++)
            {
                if (array[i] == value)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (index > length - 1)
            {
                Console.WriteLine("当前数组只有{0}，你越界了", length);
                return;
            }
            for (int i = index; i < length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            --length;
        }

        public int this[int index]
        {
            get
            {
                if (index >= length || index < 0)
                {
                    Console.WriteLine("越界");
                    return 0;
                }
                return array[index];
            }
            set
            {
                if (index >= length || index < 0)
                {
                    Console.WriteLine("越界");
                }
                array[index] = value;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            IntArray array = new IntArray();
            array.Add(100);
            array.Add(200);
            Console.WriteLine(array.Lenght);
            Console.WriteLine(array[0]);
            Console.WriteLine(array[1]);
            array[0] = 500;
            Console.WriteLine(array[0]);
            array.RemoveAt(1);
            array.Remove(100);
            Console.WriteLine(array[0]);
            Console.WriteLine(array[1]);

        }
    }
}
