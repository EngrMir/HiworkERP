namespace HiWork.Utils.Infrastructure.Contract
{
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Which database server to connect to
        /// </summary>
        string ConnectionString { get; }
    }
}