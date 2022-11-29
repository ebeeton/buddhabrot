using AutoMapper;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Automapper profile for parameter classes.
	/// </summary>
	public class ParameterProfile : Profile
	{
		/// <summary>
		/// Instantiates a <see cref="ParameterProfile"/>.
		/// </summary>
		public ParameterProfile()
		{
			CreateMap<MandelbrotParameters, Core.Models.MandelbrotParameters>()
				.ForMember(p => p.Type, opt => opt.Ignore());
			CreateMap<BuddhabrotParameters, Core.Models.BuddhabrotParameters>()
				.ForMember(p => p.Type, opt => opt.Ignore());
		}
	}
}
