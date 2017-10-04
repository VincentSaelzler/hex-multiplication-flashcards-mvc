
namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    class FlashCard
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int Multiplier { get; private set; }
        public int Multiplicand { get; private set; }
        public int Product { get { return Multiplier * Multiplicand; } }
        public int? Response { get; set; }
        public FlashCard(int multiplier, int multiplicand)
        {
            this.Multiplier = multiplier;
            this.Multiplicand = multiplicand;
        }
    }
}
