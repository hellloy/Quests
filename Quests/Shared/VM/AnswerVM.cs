using System;
using System.Collections.Generic;
using System.Text;

namespace Quests.Shared.VM
{
   public class AnswerVM
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool Finish { get; set; }
        public int MyQuestStepId { get; set; } = 0;
    }
}
