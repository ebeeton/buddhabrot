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
			CreateMap<MandelbrotParameters, Core.Parameters.MandelbrotParameters>();
			CreateMap<BuddhabrotParameters, Core.Parameters.BuddhabrotParameters>();
		}
	}
}
