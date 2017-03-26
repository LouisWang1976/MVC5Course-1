using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using PagedList;
using System.Web.UI;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    //[OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
    //[Authorize]
    public class ProductsController : BaseController
    {
        // GET: Products
        public ActionResult Index(string FilterActive, string sortBy, string keyword, int pageNo = 1)
        {
            //ViewBag.FilterActive = new SelectList(new List<string> { "True", "False" });

            var activeOptions = repoProduct.All().Select(p => p.Active.HasValue ? p.Active.Value.ToString() : "False").Distinct().ToList();

            ViewBag.FilterActive = new SelectList(activeOptions);


            DoSearchOnIndex(sortBy, keyword, pageNo);

            return View();
        }

        [HttpPost]
        public ActionResult Index(string FilterActive, Product[] data,string sortBy, string keyword, int pageNo = 1)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var prod = repoProduct.Find(item.ProductId);
                    prod.ProductName = item.ProductName;
                    prod.Price = item.Price;
                    prod.Stock = item.Stock;
                    prod.Active = item.Active;
                }
                repoProduct.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            //ViewBag.FilterActive = new SelectList(new List<string> { "True", "False" });

            var activeOptions = repoProduct.All().Select(p => p.Active.HasValue ? p.Active.Value.ToString() : "False").Distinct().ToList();

            ViewBag.FilterActive = new SelectList(activeOptions);

            DoSearchOnIndex(sortBy, keyword, pageNo);

            return View();
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult ProductOrderLines(int id)
        {
            Product product = repoProduct.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product.OrderLine);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                repoProduct.Add(product);
                repoProduct.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View= "Error_DbEntityValidationException", ExceptionType =typeof(DbEntityValidationException))]
        public ActionResult Edit(int id, FormCollection form)
        {
            var product = repoProduct.Find(id);
            if (TryUpdateModel(product, new string[] { "ProductName", "Stock", "Active" }))
            {
            }
            repoProduct.UnitOfWork.Commit();
            return RedirectToAction("Index");
            //return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = repoProduct.Find(id);
            repoProduct.Delete(product);
            repoProduct.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        private void DoSearchOnIndex(string sortBy, string keyword, int pageNo)
        {
            var all = repoProduct.All().AsQueryable();

            if (!String.IsNullOrEmpty(keyword))
            {
                all = all.Where(p => p.ProductName.Contains(keyword));
            }

            if (sortBy == "+Price")
            {
                all = all.OrderBy(p => p.Price);
            }
            else
            {
                all = all.OrderByDescending(p => p.Price);
            }

            ViewBag.keyword = keyword;

            ViewData.Model = all.ToPagedList(pageNo, 10);
        }

    }
}
