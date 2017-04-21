using SeeNowProcess.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    /// <summary>
    /// Base controller for controllers wchich use context (enables DI)
    /// </summary>
    public class DIContextBaseController : Controller
    {
        protected ISeeNowContext db = new SeeNowContext();

        public DIContextBaseController() { }

        public DIContextBaseController(ISeeNowContext context)
        {
            db = context;
        }   
    }
}