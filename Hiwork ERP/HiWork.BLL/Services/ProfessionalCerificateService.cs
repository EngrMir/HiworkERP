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

namespace HiWork.BLL.Services
{     
   //public partial interface IProfessionalCerificateService : IBaseService<ProfessionalCerificateModel, Master_StaffProfessionalCertificate>
   // {
   //     List<ProfessionalCerificateModel> SaveProfessionalCerificate(ProfessionalCerificateModel model);
   //     List<ProfessionalCerificateModel> GetAllProfessionalCerificateList(ProfessionalCerificateModel model);
   //     List<ProfessionalCerificateModel> DeleteProfessionalCerificate(ProfessionalCerificateModel model);
   // }

   // public class ProfessionalCerificateService : BaseService<ProfessionalCerificateModel, Master_StaffProfessionalCertificate>, IProfessionalCerificateService
   // {
   //     private readonly IProfessionalCerificateRepository _ProfessionalCerificateRepository;
   //     public ProfessionalCerificateService(IProfessionalCerificateRepository ProfessionalCerificateRepository)
   //         : base(ProfessionalCerificateRepository)
   //     {
   //         _ProfessionalCerificateRepository = ProfessionalCerificateRepository;
   //     }
   //     public List<ProfessionalCerificateModel> SaveProfessionalCerificate(ProfessionalCerificateModel model)
   //     {
   //         List<ProfessionalCerificateModel> ProfessionalCerificateList = null;
   //         try
   //         {
   //             Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

   //             var ProfessionalCerificate = Mapper.Map<ProfessionalCerificateModel, Master_StaffProfessionalCertificate>(model);
   //             if (ProfessionalCerificate.ID != Guid.Empty)
   //             {
   //                 ProfessionalCerificate.UpdatedBy = model.CurrentUserID;
   //                 ProfessionalCerificate.UpdatedDate = DateTime.Now;
   //                 _ProfessionalCerificateRepository.UpdateProfessionalCerificate(ProfessionalCerificate);
   //             }
   //             else
   //             {
   //                 ProfessionalCerificate.ID = Guid.NewGuid();
   //                 ProfessionalCerificate.CreatedBy = model.CurrentUserID;
   //                 ProfessionalCerificate.CreatedDate = DateTime.Now;
   //                 _ProfessionalCerificateRepository.InsertProfessionalCerificate(ProfessionalCerificate);
   //             }
   //             ProfessionalCerificateList = GetAllProfessionalCerificateList(model);
   //         }
   //         catch (Exception ex)
   //         {
   //             IErrorLogService errorLog = new ErrorLogService();
   //             string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
   //             errorLog.SetErrorLog(model.CurrentUserID, "ProfessionalCerificate", message);
   //             throw new Exception(ex.Message);
   //         }
   //         return ProfessionalCerificateList;
   //     }
   //     public List<ProfessionalCerificateModel> GetAllProfessionalCerificateList(ProfessionalCerificateModel model)
   //     {
   //         List<ProfessionalCerificateModel> ProfessionalCerificateList = new List<ProfessionalCerificateModel>();
   //         ProfessionalCerificateModel ProfessionalCerificateModel = new ProfessionalCerificateModel();
   //         try
   //         {
   //             List<Master_StaffProfessionalCertificate> ProfessionalCerificatevList = _ProfessionalCerificateRepository.GetAllProfessionalCerificateList();
   //             if (ProfessionalCerificatevList != null)
   //             {
   //                 ProfessionalCerificatevList.ForEach(a =>
   //                 {
   //                     ProfessionalCerificateModel = Mapper.Map<Master_StaffProfessionalCertificate, ProfessionalCerificateModel>(a);
   //                     ProfessionalCerificateModel.Name = Utility.GetPropertyValue(ProfessionalCerificateModel, "Name", model.CurrentCulture) == null ? string.Empty :
   //                         Utility.GetPropertyValue(ProfessionalCerificateModel, "Name", model.CurrentCulture).ToString();
   //                     ProfessionalCerificateModel.Description = Utility.GetPropertyValue(ProfessionalCerificateModel, "Description", model.CurrentCulture) == null ? string.Empty :
   //                         Utility.GetPropertyValue(ProfessionalCerificateModel, "Description", model.CurrentCulture).ToString();
   //                     ProfessionalCerificateModel.CurrentUserID = model.CurrentUserID;
   //                     ProfessionalCerificateModel.CurrentCulture = model.CurrentCulture;
   //                     ProfessionalCerificateList.Add(ProfessionalCerificateModel);
   //                 });
   //             }
   //         }
   //         catch (Exception ex)
   //         {
   //             IErrorLogService errorLog = new ErrorLogService();
   //             string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
   //             errorLog.SetErrorLog(model.CurrentUserID, "ProfessionalCerificate", message);
   //             throw new Exception(ex.Message);
   //         }
   //         return ProfessionalCerificateList;
   //     }
   //     public List<ProfessionalCerificateModel> DeleteProfessionalCerificate(ProfessionalCerificateModel model)
   //     {
   //         List<ProfessionalCerificateModel> ProfessionalCerificates = null;
   //         try
   //         {
   //             _ProfessionalCerificateRepository.DeleteProfessionalCerificate(model.ID);
   //             ProfessionalCerificates = GetAllProfessionalCerificateList(model);
   //         }
   //         catch (Exception ex)
   //         {
   //             IErrorLogService errorLog = new ErrorLogService();
   //             string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
   //             errorLog.SetErrorLog(model.CurrentUserID, "ProfessionalCerificate", message);
   //             throw new Exception(ex.Message);
   //         }
   //         return ProfessionalCerificates;
   //     }
   // }
}
