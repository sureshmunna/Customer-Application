using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using WebApplication8.Models;
using static System.Net.WebRequestMethods;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
        
    {
        
        List<Customer> _customerList = new List<Customer>();
        Uri baseAddress = new Uri("http://192.168.0.33/api");
        HttpClient client;

        public HomeController()
        { 
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            
        }
        public ActionResult list()
        {
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Customer").Result;
            if (response.IsSuccessStatusCode) { 
                string data = response.Content.ReadAsStringAsync().Result;
                _customerList=JsonConvert.DeserializeObject<List<Customer>>(data);
            }
            return View(_customerList);
        }
        [HttpGet]
        public ActionResult register() 
        { 
           return View();
        }
        [HttpPost]
        public ActionResult register(Customer obj)
        {
            string data=JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data,Encoding.UTF8,"application/json");

            HttpResponseMessage response= client.PostAsync(client.BaseAddress+"/Customer",content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Customer customer = new Customer();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Customer/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                customer = JsonConvert.DeserializeObject<Customer>(data);
            }
            return View("register",customer);
        }
        [HttpPost]
        public ActionResult Edit(Customer obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Customer/"+obj.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list");
            }
            return View("register",obj);
        }
        
        public ActionResult  Delete(int id)
        {
                                           
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/Customer/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list");
            }
            return RedirectToAction("list");
        }
    }
}