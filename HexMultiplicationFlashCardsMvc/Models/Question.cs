using System;
using System.Globalization;

namespace HexMultiplicationFlashCardsMvc.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int Multiplicand { get; private set; }
        public int Multiplier { get; private set; }
        public int Product { get { return Multiplicand * Multiplier; } }
        public int? Response { get; set; }

        //navigation
        public int RoundId { get; set; }
        //TODO: make round domain object?
        //public Round Round { get; set; }

        public Question(int multiplicand, int multiplier)
        {
            this.Multiplier = multiplier;
            this.Multiplicand = multiplicand;
        }

        public Question(ViewModels.FlashCard vmQuestion)
        {
            int multiplicand, multiplier, response;

            //check for null multiplicand and multiplier
            if
            (
                vmQuestion.Multiplicand == string.Empty ||
                vmQuestion.Multiplier == string.Empty
            )
            {
                throw new Exception($"{nameof(ViewModels.FlashCard)} is missing {nameof(ViewModels.FlashCard.Multiplicand)} or {nameof(ViewModels.FlashCard.Multiplier)}");
            }
            //attempt to parse multiplicand and multiplier
            if
            (
                int.TryParse(vmQuestion.Multiplicand, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out multiplicand) == false ||
                int.TryParse(vmQuestion.Multiplier, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out multiplier) == false
            )
            {
                throw new Exception($"{nameof(ViewModels.FlashCard)} has invalid values for {nameof(ViewModels.FlashCard.Multiplicand)} or {nameof(ViewModels.FlashCard.Multiplier)}");
            }
            //attempt to parse  response if it exists
            if
            (

                int.TryParse(vmQuestion.Response, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out response) == false &&
                vmQuestion.Response != string.Empty
            )
            {
                throw new Exception($"{nameof(ViewModels.FlashCard)} has invalid value for {nameof(ViewModels.FlashCard.Response)}.");
            }

            //navigation
            this.Id = vmQuestion.Id;
            this.RoundId = vmQuestion.RoundId;
            //parsed integers
            this.Multiplier = multiplier;
            this.Multiplicand = multiplicand;
            this.Response = response;
        }
    }
}