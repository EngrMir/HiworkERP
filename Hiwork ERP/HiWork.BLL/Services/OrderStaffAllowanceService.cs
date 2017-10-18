using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public partial interface IOrderStaffAllowanceService : IBaseService<OrderStaffAllowanceModel, Order_StaffAllowance>
    {
        bool SaveStaffAllowance(List<OrderStaffAllowanceModel> model);
        List<OrderStaffAllowanceModel> GetAllStaffAllowance(OrderStaffAllowanceModel model, Guid id);
    }
    public class OrderStaffAllowanceService : BaseService<OrderStaffAllowanceModel, Order_StaffAllowance>, IOrderStaffAllowanceService
    {
        private readonly IOrderStaffAllowanceRepository _staffallowancerepository;
        public OrderStaffAllowanceService(IOrderStaffAllowanceRepository staffallowancerepository) : base(staffallowancerepository)
        {
            _staffallowancerepository = staffallowancerepository;
        }

        public bool SaveStaffAllowance(List<OrderStaffAllowanceModel> model)
        {
            bool IsSuccessful = false;
            foreach (OrderStaffAllowanceModel allowancedetails in model)
            {
                var mappedmodel = Mapper.Map<OrderStaffAllowanceModel, Order_StaffAllowance>(allowancedetails);
                IsSuccessful = _staffallowancerepository.SaveStaffAllowance(mappedmodel);
                IsSuccessful = true;
            }
            return IsSuccessful;
        }

        public List<OrderStaffAllowanceModel> GetAllStaffAllowance( OrderStaffAllowanceModel model, Guid id)
        {
            List<Order_StaffAllowance> masterdatalist = new List<Order_StaffAllowance>();
            List<OrderStaffAllowanceModel> datalist = new List<OrderStaffAllowanceModel>();
            try
            {
                masterdatalist = _staffallowancerepository.GetAllStaffAllowance();
                if (masterdatalist != null)
                {
                    foreach (Order_StaffAllowance a in masterdatalist)
                    {
                        if (a.EstimationID == id)
                        {
                            var mappedmodel = Mapper.Map<Order_StaffAllowance, OrderStaffAllowanceModel>(a);
                            datalist.Add(mappedmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return datalist;
        }
    }
}
