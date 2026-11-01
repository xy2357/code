namespace Lesson5_属性_练习题
{
    class Student
    {
        string name;
        string sex;
        int age;
        int cSharpGrade;
        int unityGrade;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Sex
        {
            get
            {
                return sex;
            }
            set
            {
                if (value != "男" && value != "女")
                {
                    sex = "不男不女";
                }
                else
                {
                    sex = value;
                }
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                if (value > 150)
                {
                    age = 150;
                }
                else if (value < 0)
                {
                    age = 0;
                }
                else
                {
                    age = value;
                }
            }
        }

        public int CSharpGrade
        {
            get
            {
                return cSharpGrade;
            }
            set
            {
                if (value > 100)
                {
                    cSharpGrade = 100;
                }
                else if (value < 0)
                {
                    cSharpGrade = 0;
                }
                else
                {
                    cSharpGrade = value;
                }
            }
        }

        public int UnityGrade
        {
            get
            {
                return unityGrade;
            }
            set
            {
                if (value > 100)
                {
                    unityGrade = 100;
                }
                else if (value < 0)
                {
                    unityGrade = 0;
                }
                else
                {
                    unityGrade = value;
                }
            }
        }


        public void Speak()
        {
            Console.WriteLine("我叫{0},今年{1}岁,是一个{2}生", name, age, sex);
        }

        public void CalGrade()
        {
            int sum = cSharpGrade + unityGrade;
            float avg = sum / 2f;
            Console.WriteLine("总分是{0}，平均分是{1}", sum, avg);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Student s1 = new Student();
            s1.Name = "小李";
            s1.Age = 18;
            s1.Sex = "男";
            s1.CSharpGrade = 100;
            s1.UnityGrade = 10;
            s1.Speak();
            s1.CalGrade();
            Student s2 = new Student();
            s2.Speak();
            s2.CalGrade();
        }
    }
}
