using System.ComponentModel.DataAnnotations;
using Bulgarikon.Core.DTOs.AnswerDTOs;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Question;


namespace Bulgarikon.Core.DTOs.QuestionDTOs
{
    public class QuestionFormDto
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = "";

        [Required]
        public Guid QuizId { get; set; }

        public int CorrectAnswerIndex { get; set; } = 0;

        public List<AnswerFormDto> Answers { get; set; } = new()
        {
            new AnswerFormDto(),
            new AnswerFormDto(),
            new AnswerFormDto(),
            new AnswerFormDto()
        };
    }
}
