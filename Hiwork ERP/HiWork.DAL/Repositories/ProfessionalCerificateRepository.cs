using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{
    //public partial interface IProfessionalCerificateRepository : IBaseRepository<Master_StaffProfessionalCertificate>
    //{
    //    // string GetNextUserTypeId();
    //    List<Master_StaffProfessionalCertificate> GetAllProfessionalCerificateList();
    //    Master_StaffProfessionalCertificate GetProfessionalCerificate(Guid Id);
    //    Master_StaffProfessionalCertificate InsertProfessionalCerificate(Master_StaffProfessionalCertificate ProfessionalCerificate);
    //    Master_StaffProfessionalCertificate UpdateProfessionalCerificate(Master_StaffProfessionalCertificate ProfessionalCerificate);
    //    bool DeleteProfessionalCerificate(Guid Id);
    //}
    //public class ProfessionalCerificateRepository : BaseRepository<Master_StaffProfessionalCertificate, CentralDBEntities>, IProfessionalCerificateRepository
    //{
    //    private readonly CentralDBEntities _dbContext;

    //    public ProfessionalCerificateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    //    {
    //        _dbContext = new CentralDBEntities();
    //    }

    //    public bool DeleteProfessionalCerificate(Guid Id)
    //    {
    //        try
    //        {
    //            var ProfessionalCerificate = _dbContext.Master_StaffProfessionalCertificate.ToList().Find(f => f.ID == Id);
    //            if (ProfessionalCerificate != null)
    //            {
    //                _dbContext.Master_StaffProfessionalCertificate.Remove(ProfessionalCerificate);
    //                _dbContext.SaveChanges();

    //                return true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string message;
    //            message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            throw new Exception(message);
    //        }
    //        return false;
    //    }

    //    public List<Master_StaffProfessionalCertificate> GetAllProfessionalCerificateList()
    //    {
    //        try
    //        {
    //            return _dbContext.Master_StaffProfessionalCertificate.ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffProfessionalCertificate GetProfessionalCerificate(Guid Id)
    //    {
    //        try
    //        {
    //            return _dbContext.Master_StaffProfessionalCertificate.FirstOrDefault(u => u.ID == Id);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffProfessionalCertificate InsertProfessionalCerificate(Master_StaffProfessionalCertificate ProfessionalCerificate)
    //    {
    //        try
    //        {
    //            ProfessionalCerificate.ID = Guid.NewGuid();
    //            var result = _dbContext.Master_StaffProfessionalCertificate.Add(ProfessionalCerificate);
    //            _dbContext.SaveChanges();

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffProfessionalCertificate UpdateProfessionalCerificate(Master_StaffProfessionalCertificate ProfessionalCerificate)
    //    {
    //        try
    //        {
    //            _dbContext.Entry(ProfessionalCerificate).State = EntityState.Modified;
    //            _dbContext.SaveChanges();
    //            return ProfessionalCerificate;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }
    //}
}
