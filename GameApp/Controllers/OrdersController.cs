using GameApp.Gateway;
using GameApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;


namespace GameApp.Controllers
{
    public class OrdersController : Controller
    {
        OrderGateway aOrderGateway = new OrderGateway();
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Index()
        {
            return View(); 
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult GetAllOrder()
        {
            var OrderListDesc = aOrderGateway.GetAllOrder().OrderByDescending(n=>n.OrderId);

            return Json(OrderListDesc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save()
        { 
            return View();
        }
       
        [HttpPost]
        public JsonResult Save(Order n)
        {
            int OrderId;
            try
            {
                if(n.OrderNo == null) { n.OrderNo = ""; }
                if(n.PlayerId == null) { n.PlayerId = ""; }
                if(n.AccountName == null) { n.AccountName = ""; }
                if(n.AccountPassword == null) { n.AccountPassword = ""; }
                if(n.AccountSecurityCode == null) { n.AccountSecurityCode = ""; }
                if(n.AccountType == null) { n.AccountType = ""; }
                if(n.OrderNote == null) { n.OrderNote = ""; }
                if(n.OrderNote == null) { n.OrderNote = ""; }

                OrderId = aOrderGateway.SaveOrder(n);
                n.Message = "Order Send Successfull.";
                n.OrderId = OrderId;
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(n, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult DeleteOrder(int nId)
        {
            var isDeleted = aOrderGateway.DeleteOrder(nId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}