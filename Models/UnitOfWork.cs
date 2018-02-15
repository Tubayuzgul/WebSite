using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebSite.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        
            private MyWebSiteEntities _context = new MyWebSiteEntities();
            private MyWebSiteRepository<Kategoriler> _kategoriRepository;
            private MyWebSiteRepository<Makaleler> _makaleRepository;
            private bool _disposed = false;
            public MyWebSiteRepository<Kategoriler> KategoriRepository
            {
                get
                {
                    if (_kategoriRepository == null)
                        _kategoriRepository = new MyWebSiteRepository<Kategoriler>(_context);
                    return _kategoriRepository;
                }
            }
            public MyWebSiteRepository<Makaleler> MakaleRepository
            {
                get
                {
                    if (_makaleRepository == null)
                        _makaleRepository = new MyWebSiteRepository<Makaleler>(_context);
                    return _makaleRepository;
                }
            }
            public void Save()
            {
                //using (TransactionScope tScope = new TransactionScope())
                //{
                  _context.SaveChanges();
                //    tScope.Complete();
                //}
            }
            protected virtual void Dispose(bool disposing)
            {
                if (!this._disposed)
                {
                    if (disposing)
                    {
                        _context.Dispose();
                    }
                }
                this._disposed = true;
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
