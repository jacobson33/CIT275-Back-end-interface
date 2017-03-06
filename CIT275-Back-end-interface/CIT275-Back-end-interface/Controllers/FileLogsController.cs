﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIT275_Back_end_interface.Models;

namespace CIT275_Back_end_interface.Controllers
{
    public class FileLogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FileLogs
        public ActionResult Index()
        {
            return View(db.FileLogs.ToList());
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