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
    public partial interface IMessageRepository : IBaseRepository<Message>
    {
        List<Message> GetAllMESSAGE(BaseViewModel model);
        Message GetMessageByID(long Id);
        Message InsertMessage(Message Message);
        Message UpdateMessge(Message Message);
        List<Message> GetAllByReceiverID(Guid ID);
        List<Message> GetAllBySenderID(Guid ID);
        List<Message> GetDetailsBymsgid(long ID);
        bool DeleteMessage(long Id);
    }

    public class MessageRepository : BaseRepository<Message, CentralDBEntities>, IMessageRepository, IDisposable
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
        public MessageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteMessage(long ID)
        {
            try
            {
                var msg = _dbContext.Messages.ToList().Find(d => d.ID == ID);
                if (msg != null)
                {
                    _dbContext.Messages.Remove(msg);
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

        public List<Message> GetAllMESSAGE(BaseViewModel model)
        {
            try
            {
                return _dbContext.Messages.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Message GetMessageByID(long Id)
        {
                return _dbContext.Messages.FirstOrDefault(u => u.ID == Id);
        }

        public List<Message> GetAllByReceiverID(Guid ID)
        {
            return _dbContext.Messages.Where(msg => msg.ReceiverID == ID).ToList();
        }

        public List<Message> GetAllBySenderID(Guid ID)
        {
            return _dbContext.Messages.Where(msg => msg.SenderID == ID).ToList();
        }

        public List<Message> GetDetailsBymsgid(long ID)
        {
            return _dbContext.Messages.Where(msg => msg.ID == ID).ToList();
        }

        public Message InsertMessage(Message model)
        {
            try
            {
                _dbContext.Messages.Add(model);
                _dbContext.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Message UpdateMessge(Message message)
        {
                _dbContext.Entry(message).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return message;
            
        }
    }
}