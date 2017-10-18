

/* ******************************************************************************************************************
 * Repository for EmailTemplate Entity
 * Programmed by    :   Md. Al-Amin Hossain (b-Bd_14 Hossain)
 * Date             :   30-Aug-2017
 * *****************************************************************************************************************/

using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public partial interface IEmailTemplateRepository : IBaseRepository<EmailTemplate>
    {

        EmailTemplate InsertEmailTemplate(EmailTemplate template);
       List< EmailTemplate> GetEmailTemplateList();
        List<Master_EmailGroupSettings> GetEmailGroupList();
        List<Master_EmailCategorySettings> GetEmailCategoryList();
        bool UpdateEmailTemplate(EmailTemplate email);

    }
  public  class EmailTemplateRepository : BaseRepository<EmailTemplate, CentralDBEntities>, IEmailTemplateRepository
    {
        private CentralDBEntities _dbContext;
        public EmailTemplateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }



        public EmailTemplate InsertEmailTemplate(EmailTemplate template)
        {
            try
            {
                _dbContext.EmailTemplates.Add(template);
                _dbContext.SaveChanges();

                return template;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EmailTemplate> GetEmailTemplateList()
        {
            return _dbContext.EmailTemplates.ToList();
        }

        public List<Master_EmailCategorySettings>GetEmailCategoryList()
        {
            return _dbContext.Master_EmailCategorySettings.ToList();
        }
        public List<Master_EmailGroupSettings>GetEmailGroupList()
        {
            return _dbContext.Master_EmailGroupSettings.ToList();
        }
      public bool UpdateEmailTemplate(EmailTemplate email)
        {
            try
            {
                _dbContext.Entry(email).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
