using CaseStudy_19_1_23_.Context;
using CaseStudy_19_1_23_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CaseStudy_19_1_23_.Controllers
{
    public class TransferController : Controller
    {

        public Cases_Context _Context= new Cases_Context();

        


        // GET: Transfer
       
        public ActionResult TransferData()
        {
            var pro= _Context.products.Include(s => s.subCategory).ToList();
            
            ViewModel vie =new ViewModel();
            vie.Lproducts= pro;
            vie.search = (string)TempData["s"];

            var jsondata= JsonConvert.SerializeObject(vie, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            var client = new HttpClient();
           
            string baseurl = "https://localhost:44304/api/";
            client.BaseAddress = new Uri(baseurl);

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsync("MyWebAPI/TransferData", content).Result;
            // Get data from Web API
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsAsync<ViewModel>().Result;
                if (responseData.Lproducts.ToList().Count() > 0)
                {
                    TempData["Products"] = responseData.Lproducts.ToList();
                    return RedirectToAction("UIndex", "Products");
                }
                return RedirectToAction("Search", "Products");
            }
            else
            {
                return RedirectToAction("Search","Products");
            }


        }


    }
}