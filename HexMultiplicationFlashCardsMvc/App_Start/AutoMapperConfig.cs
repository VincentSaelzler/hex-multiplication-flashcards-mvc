using AutoMapper;

namespace HexMultiplicationFlashCardsMvc
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                //nothing to map yet.
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
