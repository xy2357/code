namespace 属性索引器相关练习
{
    public class Cls
    {
        private int _id;
        private Student[] _students;

        public Cls(int id, Student[] students)
        {
            Id = id;
            Students = students;
        }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public Student[] Students
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
            }
        }
        public Student this[int index]
        {
            get
            {
                return _students[index];
            }
            set
            {
                _students[index] = value;
            }
        }

        public void AddStudent(Student student)
        {
            Student[] tempStudents = new Student[_students.Length + 1];
            for (int i = 0; i < tempStudents.Length; i++)
            {
                tempStudents[i] = _students[i];
            }
            tempStudents[tempStudents.Length - 1] = student;
            _students = tempStudents;
        }

        public void DelStudent(int index)
        {
            if (_students.Length < 1)
            {
                Console.WriteLine("没有学生数据");
                return;
            }
            if (index < 0 || index > _students.Length)
            {
                Console.WriteLine("学生下标超范围");
                return;
            }
            Student[] tempStudents = new Student[_students.Length - 1];
            for (int i = 0; i < index; i++)
            {
                tempStudents[i] = _students[i];
            }
            for (int i = index; i < tempStudents.Length; i++)
            {
                tempStudents[i] = _students[i + 1];
            }
            _students = tempStudents;
        }
    }

    public class Student
    {
        private string _name;
        private int _age;
        private int _mathScore;
        private int _englishScore;

        public Student(string name, int age, int mathScore, int englishScore)
        {
            Name = name;
            Age = age;
            MathScore = mathScore;
            EnglishScore = englishScore;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value <= 0)
                {
                    _age = 1;
                }
                else if (value > 150)
                {
                    _age = 150;
                }
                else
                {
                    _age = value;
                }
            }
        }
        public int MathScore
        {
            get
            {
                return _mathScore;
            }
            set
            {
                if (value < 0)
                {
                    _mathScore = 0;
                }
                else if (value > 100)
                {
                    _mathScore = 100;
                }
                else
                {
                    _mathScore = value;
                }    
            }
        }
        public int EnglishScore
        {
            get
            {
                return _englishScore;
            }
            set
            {
                if (value < 0)
                {
                    _englishScore = 0;
                }
                else if (value > 100)
                {
                    _englishScore = 100;
                }
                else
                {
                    _englishScore = value;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Student s1 = new Student("小李",-10,-10,1000);
            Console.WriteLine(s1.Name);
            Console.WriteLine(s1.Age);
            Console.WriteLine(s1.MathScore);
            Console.WriteLine(s1.EnglishScore);
            Student[] students = new Student[] { s1 };
            Cls c1 = new Cls(1, students);
            Console.WriteLine(c1.Students[0].Name);
            Console.WriteLine(c1.Students[0].Age);
            Console.WriteLine(c1.Students[0].MathScore);
            Console.WriteLine(c1.Students[0].EnglishScore);
            Student s2 = new Student("小红", 18, 99, 99);
            c1.AddStudent(s2);
            Console.WriteLine(c1.Students[1].Name);
            c1.DelStudent(0);
            Console.WriteLine(c1.Students[0].Name);
        }
    }
}
