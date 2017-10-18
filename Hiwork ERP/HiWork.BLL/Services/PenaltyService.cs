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
    public interface IPenaltyService : IBaseService<PenaltyModel, Master_Penalty>
    {
        List<PenaltyModel> SavePenalty(PenaltyModel model);
        List<PenaltyModel> GetAllPenaltyList(BaseViewModel model);
        bool DeletePenalty(BaseViewModel model);
    }
    public class PenaltyService : BaseService<PenaltyModel, Master_Penalty>, IPenaltyService
    {
        private readonly IpenaltyRepository _penaltyRepository;

        public PenaltyService(IpenaltyRepository penaltyRepository) : base(penaltyRepository)
        {
            _penaltyRepository = penaltyRepository;
        }

        public List<PenaltyModel> GetAllPenaltyList(BaseViewModel model)
        {
            List<PenaltyModel> penaltykModel = new List<PenaltyModel>();
            PenaltyModel penaltyModel = new PenaltyModel();
            try
            {
                List<Master_Penalty> dbPenaltyList = _penaltyRepository.GetAllPenaltyList();

                if (model.ApplicationId > 1)
                    dbPenaltyList = dbPenaltyList.Where(x => x.ApplicationId == model.ApplicationId).ToList();

                if (dbPenaltyList != null)
                {
                    dbPenaltyList.ForEach(a =>
                    {
                        penaltyModel = Mapper.Map<Master_Penalty, PenaltyModel>(a);

                        penaltyModel.Application = Mapper.Map<Application, ApplicationModel>(a.Application);

                        if (penaltyModel.Application != null)
                       penaltyModel.ApplicationName = penaltyModel.Application.Name;

                        penaltyModel.Name = Utility.GetPropertyValue(penaltyModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                            Utility.GetPropertyValue(penaltyModel, "Name", model.CurrentCulture).ToString();

                        penaltyModel.Contents = Utility.GetPropertyValue(penaltyModel, "Contents", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(a, "Contents", model.CurrentCulture).ToString();
                        penaltyModel.Response = Utility.GetPropertyValue(penaltyModel, "Response", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(penaltyModel, "Response", model.CurrentCulture).ToString();

                        penaltyModel.CategoryName = Utility.GetPropertyValue(Utility.PenaltyCategoryList.Where(e => e.Id == a.CategoryNo).SingleOrDefault(), "Name", model.CurrentCulture) == null ? string.Empty :
                                                           Utility.GetPropertyValue(Utility.PenaltyCategoryList.Where(e => e.Id == a.CategoryNo).SingleOrDefault(), "Name", model.CurrentCulture).ToString();
                        penaltykModel.Add(penaltyModel);
                    });
                }

            }
            catch (Exception ex)
            {
               
            }

            return penaltykModel;
        }

        public List<PenaltyModel> SavePenalty(PenaltyModel model)
        {
            List<PenaltyModel> penaltyList = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var penalty = Mapper.Map<PenaltyModel, Master_Penalty>(model);
                penalty.ApplicationId = model.ApplicationId;
                if (model.ID == Guid.Empty)
                {
                    penalty.ID = Guid.NewGuid();
                    penalty.CreatedBy = model.CurrentUserID;
                    penalty.CreatedDate = DateTime.Now;
                    _penaltyRepository.InsertPenalty(penalty);
                }
                else
                {
                    penalty.UpdatedBy = model.CurrentUserID;
                    penalty.UpdatedDate = DateTime.Now;
                    _penaltyRepository.UpdatePenalty(penalty);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                baseViewModel.ApplicationId = model.ApplicationId;
                penaltyList = GetAllPenaltyList(baseViewModel);
            }
            catch (Exception ex)
            {
            }
            return penaltyList;
        }
        public bool DeletePenalty(BaseViewModel model)
        {
          var result=  _penaltyRepository.DeletePenalty(model);
            return result;
        }
    }
}
