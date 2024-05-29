namespace StaloŽaidimųPortalas.Models.ViewModels
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class NaudotojųSąrašas
    {
        public NaudotojoDuomenys Naudotojas { get; set; }

        public NaudotojųDuomenųSąrašas Naudotojai { get; set; }

		public class NaudotojoDuomenys
		{
			public string Id { get; set; }

			public string ElPaštas { get; set; }

			public string Slapyvardis { get; set; }
		}

		public class NaudotojųDuomenųSąrašas
		{
			public List<NaudotojoDuomenys> NaudotojoDuomenys { get; set; }

			public int? DabartinisPuslapis { get; set; }

			public int? PuslapioDydis { get; set; }

			public int? VisoPuslapių { get; set; }
		}
	}
}
