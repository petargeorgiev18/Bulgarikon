using System.ComponentModel.DataAnnotations;
using Bulgarikon.ViewModels.AnswerViewModels;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Question;


namespace Bulgarikon.ViewModels.QuestionViewModels
{
    public class QuestionFormViewModel
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = "";

        [Required]
        public Guid QuizId { get; set; }

        public int CorrectAnswerIndex { get; set; } = 0;

        public List<AnswerFormViewModel> Answers { get; set; } = new()
        {
            new AnswerFormViewModel(),
            new AnswerFormViewModel(),
            new AnswerFormViewModel(),
            new AnswerFormViewModel()
        };
    }
}
