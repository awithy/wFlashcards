using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wFlashcards.Annotations;

namespace wFlashcards
{
    public class FlashcardsViewModel : INotifyPropertyChanged
    {
        string _questionText;

        public string QuestionText
        {
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
            get { return _questionText; }
        }

        string _answerText;
        public string AnswerText
        {
            set
            {
                _answerText = value;
                OnPropertyChanged(nameof(AnswerText));
            }
            get { return _answerText; }
        }

        int _right;
        int _total;

        public string RightWrong
        {
            get { return _right + "/" + _total; }
        }

        FlashcardState _flashcardState = FlashcardState.Questioning;
        public FlashcardState FlashcardState
        {
            set
            {
                _flashcardState = value;
                OnPropertyChanged(nameof(FlashcardState));
            }
            get { return _flashcardState; }
        }

        QuestionsMachine _questionsMachine;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlashcardsViewModel()
        {
            FlashcardState = FlashcardState.Questioning;
            _questionsMachine = new QuestionsMachine();
            _ShowNextQuestion();
        }

        void _ShowNextQuestion()
        {
            var questionAndAnswer = _questionsMachine.GetRandomNext();
            QuestionText = questionAndAnswer.Question;
            AnswerText = questionAndAnswer.Answer;
            FlashcardState = FlashcardState.Questioning;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowAnswer()
        {
            FlashcardState = FlashcardState.ShowingAnswer;
        }

        public void CorrectSelected()
        {
            _ShowNextQuestion();
            _right++;
            _total++;
            OnPropertyChanged(nameof(RightWrong));
        }

        public void IncorrectSelected()
        {
            _ShowNextQuestion();
            _total++;
            OnPropertyChanged(nameof(RightWrong));
        }
    }

    public enum FlashcardState
    {
        Unknown = 0,
        Questioning = 1,
        ShowingAnswer = 2,
    }

    public class QuestionsMachine
    {
        readonly List<QuestionAndAnswer> QuestionsAndAnswers = new List<QuestionAndAnswer>();
        readonly Random _random = new Random();

        public QuestionsMachine()
        {
            var excelDataProvider = new ExcelDataProvider();
            var questionsAndAnswersData = excelDataProvider.ReadQuestionsAndAnswers();
            QuestionsAndAnswers.AddRange(questionsAndAnswersData);
        }

        public QuestionAndAnswer GetRandomNext()
        {
            var nextRandom = _random.Next(0, QuestionsAndAnswers.Count - 1);
            return QuestionsAndAnswers[nextRandom];
        }
    }

    public class QuestionAndAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class ExcelDataProvider
    {
        public IEnumerable<QuestionAndAnswer> ReadQuestionsAndAnswers()
        {
            var excelDataFactory = new LinqToExcel.ExcelQueryFactory(@"QuestionsAndAnswersData.xlsx");
            var rows = excelDataFactory.Worksheet("Sheet1");
            foreach (var row in rows)
            {
                yield return new QuestionAndAnswer {Question = row[0], Answer = row[1]};
            }
        }
    }
}