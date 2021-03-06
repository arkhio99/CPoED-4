﻿using System;
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

        static void DoSecondTask(StreamWriter output,double[] data, double[] s1, double[] errorsX,double alpha)
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
            double[] s1_01=new double[1];
            double[] errorsX_1_01=new double[1];
            DoSecondTask(output,data,s1_01,errorsX_1_01,alpha);
            
            alpha=0.3;
            double[] s1_03=new double[1];
            double[] errorsX_1_03=new double[1];
            DoSecondTask(output,data,s1_03,errorsX_1_03,alpha);

            

            
            
            
            
            }
        }
    }
}
