﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIT275_Back_end_interface.Models;
using PagedList;

namespace CIT275_Back_end_interface.Controllers
{
    public class FileLogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Partial class for filelogs, clients, and assests
        public class ClientLogExRef
        {
            public IEnumerable<Client> Clients { get; set; }
            public IEnumerable<Asset> Assets { get; set; }
            public IEnumerable<ClientAsset> ClientAssests { get; set; }
        }

        // GET: FileLogs
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Index()
        {
            var model = new ClientLogExRef();
            model.Clients = db.Clients.ToList();
            model.Assets = db.Assets.ToList();
            model.ClientAssests = db.ClientAssets.ToList();

            return View(model);
        }

        // POST: Partial FileLogsTable
        [HttpPost]
        public ActionResult LoadTable(int? clientId, int? assetId, int? page)
        {
            var model = from s in db.FileLogs
                        select s;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            //Log Files Filters
            if (clientId == null)
            {
                model = model.Take(pageSize * 2);
                model = model.OrderBy(s => s.CreateDate);
            }
            else if (assetId == null)
            {
                model = model.Where(s => s.ClientID == clientId);
                model = model.OrderBy(s => s.FileName);
            }
            else
            {
                model = model.Where(s => s.ClientID == clientId && s.AssetID == assetId);
                model = model.OrderBy(s => s.FileName);
            }

            //Passing Information
            ViewBag.GClientId = clientId;
            ViewBag.GAssetId = assetId;

            return PartialView("FileLogsTable", model.ToPagedList(pageNumber, pageSize));
        }

        // GET: FileLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileLog fileLog = db.FileLogs.Find(id);
            if (fileLog == null)
            {
                return HttpNotFound();
            }
            return View(fileLog);
        }

        // GET: FileLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileName,FilePath,ClientID,AssetID,Status,DeleteInd,Archived,BaseDate,CreateDate")] FileLog fileLog)
        {
            if (ModelState.IsValid)
            {
                db.FileLogs.Add(fileLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fileLog);
        }

        // GET: FileLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileLog fileLog = db.FileLogs.Find(id);
            if (fileLog == null)
            {
                return HttpNotFound();
            }
            return View(fileLog);
        }

        // POST: FileLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,FilePath,ClientID,AssetID,Status,DeleteInd,Archived,BaseDate,CreateDate")] FileLog fileLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fileLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fileLog);
        }

        // GET: FileLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileLog fileLog = db.FileLogs.Find(id);
            if (fileLog == null)
            {
                return HttpNotFound();
            }
            return View(fileLog);
        }

        // POST: FileLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileLog fileLog = db.FileLogs.Find(id);
            db.FileLogs.Remove(fileLog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
