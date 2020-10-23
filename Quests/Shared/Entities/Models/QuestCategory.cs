using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quests.Shared.Entities.Models
{
    public class QuestCategory
    {
        [Required]
        public int Id { get; set; }
        [Required (ErrorMessage = "Поле Название категории обязательное")]
        [DisplayName("Название категории")]
        public string Name { get; set; }
        public virtual ICollection<Quest> Quests { get; set; }
    }
}
