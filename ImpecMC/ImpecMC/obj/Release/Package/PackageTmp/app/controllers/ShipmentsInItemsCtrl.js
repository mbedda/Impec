angular.module('ImpecMC.ShipmentInItemsController', [])
.controller('ShipmentInItemsCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.model = {};

    $scope.ShipmentInId = $('#Id').val();

    $scope.new = {
        Item: {
        }
    };

    $scope.edit = {
        Item: {
        }
    };

    $scope.delete = {
        Item: {
        }
    };

    $http.get('/api/ItemsApi/GetItems/?id=' + $scope.ShipmentInId).success(function (data) {
        $scope.model = data;
    });

    $scope.createItem = function (form) {
        $scope.new.Item.ShipmentInId = $scope.ShipmentInId;
        $http.post('/api/ItemsApi/CreateItem', $scope.new.Item).success(function (data) {
            $http.get('/api/ItemsApi/GetItems/?id=' + $scope.ShipmentInId).success(function (data) {
                $scope.model = data;
            });
            $('#NewItemModal').modal('hide');
            $scope.new.Item = {};
            form.$setPristine();
            form.$setUntouched();
        });
    };

    $scope.cancelNewItem = function (form) {
        if (form) {
            form.$setPristine();
            form.$setUntouched();
            $('#NewItemModal').modal('hide');
            $scope.new.Item = {};
            $scope.new.Item.AutoDispatch = true;
        }
    };

    $scope.showEditModal = function (item) {
        $scope.edit.Item = item;
        $('#EditItemModal').modal('show');
        event.preventDefault();
    };

    $scope.editItem = function (form) {
        if (form) {
            $http.post('/api/ItemsApi/EditItem', $scope.edit.Item).success(function (data) {
                $('#EditItemModal').modal('hide');
                $scope.edit.Item = {};
                form.$setPristine();
                form.$setUntouched();
            });
        }
    };

    $scope.cancelEditItem = function (form) {
        if (form) {
            $http.get('/api/ItemsApi/GetItems/?id=' + $scope.ShipmentInId).success(function (data) {
                $scope.model = data;
            });
            form.$setPristine();
            form.$setUntouched();
            $('#EditItemModal').modal('hide');
            $scope.edit.Item = {};
            $scope.edit.Item.AutoDispatch = true;
        }
    };

    $scope.showDeleteModal = function (item) {
        $scope.delete.Item = item;
        $('#DeleteItemModal').modal('show');
        event.preventDefault();
    };

    $scope.deleteItem = function (id) {
        $http.post('/api/ItemsApi/RemoveItem/?id=' + id).success(function (data) {
            $('#DeleteItemModal').modal('hide');
            $http.get('/api/ItemsApi/GetItems/?id=' + $scope.ShipmentInId).success(function (data) {
                $scope.model = data;
            });
        });
        event.preventDefault();
    };
}]);