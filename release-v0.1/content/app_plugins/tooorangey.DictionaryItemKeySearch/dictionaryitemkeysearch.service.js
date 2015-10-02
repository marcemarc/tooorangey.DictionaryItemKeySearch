    angular.module("umbraco.services").factory("dictionarySearchService", function ($http) {  
        return {
            SearchDictionaryItems : function(){
                return $http.get("backoffice/api/DictionarySearch/SearchDictionaryItems/?searchTerm=&languageId=");
            }
        };
    });