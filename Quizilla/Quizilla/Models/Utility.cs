using Quizilla.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizilla.Models
{
    public class Utility
    {
        public static int questionNo;
        public static int quizScore = 0;

        public static void ResetQuiz()
        {
            Utility.questionNo = 1;
            Utility.quizScore = 0;
            QuizController.onlyOnceFlag = false;
        }
    }
}