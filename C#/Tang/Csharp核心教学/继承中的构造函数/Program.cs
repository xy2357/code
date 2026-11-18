namespace 继承中的构造函数
{
    class Worker
    {
        public string TypeOfJob
        {
            get;
            set;
        }
        public string JobDuties
        {
            get;
            set;
        }
        public Worker(string typeOfJob, string jobDuties)
        {
            TypeOfJob = typeOfJob;
            JobDuties = jobDuties;
        }

        public void Work()
        {
            Console.WriteLine("{0}在{1}", TypeOfJob, JobDuties);
        }
    }

    class Programmer : Worker
    {
        public Programmer() : base("程序员", "开发程序")
        {

        }
    }

    class Planner : Worker
    {
        public Planner() : base("策划", "写需求文档")
        {

        }
    }

    class Artist : Worker
    {
        public Artist() : base("美术", "画画")
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Programmer p1 = new Programmer();
            Planner p2 = new Planner();
            Artist p3 = new Artist();
            //Console.WriteLine("{0}在{1}",p1.TypeOfJob,p1.JobDuties);
            //Console.WriteLine("{0}在{1}", p2.TypeOfJob, p2.JobDuties);
            //Console.WriteLine("{0}在{1}", p3.TypeOfJob, p3.JobDuties);
            p1.Work();
            p2.Work();
            p3.Work();
        }
    }
}
