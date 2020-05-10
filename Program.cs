using System;
using System.IO;
using System.Linq;
namespace CPoED_4
{
    class Program
    {
        delegate Tout myFunc<Tin,Tout>(Tin x);
        static Tout[] Mapper<Tin,Tout>(Tin[] ar, myFunc<Tin,Tout> f)
        {
            int n=ar.Length;
            Tout[] res=new Tout[n];
            for(int i=0;i<n;i++)
                res[i]=f(ar[i]);
            return res;
        }
        static string path=@"C:\Users\vladb\Desktop\Input.txt";
        static double[] FileIntoArray(string path)
        {
            string s=File.ReadAllText(path);
            double[] res=Mapper(s.Split('\n'),t=>Convert.ToDouble(t));
            return res;
        }
        static void Main(string[] args)
        {
            
        }
    }
}
