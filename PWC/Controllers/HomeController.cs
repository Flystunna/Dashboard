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
                unitSim = "DC",
                dueDate = new DateTime(2020, 8, 17),
                manager = "Bada Irine",
                name = "Dump Workshop",
                avatar = "DW",
                unit = "Deep Consulting",
            };
           
            var Project2 = new Projects
            {
                staff = "3",
                startDate = new DateTime(2020, 7, 22),
                unitSim = "BA",
                unit = "Business Analytics",
                dueDate = new DateTime(2020, 8, 16),
                manager = "Rachael Yemisi",
                avatar = "OF",
                name = "Optimization Flow"
            };
            var Project3 = new Projects
            {
                name = "Dump Workshop",
                startDate = new DateTime(2020, 6, 14),
                dueDate = new DateTime(2020, 9, 2),
                unitSim = "DC",
                unit = "Deep Consulting",
                avatar = "DW",
                manager = "Bada Irine",
                staff = "6",

            };
            var Project4 = new Projects
            {

                name = "Fintech Analytics",
                startDate = new DateTime(2020, 6, 4),
                dueDate = new DateTime(2020, 8, 28),
                unitSim = "BA",
                unit = "Business Analytics",
                avatar = "FA",
                manager = "Adele Olaluyi",
                staff = "2",
            };

            var Project6 = new Projects
            {
                name = "Project Audit",
                startDate = new DateTime(2020, 7, 10),
                dueDate = new DateTime(2020, 8, 22),
                unitSim = "BA",
                unit = "Business Analytics",
                manager = "Adele Olaluyi",
                staff = "3",
                avatar = "PA"
            };
            var Project5 = new Projects
            {
                name = "BC Website",
                avatar = "BW",
                startDate = new DateTime(2020, 7, 7),
                dueDate = new DateTime(2020, 9, 5),
                unitSim = "DC",
                unit = "Deep Consulting",
                manager = "Bada Irine",
                staff = "1"
            };
            var Project7 = new Projects
            {
                name = "Fraud Assessment",
                staff = "4",
                startDate = new DateTime(2020, 7, 14),
                unitSim = "AD",
                unit = "Artificial Data",
                dueDate = new DateTime(2020, 8, 14),
                manager = "Demi Madaro",
                avatar = "FA",
            };


            newProject.Add(Project1);
            newProject.Add(Project2);
            newProject.Add(Project3);
            newProject.Add(Project4);
            newProject.Add(Project6);
            newProject.Add(Project7);
            newProject.Add(Project5);

            var ist = newProject.Where(c => c.dueDate >= new DateTime(2020, 8, 14) && c.dueDate <= new DateTime(2020, 8, 18)).Count();
            var second = newProject.Where(c => c.dueDate >= new DateTime(2020, 8, 21) && c.dueDate <= new DateTime(2020, 8, 25)).Count();
            var third = newProject.Where(c => c.dueDate >= new DateTime(2020, 8, 28) && c.dueDate <= new DateTime(2020, 9, 2)).Count();
            var fourth = newProject.Where(c => c.dueDate >= new DateTime(2020, 9, 5) && c.dueDate <= new DateTime(2020, 9, 9)).Count();

            List<PieChartRange> dataPoints = new List<PieChartRange>{
                new PieChartRange(ist, "14/08-18/08"),
                new PieChartRange(second, "21/08-25/08"),
                new PieChartRange(third, "26/08-02/09"),
                new PieChartRange(fourth, "05/09-09/09")
            };

          
            foreach(var item in newProject)
            {
                var firstdayofweek = helpers.firstday(helpers.yearProj(item.dueDate.GetValueOrDefault()), helpers.weekProj(item.dueDate.GetValueOrDefault()));
                var lastdayOfWeek = helpers.firstday(helpers.yearProj(item.dueDate.GetValueOrDefault()), helpers.weekProj(item.dueDate.GetValueOrDefault())).AddDays(5);
                item.weekInWords = firstdayofweek.ToString("dd-MM-yyyy") + " - " + lastdayOfWeek.ToString("dd-MM-yyyy");
            }  

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            ViewBag.BA = newProject.Where(c => c.unit == "Business Analytics").Count();
            ViewBag.DC = newProject.Where(c => c.unit == "Deep Consulting").Count();
            ViewBag.AD = newProject.Where(c => c.unit == "Artificial Data").Count();


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
