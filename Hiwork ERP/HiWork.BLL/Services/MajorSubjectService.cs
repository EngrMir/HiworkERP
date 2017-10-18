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
    public partial interface IMajorSubjectService : IBaseService<MajorSubjectModel, Master_StaffMajorSubject>
    {
        MajorSubjectModel SaveMajorSubject(MajorSubjectModel aMajorSubjectModel);
        List<MajorSubjectModel> GetAllMajorSubjectList(MajorSubjectModel aMajorSubjectModel);
        MajorSubjectModel EditMajorSubject(MajorSubjectModel aMajorSubjectModel);
        List<MajorSubjectModel> DeleteMajorSubject(MajorSubjectModel aMajorSubjectModel);
    }

    public class MajorSubjectService : BaseService<MajorSubjectModel, Master_StaffMajorSubject>, IMajorSubjectService
    {
        private readonly IMajorSubjectRepository _majorsubRepository;
        public MajorSubjectService(IMajorSubjectRepository majorsubRepository) : base(majorsubRepository)
        {
            _majorsubRepository = majorsubRepository;
        }

        public MajorSubjectModel SaveMajorSubject(MajorSubjectModel aMajorSubjectModel)
        {
            Master_StaffMajorSubject majorsub = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMajorSubjectModel, aMajorSubjectModel.CurrentCulture);
                majorsub = Mapper.Map<MajorSubjectModel, Master_StaffMajorSubject>(aMajorSubjectModel);

                if (aMajorSubjectModel.IsNew())
                {
                    majorsub.ID = Guid.NewGuid();
                    majorsub.CreatedBy = aMajorSubjectModel.CurrentUserID;
                    majorsub.CreatedDate = DateTime.Now;
                    _majorsubRepository.InsertMajorSubject(majorsub);
                }
                else
                {
                    majorsub.UpdatedBy = aMajorSubjectModel.CurrentUserID;
                    majorsub.UpdatedDate = DateTime.Now;
                    _majorsubRepository.UpdateMajorSubject(majorsub);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMajorSubjectModel.CurrentUserID, "MajorSubject", message);
                throw new Exception(ex.Message);
            }
            return aMajorSubjectModel;
        }

        public MajorSubjectModel EditMajorSubject(MajorSubjectModel aMajorSubjectModel)
        {

            try
            {
                var majorsub = Mapper.Map<MajorSubjectModel, Master_StaffMajorSubject>(aMajorSubjectModel);
                Master_StaffMajorSubject aMajorSubject = _majorsubRepository.UpdateMajorSubject(majorsub);
                MajorSubjectModel majorsubModel = Mapper.Map<Master_StaffMajorSubject, MajorSubjectModel>(aMajorSubject);

            }

            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMajorSubjectModel.CurrentUserID, "MajorSubject", message);
                throw new Exception(ex.Message);
            }


            return aMajorSubjectModel;
        }

        public List<MajorSubjectModel> GetAllMajorSubjectList(MajorSubjectModel model)
        {
            List<MajorSubjectModel> majorSubjectList = new List<MajorSubjectModel>();
            MajorSubjectModel majorSubjectModel = new MajorSubjectModel();

            try
            {

                List<Master_StaffMajorSubject> repoMajorSubjectList = _majorsubRepository.GetAllMajorSubjectList();
                if (repoMajorSubjectList != null)
                {
                          foreach (Master_StaffMajorSubject msm in repoMajorSubjectList)
                    {

                        majorSubjectModel = Mapper.Map<Master_StaffMajorSubject, MajorSubjectModel>(msm);

                        majorSubjectModel.Name = Utility.GetPropertyValue(majorSubjectModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(majorSubjectModel, "Name", model.CurrentCulture).ToString();
                        //majorSubjectModel.Code = Utility.GetPropertyValue(majorSubjectModel, "Code", model.CurrentCulture) == null ? string.Empty :
                        //                                      Utility.GetPropertyValue(majorSubjectModel, "Code", model.CurrentCulture).ToString();
                        majorSubjectModel.CurrentUserID = model.CurrentUserID;
                        majorSubjectModel.CurrentCulture = majorSubjectModel.CurrentCulture;
                        majorSubjectModel.CurrentUserID = majorSubjectModel.CurrentUserID;


                        majorSubjectList.Add(majorSubjectModel);
                    }

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "MajorSubject", message);
                throw new Exception(ex.Message);
            }

            return majorSubjectList;
        }
        public List<MajorSubjectModel> DeleteMajorSubject(MajorSubjectModel aMajorSubjectModel)
        {
            List<MajorSubjectModel> majorsubjects;
            try
            {
                _majorsubRepository.DeleteMajorSubject(aMajorSubjectModel.ID);
                majorsubjects = GetAllMajorSubjectList(aMajorSubjectModel);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMajorSubjectModel.CurrentUserID, "MajorSubject", message);
                throw new Exception(ex.Message);
            }
            return majorsubjects;
        }
    }
}
