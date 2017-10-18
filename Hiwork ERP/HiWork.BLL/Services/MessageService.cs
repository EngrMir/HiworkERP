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

    public interface IMessageService : IBaseService<MessageModel, Message>
    {
         MessageModel SaveMessage(MessageModel model);
        //List<MessageModel> GetAllMessageList(BaseViewModel model);
        List<MessageModel> GetAllMessageByID(BaseViewModel model);
    }
    public class MessageService : BaseService<MessageModel, Message>, IMessageService
    {
        private IMessageRepository msgRepository;
        private CentralDBEntities _dbContext;

        public MessageService(IMessageRepository _msgRepository) : base(_msgRepository)
        {
            msgRepository = _msgRepository;
        }
        

        public MessageModel SaveMessage(MessageModel model)
        {
            IUnitOfWork ouw = new UnitOfWork();
            IMessageRepository rep = new MessageRepository(ouw);
            IMessageService service = new MessageService(rep);
            var map = Mapper.Map<MessageModel, Message>(model);
            try
            {
                if (model.ID>0)
                {
                    rep.UpdateMessge(map);
                }
                else
                {
                    map.CreatedDate = DateTime.Now;
                    rep.InsertMessage(map);
                }
            }
            catch(Exception ex)
            {
                model = null;
               // IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                //errorLog.SetErrorLog(model.CurrentUserID, "TranslatorRegistrationFromTranspro", message);
                throw new Exception(ex.Message);

            }
            return model;

        }

        public List<MessageModel> GetAllMessageByID(BaseViewModel model)
        {
            MessageModel msg;
            var receivemsgList = new List<MessageModel>();
            try
            {
                var msgList = _dbContext.Messages;
                if (msgList != null)
                {
                    foreach (Message msgData in msgList)
                    {
                        msg = Mapper.Map<Message, MessageModel>(msgData);
                        receivemsgList.Add(msg);
                    }
                }
            }
            catch (Exception ex)
            {
               
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            return receivemsgList;
        }

        
    }

    }

 