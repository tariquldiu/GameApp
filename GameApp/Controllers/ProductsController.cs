using GameApp.Gateway;
using GameApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameApp.Controllers
{
    public class ProductsController : Controller
    {
        ProductGateway aProductGateway = new ProductGateway();
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Index()
        {
            return View(); 
        } 
        public ActionResult GetAllProduct()
        {
            var ProductListDesc = aProductGateway.GetAllProduct().OrderByDescending(n=>n.ProductId);

            return Json(ProductListDesc, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Save()
        { 
            return View();
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult Save(Product n)
        {
            try
            {
                if(n.ProductType == null) { n.ProductType = ""; }
                aProductGateway.SaveProduct(n);
                n.Message = "Product Save Successfull.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(n, JsonRequestBehavior.AllowGet);
             
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult DeleteProduct(int nId)
        {
            var isDeleted = aProductGateway.DeleteProduct(nId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}