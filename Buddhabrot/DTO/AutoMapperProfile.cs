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
				.ForMember(p => p.CreatedUtc, opt => opt.Ignore())
				.ForMember(p => p.StartedUtc, opt => opt.Ignore())
				.ForMember(p => p.CompletedUtc, opt => opt.Ignore())
				.ForMember(p => p.Image, opt => opt.Ignore())
				.ForMember(p => p.State, opt => opt.Ignore());

			CreateMap<BuddhabrotRequest, Plot>()
				.ForMember(p => p.Id, opt => opt.Ignore())
				.ForMember(p => p.PlotType, opt => opt.MapFrom(b => PlotType.Buddhabrot))
				.ForMember(p => p.PlotParams, opt => opt.MapFrom(b => JsonConvert.SerializeObject(b.Parameters)))
				.ForMember(p => p.CreatedUtc, opt => opt.Ignore())
				.ForMember(p => p.StartedUtc, opt => opt.Ignore())
				.ForMember(p => p.CompletedUtc, opt => opt.Ignore())
				.ForMember(p => p.Image, opt => opt.Ignore())
				.ForMember(p => p.State, opt => opt.Ignore());

			CreateMap<Plot, BuddhabrotResponse>()
				.ForMember(b => b.Parameters, opt => opt.MapFrom(p => JsonConvert.DeserializeObject<BuddhabrotParameters>(p.PlotParams ?? string.Empty)));
			CreateMap<Plot, MandelbrotResponse>()
				.ForMember(b => b.Parameters, opt => opt.MapFrom(p => JsonConvert.DeserializeObject<MandelbrotParameters>(p.PlotParams ?? string.Empty)));
		}
	}
}
