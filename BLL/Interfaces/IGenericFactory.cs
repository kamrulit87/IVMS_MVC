using BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenericFactory<T> : IDisposable where T : class
    {

        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        bool HasData(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        Int64 Count();
        bool IsDuplicate(T entity);
        void Add(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Edit(T entity);
        Result Save();
        String getHTML();
        bool hasDuplicates<T>(List<T> myList);
        IQueryable<T> GetLastRecord();
    }
}
