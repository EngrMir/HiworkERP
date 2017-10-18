using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.DAL.Repositories
{
    public partial interface INarrationVoiceFilesRepository : IBaseRepository<Staff_NarrationVoiceFiles>
    {
        List<Staff_NarrationVoiceFiles> GetAllNarrationVoiceFileList();
        Staff_NarrationVoiceFiles GetNarrationVoiceFile(Guid Id);
        Staff_NarrationVoiceFiles InsertNarrationVoiceFiles(Staff_NarrationVoiceFiles narrationVoiceFiles);
        Staff_NarrationVoiceFiles UpdateNarrationVoiceFiles(Staff_NarrationVoiceFiles userNarrationVoiceFiles);
        bool DeleteNarrationVoiceFiles(Guid Id);
    }

    public class NarrationVoiceFilesRepository : BaseRepository<Staff_NarrationVoiceFiles, CentralDBEntities>, INarrationVoiceFilesRepository
    {
        private readonly CentralDBEntities _dbContext;

        public NarrationVoiceFilesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteNarrationVoiceFiles(Guid Id)
        {
            try
            {
                var nvFiles = _dbContext.Staff_NarrationVoiceFiles.Find(Id);
                if (nvFiles != null)
                {
                    _dbContext.Staff_NarrationVoiceFiles.Remove(nvFiles);
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

        public List<Staff_NarrationVoiceFiles> GetAllNarrationVoiceFileList()
        {
            try
            {
                return _dbContext.Staff_NarrationVoiceFiles.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationVoiceFiles GetNarrationVoiceFile(Guid Id)
        {
            try
            {
                return _dbContext.Staff_NarrationVoiceFiles.Find(Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationVoiceFiles InsertNarrationVoiceFiles(Staff_NarrationVoiceFiles narrationVoiceFiles)
        {
            try
            {
                narrationVoiceFiles.ID = Guid.NewGuid();
                var result = _dbContext.Staff_NarrationVoiceFiles.Add(narrationVoiceFiles);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationVoiceFiles UpdateNarrationVoiceFiles(Staff_NarrationVoiceFiles userNarrationVoiceFiles)
        {
            try
            {
                _dbContext.Entry(userNarrationVoiceFiles).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userNarrationVoiceFiles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
