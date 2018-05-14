namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    public class FlashCard
    {
        public int Id { get; set; }
        public string Multiplicand { get; set; }
        public string Multiplier { get; set; }
        public string Product { get; set; }
        public string Response { get; set; }

        //navigation
        public int RoundId { get; set; }
        public Round Round { get; set; }
    }
}
//TODO: document why domain based approach is better here. Specifically, when posting a VM with 
// public int? Response = "f" for exammple, the model binder (even using the override CreateModel()) would
// interpret that as null. Therefore, changing to having the domain model be int and the view model be string
//TODO: refactor and call this question
//public class FlashCard
//{
//    public int Id { get; set; }
//    public int Multiplicand { get; private set; }
//    public int Multiplier { get; private set; }
//    public int Product { get { return Multiplicand * Multiplier; } }
//    public int? Response { get; set; }

//    //navigation
//    public int RoundId { get; set; }
//    public Round Round { get; set; }

//    public FlashCard(int multiplicand, int multiplier)
//    {
//        this.Multiplier = multiplier;
//        this.Multiplicand = multiplicand;
//    }
//}
//public class FlashCard
//{
//    public int Id { get; set; }
//    public int Multiplicand { get; private set; }
//    public int Multiplier { get; private set; }
//    public int Product { get { return Multiplicand * Multiplier; } }
//    public int? Response { get; set; }

//    //navigation
//    public int RoundId { get; set; }
//    public Round Round { get; set; }

//    public FlashCard(int multiplicand, int multiplier)
//    {
//        this.Multiplier = multiplier;
//        this.Multiplicand = multiplicand;
//    }
//}
//https://stackoverflow.com/questions/9413313/posting-data-when-my-view-model-has-a-constructor-does-not-work#_=_
//public class FlashCardModelBinder : DefaultModelBinder
//{
//    protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
//    {
//        //get incoming
//        var multiplicandVpr = bindingContext.ValueProvider.GetValue(nameof(FlashCard.Multiplicand));
//        var multiplierVpr = bindingContext.ValueProvider.GetValue(nameof(FlashCard.Multiplier));
//        var responseVpr = bindingContext.ValueProvider.GetValue(nameof(FlashCard.Response));

//        //check for null and parse
//        if (multiplicandVpr == null || multiplierVpr == null)
//        {
//            throw new Exception($"{nameof(FlashCard)} is missing {nameof(FlashCard.Multiplicand)} or {nameof(FlashCard.Multiplier)}");
//        }

//        int multiplicand, multiplier;
//        if (int.TryParse(multiplicandVpr.AttemptedValue, out multiplicand) == false || int.TryParse(multiplierVpr.AttemptedValue, out multiplier) == false)
//        {
//            throw new Exception($"{nameof(FlashCard)} has invalid values for {nameof(FlashCard.Multiplicand)} or {nameof(FlashCard.Multiplier)}");
//        }

//        //create object
//        var flashCard = new FlashCard(multiplicand, multiplier);

//        if (responseVpr.AttemptedValue != string.Empty)
//        {
//            int response;
//            if (int.TryParse(responseVpr.AttemptedValue, out response))
//            {
//                flashCard.Response = response;
//            }
//            else
//            {
//                throw new Exception($"{nameof(FlashCard)} has invalid value for {nameof(FlashCard.Response)}.");
//            }
//        }

//        return flashCard;
//    }
//}
