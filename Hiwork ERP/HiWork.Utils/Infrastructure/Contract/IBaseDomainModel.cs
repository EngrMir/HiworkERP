namespace HiWork.Utils.Infrastructure.Contract
{
    public interface IBaseDomainModel
    {
        //void SetProperties();
        void SetCreateProperties(long userId);
        void SetUpdateProperties(long userId);
        //void MarkAsDeleted(long userId);
    }
}