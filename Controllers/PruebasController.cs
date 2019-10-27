using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP_Final_2019_v._0.Controllers
{
    public class PruebasController : Controller
    {
        // GET: Pruebas
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.p = 33;
            return View();
        }

        [HttpPost]
        public string Index(Login l)
        {
            return "valor: " + l.Valor;
        }
    }
}