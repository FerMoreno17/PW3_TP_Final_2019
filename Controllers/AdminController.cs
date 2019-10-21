using PracticaEF.Data;
using PracticaEF.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TP_Final_2019_v._0.Controllers
{
    public class AdminController : Controller
    {
        AdministradorServicio admin = new AdministradorServicio();
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
            return View();

            }
        }
        
        [HttpGet]
        public ActionResult Perfil(int id)
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                var resp = admin.BuscarPorId(id);
                return View(resp);
            }
        }

        [HttpPost]
        public string PerfilMod(Usuarios u, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file != null && file.ContentLength>0 && file.ContentLength < 10200)
                {
                    var filename = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/users"), filename);
                    file.SaveAs(path);
                    u.Foto = filename;
                    admin.ModificarUsuario(u);
                }
                return "modelo valido: " + u.Apellido + u.Foto;
            }
            return "error";
        }


        public ActionResult SessionFinish()
        {
            Session["session"] = null;
            return RedirectToAction("../Index/Inicio");
        }
    }
}