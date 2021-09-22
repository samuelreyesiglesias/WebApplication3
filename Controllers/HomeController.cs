using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using static WebApplication3.Startup;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly OperationService OperationService_;
        private readonly IOperationTransient OperationTransient_;
        private readonly IOperationScoped OperationScoped_;
        private readonly IOperationSingleton OperationSingleton_;
        private readonly IOperationSingletonInstance OperationSingletonInstance_;
        public HomeController(ILogger<HomeController> logger, OperationService OService,IOperationTransient OTtransient,IOperationScoped OScope,IOperationSingleton OSingleton, IOperationSingletonInstance OSingletonInstance)
        {
            OperationService_ = OService;
            OperationTransient_ = OTtransient;
            OperationScoped_ = OScope;
            OperationSingleton_ = OSingleton;
            OperationSingletonInstance_ = OSingletonInstance;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // viewbag contains controller-requested services
            ViewBag.Transient = OperationTransient_;
            ViewBag.Scoped = OperationScoped_;
            ViewBag.Singleton = OperationSingleton_;
            ViewBag.SingletonInstance = OperationSingletonInstance_;


            // operation service has its own requested services
            ViewBag.Service = OperationService_;
            return View();
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
