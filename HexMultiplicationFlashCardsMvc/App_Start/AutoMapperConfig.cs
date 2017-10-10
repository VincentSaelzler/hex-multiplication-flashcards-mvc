using AutoMapper;
using HexMultiplicationFlashCardsMvc.DAL;
using HexMultiplicationFlashCardsMvc.ViewModels;

namespace HexMultiplicationFlashCardsMvc
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<QuizVm, Quiz>();
                // .ForMember(dest => dest., opts => opts.Ignore);//nothing to map yet.
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
