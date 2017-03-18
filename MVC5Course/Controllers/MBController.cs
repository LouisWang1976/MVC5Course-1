using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MBController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Form1(FormCollection form)
        {
            return Content(form["Username"]);
        }

    }
}