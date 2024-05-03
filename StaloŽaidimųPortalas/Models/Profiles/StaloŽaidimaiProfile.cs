namespace StaloŽaidimųPortalas.Models.Profiles
{
	using AutoMapper;

	public class StaloŽaidimaiProfile: Profile
	{
		public StaloŽaidimaiProfile()
		{
			CreateMap<Entities.StaloŽaidimas, ViewModels.StaloŽaidimai.StaloŽaidimas>();
		}
	}
}
