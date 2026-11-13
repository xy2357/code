namespace Lesson9_拓展方法
{
    class Student
    {
        private int mathScore;
        private int englishScore;

        public Student(int mathScore, int englishScore)
        {
            MathScore = mathScore;
            EnglishScore = englishScore;
        }

        public int MathScore
        {
            get
            {
                return mathScore;
            }

            set
            {
                mathScore = value;
            }
        }

        public int EnglishScore
        {
            get
            {
                return englishScore;
            }
            set
            {
                englishScore = value;
            }
        }

    }

    static class StudentExtension
    {
        public static int AddScore(this Student s,Student s2)
        {
            return s.MathScore + s.EnglishScore + s2.MathScore + s2.EnglishScore;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Student s = new Student(80, 90);
            Student s2 = new Student(20, 10);
            Console.WriteLine(s.MathScore);
            Console.WriteLine(s.EnglishScore);
            Console.WriteLine(s.AddScore(s2));
            Console.WriteLine(StudentExtension.AddScore(s, s2));
        }


    }
}
