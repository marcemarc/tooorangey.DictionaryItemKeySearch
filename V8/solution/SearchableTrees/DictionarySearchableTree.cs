using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tooorangey.DictionaryItemKeySearch.Dtos;
using Umbraco.Core.Scoping;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.Trees;

namespace tooorangey.DictionaryItemKeySearch.SearchableTrees
{
    public class DictionarySearchableTree : ISearchableTree
    {
        private IScopeProvider _scopeProvider;
        public DictionarySearchableTree(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
        public string TreeAlias => Umbraco.Core.Constants.Trees.Dictionary;

        public IEnumerable<SearchResultEntity> Search(string query, int pageSize, long pageIndex, out long totalFound, string searchFrom = null)
        {
            totalFound = 0;
            List<SearchResultEntity> searchResults = new List<SearchResultEntity>();
            if (!string.IsNullOrEmpty(query) && query.Length > 2)
            {
                var dictionaryTerms = PerformSearch(query);
                totalFound = dictionaryTerms.Count();
                foreach (var dicTerm in dictionaryTerms)
                {
                    //is the match on the dictionaryTerm or the value
                    if (dicTerm.Key.StartsWith(query, System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        searchResults.Add(new SearchResultEntity() { Name = dicTerm.Key, Id = dicTerm.PrimaryKey, Key = dicTerm.UniqueId });
                    }
                    else
                    {
                        var languageTextMatches = new CommaDelimitedStringCollection();
                        foreach (var languageText in dicTerm.LanguageTextDtos)
                        {
                            languageTextMatches.Add(languageText.LanguageIsoCode);
                        }
                        searchResults.Add(new SearchResultEntity() { Name = dicTerm.Key + " (" + languageTextMatches.ToString() + ")", Id = dicTerm.PrimaryKey, Key = dicTerm.UniqueId });
                    }

                }
            }
            return searchResults;
        }

        public IEnumerable<DictionaryDto> PerformSearch(string searchTerm)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = "select cd.*, clt.*, ul.LanguageIsoCode from cmsDictionary cd INNER JOIN cmsLanguageText clt on cd.id = clt.UniqueId INNER JOIN umbracoLanguage ul on ul.[id] = clt.languageId WHERE cd.[key] like @0 OR clt.[value] like @1 order by cd.[key]";
                return scope.Database.FetchOneToMany<DictionaryDto>(x => x.LanguageTextDtos, sql, searchTerm + "%", "%" + searchTerm + "%");
            }
        }
    }
}
