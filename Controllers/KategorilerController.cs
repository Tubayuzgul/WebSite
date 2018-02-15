using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWebSite.Models;
using System.Net;

namespace MyWebSite.Controllers
{
    public class KategorilerController : Controller
    {
        private MyWebSiteEntities db = new MyWebSiteEntities();
        private UnitOfWork work = new UnitOfWork();
        // GET: Kategoriler Listeleme
        public ActionResult KategorilerIndex()
        {
           
                return View(db.Kategoriler.ToList());
          
                
        }
        // Kategori oluştuma GET
        public ActionResult KategorilerCreate()
        {
            return View();
        }
        //Kategori oluşturma Save kısmı
        [HttpPost]
        [ValidateAntiForgeryToken] // gelen istek kendi localimizden mi kontrol eder.Viewde @Html.AntiForgeryToken() yazılır
        public ActionResult KategorilerCreate([Bind(Include = "KategorId,KategoriBaslik,KategoriIcerik")] Kategoriler kategoriekle) //post işlemi ile modelden gelmesini istediimiz propert
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (kategoriekle.KategoriIcerik==null)
                    {
                        ModelState.AddModelError("KategoriIcerik", "İçerik boş geçilemez");
                        ModelState.AddModelError("", "Boş geçilemez"); //uyarıyı en üstte de göstersin diye
                    }
                    else
                    {
                        work.KategoriRepository.Insert(kategoriekle);
                        work.Save();
                        return RedirectToAction("KategorilerIndex");
                    }
                       
                  
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "İşlem başarısız oldu. Tekrar deneyiniz.");
            }
            return View(kategoriekle);
        }

        //Kategori Düzenleme
        public ActionResult KategorilerEdit(int? Id)
        {
            if (Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
                Kategoriler kategoriedit = work.KategoriRepository.FindById(Id);
                if (kategoriedit== null)
                {
                    return HttpNotFound();
                }
                return View(kategoriedit);
                        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult KategorilerEdit([Bind(Include = "KategorId,KategoriBaslik,KategoriIcerik")]Kategoriler kategoriedit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (kategoriedit.KategoriIcerik == null)
                    {
                        ModelState.AddModelError("KategoriIcerik", "İçerik boş geçilemez");
                        ModelState.AddModelError("", "Boş geçilemez"); //uyarıyı en üstte de göstersin diye
                    }
                    else
                    {
                        work.KategoriRepository.Update(kategoriedit);
                        work.Save();
                        return RedirectToAction("KategorilerIndex");
                    }
                   
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "İşlem Başarısz");
            }
            return View(kategoriedit);
        }


        public ActionResult KategrilerDelete(int? Id)
        {
            if (Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriSil = work.KategoriRepository.FindById(Id);
            return View(kategoriSil);
        }

        [HttpPost,ActionName("KategrilerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult KategrilerDeleteConfirmed(int Id)
        {
            Kategoriler kategoriSil = work.KategoriRepository.FindById(Id);
            work.KategoriRepository.Delete(kategoriSil);
            work.Save();
            return RedirectToAction("KategorilerIndex");
        }

        public ActionResult KategorilerDetails(int? Id)
        {
            if (Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoridetail = work.KategoriRepository.FindById(Id);
            if (kategoridetail==null)
            {
                return HttpNotFound();
            }
            return View(kategoridetail);
        }
         
        //işlem başarılıysa gönderilecek mesaj
        private void SuccesMessage(string mesaj)
        {
            TempData["SuccesMesage"] = mesaj;
        }
        //işlem başarısızsa gönderilecek mesaj
        private void ErorMessage()
        {
            TempData["ErorMessage"] = "İşlem sırasında hata oluştu";
        }
    }
}