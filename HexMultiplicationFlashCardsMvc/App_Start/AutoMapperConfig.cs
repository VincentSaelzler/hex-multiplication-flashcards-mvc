using AutoMapper;

namespace HexMultiplicationFlashCardsMvc
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ViewModels.Quiz, DAL.Quiz>()
                    .ForMember(db => db.PersonId, opt => opt.Ignore())
                    .ForMember(db => db.Student, opt => opt.Ignore())
                    .ForMember(db => db.Round, opt => opt.MapFrom(vm => vm.Rounds));

                cfg.CreateMap<ViewModels.Round, DAL.Round>()
                    .ForMember(db => db.Question, opt => opt.MapFrom(vm => vm.Questions));

                cfg.CreateMap<ViewModels.FlashCard, DAL.Question>();


                //.ForMember(db => db.Round, opt => opt.Ignore());
                //cfg.CreateMap<ViewModels.FlashCard, DAL.Question>()
                //  //.ForMember(q => q.Id, opt => opt.Ignore())  //TODO: ensure we never create qs and expect a certain ID
                //  //.ForMember(q => q.RoundId, opt => opt.Ignore());
                //  .ForMember(q => q.Round, opt => opt.Ignore());
                //cfg.CreateMap<DAL.Question, ViewModels.FlashCard>();
                //// .ForMember(fc => fc.)
                ////.ForMember(q => q.Response, opt => opt.Ignore());
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
