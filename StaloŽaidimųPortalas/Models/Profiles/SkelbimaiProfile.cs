namespace StaloŽaidimųPortalas.Models.Profiles
{
	using AutoMapper;

	public class SkelbimaiProfile : Profile
	{
		public SkelbimaiProfile()
		{
			CreateMap<Entities.StaloŽaidimas, ViewModels.Skelbimai.Skelbimai.StaloŽaidimai>();
		}
	}
}
