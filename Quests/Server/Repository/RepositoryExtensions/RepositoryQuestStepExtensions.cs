using System;
using Quests.Shared.Entities.Models;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Quests.Server.Repository.RepositoryExtensions
{
    public static class RepositoryQuestStepExtensions
    {
        public static IQueryable<QuestStep> Search(this IQueryable<QuestStep> questSteps, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return questSteps;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return questSteps.Include(x=>x.Quest).Where(q => q.Name.ToLower().Contains(lowerCaseSearchTerm) || q.Quest.Name.ToLower().Contains(lowerCaseSearchTerm) || q.Description.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<QuestStep> Sort(this IQueryable<QuestStep> questSteps, string orderByQueryString) 
        { 
            if (string.IsNullOrWhiteSpace(orderByQueryString)) 
                return questSteps.OrderBy(e => e.Name); 
            
            var orderParams = orderByQueryString.Trim().Split(','); 
            var propertyInfos = typeof(QuestStep).GetProperties(BindingFlags.Public | BindingFlags.Instance); 
            var orderQueryBuilder = new StringBuilder(); 
            
            foreach (var param in orderParams) 
            { 
                if (string.IsNullOrWhiteSpace(param)) 
                    continue; 
                
                var propertyFromQueryName = param.Split(" ")[0]; 
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase)); 
                
                if (objectProperty == null) 
                    continue; 
                
                var direction = param.EndsWith(" desc") ? "descending" : "ascending"; 
                orderQueryBuilder.Append($"{objectProperty.Name} {direction}, "); 
            } 
            
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' '); 
            if (string.IsNullOrWhiteSpace(orderQuery)) 
                return questSteps.OrderBy(e => e.Name); 
            
            return questSteps.OrderBy(orderQuery); 
        }
    }
}
