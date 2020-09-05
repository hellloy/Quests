using System;
using Quests.Shared.Entities.Models;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Quests.Server.Repository.RepositoryExtensions
{
    public static class RepositoryQuestExtensions
    {
        public static IQueryable<Quest> Search(this IQueryable<Quest> quests, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return quests;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return quests.Where(q => q.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Quest> Sort(this IQueryable<Quest> quests, string orderByQueryString) 
        { 
            if (string.IsNullOrWhiteSpace(orderByQueryString)) 
                return quests.OrderBy(e => e.Name); 
            
            var orderParams = orderByQueryString.Trim().Split(','); 
            var propertyInfos = typeof(Quest).GetProperties(BindingFlags.Public | BindingFlags.Instance); 
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
                return quests.OrderBy(e => e.Name); 
            
            return quests.OrderBy(orderQuery); 
        }
    }
}
