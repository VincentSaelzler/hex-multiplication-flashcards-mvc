
using System;
using System.Web.Mvc;

namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    //TODO: refactor and call this question
    public class FlashCard
    {
        public int Id { get; set; }
        public int Multiplicand { get; private set; }
        public int Multiplier { get; private set; }
        public int Product { get { return Multiplicand * Multiplier; } }
        public int? Response { get; set; }

        //navigation
        public int RoundId { get; set; }
        public Round Round { get; set; }

        public FlashCard(int multiplicand, int multiplier)
        {
            this.Multiplier = multiplier;
            this.Multiplicand = multiplicand;
        }
    }
    //https://stackoverflow.com/questions/9413313/posting-data-when-my-view-model-has-a-constructor-does-not-work#_=_
    public class FlashCardModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            //get incoming
            var multiplicandVpr = bindingContext.ValueProvider.GetValue(nameof(FlashCard.Multiplicand));
            var multiplierVpr = bindingContext.ValueProvider.GetValue(nameof(FlashCard.Multiplier));

            //check for null and parse
            if (multiplicandVpr == null || multiplierVpr == null)
            {
                throw new Exception($"{nameof(FlashCard)} is missing {nameof(FlashCard.Multiplicand)} or {nameof(FlashCard.Multiplier)}");
            }
            int multiplicand, multiplier;
            if (int.TryParse(multiplicandVpr.AttemptedValue, out multiplicand) == false || int.TryParse(multiplierVpr.AttemptedValue, out multiplier) == false)
            {
                throw new Exception($"{nameof(FlashCard)} has invalid values for {nameof(FlashCard.Multiplicand)} or {nameof(FlashCard.Multiplier)}");
            }

            //create object
            return new FlashCard(multiplicand, multiplier);
        }
    }
}
