angular.module('ImpecMC.ShipmentOutDTsController', [])
.controller('ShipmentOutDTsCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.model = {};

    $scope.ShipmentOutId = $('#Id').val();

    $scope.new = {
        DT: {
        },
        DTItem: {
        }
    };

    $scope.edit = {
        DT: {
        },
        DTItem: {
        }
    };

    $scope.delete = {
        DT: {
        },
        DTItem: {
        }
    };

    $scope.selected = {
        DT: {
        },
        DTItem: {
        }
    };

    $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
        $scope.model = data;
    });

    $scope.createDT = function (form) {
        $scope.new.DT.ShipmentOutId = $scope.ShipmentOutId;
        $http.post('/api/DTsApi/CreateDT', $scope.new.DT).success(function (data) {
            $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
                $scope.model = data;
            });
            $('#NewDTModal').modal('hide');
            $scope.new.DT = {};
            form.$setPristine();
            form.$setUntouched();
        });
    };

    $scope.cancelNewDT = function (form) {
        if (form) {
            form.$setPristine();
            form.$setUntouched();
            $('#NewDTModal').modal('hide');
            $scope.new.DT = {};
            $scope.new.DT.AutoDispatch = true;
        }
    };

    $scope.showEditModal = function (item) {
        $scope.edit.DT = item;
        $('#EditDTModal').modal('show');
        event.preventDefault();
    };

    $scope.editDT = function (form) {
        if (form) {
            $http.post('/api/DTsApi/EditDT', $scope.edit.DT).success(function (data) {
                $('#EditDTModal').modal('hide');
                $scope.edit.DT = {};
                form.$setPristine();
                form.$setUntouched();
            });
        }
    };

    $scope.cancelEditDT = function (form) {
        if (form) {
            $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
                $scope.model = data;
            });
            form.$setPristine();
            form.$setUntouched();
            $('#EditDTModal').modal('hide');
            $scope.edit.DT = {};
            $scope.edit.DT.AutoDispatch = true;
        }
    };

    $scope.showDeleteModal = function (item) {
        $scope.delete.DT = item;
        $('#DeleteDTModal').modal('show');
        event.preventDefault();
    };

    $scope.deleteDT = function (id) {
        $http.post('/api/DTsApi/RemoveDT/?id=' + id).success(function (data) {
            $('#DeleteDTModal').modal('hide');
            $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
                $scope.model = data;
            });
        });
        event.preventDefault();
    };


    $scope.showItemsModal = function (item) {
        $scope.selected.DT = item;
        $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
            $scope.dtItemModel = data;
        });
        $('#DTItemsModal').modal('show');
        event.preventDefault();
    };




    $scope.items = {};

    $http.get('/api/DTItemsApi/GetItems/').success(function (data) {
        $scope.items = data;
    });



    $scope.dtItemModel = {};
    

    $scope.createDTItem = function (form) {
        $scope.new.DTItem.DeliveryTicketId = $scope.selected.DT.Id;
        $http.post('/api/DTItemsApi/CreateDTItem', $scope.new.DTItem).success(function (data) {
            $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
                $scope.model = data;
            });
            $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
                $scope.dtItemModel = data;
            });
            $('#NewDTItemModal').modal('hide');
            $scope.new.DTItem = {};
            form.$setPristine();
            form.$setUntouched();
        });
    };

    $scope.cancelNewDTItem = function (form) {
        if (form) {
            form.$setPristine();
            form.$setUntouched();
            $('#NewDTItemModal').modal('hide');
            $scope.new.DTItem = {};
            $scope.new.DTItem.AutoDispatch = true;
        }
    };

    $scope.showEditDTItemModal = function (item) {
        $scope.edit.DTItem = item;
        $('#EditDTItemModal').modal('show');
        event.preventDefault();
    };

    $scope.editDTItem = function (form) {
        if (form) {
            $http.post('/api/DTItemsApi/EditDTItem', $scope.edit.DTItem).success(function (data) {
                $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
                    $scope.dtItemModel = data;
                });
                $('#EditDTItemModal').modal('hide');
                $scope.edit.DTItem = {};
                form.$setPristine();
                form.$setUntouched();
            });
        }
    };

    $scope.cancelEditDTItem = function (form) {
        if (form) {
            $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
                $scope.dtItemModel = data;
            });
            form.$setPristine();
            form.$setUntouched();
            $('#EditDTItemModal').modal('hide');
            $scope.edit.DTItem = {};
            $scope.edit.DTItem.AutoDispatch = true;
        }
    };

    $scope.showDeleteDTItemModal = function (item) {
        $scope.delete.DTItem = item;
        $('#DeleteDTItemModal').modal('show');
        event.preventDefault();
    };

    $scope.cancelDeleteDTItem = function () {
        if (form) {
            $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
                $scope.dtItemModel = data;
            });
            $('#DeleteDTItemModal').modal('hide');
        }
    };

    $scope.deleteDTItem = function (id) {
        $http.post('/api/DTItemsApi/RemoveDTItem/?id=' + id).success(function (data) {
            $('#DeleteDTItemModal').modal('hide');
            $http.get('/api/DTsApi/GetDTs/?id=' + $scope.ShipmentOutId).success(function (data) {
                $scope.model = data;
            });
            $http.get('/api/DTItemsApi/GetDTItems/?id=' + $scope.selected.DT.Id).success(function (data) {
                $scope.dtItemModel = data;
            });
        });
        event.preventDefault();
    };




}]);