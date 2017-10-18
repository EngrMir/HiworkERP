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
      public partial interface IOrderDetailsRepository : IBaseRepository<OrderDetail>
    {
        List<OrderDetail> GetAllOrderDetailsList();
        List<OrderDetail> GetAllOrderDetailsListByOrderID(Guid Id);
        OrderDetail GetOrderDetails(Guid Id);
        OrderDetail InsertOrderDetails(OrderDetail OrderDetails);
        OrderDetail UpdateOrderDetails(OrderDetail OrderDetails);
        bool DeleteOrderDetails(Guid Id);
        Estimation GetEstimationByEstimationID(Guid Id);
        List<OrderDetail> GetOrderDetailsByEstimationNo(string EstimationID);
        List<Order> SaveOrder(Order aOrder);
        bool SaveOrderDetail(OrderDetail aOrder);
        EstimationDetail GetEstimationDetailByEstimationID(Guid Id);
        string GetStaffNameByID(Guid Id);        
        EstimationDetail getEstimationDetailByID(Guid Id);
        Staff getStaffByID(Guid Id);
        Master_EstimationServiceType GetEstimationServiceType(Guid Id);
        ProfitShareSetting getProfitShareSetting();
    }

    public class OrderDetailsRepository : BaseRepository<OrderDetail, CentralDBEntities>, IOrderDetailsRepository, IDisposable
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
        public Staff getStaffByID(Guid Id)
        {
            return _dbContext.Staffs.FirstOrDefault(d => d.ID == Id);
        }
        public ProfitShareSetting getProfitShareSetting()
        {
            return _dbContext.ProfitShareSettings.FirstOrDefault();
        }
        public string GetStaffNameByID(Guid Id)
        {
            Staff aStaff = _dbContext.Staffs.FirstOrDefault(d => d.ID == Id);
            return aStaff.StaffEmailID;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //Garbase collector
        }

        public Master_EstimationServiceType GetEstimationServiceType(Guid Id)
        {
            try
            {
                Master_EstimationServiceType aestimationServiceType = _dbContext.Master_EstimationServiceType.FirstOrDefault(d => d.ID == Id);
                if (aestimationServiceType != null)
                {
                    return aestimationServiceType;
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public EstimationDetail getEstimationDetailByID(Guid Id)
        {
            try
            {
                EstimationDetail aEstimationDetail = _dbContext.EstimationDetails.FirstOrDefault(d => d.ID == Id);
                if (aEstimationDetail != null)
                {
                    return aEstimationDetail;
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public EstimationDetail GetEstimationDetailByEstimationID(Guid Id)
        {
            try
            {
                EstimationDetail aEstimationDetail = _dbContext.EstimationDetails.FirstOrDefault(d => d.EstimationID == Id);
                if (aEstimationDetail != null)
                {
                    return aEstimationDetail;
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public List<Order> SaveOrder(Order aOrder)
        {
            try
            {
                var result = _dbContext.Orders.Add(aOrder);
                _dbContext.SaveChanges();
                return _dbContext.Orders.ToList();
            }
            catch (Exception ex) { }
            return null;
        }
        public List<OrderDetail> GetOrderDetailsByEstimationNo(string EstimationNo)
        {
            try
            {
                Estimation aEstimation = _dbContext.Estimations.FirstOrDefault(d => d.EstimationNo == EstimationNo);
                Order aOrder = _dbContext.Orders.FirstOrDefault(d => d.EstimationID == aEstimation.ID);

                List<OrderDetail> alist = _dbContext.OrderDetails.Where(d => d.OrderID == aOrder.ID).ToList();
                OrderDetail aaa = _dbContext.OrderDetails.FirstOrDefault(d => d.OrderID == aOrder.ID);

                return alist;
            }
            catch (Exception ex) { }
            return null;
        }
        public bool SaveOrderDetail(OrderDetail aaOrderDetail)
        {
            try
            {
                //Order aOrder = new Order();
                //aOrder.ID = aaOrderDetail.OrderID;
                //string estid = "D61C6C4C-3AED-4B42-BB53-590106C35F69";
                //string comid = "087B00CC-9E7F-43C4-A131-063ACC8F498D";
                //aOrder.RegistrationID = 12345; //new Guid(regid);
                //aOrder.OrderNo = "do quick";
                //aOrder.EstimationID = new Guid(estid);
                //aOrder.CompanyID = new Guid(comid);
                //aOrder.QuoatedPrice = 55;
                //aOrder.ConsumptionTax = 44;
                //aOrder.CostExclTax = 44;
                //aOrder.BillingAmount = 44;
                //aOrder.OriginalCost = 44;
                //aOrder.WithdrawlAmount = 4;
                //aOrder.Profit = 4;
                //aOrder.GrossInterestProfit = 44;
                //aOrder.IsDeposited = true;
                

               
               // var result = _dbContext.OrderDetails.Add(aaOrderDetail);
                var order = (from ac in _dbContext.Orders
                             where ac.ID == aaOrderDetail.OrderID
                             select ac).FirstOrDefault();

                OrderDetail aOrderDetail = new OrderDetail();
                aOrderDetail.ID = Guid.NewGuid();
                aOrderDetail.Order = order;
                aOrderDetail.OrderID = aaOrderDetail.OrderID;
                aOrderDetail.EstimationDetailsID = aaOrderDetail.EstimationDetailsID;
                aOrderDetail.PaymentAmountExcludingTax = aaOrderDetail.PaymentAmountExcludingTax;
                aOrderDetail.PaymentAmountIncludingTax = aaOrderDetail.PaymentAmountIncludingTax;
                aOrderDetail.StaffID = aaOrderDetail.StaffID;
                aOrderDetail.WorkingStatus = 1;
                aOrderDetail.DeliveryDate = aaOrderDetail.DeliveryDate;
                aOrderDetail.EvaluationAmount = aaOrderDetail.EvaluationAmount;
                aOrderDetail.AgencyCommission = aaOrderDetail.AgencyCommission;
                aOrderDetail.ComplainDetails = aaOrderDetail.ComplainDetails;
                aOrderDetail.RemitanceDate = DateTime.Now;
                aOrderDetail.DepositAmount = aaOrderDetail.DepositAmount;
                var K = _dbContext.OrderDetails.Add(aOrderDetail);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                string a = ex.Message.ToString();
            }
            return false;
        }
        public OrderDetailsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        
     
        public Estimation GetEstimationByEstimationID(Guid Id)
        {
            try
            {
                Estimation estimation = _dbContext.Estimations.FirstOrDefault(d => d.ID == Id);               
                if (estimation != null)
                {
                    return estimation;
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public bool DeleteOrderDetails(Guid Id)
        {
            try
            {
                var OrderDetails = _dbContext.OrderDetails.ToList().Find(d => d.ID == Id);
                if (OrderDetails != null)
                {
                    _dbContext.OrderDetails.Remove(OrderDetails);
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

        public List<OrderDetail> GetAllOrderDetailsList()
        {
            try
            {
                return _dbContext.OrderDetails.ToList();
            }
            catch (Exception ex) { }

            return null;
        }
        public List<OrderDetail> GetAllOrderDetailsListByOrderID(Guid Id)
        {
            try
            {
                return _dbContext.OrderDetails.Where(d => d.OrderID == Id).ToList();
            }
            catch (Exception ex) { }

            return null;
        }
        public OrderDetail GetOrderDetails(Guid Id)
        {
            try
            {
                return _dbContext.OrderDetails.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public OrderDetail InsertOrderDetails(OrderDetail OrderDetails)
        {
            try
            {
                var result = _dbContext.OrderDetails.Add(OrderDetails);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public OrderDetail UpdateOrderDetails(OrderDetail OrderDetails)
        {
            try
            {
                _dbContext.Entry(OrderDetails).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return OrderDetails;
            }
            catch (Exception ex) { }

            return null;
        }    
    }
}
