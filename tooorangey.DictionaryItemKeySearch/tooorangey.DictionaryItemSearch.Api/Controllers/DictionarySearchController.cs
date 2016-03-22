using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using umbraco.cms.businesslogic;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;
using Language = umbraco.cms.businesslogic.language.Language;


namespace tooorangey.DictionaryItemSearch.Api
{
    public class DictionarySearchController : UmbracoAuthorizedApiController
    {
        //
        // GET: /DictionarySearch/

        private List<Dictionary.DictionaryItem> GetAllDictionaryItems()
        {
            var allDictionaryItems = new List<Dictionary.DictionaryItem>();
            foreach (Dictionary.DictionaryItem item in Dictionary.getTopMostItems)
            {
                this.AddToList(item, ref allDictionaryItems);
            }
            return allDictionaryItems;
        }
        private void AddToList(Dictionary.DictionaryItem item, ref List<Dictionary.DictionaryItem> list)
        {
            list.Add(item);
            if (!item.hasChildren)
            {
                return;
            }
            foreach (Dictionary.DictionaryItem child in item.Children)
            {
                AddToList(child, ref list);
            }
        }
       [HttpGet]
        public IEnumerable<Language> GetAllLanguages()
        {
            var allLangs = Language.GetAllAsList();
            return allLangs;
        }

        [HttpPost]
        public HttpResponseMessage UpdateDictionaryItem(DictionarySearchItem updatedItem)
        {

            int langId = Int32.Parse(updatedItem.Id);
            string key = updatedItem.Key;
            string value = updatedItem.Value;
              var ls = Services.LocalizationService;

            ILanguage lang = ls.GetLanguageById(langId);
            if (lang == null){
                   return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            if (!ls.DictionaryItemExists(key))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
      
            var dicItem = ls.GetDictionaryItemByKey(key);

            foreach(var dicTran in dicItem.Translations){
                if (dicTran.Language.Id == lang.Id ){
           
                    dicTran.Value = value;
                }
            }
 
            ls.Save(dicItem);
    
       
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public DictionarySearchResult SearchDictionaryItems(string searchTerm, int languageId)
        {
            var allDictionaryItems = this.GetAllDictionaryItems();
             var matchingItems = new List<Dictionary.DictionaryItem>();
        var containingItems = new List<Dictionary.DictionaryItem>();
        var searchResult = new DictionarySearchResult() { SearchResults = new List<DictionarySearchItem>(), HasExactMatch = false };

        if (!String.IsNullOrWhiteSpace(searchTerm))
        {
            // need to loop through all dictionary items and find any matching the search text
            foreach (var item in allDictionaryItems)
            {
                string dicValue = item.Value(languageId);
                if (dicValue.ToLower() == searchTerm.Trim().ToLower())
                {
                    // exact match
                    searchResult.HasExactMatch = true;
                    matchingItems.Add(item);
                }
                else // only search for near matches if there are no exact matches is that the right logic ?
                {
                    if (dicValue.ToLower().Contains(searchTerm.Trim().ToLower()) ||item.key.ToLower() == searchTerm.Trim().ToLower())
                    {
                        // contains
                        containingItems.Add(item);

                    }
                }
            }
            if (matchingItems.Any()) //bind any matches
            {
                searchResult.SearchResults = matchingItems.Select(f => new DictionarySearchItem() { Key = f.key, Id = f.id.ToString(), Value = f.Value(languageId) }).ToList();
            }
            else
            {
                if (containingItems.Any()) //bind any near matches
                {
                    searchResult.SearchResults = containingItems.Select(f => new DictionarySearchItem() { Key = f.key, Id = f.id.ToString(), Value = f.Value(languageId) }).ToList();
                }

                else
                {
                    searchResult.StatusMessage = "No Matching Search Results";
                }
            }
          


        }
              return searchResult;

        }

        public class DictionarySearchResult
        {
            public string StatusMessage { get; set; }
            public List<DictionarySearchItem> SearchResults { get; set; }
            public bool HasSearchResults { get { return this.SearchResults.Any(); } }
            public bool HasExactMatch { get; set; }
            public int Count { get { return this.SearchResults.Any() ? this.SearchResults.Count() : 0; } }


        }
        public class DictionarySearchItem
        {
            public string Id { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
