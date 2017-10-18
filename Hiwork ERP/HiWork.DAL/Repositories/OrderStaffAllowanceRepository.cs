using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public partial interface IOrderStaffAllowanceRepository: IBaseRepository<Order_StaffAllowance>
    {
          bool SaveStaffAllowance(Order_StaffAllowance model);
         List<Order_StaffAllowance> GetAllStaffAllowance();
    }
   public class OrderStaffAllowanceRepository : BaseRepository<Order_StaffAllowance, CentralDBEntities>, IOrderStaffAllowanceRepository        
    {
        private CentralDBEntities _dbContext;

        public OrderStaffAllowanceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool SaveStaffAllowance(Order_StaffAllowance model)
        {   
            bool IsSuccessful = false;
            //Guid AllowanceID;
            //Guid OrderID;
            //AllowanceID = Guid.NewGuid();
            //model.ID = AllowanceID;
            //if (model.OrderID == Guid.Empty)
            //{
            //    OrderID = Guid.NewGuid();
            //    model.OrderID = OrderID;
            //}


             if(model.ID==Guid.Empty)
            {
                Guid AllowanceID = Guid.NewGuid();
                model.ID = AllowanceID;
                //insert
                try
                {
                    _dbContext.Order_StaffAllowance.Add(model);
                    _dbContext.SaveChanges();
                    IsSuccessful = true;
                    return IsSuccessful;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

             else
            {
                //update
                try
                {
                    _dbContext.Entry(model).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    IsSuccessful = true;
                    return IsSuccessful;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
            }
        }

        public List<Order_StaffAllowance> GetAllStaffAllowance()
        {
           return _dbContext.Order_StaffAllowance.ToList();
        }
    }
}
