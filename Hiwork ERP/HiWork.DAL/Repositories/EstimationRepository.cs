using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace HiWork.DAL.Repositories
{     
     public partial interface IEstimationRepository : IBaseRepository<Estimation>
    {
        List<Estimation> GetAllEstimationList();
        Estimation GetEstimation(Guid Id);
        Estimation InsertEstimation(Estimation Estimation);
        Estimation UpdateEstimation(Estimation Estimation);
        bool DeleteEstimation(Guid Id);
        EstimationProject SaveEstimationProject(EstimationProject model);
        List<Estimation> GetSelectedEstimationList(List<Guid> IdList);
        long? GetEstimationProjectNextNumber(BaseViewModel model); 

        
    }

    public class EstimationRepository : BaseRepository<Estimation, CentralDBEntities>, IEstimationRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
  

        public EstimationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteEstimation(Guid Id)
        {
            try
            {
                var Estimation = _dbContext.Estimations.ToList().Find(d => d.ID == Id);
                if (Estimation != null)
                {
                    _dbContext.Estimations.Remove(Estimation);
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

        public List<Estimation> GetAllEstimationList()
        {
            try
            {
                return _dbContext.Estimations.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Estimation GetEstimation(Guid Id)
        {
            try
            {
                return _dbContext.Estimations.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Estimation InsertEstimation(Estimation Estimation)
        {
            try
            {
                var result = _dbContext.Estimations.Add(Estimation);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Estimation UpdateEstimation(Estimation Estimation)
        {
            try
            {
                _dbContext.Entry(Estimation).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Estimation;
            }
            catch (Exception ex) { }

            return null;
        }
        public EstimationProject SaveEstimationProject(EstimationProject model)
        {
            try
            {
              var data= _dbContext.EstimationProjects.Add(model);
                _dbContext.SaveChanges();
                return data;
            }
            catch(Exception ex) { }
            return null;
        }
        public List<Estimation> GetSelectedEstimationList(List<Guid> IdList)
        {
            try
            {
                return _dbContext.Estimations.Where(e => IdList.Contains(e.ID)).ToList();
                    
            }
            catch(Exception ex)
            {

            }
            return null;
        }
        public long? GetEstimationProjectNextNumber(BaseViewModel model)
        {
            long? RegistrationID;
            var project = _dbContext.EstimationProjects.ToList();

            return RegistrationID = project.Count == 0 ? 0 : project.Where(e => e.ApplicationId == model.ApplicationId).LastOrDefault().RegistrationID;
  
        }
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

    }
}
