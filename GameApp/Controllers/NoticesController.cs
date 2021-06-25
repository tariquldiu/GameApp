using GameApp.Gateway;
using GameApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameApp.Controllers
{
    public class NoticesController : Controller
    {
        NoticeGateway aNoticeGateway = new NoticeGateway();
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Index()
        {
            return View();
        } 
        public ActionResult GetAllNotice()
        {
            var NoticeListDesc = aNoticeGateway.GetAllNotice().OrderByDescending(n=>n.NoticeId);

            return Json(NoticeListDesc, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Save()
        { 
            return View();
        }
        [HttpPost]
        public JsonResult Save(Notice n)
        {
            try
            {
                aNoticeGateway.SaveNotice(n);
                n.Message = "Notice Save Successfull.";
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
        public JsonResult DeleteNotice(int nId)
        {
            var isDeleted = aNoticeGateway.DeleteNotice(nId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}