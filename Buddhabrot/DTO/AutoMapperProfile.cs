using AutoMapper;
using Buddhabrot.Core.Models;
using Newtonsoft.Json;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Automapper profile for parameter classes.
	/// </summary>
	public class AutoMapperProfile : Profile
	{
		/// <summary>
		/// Instantiates a <see cref="AutoMapperProfile"/>.
		/// </summary>
		public AutoMapperProfile()
		{
			CreateMap<MandelbrotRequest, Plot>()
				.ForMember(p => p.Id, opt => opt.Ignore())
				.ForMember(p => p.PlotType, opt => opt.MapFrom(m => PlotType.Mandelbrot))
				.ForMember(p => p.PlotParams, opt => opt.MapFrom(m => JsonConvert.SerializeObject(m.Parameters)))
				.ForMember(p => p.CreatedUTC, opt => opt.Ignore())
				.ForMember(p => p.PlotBeginUTC, opt => opt.Ignore())
				.ForMember(p => p.PlotEndUTC, opt => opt.Ignore())
				.ForMember(p => p.ImageData, opt => opt.Ignore());

			CreateMap<BuddhabrotRequest, Plot>()
				.ForMember(p => p.Id, opt => opt.Ignore())
				.ForMember(p => p.PlotType, opt => opt.MapFrom(b => PlotType.Buddhabrot))
				.ForMember(p => p.PlotParams, opt => opt.MapFrom(b => JsonConvert.SerializeObject(b.Parameters)))
				.ForMember(p => p.CreatedUTC, opt => opt.Ignore())
				.ForMember(p => p.PlotBeginUTC, opt => opt.Ignore())
				.ForMember(p => p.PlotEndUTC, opt => opt.Ignore())
				.ForMember(p => p.ImageData, opt => opt.Ignore());
		}
	}
}
