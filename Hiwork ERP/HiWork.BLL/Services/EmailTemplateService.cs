/* ******************************************************************************************************************
 * Service for EmailTemplate Entity
 * Programmed by    :   Md. Al-Amin Hossain (b-Bd_14 Hossain)
 * Date             :   30-Aug-2017
 * *****************************************************************************************************************/


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
    public partial interface IEmailTemplateService : IBaseService<EmailTemplateModel, EmailTemplate>
    {
        List<EmailTemplateModel> SaveEmailTemplate(EmailTemplateModel aEmailTemplate);
        List<EmailTemplateModel> GetEmailTemplateList(BaseViewModel model);
        List<MasterEmailCategorySettingsModel> GetEmailCategoryList(BaseViewModel model);
        List<MasterEmailGroupSettingsModel> GetEmailGroupList(BaseViewModel model);
        bool UpdateEmailTemplate(EmailTemplateModel aEmailTemplate);
    }
    public class EmailTemplateService : BaseService<EmailTemplateModel, EmailTemplate>, IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _templateRepository;
        public EmailTemplateService(IEmailTemplateRepository templateRepository) : base(templateRepository)
        {
            _templateRepository = templateRepository;
        }
        public List<EmailTemplateModel> SaveEmailTemplate(EmailTemplateModel aEmailTemplate)
        {
            BaseViewModel model=new BaseViewModel();
            model.CurrentUserID = aEmailTemplate.CurrentUserID;
            model.CurrentCulture = aEmailTemplate.CurrentCulture;
            List<EmailTemplateModel> EmailItemList = new List<EmailTemplateModel>();
            List<EmailTemplateModel> templates = null;
             Utility.SetDynamicPropertyValue(aEmailTemplate, aEmailTemplate.CurrentCulture);

            var template = Mapper.Map<EmailTemplateModel, EmailTemplate>(aEmailTemplate);

            _templateRepository.InsertEmailTemplate(template);
            EmailItemList = GetEmailTemplateList(model); 

            return EmailItemList;
             
        }

        public List<EmailTemplateModel> GetEmailTemplateList(BaseViewModel model)
        {
           

            List<EmailTemplate> dataList;
            List<EmailTemplateModel> templateList = new List<EmailTemplateModel>();
            EmailTemplateModel templateModel = new EmailTemplateModel();

            try
            {
                dataList = _templateRepository.GetEmailTemplateList();
                if (dataList != null)
                {
                    foreach (EmailTemplate a in dataList)
                    {
                        templateModel = Mapper.Map<EmailTemplate, EmailTemplateModel>(a);
                        
                        templateModel.Name = Utility.GetPropertyValue(templateModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(templateModel, "Name", model.CurrentCulture).ToString();
                        templateModel.Subject = Utility.GetPropertyValue(templateModel, "Subject", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(templateModel, "Subject", model.CurrentCulture).ToString();
                        templateModel.Body = Utility.GetPropertyValue(templateModel, "Body", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(templateModel, "Body", model.CurrentCulture).ToString();
                        
                        templateModel.CurrentUserID = model.CurrentUserID;
                        templateModel.CurrentCulture = model.CurrentCulture;
                        templateList.Add(templateModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            templateList.Sort(CompareDivisionByName);
            return templateList;
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "TemplateGroupID", message);
            return message;
        }
        private int CompareDivisionByName(EmailTemplateModel dataModel1, EmailTemplateModel dataModel2)
        {
            int cmpresult=0;
            //cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }

        public bool UpdateEmailTemplate(EmailTemplateModel aEmailTemplate)
        {
            Utility.SetDynamicPropertyValue(aEmailTemplate, aEmailTemplate.CurrentCulture);
            var email = Mapper.Map<EmailTemplateModel, EmailTemplate>(aEmailTemplate);
            var isUpadate = _templateRepository.UpdateEmailTemplate(email);
            //var countryModel = Mapper.Map<Master_Country, CountryModel>(aCountry);
            
            return isUpadate;
        }

        public List<MasterEmailGroupSettingsModel> GetEmailGroupList(BaseViewModel model)
        {
            List<Master_EmailGroupSettings> dataList;
            List<MasterEmailGroupSettingsModel> GroupList = new List<MasterEmailGroupSettingsModel>();
            MasterEmailGroupSettingsModel GroupModel = new MasterEmailGroupSettingsModel();

            dataList = _templateRepository.GetEmailGroupList();
            if (dataList != null)
            {
                foreach (Master_EmailGroupSettings a in dataList)
                {
                    GroupModel = Mapper.Map<Master_EmailGroupSettings, MasterEmailGroupSettingsModel>(a);

                    GroupModel.Name = Utility.GetPropertyValue(GroupModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                    Utility.GetPropertyValue(GroupModel, "Name", model.CurrentCulture).ToString();

                    GroupModel.CurrentUserID = model.CurrentUserID;
                    GroupModel.CurrentCulture = model.CurrentCulture;
                    GroupList.Add(GroupModel);
                }
            }
            return GroupList;

        }

        public List<MasterEmailCategorySettingsModel> GetEmailCategoryList(BaseViewModel model)
        {
            List<Master_EmailCategorySettings> dataList;
            List<MasterEmailCategorySettingsModel> CategoryList = new List<MasterEmailCategorySettingsModel>();
            MasterEmailCategorySettingsModel CategoryModel = new MasterEmailCategorySettingsModel();

            dataList = _templateRepository.GetEmailCategoryList();
            if (dataList != null)
            {
                foreach (Master_EmailCategorySettings a in dataList)
                {
                    CategoryModel = Mapper.Map<Master_EmailCategorySettings, MasterEmailCategorySettingsModel>(a);

                    CategoryModel.Name = Utility.GetPropertyValue(CategoryModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                    Utility.GetPropertyValue(CategoryModel, "Name", model.CurrentCulture).ToString();

                    CategoryModel.CurrentUserID = model.CurrentUserID;
                    CategoryModel.CurrentCulture = model.CurrentCulture;
                    CategoryList.Add(CategoryModel);
                }
            }
            return CategoryList;

        }

    }
}
