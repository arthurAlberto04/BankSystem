using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Db
{
    public class BankDAL<T> where T : class
    {
        private readonly BankContext context;

        public BankDAL(BankContext ctx)
        {
            this.context = ctx;
        }

        public IEnumerable<T> List()
        {
            return context.Set<T>().ToList();
        }

        public void Add(T obj)
        {
              context.Set<T>().Add(obj);
              context.SaveChanges();
        }
        public void Remove(T obj)
        {
            context.Set<T>().Remove(obj);
            context.SaveChanges();
        }
        public void Update(T obj)
        {
            context.Set<T>().Update(obj);
            context.SaveChanges();
        }
        public T Recoverby(Func<T, bool> condition)
        {
            return context.Set<T>().FirstOrDefault(condition);
        }
        public IEnumerable<T> RecoverAllBy(Func<T, bool> condition)
        {
            return context.Set<T>().Where(condition).ToList();
        }
    }
}
