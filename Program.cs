using System;
using System.IO;
using System.Linq;
namespace CPoED_4
{
    class Program
    {
        //delegate Tout myFunc<Tin,Tout>(Tin x);
        static Tout[] Mapper<Tin,Tout>(Tin[] ar, Func<Tin,Tout> f)//Функция для маппинга
        {
            int n=ar.Length;
            Tout[] res=new Tout[n];
            for(int i=0;i<n;i++)
                res[i]=f(ar[i]);
            return res;
        }
        static string pathInput=@"C:\Users\vladb\Desktop\Input.txt";
        static string pathOutput="out.txt";
        static double[] FileIntoArray(string path)
        {
            string s=File.ReadAllText(path);
            double[] res=Mapper(s.Split('\n'),t=>Convert.ToDouble(t));
            return res;
        }

        static double GetMathExpect(double[] ar)//Получение мат. ожидания
        {
            double res=0;
            double n=ar.Length;
            for(int i=0;i<n;i++)
            {
                res+=ar[i];
            }
            return res/n;
        }
        static double GetDispersion(double[] ar, double MathExpect)//Получение дисперсии
        {
            double res=0;
            double n=ar.Length;
            for(int i=0;i<n;i++)
                res+=(ar[i]-MathExpect)*(ar[i]-MathExpect);
            return res/n;
        }
        static double GeS1By_t(double alpha, double x,double s1Last)
        {
            return alpha*x+(1-alpha)*s1Last;
        }
        static void Main(string[] args)
        {
            using (var output = new StreamWriter(pathOutput))
            {
            output.WriteLine("1 пункт:");
            double[] data=FileIntoArray(pathInput);
            int n=data.Length;
            for(int i=0;i<n;i++)
                output.WriteLine($"{i+1} неделя: {data[i]}");
            double mathExpect=GetMathExpect(data);
            output.WriteLine($"Математическое ожидание: {mathExpect}");
            double dispersion=GetDispersion(data,mathExpect);
            output.WriteLine($"Дисперсия: {dispersion}");

            output.WriteLine("\n\n2 пункт:");
            double alpha =0.1;

            double[] s1=new double[n];
            for(int i=0;i<5;i++)
                s1[0]+=data[i];
            s1[0]/=5;
            for(int i=1;i<n;i++)
            {
                s1[i]=GeS1By_t(alpha,data[i],s1[i-1]);
            }
            output.WriteLine("Предсказанные значения:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{i+1} неделя: {s1[i]}");

            double[] errorsX_1=new double[n];
            for(int i=0;i<n;i++)
                errorsX_1[i]=s1[i]-data[i];
            output.WriteLine("\n\nОшибки:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{i+1} неделя: {errorsX_1[i]}");

            double dispersionOfErrors_1=GetDispersion(errorsX_1,GetMathExpect(errorsX_1));
            output.WriteLine($"Дисперсия: {dispersionOfErrors_1}");
            

            }
        }
    }
}
