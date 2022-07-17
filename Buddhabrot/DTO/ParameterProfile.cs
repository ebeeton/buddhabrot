﻿using AutoMapper;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Automapper profile for parameter classes.
	/// </summary>
	public class ParameterProfile : Profile
	{
		public ParameterProfile()
		{
			CreateMap<MandelbrotParameters, Core.Parameters.MandelbrotParameters>();
			CreateMap<BuddhabrotParameters, Core.Parameters.BuddhabrotParameters>();
		}
	}
}