using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PWC.Models;

namespace PWC.Controllers
{
    public static class helpers 
    { 
        public static DateTime firstday(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMon = jan1.AddDays(daysOffset);
            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            if(firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            var result = firstMon.AddDays(weekOfYear * 7);
            return result;
        }
        public static string GetInitials(string name)
        {
            if(!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
            {
                string result;
                var nameSplit = name.ToUpper().Split(' ');
                if (nameSplit.Length == 1)
                {
                    result = nameSplit[0] != null ? nameSplit[0].Substring(0, 1) : "?";
                }
                else
                {
                    result = nameSplit[0].Substring(0, 1) + nameSplit[1].Substring(0, 1);
                }
                return result;
            }
            return "?";
        }
        public static int weekProj(DateTime d) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        public static int yearProj(DateTime d) => CultureInfo.CurrentCulture.Calendar.GetYear(d);
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Projects> newProject = new List<Projects>();
            var Project1 = new Projects
            {
                staff = "5",
                startDate = new DateTime(2020, 6, 14),
                dueDate = new DateTime(2020, 9, 17),
                manager = "Bada Irine",
                name = "Dump Workshop",
                unit = "Deep Consulting",
            };
           
            var Project2 = new Projects
            {
                staff = "3",
                startDate = new DateTime(2020, 7, 22),
                unit = "Business Analytics",
                dueDate = new DateTime(2020, 9, 16),
                manager = "Rachael Yemisi",
                name = "Optimization Flow"
            };
            var Project3 = new Projects
            {
                name = "Dump Workshop",
                startDate = new DateTime(2020, 6, 14),
                dueDate = new DateTime(2020, 10, 2),
                unit = "Deep Consulting",
                manager = "Bada Irine",
                staff = "6",

            };
            var Project4 = new Projects
            {

                name = "Fintech Analytics",
                startDate = new DateTime(2020, 6, 4),
                dueDate = new DateTime(2020, 9, 28),
                unit = "Business Analytics",
                manager = "Adele Olaluyi",
                staff = "2",
            };

            var Project6 = new Projects
            {
                name = "Project Audit",
                startDate = new DateTime(2020, 7, 10),
                dueDate = new DateTime(2020, 9, 22),
                unit = "Business Analytics",
                manager = "Adele Olaluyi",
                staff = "3",
            };
            var Project5 = new Projects
            {
                name = "BC Website",
                startDate = new DateTime(2020, 7, 7),
                dueDate = new DateTime(2020, 10, 5),
                unit = "Deep Consulting",
                manager = "Bada Irine",
                staff = "1"
            };
            var Project7 = new Projects
            {
                name = "Fraud Assessment",
                staff = "4",
                startDate = new DateTime(2020, 7, 14),
                unit = "Artificial Data",
                dueDate = new DateTime(2020, 9, 14),
                manager = "Demi Madaro",
            };


            newProject.Add(Project1);
            newProject.Add(Project2);
            newProject.Add(Project3);
            newProject.Add(Project4);
            newProject.Add(Project6);
            newProject.Add(Project7);
            newProject.Add(Project5);

            var ist = newProject.Where(c => c.dueDate >= new DateTime(2020, 9, 14) && c.dueDate <= new DateTime(2020, 9, 18)).Count();
            var second = newProject.Where(c => c.dueDate >= new DateTime(2020, 9, 21) && c.dueDate <= new DateTime(2020, 9, 25)).Count();
            var third = newProject.Where(c => c.dueDate >= new DateTime(2020, 9, 28) && c.dueDate <= new DateTime(2020, 10, 2)).Count();
            var fourth = newProject.Where(c => c.dueDate >= new DateTime(2020, 10, 5) && c.dueDate <= new DateTime(2020, 10, 9)).Count();

            List<PieChartRange> dataPoints = new List<PieChartRange>{
                new PieChartRange(ist, "14/09-18/09"),
                new PieChartRange(second, "21/09-25/09"),
                new PieChartRange(third, "26/09-02/10"),
                new PieChartRange(fourth, "05/10-09/10")
            };

          
            foreach(var item in newProject)
            {
                var firstdayofweek = helpers.firstday(helpers.yearProj(item.dueDate.GetValueOrDefault()), helpers.weekProj(item.dueDate.GetValueOrDefault())).AddDays(1);
                var lastdayOfWeek = helpers.firstday(helpers.yearProj(item.dueDate.GetValueOrDefault()), helpers.weekProj(item.dueDate.GetValueOrDefault())).AddDays(5);
                item.weekInWords = firstdayofweek.ToString("dddd MMM dd yyyy") + " - " + lastdayOfWeek.ToString("dddd MMM dd yyyy");
                item.avatar = helpers.GetInitials(item.name);
                item.unitSim = helpers.GetInitials(item.unit);
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);



            ViewBag.BA = newProject.Where(c => c.unit == "Business Analytics").Count();
            ViewBag.DC = newProject.Where(c => c.unit == "Deep Consulting").Count();
            ViewBag.AD = newProject.Where(c => c.unit == "Artificial Data").Count();

            //return View(group);
            return View(newProject.OrderBy(c => c.dueDate));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}




//var group = newProject.GroupBy(item => item.weekInWords).Select(group => new GroupProjets
//{ 
//    key = group.Key, 
//    Projects = group.ToList() 

//}).ToList();
//var groups = newProject.GroupBy(item => item.weekInWords).Select(group => new
//{
//    desc = group.Key,
//    items = group.ToList()

//}).ToList();
