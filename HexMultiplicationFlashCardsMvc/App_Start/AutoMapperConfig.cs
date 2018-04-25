using AutoMapper;

namespace HexMultiplicationFlashCardsMvc
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            //http://docs.automapper.org/en/stable/5.0-Upgrade-Guide.html
            //careful with circular references
            //options are to use .MaxDepth or .PreserveReferences
            //i was unable to determine whether one was "better" than the other

           //All of the confusion around circular references was caused becasue the Round objects did
           //not throw exceptions (even though they have self-referncing Round/Quiz) but the Flashcard/Question
           //objects did.

           //The documentation (linked above) was actually very helpful when read carefully
           //strting from 6.1.0 PreserveReferences is set
           //AUTOMATICALLY at config time whenever the recursion can be detected statically"
            
           //not willing to invest any more time in this rabbit-hole. At this point, I am assuming the circular reference
           //for round were auto-detected, but the circular referece for question was not. Maybe due to inconsistent naming
           //with Question/Flashcard being used interchangibly?

            Mapper.Initialize(cfg =>
            {
                //view models to DB
                cfg.CreateMap<ViewModels.Quiz, DAL.Quiz>()
                    .ForMember(db => db.PersonId, opt => opt.Ignore())
                    .ForMember(db => db.Student, opt => opt.Ignore())
                    .ForMember(db => db.Round, opt => opt.MapFrom(vm => vm.Rounds));

                cfg.CreateMap<ViewModels.Round, DAL.Round>()
                    .ForMember(db => db.Question, opt => opt.MapFrom(vm => vm.Questions));

                cfg.CreateMap<ViewModels.FlashCard, DAL.Question>();

                //DB to view models
                cfg.CreateMap<DAL.Quiz, ViewModels.Quiz>()
                    .ForMember(vm => vm.MinMultiplicand, opt => opt.Ignore())
                    .ForMember(vm => vm.MinMultiplier, opt => opt.Ignore())
                    .ForMember(vm => vm.MaxMultiplicand, opt => opt.Ignore())
                    .ForMember(vm => vm.MaxMultiplier, opt => opt.Ignore())
                    .ForMember(vm => vm.Rounds, opt => opt.MapFrom(db => db.Round));

                cfg.CreateMap<DAL.Round, ViewModels.Round>()
                    .ForMember(vm => vm.Questions, opt => opt.MapFrom(db => db.Question));

                cfg.CreateMap<DAL.Question, ViewModels.FlashCard>()
                    .PreserveReferences();
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}