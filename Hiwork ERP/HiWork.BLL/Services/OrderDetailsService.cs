using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using HiWork.BLL.ServiceHelper;
using System.Data.Entity;


namespace HiWork.BLL.Services
{
    public partial interface IOrderDetailsService : IBaseService<OrderDetailsModel, OrderDetail>
    {
        EstimationModel GetEstimationByEstimationNo(BaseViewModel model, string EstimationNo);
        OrderModel GetOrder(BaseViewModel model, Guid estimationId);
        List<DetailsViewModel> GetDetailsList(BaseViewModel model, Guid estimationId);
        bool Save(CommonModelHelper model);
        List<DetailsViewModel> GetOrderDetailsList(BaseViewModel model, Guid orderId);
        ProfitShareSetting getProfitShareSetting();
    }
    public class OrderDetailsService : BaseService<OrderDetailsModel, OrderDetail>, IOrderDetailsService
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository) : base(orderDetailsRepository)
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            _orderDetailsRepository = orderDetailsRepository;
        }
        public ProfitShareSetting getProfitShareSetting()
        {
            return _orderDetailsRepository.getProfitShareSetting();
        }
        public string GenerateOrderNo()
        {
            var dt = DateTime.Now;
            var maxReg = _dbContext.Orders.OrderByDescending(o => o.RegistrationID).FirstOrDefault()?.RegistrationID ?? 1;
            var str = $"{dt.Year}{dt.Month}{dt.Day}HI{maxReg}";
            return str;
        }

        public string GenerateInvoiceNo()
        {
            var dt = DateTime.Now;
            var maxReg = _dbContext.Orders.OrderByDescending(o => o.RegistrationID).FirstOrDefault()?.RegistrationID ?? 1;
            var str = $"{dt.Year}{dt.Month}{dt.Day}INV{maxReg}";
            return str;
        }

        public bool Save(CommonModelHelper model)
        {
            var isSuccess = true;
            try
            {
                var culturalItems = new List<string> { "CoordinatorPrecautions", "CoordinatorNotes", "NotesToStaff", "ComplainDetails", "AccountingRelatedMemo" };
                ModelBinder.SetCulturalValue(model.Order, model, culturalItems);
                model.Order.ApplicationID = model.ApplicationID;
                model.Order.OrderStatus = (int)OrderStatus.Ordered;
                //Save Order
                if (model.Order.ID == Guid.Empty)
                {
                    model.Order.ID = Guid.NewGuid();
                    model.Order.InvoiceNo = GenerateInvoiceNo();
                    model.Order.Estimation = _dbContext.Estimations.Find(model.Estimation.ID);
                    model.Order.OrderNo = GenerateOrderNo();
                    model.Order.CompanyID = model.Estimation.ClientID;
                    model.Order.Company = _dbContext.Companies.Find(model.Order.CompanyID);
                    _dbContext.Orders.Add(model.Order);
                }
                else
                {
                    _dbContext.Entry(model.Order).State = EntityState.Modified;
                }
                //Save or update Order details
                foreach (var item in model.OrderDetails)
                {
                    item.OrderID = model.Order.ID;
                    item.Order = model.Order;
                    ModelBinder.ModifyGuidValue(item);
                    if (item.ID == Guid.Empty)
                    {
                        item.ID = Guid.NewGuid();
                        _dbContext.OrderDetails.Add(item);
                    }
                    else
                    {
                        _dbContext.Entry(item).State = EntityState.Modified;
                    }
                }
                //Update Estimation
                //_dbContext.Entry(model.Estimation).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "OrderDetails", message);
                throw new Exception(message);
            }
            return isSuccess;
        }

        public EstimationModel GetEstimationByEstimationNo(BaseViewModel model, string EstimationID)
        {
            EstimationModel estimationModel = new EstimationModel();
            Estimation estimation;
            try
            {
                Guid Id = new Guid(EstimationID);
                estimation = _orderDetailsRepository.GetEstimationByEstimationID(Id);
                estimationModel = Mapper.Map<Estimation, EstimationModel>(estimation);
                estimationModel.CurrentUserID = model.CurrentUserID;
                estimationModel.CurrentCulture = model.CurrentCulture;
                estimationModel.EstimateRouteName = Utility.GetPropertyValue(estimation.Master_EstimationRoutes, "Name", model.CurrentCulture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(estimation.Master_EstimationRoutes, "Name", model.CurrentCulture).ToString();
                estimationModel.ClientName = Utility.GetPropertyValue(estimation.Company, "Name", model.CurrentCulture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(estimation.Company, "Name", model.CurrentCulture).ToString();
                estimationModel.OutwardSalesName = Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture).ToString();
                estimationModel.CoordinatorName = Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture) == null ? string.Empty :
                                                    Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture).ToString();
                estimationModel.SalesPersonName = Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture) == null ? string.Empty :
                                                   Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture).ToString();
                estimationModel.LargeSalesName = Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture) == null ? string.Empty :
                                                  Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture).ToString();
                estimationModel.TotalWithTax = estimation.TotalWithTax;
                //estimationModel.BusinessCategoryName = Utility.GetPropertyValue(estimation.master, "Name", model.CurrentCulture) == null ? string.Empty :
                //                               Utility.GetPropertyValue(estimation.Employee, "Name", model.CurrentCulture).ToString();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "OrderDetails", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return estimationModel;
        }

        public OrderModel GetOrder(BaseViewModel model, Guid estimationId)
        {
            var o = _dbContext.Orders.Where(x=> x.Estimation.ID == estimationId).AsNoTracking().FirstOrDefault() ?? new Order();
            var orderModel = new OrderModel
            {
                ID = o.ID,
                RegistrationID = o.RegistrationID,
                ApplicationID = o.ApplicationID,
                OrderNo = o.OrderNo,
                InvoiceNo = o.InvoiceNo,
                EstimationID = o.EstimationID,
                CompanyID = o.CompanyID,
                DeliveryDescription = o.GetType()?.GetProperty($"DeliveryDescription_{model.CurrentCulture}")?.GetValue(o, null)?.ToString()??"",
                QuoatedPrice = o.QuoatedPrice,
                ConsumptionTax = o.ConsumptionTax,
                CostExclTax = o.CostExclTax,
                BillingAmount = o.BillingAmount,
                OriginalCost = o.OriginalCost,
                WithdrawlAmount = o.WithdrawlAmount,
                Profit = o.Profit,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus,
                GrossInterestProfit = o.GrossInterestProfit,
                IsDeposited = o.IsDeposited,
                Remarks = o.Remarks,
                Deadline = o.Deadline,
                EstimatedDateOfDeposit = o.EstimatedDateOfDeposit,
                ClientComplain = o.ClientComplain,
                StaffComplain = o.StaffComplain,
                NonStaffComplain = o.NonStaffComplain,
                ComplainDate = o.ComplainDate,
                ResponseComplainDate = o.ResponseComplainDate,
                ComplainDetails = o.GetType()?.GetProperty($"ComplainDetails_{model.CurrentCulture}")?.GetValue(o, null)?.ToString() ?? "",
                AccountingRelatedMemo = o.GetType()?.GetProperty($"AccountingRelatedMemo_{model.CurrentCulture}")?.GetValue(o, null)?.ToString() ?? "",
                CostInclTax = o.CostInclTax,
                PaymentinInstallment = o.PaymentinInstallment,
                InternalPayment = o.InternalPayment,
                ExternalPayment = o.FrontSalesCharge,
                Shand = o.SCharge,
                Bhand = o.BCharge,
                Chand = o.CCharge,
                Partner = o.PartnerCharge,
                NetProfit = o.NetProfit,
                NetMarginRate = o.NetMarginRate,
                GrossProfit = o.GrossProfit,
                GrossMarginRate = o.GrossMarginRate,
                OrderAmount = o.OrderAmount,
                CoordinatorMemo = o.CoordinatorMemo,
                CoordinatorPrecautions = o.GetType()?.GetProperty($"CoordinatorPrecautions_{model.CurrentCulture}")?.GetValue(o, null)?.ToString() ?? "",
                CoordinatorNotes = o.GetType()?.GetProperty($"CoordinatorNotes_{model.CurrentCulture}")?.GetValue(o, null)?.ToString() ?? "",
                NotesToStaff = o.GetType()?.GetProperty($"NotesToStaff_{model.CurrentCulture}")?.GetValue(o, null)?.ToString() ?? "",
                IsInternalComplain = o.IsInternalComplain,
                IsClientComplain = o.IsClientComplain,
                DirectManuscript = o.DirectManuscript_cn,
                Coordinator2ID = o.Coordinator2ID,
                EmailCCFullString = o.EmailCCFullString,
                PaymentDateByMonth = o.PaymentDateByMonth,
                IsInternalResponse = o.IsInternalResponse
            };
            return orderModel;
        }

        public List<DetailsViewModel> GetDetailsList(BaseViewModel model, Guid estimationId)
        {
            var items = new List<DetailsViewModel>();
            try
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                items = (from e in _dbContext.Estimations
                         join ed in _dbContext.EstimationDetails on e.ID equals ed.Estimation.ID
                         join l1 in _dbContext.Master_Language on ed.SourceLanguageID equals l1.ID
                         join l2 in _dbContext.Master_Language on ed.TargetLanguageID equals l2.ID
                         join od in _dbContext.OrderDetails on ed.ID equals od.EstimationDetail.ID into tempOd
                         from leftOd in tempOd.DefaultIfEmpty()
                         join s in _dbContext.Staffs on leftOd.StaffID equals s.ID into tempS
                         from leftS in tempS.DefaultIfEmpty()
                         join est in _dbContext.Master_EstimationServiceType on ed.ServiceType equals est.ID into tempEst
                         from leftEst in tempEst.DefaultIfEmpty()
                         join ws in _dbContext.Master_WorkingStatus on leftOd.WorkingStatus equals ws.ID into tempWs
                         from leftWs in tempWs.DefaultIfEmpty()
                         where e.ID == estimationId
                         select new DetailsViewModel
                         {
                             EstimationDetailsID = ed.ID,
                             EstimationID = ed.Estimation.ID,
                             Source = l1,
                             Target = l2,
                             ServiceType = leftEst,
                             UnitPrice1 = ed.UnitPrice1,
                             PageCount1 = ed.PageCount1,
                             Discount1 = ed.Discount1,
                             DeliveryDate = leftOd.DeliveryDate,
                             FinalDeliveryDate = e.FinalDeliveryDate,
                             StaffID = leftS.ID,
                             Staff = leftS,
                             WorkingStatusObj = leftWs,
                             WorkingStatus = leftWs.ID,
                             EvaluationAmount = ed.UnitPrice1,
                             PaymentAmountExcludingTax = leftOd.PaymentAmountExcludingTax,
                             PaymentAmountIncludingTax = leftOd.PaymentAmountIncludingTax,
                             StaffPaymentCostExcludingTax = leftOd.StaffPaymentCostExcludingTax,
                             StaffPaymentCostIncludingTax = leftOd.StaffPaymentCostIncludingTax,
                             TotalStaffPaymentCost = leftOd.TotalStaffPaymentCost,
                             AgencyCommission = leftOd.AgencyCommission,
                             RemitanceDate = leftOd.RemitanceDate,
                             DepositAmount = leftOd.DepositAmount,
                             EmailAddress = leftS.StaffEmailID,
                             ComplainDetails = leftOd.ComplainDetails,
                             Condition = leftWs.ID,
                             StaffNo = string.Empty,
                             Penalty = leftOd.Penalty == null ? 0 : leftOd.Penalty,
                             Residence = string.Empty,
                             WithTranslation = ed.WithTranslation,
                             LengthMinute = ed.LengthMinute
                         }).ToList();
                items.ForEach(i =>
                  {
                      i.SourceLanguage = i.Source == null ? string.Empty : i.Source.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(i.Source, null)?.ToString();
                      i.TargetLanguage = i.Target == null ? string.Empty : i.Target.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(i.Target, null)?.ToString();
                      i.WorkingStatusName = i.WorkingStatusObj == null ? string.Empty : i.WorkingStatusObj.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(i.WorkingStatusObj, null)?.ToString();
                      i.ServiceTypeName = i.ServiceType == null ? string.Empty : i.ServiceType.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(i.ServiceType, null)?.ToString();
                      i.StaffName = i.Staff == null ? string.Empty : $"{i.Staff.GetType().GetProperty($"FirstName_{model.CurrentCulture}").GetValue(i.Staff, null)} {i.Staff.GetType().GetProperty($"LastName_{model.CurrentCulture}").GetValue(i.Staff, null)}";
                      i.Residence = i.Staff == null ? string.Empty : i.Staff.GetType().GetProperty($"Address_{model.CurrentCulture}").GetValue(i.Staff, null)?.ToString();
                      i.DeliveryDate = i.DeliveryDate ?? i.FinalDeliveryDate;
                      i.WorkingStatusName = string.Empty;
                      i.Source = null;
                      i.Target = null;
                      i.ServiceType = null;
                      i.Staff = null;
                      i.WorkingStatusObj = null;
                  });
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "OrderDetails", message);
                throw new Exception(message);
            }
            finally
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return items;
        }

        public List<DetailsViewModel> GetOrderDetailsList(BaseViewModel model, Guid orderId)
        {
            var items = new List<DetailsViewModel>();
            try
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                items = (from o in _dbContext.Orders
                         join od in _dbContext.OrderDetails on o.ID equals od.Order.ID
                         join ed in _dbContext.EstimationDetails on od.EstimationDetailsID equals ed.ID
                         join s in _dbContext.Staffs on od.StaffID equals s.ID into tempS
                         from leftS in tempS.DefaultIfEmpty()
                         join ws in _dbContext.Master_WorkingStatus on od.WorkingStatus equals ws.ID into tempWs
                         from leftWs in tempWs.DefaultIfEmpty()
                         where o.ID == orderId
                         select new DetailsViewModel
                         {
                             OrderID = o.ID,
                             ID = od.ID,
                             EstimationID = ed.Estimation.ID,
                             EstimationDetailsID = ed.ID,
                             UnitPrice1 = ed.UnitPrice1,
                             PageCount1 = ed.PageCount1,
                             Discount1 = ed.Discount1,
                             DeliveryDate = od.DeliveryDate,
                             StaffID = leftS.ID,
                             Staff = leftS,
                             WorkingStatus = od.WorkingStatus,
                             EvaluationAmount = ed.UnitPrice1,
                             PaymentAmountExcludingTax = od.PaymentAmountExcludingTax,
                             PaymentAmountIncludingTax = od.PaymentAmountIncludingTax,
                             StaffPaymentCostExcludingTax = od.StaffPaymentCostExcludingTax,
                             StaffPaymentCostIncludingTax = od.StaffPaymentCostIncludingTax,
                             TotalStaffPaymentCost = od.TotalStaffPaymentCost,
                             AgencyCommission = od.AgencyCommission,
                             RemitanceDate = od.RemitanceDate,
                             DepositAmount = od.DepositAmount,
                             EmailAddress = leftS.StaffEmailID,
                             ComplainDetails = od.ComplainDetails,
                             Condition = leftWs.ID,
                             StaffNo = string.Empty,
                             Evaluation = ed.UnitPrice1,
                             Penalty = od.Penalty == null ? 0 : od.Penalty,
                             Residence = string.Empty
                         }).ToList();
                items.ForEach(i =>
                {
                    i.StaffName = i.Staff == null ? string.Empty : $"{i.Staff.GetType().GetProperty($"FirstName_{model.CurrentCulture}").GetValue(i.Staff, null)} {i.Staff.GetType().GetProperty($"LastName_{model.CurrentCulture}").GetValue(i.Staff, null)}";
                    i.WorkingStatusName = string.Empty;
                    i.Staff = null;
                });
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "OrderDetails", message);
                throw new Exception(message);
            }
            finally
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return items;
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Employee", message);
            return message;
        }    
    }
}
