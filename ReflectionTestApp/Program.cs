using System;
using System.Reflection;

namespace ReflectionTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // O reportName seria o conjunto recebido no input.
            string reportName = "Payable";

            // Aqui a gente deve tentar encontrar o relatório baseado no nome dele. Como não sabemos nesse momento se é um relatório de AP, AR ou Receita, vale a pena
            // abstrair esse trecho fazendo uma validação caso encontre o relatório em um dos 3 tipos. Senão, estora uma exception.
            Type type = Type.GetType($"ReflectionTestApp.Entities.{reportName}Report");
            if (type == null)
            {
                throw new Exception("Type not found.");
            }

            // Aqui entra a lógica de usar Reflection, podendo deixar a chamada mais genérica.
            IScheduleReport report = new ScheduleReport();
            MethodInfo method = typeof(IScheduleReport).GetMethod("Schedule");
            MethodInfo genericMethod = method.MakeGenericMethod(type);
            genericMethod.Invoke(report, new object[]
            {
                DateTime.Now, DateTime.Now.AddDays(1), "test.user@stone.com.br"
            });
        }
    }
}

namespace ReflectionTestApp
{
    public class ScheduleReport : IScheduleReport
    {
        public string Schedule<T>(DateTime startDate, DateTime endDate, string username)
        {
            Console.WriteLine($"The user {username} made a schedule execution for the {typeof(T)}, from {startDate} to {endDate}");
            return $"The user {username} made a schedule execution for the {typeof(T)}, from {startDate} to {endDate}";
        }
    }

    public interface IScheduleReport
    {
        string Schedule<T>(DateTime startDate, DateTime endDate, string username);
    }
}
