using AutoMapper;
using Jido.Models;
using Jido.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.UI.Mappings
{
    public class CommandsMappings : Profile
    {
        public CommandsMappings()
        {
            CreateMap<HighLevelCommand, HighLevelCommandViewModel>();
            CreateMap<HighLevelCommandViewModel, HighLevelCommand>();

            CreateMap<ConstantCommand, ConstantCommandViewModel>();
            CreateMap<ConstantCommandViewModel, ConstantCommand>();

            CreateMap<CompositeHighLevelCommand, CompositeHighLevelCommandViewModel>()
                .IncludeBase<HighLevelCommand, HighLevelCommandViewModel>();
            CreateMap<CompositeHighLevelCommandViewModel, CompositeHighLevelCommand>()
                .IncludeBase<HighLevelCommandViewModel, HighLevelCommand>();

            CreateMap<BasicHighLevelCommand, BasicHighLevelCommandViewModel>()
                .IncludeBase<HighLevelCommand, HighLevelCommandViewModel>();
            CreateMap<BasicHighLevelCommandViewModel, BasicHighLevelCommand>()
                .IncludeBase<HighLevelCommandViewModel, HighLevelCommand>();

            CreateMap<LowLevelCommand, LowLevelCommandViewModel>();
            CreateMap<LowLevelCommandViewModel, LowLevelCommand>();

            CreateMap<PressCommand, PressCommandViewModel>()
                .IncludeBase<LowLevelCommand, LowLevelCommandViewModel>();
            CreateMap<PressCommandViewModel, PressCommand>()
                .IncludeBase<LowLevelCommandViewModel, LowLevelCommand>();

            CreateMap<WaitCommand, WaitCommandViewModel>()
                .IncludeBase<LowLevelCommand, LowLevelCommandViewModel>();
            CreateMap<WaitCommandViewModel, WaitCommand>()
                .IncludeBase<LowLevelCommandViewModel, LowLevelCommand>();
        }
    }
}
