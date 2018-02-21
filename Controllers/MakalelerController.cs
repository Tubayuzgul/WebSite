using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWebSite.Models;
using System.IO;

namespace MyWebSite.Controllers
{
    public class MakalelerController : Controller
    {
        MyWebSiteEntities db = new MyWebSiteEntities();
        UnitOfWork work = new UnitOfWork();
        #region Listeleme
        // GET: Makaleler
        //Makaleler Listeleme Actionu
        //Include ile Kategoriler tablosuna join yapılmış oluyor
        public ActionResult MakalelerIndex()
        {
            var makaleler = db.Makaleler.Include(m => m.Kategoriler);

            return View(makaleler);
        }
        #endregion
        #region Ekleme
        // Makaleler get actionu
        //model boş olarak viewe gönderilecek.Bağlı olduğu kategoriler dropdowna gelsin diye viewbag ile kategorileri gösterdik.
        //kategorileri dropdownda gösterirken arka kısımda valueda KategoriId yi ön yüzde KategoriBaslik gösterilecek.
        public ActionResult MakalelerCreate()
        {
            ViewBag.MakaleKategoriId = new SelectList(db.Kategoriler, "KategorId", "KategoriBaslik");
            
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakalelerCreate([Bind(Include = "MakaleId,MakaleBaslik,MakaleIcerik,MakaleKategoriId,ResimUrl,Kategoriler")] Makaleler makalecreate,HttpPostedFileBase uploadfile)
        {
            try
            {
                //resim yükleme
                if (uploadfile.ContentLength>0 && uploadfile!=null)
                {
                    ViewBag.ResimHatsi = "";
                    string resimGuid = Guid.NewGuid().ToString();
                    string FilePath = Path.Combine(Server.MapPath("~/Content/Images"), resimGuid + Path.GetFileName(uploadfile.FileName));
                    uploadfile.SaveAs(FilePath);
                    makalecreate.ResimUrl = resimGuid + Path.GetFileName(uploadfile.FileName);
                }



                if (ModelState.IsValid)
                {
                    if (makalecreate.MakaleBaslik == null)
                    {
                        ModelState.AddModelError("MakaleBaslik", "Makale Başlığı boş geçilemez");

                    }
                    if (makalecreate.MakaleKategoriId == null)
                    {
                        ModelState.AddModelError("MakaleKategoriId", "Lütfen kategori seçiniz");
                    }
                    else
                    {
                        
                        work.MakaleRepository.Insert(makalecreate);
                       
                        work.Save();
                        return RedirectToAction("MakalelerIndex");
                    }
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "İşlem başarısız oldu tekrar deneyiniz");
            }
            ViewBag.MakaleKategoriId = new SelectList(db.Kategoriler, "KategorId", "KategoriBaslik", makalecreate.MakaleKategoriId);

            return View();
        }
        #endregion

        public ActionResult MakalelerEdit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makaleler makaleEdit = work.MakaleRepository.FindById(Id);
            if (makaleEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakaleKategoriId = new SelectList(db.Kategoriler, "KategorId", "KategoriBaslik", makaleEdit.MakaleKategoriId);
            return View(makaleEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakalelerEdit([Bind(Include = "MakaleId,MakaleBaslik,MakaleIcerik,MakaleKategoriId,Kategoriler")] Makaleler makalelerEdit)
        {
            if (ModelState.IsValid)
            {
                if (makalelerEdit.MakaleKategoriId==null)
                {
                    ModelState.AddModelError("MakaleKategoriId", "Kategori boş geçilemez");
                    ModelState.AddModelError("", "Tüm alanları kontrol edin");
                }
                else
                {
                    work.MakaleRepository.Update(makalelerEdit);
                    work.Save();
                    return RedirectToAction("MakalelerIndex");

                }
            }
            ViewBag.MakaleKategoriId = new SelectList(db.Kategoriler, "KategorId", "KategoriBaslik", makalelerEdit.MakaleKategoriId);
            return View(makalelerEdit);
        }

        public ActionResult MakalelerDelete(int? Id)
        {
            if (Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Makaleler makalesil = work.MakaleRepository.FindById(Id);
            if (makalesil==null)
            {
                HttpNotFound();
            }
            return View(makalesil);
        }

        [HttpPost,ActionName("MakalelerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult MakalelerDeleteConfirmed(int Id)
        {
            Makaleler makalesil= work.MakaleRepository.FindById(Id);
            work.MakaleRepository.Delete(makalesil);
            work.Save();
            return RedirectToAction("MakalelerIndex");
        }


    }
}