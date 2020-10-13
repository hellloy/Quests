using System;
using System.Collections.Generic;
using Quests.Shared.Entities.Models;

namespace Quests.Shared.VM
{
    public class UsersVMQuests
    {
        public List<UserVm> Users { get; set; }
        public List<UserQuest> Quests { get; set; }
    }

    public class UserQuest
    {
        public string UserId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Price { get; set; }
        public MyQuestStatus Status { get; set; }
    }
}