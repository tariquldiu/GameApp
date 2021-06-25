var app = angular.module("myApp", []);
app.controller("Order", function ($scope, $http, $filter) {
    Clear();
    function Clear() {
        $scope.btnSave = 'Save Order';
        $scope.Order = {};
        $scope.GameTopupToOrder = {};
        $scope.Product = {}
        $scope.Order.OrderId = 0;
        $scope.IsNagad = false;
        $scope.IsBkash = false;
        $scope.IsRocket = false;
        $scope.TotalPayAmount = 0;
        $scope.Order.ProductIds = "";
        $scope.Order.AccountId = 0;
        $scope.OrderList = [];
        $scope.ProductList = [];
        $scope.PaymentGatewayList = [];
        $scope.AccountTypeList = [{ TypeId: 0, TypeName: 'Facebook' }, { TypeId: 1, TypeName: 'Gmail' }];
        LoadGameTopupToOrder();
        GetAllOrder();
        GetAllProduct();
        GetAllPaymentGateway();
    }
    function LoadGameTopupToOrder() {
        var GameTopupToOrder = sessionStorage.getItem("GameTopupToOrder");
        if (GameTopupToOrder != null) {
            $scope.GameTopupToOrder = JSON.parse(sessionStorage.GameTopupToOrder);
        }
    }
    function GetAllPaymentGateway() {
        $http({
            method: "get",
            url: "/PaymentGateways/GetAllPaymentGateway",
        }).then(function (response) {
            $scope.PaymentGatewayList = response.data;

        }, function () {
            alert("Error Occur");
        })
    }
    function GetAllProduct() {
        $http({
            method: "get",
            url: "/Products/GetAllProduct",
        }).then(function (response) {
            $scope.TempProductList = response.data;
            var productIdArr = $scope.GameTopupToOrder.ProductIds.split(",");
            for (var i = 0; i < productIdArr.length; i++) {
                for (var j = 0; j < $scope.TempProductList.length; j++) {
                    if (productIdArr[i] == $scope.TempProductList[j].ProductId) {
                        $scope.ProductList.push($scope.TempProductList[j]);
                    }
                }
            }
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
    $scope.CheckProductIds = function (row) {
        var isCheck = true;
        var chkProductIdsArray = $scope.Order.ProductIds.split(",");
        for (var i = 0; i < chkProductIdsArray.length; i++) {
            if (chkProductIdsArray[i] == row.ProductId) {
                isCheck = false;
            }
        }

        if (isCheck) {
            $scope.Order.ProductIds += $scope.Order.ProductIds === '' ? ('' + row.ProductId) : (',' + row.ProductId);

        }
        else {
            var productIdsArray = $scope.Order.ProductIds.split(",");
            $scope.Order.ProductIds = "";
            for (var i = 0; i < productIdsArray.length; i++) {
                if (productIdsArray[i] != row.ProductId) {
                    $scope.Order.ProductIds += $scope.Order.ProductIds === '' ? ('' + productIdsArray[i]) : (',' + productIdsArray[i]);
                }
            }
        }

    }
    $scope.ShowBkash = function () {
        $scope.IsNagad = false;
        $scope.IsBkash = true;
        $scope.IsRocket = false;
        var productPrice = $scope.Product.ProductPrice;
        var onePercentProductPrice = productPrice / 100;
        var percentChargeAmount = onePercentProductPrice * $scope.PaymentGatewayList[2].ChargeAmount;
        var percentDiscountAmount = onePercentProductPrice * $scope.PaymentGatewayList[2].DiscountAmount;

        $scope.TotalPayAmount = ((percentChargeAmount + $scope.Product.ProductPrice) - percentDiscountAmount).toFixed(1);

    }
    $scope.ShowRocket = function () {
        $scope.IsNagad = false;
        $scope.IsBkash = false;
        $scope.IsRocket = true;
        var productPrice = $scope.Product.ProductPrice;
        var onePercentProductPrice = productPrice / 100;
        var percentChargeAmount = onePercentProductPrice * $scope.PaymentGatewayList[1].ChargeAmount;
        var percentDiscountAmount = onePercentProductPrice * $scope.PaymentGatewayList[1].DiscountAmount;

        $scope.TotalPayAmount = ((percentChargeAmount + $scope.Product.ProductPrice) - percentDiscountAmount).toFixed(1);

    }
   
    $scope.ShowNagad = function () {
        $scope.IsNagad = true;
        $scope.IsBkash = false;
        $scope.IsRocket = false;
        var productPrice = $scope.Product.ProductPrice;
        var onePercentProductPrice = productPrice / 100;
        var percentChargeAmount = onePercentProductPrice * $scope.PaymentGatewayList[0].ChargeAmount;
        var percentDiscountAmount = onePercentProductPrice * $scope.PaymentGatewayList[0].DiscountAmount;

        $scope.TotalPayAmount = ((percentChargeAmount + $scope.Product.ProductPrice) - percentDiscountAmount).toFixed(1);

    }
    function GetAllOrder() {
        $http({
            method: "get",
            url: "/Orders/GetAllOrder",
        }).then(function (response) {
            $scope.OrderList = response.data;
            angular.forEach($scope.OrderList, function (Order) {
                var parsedDate = new Date(parseInt(Order.UpdatedDate.substr(6)));
                if (parsedDate != null && parsedDate != undefined) {
                    var date = ($filter('date')(parsedDate, 'dd/MM/yyyy')).toString();
                    Order.UpdatedDateSt = date;
                }

            });

        }, function () {
            alert("Error Occur");
        })
    }
    $scope.SetAccountType = function (accountType) {
        $scope.Order.AccountId = accountType.TypeId;
    }
    $scope.PlaceOrder = function () {
        if ($scope.Order.OrderId == 0) {
            $scope.Order.IsActive = true;

            if (($scope.GameTopupToOrder.IsEnterPlayerId == true) && ($scope.Order.PlayerId == undefined || $scope.Order.PlayerId == "")) {
                alert("Please input Player Id");
                return;
            }
            if (($scope.GameTopupToOrder.IsSocialAccount == true) && ($scope.Order.AccountName == undefined || $scope.Order.AccountName == "")) {
                alert("Please input Account No");
                return;
            }
            if (($scope.GameTopupToOrder.IsSocialAccount == true) && ($scope.Order.AccountPassword == undefined || $scope.Order.AccountPassword == "")) {
                alert("Please input Account Password");
                return;
            }
            if (($scope.Order.AccountId == 1) && ($scope.GameTopupToOrder.IsSocialAccount == true) && ($scope.Order.AccountSecurityCode == undefined || $scope.Order.AccountSecurityCode == "")) {
                alert("Please input Account Security Code");
                return;
            }
            if ($scope.Order.AccountId == 1 && $scope.GameTopupToOrder.IsSocialAccount == true) {
                $scope.Order.AccountType = 'Gmail';
            }
            else if ($scope.Order.AccountId == 0 && $scope.GameTopupToOrder.IsSocialAccount == true) {
                $scope.Order.AccountType = 'Facebook';
            }
            else if ($scope.GameTopupToOrder.IsEnterPlayerId == true) {
                $scope.Order.AccountType = 'Player Id';
            }
            var comaExist = $scope.Order.ProductIds.includes(",");
            if (comaExist) {
                alert("Please select one product at a time");
                return;
            }
            $scope.Order.GameTopupId = $scope.GameTopupToOrder.GameTopupId;
            for (var i = 0; i < $scope.ProductList.length; i++) {
                if (Number($scope.Order.ProductIds) == $scope.ProductList[i].ProductId) {
                    $scope.Product=$scope.ProductList[i];
                }
            }
            var userId = localStorage.getItem('UserId');
            if (userId != null) {
                $('#paymentModal').modal('show');
            }
            else {
                alert("Please login first.");
                window.location.href = "/Users/Login";
            }
            
        }

    }
    $scope.BackToHome = function () {
        window.location.href = "/Home/Index";
    }
    $scope.ViewProduct = function (order) {
        $scope.OrderDetails = order;
        $scope.ProductList = [];
        $http({
            method: "get",
            url: "/Products/GetAllProduct",
        }).then(function (response) {
            $scope.TempProductList = response.data;
            var chkProductIdsArray = order.ProductIds.split(",");
            for (var i = 0; i < chkProductIdsArray.length; i++) {
                for (var j = 0; j < $scope.TempProductList.length; j++) {
                    if (chkProductIdsArray[i] == $scope.TempProductList[j].ProductId) {
                        $scope.ProductList.push($scope.TempProductList[j]);
                    }
                }
            }
        }, function () {
            alert("Error Occur");
        })

    }
    $scope.SavePayment = function () {
        var userId = localStorage.getItem('UserId');
        if (userId == null) {
            alert("Please login first.");
            return;
        }
        $scope.Order.UserId = userId;
        $scope.Order.Amount = $scope.TotalPayAmount;
         $http({
                method: "post",
                url: "/Orders/Save",
                datatype: "json",
                data: JSON.stringify($scope.Order)
            }).then(function (response) {
                alert("Your order successfully submitted. Your order will be delivered within one business day. Thanks for the order.");
                if (response.data.Message == 'Order Send Successfull.') {
                    window.location.href = '/Home/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })

    }
    
    $scope.CancelPayment = function () {
        window.location.href = "/Orders/Save";
    }
    $scope.ClosePayment = function () {
        window.location.href = "/Orders/Save";
    }
    $scope.BackToOrderList = function () {
        window.location.href = "/Orders/Index";
    }
    $scope.AddOrder = function () {
        window.location.href = "/Orders/Save";
    }
    $scope.EditOrder = function (Order) {
        sessionStorage.setItem("Order", JSON.stringify(Order));
        window.location.href = "/Orders/Save";
    }
    $scope.DeleteOrder = function (Order) {
        if (!confirm('Are you sure to delete this?')) { return; }
        $http({
            method: "post",
            url: "/Orders/DeleteOrder?nId=" + Order.OrderId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/Orders/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }


});
