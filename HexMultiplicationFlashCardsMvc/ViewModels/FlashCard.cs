
namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    //TODO: refactor and call this question
    public class FlashCard
    {
        public int Id { get; set; }
        public int Multiplier { get; private set; }
        public int Multiplicand { get; private set; }
        public int Product { get { return Multiplier * Multiplicand; } }
        public int? Response { get; set; }

        //navigation
        public int RoundId { get; set; }
        public Round Round { get; set; }

        public FlashCard(int multiplier, int multiplicand)
        {
            this.Multiplier = multiplier;
            this.Multiplicand = multiplicand;
        }
    }
}
