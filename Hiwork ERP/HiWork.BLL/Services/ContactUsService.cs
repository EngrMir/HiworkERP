using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
   public partial interface IContactUsService : IBaseService<ContactUsModel,ContactU>
    {
        ContactUsModel SaveContactUs(ContactUsModel contactus);
        List<ContactUsModel> GetContactus(BaseViewModel contactus);
        List<ContactUsModel> DeleteContactUs(ContactUsModel contactus);

    }
   public class ContactUsService: BaseService<ContactUsModel, ContactU>, IContactUsService
    {
       private readonly IContactUsRepository _contactusRepository;
        public ContactUsService(IContactUsRepository contactusRepository) : base(contactusRepository)
        {
            _contactusRepository = contactusRepository;
        }
       public ContactUsModel SaveContactUs(ContactUsModel contactus)
        {
            if(!contactus.IsApplication)
            {
                Utility.SetDynamicPropertyValue(contactus, contactus.CurrentCulture);
                var contact = Mapper.Map<ContactUsModel, ContactU>(contactus);
                if (contact.ID == Guid.Empty)
                {
                    contact.ID = Guid.NewGuid();
                    contact.CreatedDate = DateTime.Now;
                    _contactusRepository.InsertContactus(contact);
                }
                else
                {
                    _contactusRepository.UpdateContact(contact);
                }
            }



            new EmailService().SendContactusEmail(contactus);


            return contactus;
        }
        public List<ContactUsModel> GetContactus(BaseViewModel contactus)
        {
            List<ContactUsModel> contactusList = new List<ContactUsModel>();
            ContactUsModel contactmodel = new ContactUsModel();
            try
           {
                List<ContactU> repocontact = _contactusRepository.GetContactus();
                if(repocontact != null)
                {
                     foreach(ContactU contact in repocontact)
                                                             {
                         contactmodel = Mapper.Map<ContactU,ContactUsModel >(contact);
                        //contactmodel.Division = Mapper.Map<Master_Division, DivisionModel>(contact.Master_Division);
                        contactmodel.UserInformation = Mapper.Map<UserInformation, UserInfoModel>(contact.UserInformation);
                        contactmodel.Name= Utility.GetPropertyValue(contactmodel, "Name", contactus.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(contactmodel, "Name", contactus.CurrentCulture).ToString();
                        contactmodel.Comment = Utility.GetPropertyValue(contactmodel, "Comment", contactus.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(contactmodel, "Comment", contactus.CurrentCulture).ToString();
                        //if (contactmodel.Division != null)
                        //    contactmodel.Division.Name = Utility.GetPropertyValue(contactmodel.Division, "Name", contactus.CurrentCulture) == null ? string.Empty :
                        //                                          Utility.GetPropertyValue(contactmodel.Division, "Name", contactus.CurrentCulture).ToString();
                        contactmodel.CurrentUserID = contactus.CurrentUserID;
                        contactmodel.CurrentCulture = contactus.CurrentCulture;
                        contactusList.Add(contactmodel);

                    }
                }

            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(contactus.CurrentUserID, "ContactUs", message);
                throw new Exception(ex.Message);
            }
            return contactusList;
        }
      public List<ContactUsModel> DeleteContactUs(ContactUsModel contactus)
        {
            try
            {
                contactus.IsDeleted = true;
                this.SaveContactUs(contactus);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(contactus.CurrentUserID, "ContactUs", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = contactus.CurrentCulture;
            model.CurrentUserID = contactus.CurrentUserID;
            return this.GetContactus(model);
        }

    }
}
