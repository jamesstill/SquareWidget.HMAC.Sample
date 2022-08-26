using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleUI.Models;
using SquareWidget.HMAC.Client.Core;

namespace SampleUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseAddress;
        private readonly ClientCredentials _credentials;
        private readonly HmacHttpClient _client;

        // use DI in a real app
        public HomeController()
        {
            _baseAddress = "https://localhost:5001";
            _credentials = new ClientCredentials
            {
                ClientId = "TestClient",
                ClientSecret = "P@ssw0rd"
            };

            _client = new HmacHttpClient(_baseAddress, _credentials);
        }

        public IActionResult Index()
        {
            var requestUri = "api/widgets";
            List<Widget> widgets;

            try
            {
                widgets = _client.Get<List<Widget>>(requestUri).Result ?? new List<Widget>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw;
            }

            return View(widgets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                var item = new Widget
                {
                    Name = collection["Name"],
                    Shape = collection["Shape"]
                };

                var requestUri = "api/widgets";
                item = _client.Post(requestUri, item).Result;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var requestUri = "api/widgets/" + id;
            var item = _client.Get<Widget>(requestUri).Result;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var item = new Widget
                {
                    ID = id,
                    Name = collection["Name"],
                    Shape = collection["Shape"]
                };

                var requestUri = "api/widgets/" + id;
                item = _client.Put(requestUri, item).Result;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var requestUri = "api/widgets/" + id;
            var item = _client.Get<Widget>(requestUri).Result;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var requestUri = "api/widgets/" + id;
                var statusCode = _client.Delete(requestUri).Result;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
