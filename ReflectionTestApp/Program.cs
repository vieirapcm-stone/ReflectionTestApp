using ReflectionTestApp.Entities;
using System;
using System.Reflection;

namespace ReflectionTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string reportName = "Payable";

            Type type = Type.GetType($"ReflectionTestApp.Entities.{reportName}Report");
            if (type == null)
            {
                throw new Exception("Type not found.");
            }

            Console.WriteLine("Type" + type);
            var instance = Assembly.GetAssembly(type).CreateInstance(type.FullName);
            Console.WriteLine("Instance type: " + instance.GetType());

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
