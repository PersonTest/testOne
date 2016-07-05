using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {
                Extendtest();
            }
            if (false)
            {
                testSplit();
            }
            int[] str = { 1, 2, 3, 4, 5 };
            foreach (var item in str)
            {
                Console.WriteLine(item);
                if (item == 3)
                {                    
                    break;
                }                
            }
            Console.WriteLine(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 06:00:00"));
            var guid = Guid.NewGuid();   
            Console.WriteLine(guid);
            if (false)
            {
                Tel();
            }            
            Console.ReadKey();
        }

        private static void Tel()
        {
            int[] arr = new int[] { 8, 2, 1, 0, 3 };
            int[] index = new int[] { 2, 0, 3, 2, 4, 0, 1, 3, 2, 3, 3 };
            string tel = "";
            foreach (var i in index)
            {
                tel += arr[i];
            }
            Console.WriteLine("telephone: " + tel + "");
        }

        private static void testSplit()
        {
            try
            {
                string filename = "2828156337_136741923.jpg";
                var prename = filename.Split('.')[0].ToString();
                Console.WriteLine(prename);


                var url = "https://bjs-s3-mdm-prod-community-app.s3.cn-north-1.amazonaws.com.cn/DEV/Community_ArticleWorkRole/2828156337_136741923.jpg";
                WebRequest request = (WebRequest)HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                var contentlength = response.ContentLength;
                var contenttype = response.ContentType;
            }
            catch (Exception ex)
            {
                throw ex;
            }





            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            //req.Method = "GET";
            //req.ContentType = "image/jpeg";
            //req.Headers.Add("Authorization", "BD3BEE48-F58D-4997-BAEF-73B3AC2FDF68");
            //Stream stream = req.GetRequestStream();

            Console.ReadKey();
        }

        private static void Extendtest()
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
        public abstract void canSee();
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

        public override void canSee()
        {
            throw new NotImplementedException();
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
