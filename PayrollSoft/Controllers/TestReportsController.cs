using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa;
using System.Web.Mvc;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{
    public class TestReportsController : Controller
    {
        private List<RotativaTest> _rotativaTest;

        public TestReportsController() 
        {
            var imagePath = "woman.jpg";

            _rotativaTest = new List<RotativaTest>()
            {
             new RotativaTest {CustomerId = 1, FirstName = "Jalpesh",
                               LastName = "Vadgama", ProfileImage = imagePath},
                new RotativaTest {CustomerId = 1, FirstName = "Vishal", 
                               LastName = "Vadgama", ProfileImage = imagePath}
            };
        }

    
        public ActionResult Index()
        {
            return View(_rotativaTest);
        }

        public ActionResult Print()
        {
            return new ActionAsPdf("Index", _rotativaTest);
        }

    }
}
