using Brights.TestTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
                model.Title = GetWebPageTitle(urlString);
                return model;
            }
            catch
            {
                return null;
            }
        }

        private string GetWebPageTitle(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest.Create(url) as HttpWebRequest);
                HttpWebResponse response = (request.GetResponse() as HttpWebResponse);

                using (Stream stream = response.GetResponseStream())
                {
                    // regexp to check for <title></title> block
                    Regex titleCheck = new Regex(@"<title>\s*(.+?)\s*</title>",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    //buffer max size 
                    int bytesToRead = 5000;
                    byte[] buffer = new byte[bytesToRead];
                    
                    //fill buffer with response data
                    stream.Read(buffer, 0, bytesToRead);

                    string contents = Encoding.UTF8.GetString(buffer, 0, bytesToRead);

                    Match match = titleCheck.Match(contents);

                    string title = string.Empty;
                    if (match.Success)
                    {
                        // found a <title></title> match
                        title = match.Groups[1].Value.ToString();
                    }
                    else if (contents.Contains("</head>") || string.IsNullOrWhiteSpace(title))
                    {
                        //no title found
                        title = "[Tag 'title' wasn't found in current URL]";
                    }
                    return title;
                }
            }
            catch
            {
                return "Failed to find title of current URL";
            }
        }
    }
}