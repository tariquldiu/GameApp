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
    public class GameTopupsController : Controller
    {
        GameTopupGateway aGameTopupGateway = new GameTopupGateway();
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Index()
        {
            return View(); 
        } 
        public ActionResult GetAllGameTopup()
        {
            var GameTopupListDesc = aGameTopupGateway.GetAllGameTopup().OrderByDescending(n=>n.GameTopupId);

            return Json(GameTopupListDesc, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Save()
        { 
            return View();
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult SaveFiles(string description)
        {
            GameTopup gameTopup = new GameTopup();
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Request.Files != null)
            {
                var file = Request.Files[0];
                actualFileName = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;

                try
                {
                    file.SaveAs(Path.Combine(Server.MapPath("~/Scripts/UploadedFiles"), fileName));
                    var gameTopupId = Session["GameTopupId"];
                    if (gameTopupId != null)
                    {
                        gameTopup.GameTopupId = (int)gameTopupId;
                        gameTopup.ImageUrl = fileName;
                        aGameTopupGateway.UpdateGameTopupImagePath(gameTopup);
                    }
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }

            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult Save(GameTopup n)
        {
            int gameTopupId;
            try
            {
                if (n.PrePurchaseMessage == null) { n.PrePurchaseMessage = ""; }
                if (n.OfferName == null) { n.OfferName = ""; }

                gameTopupId = aGameTopupGateway.SaveGameTopup(n);
                n.Message = "Topup Save Successfull.";
                n.GameTopupId = gameTopupId;
                Session["GameTopupId"] = gameTopupId;
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(n, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public JsonResult DeleteGameTopup(int nId)
        {
            var isDeleted = aGameTopupGateway.DeleteGameTopup(nId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}