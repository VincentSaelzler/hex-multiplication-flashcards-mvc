using AutoMapper;

namespace HexMultiplicationFlashCardsMvc
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ViewModels.FlashCard, DAL.Question>()
                  //.ForMember(q => q.Id, opt => opt.Ignore())  //TODO: ensure we never create qs and expect a certain ID
                  //.ForMember(q => q.RoundId, opt => opt.Ignore());
                  .ForMember(q => q.Round, opt => opt.Ignore());
                cfg.CreateMap<DAL.Question, ViewModels.FlashCard>();
                // .ForMember(fc => fc.)
                //.ForMember(q => q.Response, opt => opt.Ignore());
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
