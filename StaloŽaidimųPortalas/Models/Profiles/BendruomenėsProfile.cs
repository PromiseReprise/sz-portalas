namespace StaloŽaidimųPortalas.Models.Profiles
{
	using AutoMapper;

	public class BendruomenėsProfile : Profile
	{
		public BendruomenėsProfile()
		{
			CreateMap<Entities.Bendruomenė, ViewModels.Bendruomenės.Bendruomenės.Bendruomenė>();
		}
	}
}
