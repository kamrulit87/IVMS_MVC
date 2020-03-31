using BLL.Common;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;


namespace BLL.Factory
{
    public abstract class GenericFactory<C, T> :
          IGenericFactory<T>
        where T : class
        where C : DbContext, new()
    {
        Result _result = new Result();
        private C _entities = new C();
        protected C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().AsNoTracking().Where(predicate);
            return query;
        }

        public bool HasData(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return FindBy(predicate).Any();
        }

        public int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return FindBy(predicate).Count();
        }

        public Int64 Count()
        {
            return _entities.Set<T>().Count();
        }

        public virtual bool IsDuplicate(T entity)
        {
            var hs = new HashSet<T>();

            for (var i = 0; i < 1; ++i)
            {
                if (!hs.Add(entity)) return true;
            }
            return false;
        }

        public virtual bool hasDuplicates<T>(List<T> myList)
        {
            var hs = new HashSet<T>();

            for (var i = 0; i < myList.Count; ++i)
            {
                if (!hs.Add(myList[i])) return true;
            }
            return false;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> list = _entities.Set<T>().Where(predicate);
            foreach (var entity in list)
            {
                _entities.Set<T>().Remove(entity);
            }
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual Result Save()
        {
            try
            {
                _entities.SaveChanges();
                _result.isSucess = true;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _result.message = e.InnerException.InnerException.Message;
                    return _result;
                }
                else
                {
                    _result.message = e.Message;
                }
                _result.isSucess = false;                
            }

            if (!_result.isSucess)
            {
                try
                {
                    _entities.SaveChanges();                   
                    _result.isSucess = true;
                }
                catch (DbEntityValidationException e)
                {

                    StringBuilder buildMessage = new StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            buildMessage.Append(ve.PropertyName);
                            buildMessage.Append(" ");
                            buildMessage.Append(ve.ErrorMessage);
                        }
                    }
                    _result.isSucess = false;
                    _result.message = buildMessage.ToString();
                    return _result;
                }
            }
            if (_result.isSucess)
            {
                _result.message = "Save Successful!";
            }
            return _result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _entities.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual String getHTML()
        {
            return "";
        }
        public IQueryable<T> GetLastRecord()
        {
            // IQueryable<C> query = _entities.Set<C>().AsNoTracking().Where(predicate);
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

    }
}
