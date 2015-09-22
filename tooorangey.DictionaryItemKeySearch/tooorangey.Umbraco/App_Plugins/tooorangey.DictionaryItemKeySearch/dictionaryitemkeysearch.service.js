    angular.module("umbraco.services").factory("dictionarySearchService", function ($http) {  
        return {
            SearchDictionaryItems : function(){
                return $http.get("/umbraco/backoffice/api/DictionarySearch/SearchDictionaryItems/?searchTerm=&languageId=");
            }
        };
    });