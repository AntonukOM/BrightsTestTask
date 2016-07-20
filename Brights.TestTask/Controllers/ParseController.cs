using Brights.TestTask.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Brights.TestTask.Controllers
{
    public class ParseController : Controller
    {
        // GET: Parse
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTitleList(string urlList)
        {
            string[] parsedList = urlList.Split(new char[] { '\n', ';', ' ', '\t', ',' });
            List<UrlModel> modelList = new List<UrlModel>();
            for (int i = 0; i < parsedList.Length; ++i)
            {
                if (string.IsNullOrWhiteSpace(parsedList[i]))
                {
                    continue;
                }
                var model = GetModel(parsedList[i]);
                if (model != null)
                {
                    modelList.Add(model);
                }
            }
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create correct format model
        /// </summary>
        /// <param name="urlString">Except any string</param>
        /// <returns>UrlModel or null if parse fail</returns>
        private UrlModel GetModel(string urlString)
        {           
            try
            {                
                //get uri from arrived string
                var uri = new Uri(urlString);

                var model = new UrlModel();
                //set required url
                model.Url = uri.Scheme + @"://" + uri.Host;

                //find title
                string[] hostArray = uri.Host.Split('.');
                int index = 0;
                                
                if (hostArray.Length > 2)
                {
                    index = 1;
                }
                //set title and make first letter uppercase
                model.Title = char.ToUpper(hostArray[index][0]) + hostArray[index].Substring(1);
                return model;
            }
            catch
            {
                return null;
            }
        }
    }
}