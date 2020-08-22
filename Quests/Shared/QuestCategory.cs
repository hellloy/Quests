using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quests.Shared
{
    public class QuestCategory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Название категории")]
        public string Name { get; set; }
        public virtual ICollection<Quest> Quests { get; set; }
    }
}
