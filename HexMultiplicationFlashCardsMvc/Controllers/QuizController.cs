using HexMultiplicationFlashCardsMvc.Repositories;
using HexMultiplicationFlashCardsMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Index()
        {
            //Students/Create
            int studentId = FlashCardRepo.NewStudent();

            //Quizzes/Create
            int quizId = FlashCardRepo.NewQuiz(studentId);

            IEnumerable<FlashCard> cards = NewCards();
            CreateRound(quizId, cards);

            //Quizzes/5
            Debug.WriteLine($"Done! Overall {FlashCardRepo.NumQuizQuestions(quizId)} Attempts, {FlashCardRepo.NumQuizQuestionsCorrect(quizId)} Correct, and {FlashCardRepo.NumQuizQuestionsWrong(quizId)} Wrong.");
            Console.ReadLine();

            return View();
        }
        public static void ActionEditRound(int roundId)
        {
            //get info
            IEnumerable<FlashCard> flashCards = FlashCardRepo.GetCards(roundId);
            int roundNumber = FlashCardRepo.GetCurrentRoundNum(roundId);

            //prompt
            foreach (var fc in flashCards)
            {
                Debug.WriteLine($"{fc.Multiplicand} X {fc.Multiplier} ?");
                fc.Response = int.Parse(Console.ReadLine());
            }

            //save (this would actually be a POST to //Rounds/Edit/4)
            FlashCardRepo.SaveQuestions(flashCards);
            Debug.WriteLine($"Round {roundNumber} Complete. {flashCards.Where(fc => fc.Response == fc.Product).Count()} Correct, {flashCards.Where(fc => fc.Response != fc.Product).Count()} Wrong.");

            //generate a new round if necessary
            if (flashCards.Any(fc => fc.Response != fc.Product))
            {
                flashCards = flashCards.Where(fc => fc.Response != fc.Product);
                CreateRound(FlashCardRepo.GetQuizId(roundId), flashCards);
            }
        }
        private static void CreateRound(int quizId, IEnumerable<FlashCard> cards)
        {
            //create new round and add questions
            int roundNumber = FlashCardRepo.GetMaxRoundNum(quizId) + 1; //returns a default int value of 0 when no rounds exist for the quiz yet
            int roundId = FlashCardRepo.NewRound(quizId, roundNumber);
            FlashCardRepo.NewQuestions(roundId, cards);

            //call the edit page
            ActionEditRound(roundId);
        }
        private static IEnumerable<FlashCard> NewCards()
        {
            const int defaultMinValue = 4;  //arbitrary choice
            const int defaultMaxValue = 5;  //arbitrary choice

            //get maximum factors
            Debug.WriteLine("");
            Debug.WriteLine("Choose the Minimum Multiplier");
            int minMultiplier = ParseIntValue(defaultMinValue);
            Debug.WriteLine("");
            Debug.WriteLine("Choose the Minimum Multiplicand");
            int minMultiplicand = ParseIntValue(defaultMinValue);
            Debug.WriteLine("");
            Debug.WriteLine("Choose the Max Multiplier");
            int maxMultiplier = ParseIntValue(defaultMaxValue);
            Debug.WriteLine("");
            Debug.WriteLine("Choose the Max Multiplicand");
            int maxMultiplicand = ParseIntValue(defaultMaxValue);

            //populate card list
            IList<FlashCard> cards = new List<FlashCard>();
            for (int multiplier = minMultiplier; multiplier <= maxMultiplier; multiplier++)
            {
                for (int multiplicand = minMultiplicand; multiplicand <= maxMultiplicand; multiplicand++)
                {
                    cards.Add(new FlashCard(multiplier, multiplicand));
                }
            }

            return cards;
        }
        public static void ShowIntroduction()
        {
            //intro text
            Debug.WriteLine("DIRECTIONS");
            Debug.WriteLine("This program repeatedly asks multication problems.");
            Debug.WriteLine($"Type your answer in hex format, and then push 'Enter'.");
        }
        public static int ParseIntValue(int defaultValue) //TODO: move this out of this class
        {
            const string hexFormat = "X";

            //containers
            int value;
            bool valueSelected = false;

            do
            {
                //container
                string resp;

                //prompt
                Debug.WriteLine($"Push 'Enter' for default value of '{defaultValue.ToString(hexFormat)}'");

                //gather response
                resp = Console.ReadLine();

                valueSelected = false;

                if (resp == string.Empty)
                {
                    //use default value
                    valueSelected = true;
                    Debug.WriteLine($"Using default value {defaultValue.ToString(hexFormat)}");
                    value = defaultValue;
                }
                else
                {
                    //parse entered response
                    if (int.TryParse(resp, System.Globalization.NumberStyles.HexNumber, null, out value))
                    {
                        //use parsed response
                        valueSelected = true;
                        Debug.WriteLine($"Using entered value {value.ToString(hexFormat)}");
                    }
                    else
                    {
                        //display guidance
                        Debug.WriteLine($"Enter a number in a valid hex format");
                    }
                }
            } while (valueSelected == false);

            return value;
        }
    }
}