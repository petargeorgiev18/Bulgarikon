using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Common.DataModelsValidation
{
    public static class EntityClassesValidations
    {
        public static class Answer
        {
            public const int TextMaxLength = 100;
            public const int TextMinLength = 15;
        }
        public static class Artifact
        {
            public const int NameArtifactMaxLength = 100;
            public const int NameArtifactMinLength = 3;
            public const int MaterialMaxLength = 50;
            public const int MaterialMinLength = 3;
            public const int LocationMaxLength = 100;
            public const int LocationMinLength = 3;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 20;
            public const int ImageUrlMaxLength = 2083;
            public const int YearMaxValue = 1899;
            public const int YearMinValue = -300000;
        }
        public static class Civilization
        {
            public const int NameCivilizationMaxLength = 100;
            public const int NameCivilizationMinLength = 3;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 20;
            public const int ImageUrlMaxLength = 2048;
            public const int StartYearMaxValue = 2025;
            public const int StartYearMinValue = -300000;
            public const int EndYearMaxValue = 2025;
            public const int EndYearMinValue = -300000;
        }
        public static class Era
        {
            public const int NameEraMaxLength = 100;
            public const int NameEraMinLength = 3;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 20;
            public const int StartYearMaxValue = 2025;
            public const int StartYearMinValue = -300000;
            public const int EndYearMaxValue = 2025;
            public const int EndYearMinValue = -300000;
        }
        public static class Event
        {
            public const int NameEventMaxLength = 100;
            public const int NameEventMinLength = 3;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 20;
            public const int LocationMaxLength = 100;
            public const int LocationMinLength = 3;
            public const int ImageUrlMaxLength = 2048;
            public const int YearMaxValue = 2025;
            public const int YearMinValue = -300000;
            public const int TitleMaxLength = 50;
            public const int TitleMinLength = 10;
            public const int StartYearMaxValue = 2025;
            public const int StartYearMinValue = -300000;
            public const int EndYearMaxValue = 2025;
            public const int EndYearMinValue = -300000;
        }
        public static class Figure
        {
            public const int NameFigureMaxLength = 100;
            public const int NameFigureMinLength = 3;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 20;
            public const int BirthYearMaxValue = 2025;
            public const int BirthYearMinValue = -300000;
            public const int DeathYearMaxValue = 2025;
            public const int DeathYearMinValue = -300000;
        }
        public static class Image
        {
            public const int UrlMaxLength = 2083;
        }
        public static class Question
        {
            public const int TextMaxLength = 80;
            public const int TextMinLength = 15;
        }
        public static class User
        {
            public const int UsernameMaxLength = 20;
            public const int UsernameMinLength = 3;
            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;
            public const int EmailMaxLength = 100;
            public const int EmailMinLength = 10;
            public const int ProfileImageUrlMaxLength = 2048;
        }
        public static class Quiz
        {
            public const int TitleMaxLength = 100;
            public const int TitleMinLength = 5;
        }
        public static class QuizResult
        {
            public const int ScoreMinValue = 0;
            public const int ScoreMaxValue = 100;
        }
        public static class Feedback
        {
            public const int CommentMaxLength = 1000;
            public const int CommentMinLength = 10;
        }
    }
}
