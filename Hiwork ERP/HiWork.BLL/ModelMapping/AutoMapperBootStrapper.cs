using AutoMapper;

namespace HiWork.BLL.ModelMapping 
{
	public partial class AutoMapperBootStrapper
	{
		public static void Initialize()
        {
            Mapper.AddProfile(new DatabaseToDomainProfile());
            Mapper.AddProfile(new DomainToDatabaseProfile());
        }
	}
}

