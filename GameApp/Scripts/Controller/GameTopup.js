var app = angular.module("myApp", []);
//app.directive("selectNgFiles", function () {
//    return {
//        require: "ngModel", link: function postLink(scope, elem, attrs, ngModel) { elem.on("change", function (e) { var files = elem[0].files; ngModel.$setViewValue(files); }) }
//    }
//});
app.controller("GameTopup", function ($scope, $http, $filter, FileUploadService) {
    Clear();
    function Clear() {
        $scope.btnSave = 'Save Topup';
        $scope.GameTopup = {};
        $scope.GameTopup.GameTopupId = 0;
        $scope.GameTopup.ProductIds = "";
        $scope.GameTopupList = [];
        $scope.ProductList = [];
        $scope.Count = 0;
        LoadGameTopup();
        GetAllGameTopup();
        GetAllProduct();
        ClearFileUpload();
    }
    function LoadGameTopup() {
        var GameTopup = sessionStorage.getItem("GameTopup");
        if (GameTopup != null) {
            $scope.btnSave = 'Update Topup';
            $scope.GameTopup = JSON.parse(sessionStorage.GameTopup);
            $scope.GameTopup.ProductIds = "";
        }
        sessionStorage.removeItem("GameTopup");
    }
    function GetAllProduct() {
        $http({
            method: "get",
            url: "/Products/GetAllProduct",
        }).then(function (response) {
            $scope.ProductList = response.data;
            angular.forEach($scope.ProductList, function (Product) {
                var parsedDate = new Date(parseInt(Product.UpdatedDate.substr(6)));
                if (parsedDate != null && parsedDate != undefined) {
                    var date = ($filter('date')(parsedDate, 'dd/MM/yyyy')).toString();
                    Product.UpdatedDateSt = date;
                }

            });

        }, function () {
            alert("Error Occur");
        })
    }
    
    function GetAllGameTopup() {
        $http({
            method: "get",
            url: "/GameTopups/GetAllGameTopup",
        }).then(function (response) {
            $scope.GameTopupList = response.data;
            angular.forEach($scope.GameTopupList, function (GameTopup) {
                var parsedDate = new Date(parseInt(GameTopup.UpdatedDate.substr(6)));
                if (parsedDate != null && parsedDate != undefined) {
                    var date = ($filter('date')(parsedDate, 'dd/MM/yyyy')).toString();
                    GameTopup.UpdatedDateSt = date;
                }
               
            });
            
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.SelectCheckBox = function (row,value) {
        var isCheck = value;
        if (isCheck) {
            $scope.Count++;
            $scope.GameTopup.ProductIds += $scope.GameTopup.ProductIds === '' ? ('' + row.ProductId) : (',' + row.ProductId);
           
        }
        else {
            var productIdsArray = $scope.GameTopup.ProductIds.split(",");
            $scope.GameTopup.ProductIds = "";
            for (var i = 0; i < productIdsArray.length; i++) {
                if (productIdsArray[i] != row.ProductId) {
                    $scope.GameTopup.ProductIds += $scope.GameTopup.ProductIds === '' ? ('' + productIdsArray[i]) : (',' + productIdsArray[i]);
                }
            }
            $scope.Count --;
        }
        
    }
    $scope.SaveGameTopup = function () {
        if ($scope.GameTopup.GameTopupId == 0) {
            $scope.GameTopup.ImageUrl = 'heroic/hdu.png';
            $http({
                method: "post",
                url: "/GameTopups/Save",
                datatype: "json",
                data: JSON.stringify($scope.GameTopup)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Topup Save Successfull.') {
                    SaveFile();
                    window.location.href = '/GameTopups/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        else {
            $http({
                method: "post",
                url: "/GameTopups/Save",
                datatype: "json",
                data: JSON.stringify($scope.GameTopup)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Topup Save Successfull.') {
                    SaveFile();
                    window.location.href = '/GameTopups/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
     
    }
    //......Start file upload ......
    function ClearFileUpload() {
        $scope.Message = "";
        $scope.FileInvalidMessage = "";
        $scope.SelectedFileForUpload = null;
        $scope.FileDescription = "Hi";
        $scope.IsFormSubmitted = false;
        $scope.IsFileValid = false;
        $scope.IsFormValid = false;

    }
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ($scope.SelectedFileForUpload != null) {
            if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                $scope.FileInvalidMessage = "";
                isValid = true;
            }
            else {
                $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
            }
        }
        else {
            $scope.FileInvalidMessage = "Image required!";
        }
        $scope.IsFileValid = isValid;
    };

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    
    function SaveFile() {
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        $scope.ChechFileValid($scope.SelectedFileForUpload);
        if ($scope.IsFileValid) {
            FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.FileDescription).then(function (d) {
                ClearForm();
            }, function (e) {
                alert(e);
            });
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    }
    //Clear form 
    function ClearForm() {
        $scope.FileDescription = "";
        //as 2 way binding not support for File input Type so we have to clear in this way
        //you can select based on your requirement
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });

        $scope.f1.$setPristine();
        $scope.IsFormSubmitted = false;
    }

 
    //......End file upload......

    $scope.BackToGameTopupList = function () {
        window.location.href = "/GameTopups/Index";
    }
    $scope.AddGameTopup = function () {
        window.location.href = "/GameTopups/Save";
    }
    $scope.EditGameTopup = function (GameTopup) {
        sessionStorage.setItem("GameTopup", JSON.stringify(GameTopup));
        window.location.href = "/GameTopups/Save";
    }
    $scope.DeleteGameTopup = function (GameTopup) {
        if (!confirm('Are you sure to delete this?')) { return;} 
        $http({
            method: "post",
            url: "/GameTopups/DeleteGameTopup?nId=" + GameTopup.GameTopupId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/GameTopups/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
   
  
}).factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2
    var fac = {};
    fac.UploadFile = function (file, description) {
        var formData = new FormData();
        formData.append("file", file);
        //We can send more data to server using append  
        formData.append("description", description);
        var promise = $http.post("/GameTopups/SaveFiles", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
            .then(function (response) {
                return response.data;
            })
            .catch(function (error) {
                console.log("File Upload Failed!");
                return $q.reject(error);
            });
        //.success(function (d) {
        //    defer.resolve(d);
        //})
        //.error(function () {
        //    defer.reject("File Upload Failed!");
        //});
        return promise;
        //var defer = $q.defer();
        //$http.post("/GameTopups/SaveFiles", formData,
        //    {
        //        withCredentials: true,
        //        headers: { 'Content-Type': undefined },
        //        transformRequest: angular.identity
        //    })
        //    .success(function (d) {
        //        defer.resolve(d);
        //    })
        //    .error(function () {
        //        defer.reject("File Upload Failed!");
        //    });

        //return defer.promise;

    }
    return fac;

});
