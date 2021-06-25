using GameApp.Gateway;
using GameApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameApp.Controllers
{
    public class PaymentGatewaysController : Controller
    {
        PaymentGatewayGateway aPaymentGateway = new PaymentGatewayGateway();
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Index()
        {
            return View();
        } 
        public ActionResult GetAllPaymentGateway()
        {
            var paymentGatewayListDesc = aPaymentGateway.GetAllPaymentGateway().OrderByDescending(p=>p.GatewayId);

            return Json(paymentGatewayListDesc, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Save()
        { 
            return View();
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult Save(PaymentGateway n)
        {
            try
            {
                aPaymentGateway.SavePaymentGateway(n);
                n.Message = "Payment Gateway Save Successfull.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(n, JsonRequestBehavior.AllowGet);
             
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Edit() 
        {
            return View();
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult DeletePaymentGateway(int nId)
        {
            var isDeleted = aPaymentGateway.DeletePaymentGateway(nId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}