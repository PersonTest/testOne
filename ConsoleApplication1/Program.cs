using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            animal a = new animal();
            animal an = new cat();
            cat c = new cat();
            a.caneat();
            a.canlive();
            a.cansleep();
            Console.WriteLine("........................");
            an.caneat();
            an.canlive();
            an.cansleep();
            Console.WriteLine("........................");
            c.caneat();
            c.canlive();
            c.cansleep();
            Console.WriteLine("........................");
            Console.ReadKey();
        }
    }
    /// <summary>
    /// 基类方法
    /// </summary>
    public abstract class Biology
    {
        public virtual void canlive()
        {
            Console.WriteLine("can live");
        }
    }
    /// <summary>
    /// 派生类
    /// </summary>
    public class animal : Biology
    {
        public override void canlive()
        {
            Console.WriteLine("animal can live");
        }

        public void cansleep()
        {
            Console.WriteLine("animal can sleep");
        }
        public virtual void caneat()
        {
            Console.WriteLine("animal can eat");
        }
    }
    public class cat : animal
    {
        public override void caneat()
        {
            //base.caneat();
            Console.WriteLine("cat can eat");
        }

        public new void cansleep()
        {
            Console.WriteLine("cat can sleep");
        }
    }
}
