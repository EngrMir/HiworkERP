using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace HiWork.DAL.Repositories
{
    public partial interface IOrderRepository : IBaseRepository<Order>
    {
        List<Order> GetAllOrderList();
        Order GetOrder(Guid Id);
        Order InsertOrder(Order Order);
        Order UpdateOrder(Order Order);
        bool DeleteOrder(Guid Id);
    }

    public class OrderRepository : BaseRepository<Order, CentralDBEntities>, IOrderRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //Garbase collector
        }
        public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteOrder(Guid Id)
        {
            try
            {
                var Order = _dbContext.Orders.ToList().Find(d => d.ID == Id);
                if (Order != null)
                {
                    _dbContext.Orders.Remove(Order);
                    _dbContext.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            return false;
        }

        public List<Order> GetAllOrderList()
        {
            try
            {
                return _dbContext.Orders.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Order GetOrder(Guid Id)
        {
            try
            {
                return _dbContext.Orders.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Order InsertOrder(Order Order)
        {
            try
            {
                var result = _dbContext.Orders.Add(Order);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Order UpdateOrder(Order Order)
        {
            try
            {
                _dbContext.Entry(Order).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Order;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
