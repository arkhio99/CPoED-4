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

        static double GetDispersionOfErrors(double[] s1,double n)// n - порядок полинома 
        {
            double res=0;
            double N=s1.Length;
            double mathExpect=GetMathExpect(s1);
            for(int i=0;i<N;i++)
                res+=(s1[i]-mathExpect)*(s1[i]-mathExpect);
            return res/(N-n-1);
        }

        static void DoSecondTask(StreamWriter output,double[] data,ref double[] s1,ref double[] errorsX,double alpha)
        {
            output.WriteLine($"\nАльфа: {alpha:f1}");
            int n=data.Length;
            s1=new double[n];
            for(int i=0;i<5;i++)
                s1[0]+=data[i];
            s1[0]/=5;
            for(int i=1;i<n;i++)
            {
                s1[i]=GeS1By_t(alpha,data[i],s1[i-1]);
            }
            output.WriteLine("Предсказанные значения:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{i+1} неделя: {s1[i]:f3}");

            double[] errorsX_1=new double[n];
            for(int i=0;i<n;i++)
                errorsX_1[i]=s1[i]-data[i];
            output.WriteLine("\n\nОшибки:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{i+1} неделя: {errorsX_1[i]:f3}");

            double dispersionOfErrors_1=GetDispersionOfErrors(s1,0);
            output.WriteLine($"Дисперсия: {dispersionOfErrors_1}");
        }
        
        static double SumWithFunc(double[] x,Func<double,double> f)
        {
            double res=0;
            int n=x.Length;
            for(int i=0;i<n;i++)
            {
                res+=f(x[i]);
            }
            return res;
        }
        static void DoFourthTask(double[] data,ref double[] s1,ref double[] s2,ref double[] a0,ref double[] a1, double alpha, double _a0, double _a1)
        {
            int n=data.Length;
            a0=new double[n];
            a1=new double[n];
            a0[0]=_a0;
            a1[0]=_a1;
            double beta=1-alpha;
            for(int i=1;i<n;i++)
            {
                a0[i]=data[i]+beta*beta*(a0[i-1]+a1[i-1]-data[i]);
                a1[i]=a1[i-1]+alpha*alpha*(a0[i-1]+a1[i-1]-data[i]);
            }
            s1=new double[n];
            s2=new double[n];
            for(int i=0;i<n;i++)
            {
                s1[i]=a0[i]-(beta/alpha)*a1[i];
                s2[i]=a0[i]-(2*beta/alpha)*a1[i];
            }
        }
        static void Main(string[] args)
        {
            // названия переменных в формате <имя>_<альфа>_<пункт>
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
            double[] s1_01_2=new double[0];
            double[] errorsX_01_2=new double[0];
            DoSecondTask(output,data,ref s1_01_2,ref errorsX_01_2,alpha);
            
            alpha=0.3;
            double[] s1_03_2=new double[0];
            double[] errorsX_03_2=new double[0];
            DoSecondTask(output,data,ref s1_03_2,ref errorsX_03_2,alpha);

            
            output.WriteLine("\n\n3 пункт:");
            double[] t=new double[n];
            for(int i=0;i<n;i++)
            t[i]=i+1;

            double sumOft=SumWithFunc(t,t=>t);
            double sumOfx=SumWithFunc(data,t=>t);
            double sumofSqrsOft=SumWithFunc(t,t=>t*t);
            double sumOfxt=0;
            for(int i=0;i<n;i++)
                sumOfxt+=data[i]*t[i];
            double _a1=(n*sumOfxt-sumOft*sumOfx)/(n*sumofSqrsOft-sumOft*sumOft);
            double _a0=(sumOfx-_a1*sumOft)/n;
            string s=$"X(t)={_a1:f2}*t";
            s+=_a0<0?$"{_a0:f2}":$"+{_a0:f2}";
            output.WriteLine(s);
            
            output.WriteLine("\n\n4 пункт:");
            alpha=0.1;
            double[] s1_01_4=new double[0];
            double[] s2_01_4=new double[0];
            double[] a0_01_4=new double[0];
            double[] a1_01_4=new double[0];
            DoFourthTask(data,
                        ref s1_01_4,
                        ref s2_01_4,
                        ref a0_01_4,
                        ref a1_01_4,
                        alpha,_a0,_a1);
            output.WriteLine("Для альфа, равного 0.1:\nS1:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{s1_01_4[i]:f2}");
            output.WriteLine("\nS2:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{s2_01_4[i]:f2}");
            alpha=0.3;
            double[] s1_03_4=new double[0];
            double[] s2_03_4=new double[0];
            double[] a0_03_4=new double[0];
            double[] a1_03_4=new double[0];
            DoFourthTask(data,
                        ref s1_03_4,
                        ref s2_03_4,
                        ref a0_03_4,
                        ref a1_03_4,
                        alpha,_a0,_a1);
            output.WriteLine("\nДля альфа, равного 0.3:\nS1:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{s1_03_4[i]:f2}");
            output.WriteLine("\nS2:");
            for(int i=0;i<n;i++)
                output.WriteLine($"{s2_03_4[i]:f2}");

            
            }
        }
    }
}
