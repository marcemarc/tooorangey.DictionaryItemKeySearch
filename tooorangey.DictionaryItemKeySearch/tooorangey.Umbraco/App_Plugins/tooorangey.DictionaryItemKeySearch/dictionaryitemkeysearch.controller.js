angular.module("umbraco").controller("tooorangey.DictionaryItemKeySearch", function ($scope, $http,angularHelper,notificationsService) {

    $scope.isSearch = false;
    $scope.hasResults = false;

    $scope.search = function () {

        var selectedLanguageId = $scope.selectedLanguage.id;

        var searchTerm = $scope.searchTerm;
        $scope.editMode = false;

        $http.get("backoffice/api/DictionarySearch/SearchDictionaryItems/?searchTerm=" + searchTerm + "&languageId=" + selectedLanguageId).then(function (response) {
            var matchingItems = response.data;

            $scope.isSearch = true;
            $scope.StatusMessage = matchingItems.StatusMessage;
            $scope.hasResults = matchingItems.HasSearchResults;
            $scope.dictionaryItems = matchingItems.SearchResults;

        });
    };

    $scope.load = function () {

        $scope.editMode = false;
        $scope.SearchTerm = "";
        //get all languages
        $http.get("backoffice/api/DictionarySearch/GetAllLanguages").then(function (response) {
            var allLanguages = response.data;

            $scope.Languages = allLanguages;

            //get current user
            //set selected language to be current users language
            $scope.selectedLanguage = $scope.Languages[0];

        });
    }
    $scope.edit = function (id) {
        $scope.editForm.$setPristine();
            //get dictionary item value for the key and selected language
            top.location = '/umbraco/#/settings/dictionary/edit/' + id;

           // $scope.EditMode = true;
          //  $scope.EditItemText = "the text from the dictionary item"

        };


        $scope.showEditForm = function (editItem) {
            $scope.editMode = true;
            $scope.editItemKey = editItem.Key;
            $scope.editItemText = editItem.Value;

        }
        $scope.cancelEditForm = function () {

            $scope.editMode = false;

            $scope.editItemKey = '';
            $scope.editItemText = '';
            $scope.editForm.$setPristine();
            $scope.search();

        }
        $scope.saveItem = function () {
            var selectedLanguageId = $scope.selectedLanguage.id;
            var editItemKey = $scope.editItemKey;
            var editItemText = $scope.editItemText;
            {msg:'hello word!'}
            $http.post("backoffice/api/DictionarySearch/UpdateDictionaryItem", { Id: String(selectedLanguageId), key: editItemKey, value: editItemText }
            ).then(function (response) {
                //check response status


                if (response.status == 200){
                    notificationsService.success('Dictionary Item ' + editItemKey + ' updated to ', editItemText);

                    $scope.editMode = false;
                    $scope.editItemKey = '';
                    $scope.editItemText = '';
                    $scope.editForm.$setPristine();
                    $scope.search();

                }
                else {
                    notificationsService.error('Error Updating Dictionary Item ' + editItemKey + ': ', 'Language or Dictionary Key was not found');
                }
            });
       };





   $scope.load();


});
