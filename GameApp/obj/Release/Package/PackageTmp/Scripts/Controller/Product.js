var app = angular.module("myApp", []);
app.controller("Product", function ($scope, $http, $filter) {
    Clear();
    function Clear() {
        $scope.btnSave = 'Save Product';
        $scope.Product = {};
        $scope.Product.ProductId = 0;
        $scope.ProductList = [];
        LoadProduct();
        GetAllProduct();
    }
    function LoadProduct() {
        var Product = sessionStorage.getItem("Product");
        if (Product != null) {
            $scope.btnSave = 'Update Product';
            $scope.Product = JSON.parse(sessionStorage.Product);
        }
        sessionStorage.removeItem("Product");
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
   
    $scope.SaveProduct = function () {
        if ($scope.Product.ProductId == 0) {
            $scope.Product.IsActive = true;
           
            $http({
                method: "post",
                url: "/Products/Save",
                datatype: "json",
                data: JSON.stringify($scope.Product)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Product Save Successfull.') {
                    window.location.href = '/Products/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        else {
            $http({
                method: "post",
                url: "/Products/Save",
                datatype: "json",
                data: JSON.stringify($scope.Product)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Product Save Successfull.') {
                    window.location.href = '/Products/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
     
    }

    $scope.BackToProductList = function () {
        window.location.href = "/Products/Index";
    }
    $scope.AddProduct = function () {
        window.location.href = "/Products/Save";
    }
    $scope.EditProduct = function (Product) {
        sessionStorage.setItem("Product", JSON.stringify(Product));
        window.location.href = "/Products/Save";
    }
    $scope.DeleteProduct = function (Product) {
        if (!confirm('Are you sure to delete this?')) { return;} 
        $http({
            method: "post",
            url: "/Products/DeleteProduct?nId=" + Product.ProductId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/Products/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
   
  
});
