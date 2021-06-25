var app = angular.module("myApp", []);
app.controller("PaymentGateway", function ($scope, $http, $filter) {
    Clear();
    function Clear() {
        $scope.btnSave = 'Save Payment Gateway';
        $scope.PaymentGateway = {};
        $scope.PaymentGateway.GatewayId = 0;
        $scope.PaymentGatewayList = [];
        LoadPaymentGateway();
        GetAllPaymentGateway();
    }
    function LoadPaymentGateway() {
        var PaymentGateway = sessionStorage.getItem("PaymentGateway");
        if (PaymentGateway != null) {
            $scope.btnSave = 'Update Payment Gateway';
            $scope.PaymentGateway = JSON.parse(sessionStorage.PaymentGateway);
        }
        sessionStorage.removeItem("PaymentGateway");
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
   
    $scope.SavePaymentGateway = function () {
        $scope.PaymentGateway.GatewayName = $scope.PaymentGateway.GatewayName.trim();
        if ($scope.PaymentGateway.GatewayId == 0) {
            $http({
                method: "post",
                url: "/PaymentGateways/Save",
                datatype: "json",
                data: JSON.stringify($scope.PaymentGateway)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Payment Gateway Save Successfull.') {
                    window.location.href = '/PaymentGateways/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        else {
            $http({
                method: "post",
                url: "/PaymentGateways/Save",
                datatype: "json",
                data: JSON.stringify($scope.PaymentGateway)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Payment Gateway Save Successfull.') {
                    window.location.href = '/PaymentGateways/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
     
    }

    $scope.BackToPaymentGatewayList = function () {
        window.location.href = "/PaymentGateways/Index";
    }
    $scope.AddPaymentGateway = function () {
        window.location.href = "/PaymentGateways/Save";
    }
    $scope.EditPaymentGateway = function (PaymentGateway) {
        sessionStorage.setItem("PaymentGateway", JSON.stringify(PaymentGateway));
        window.location.href = "/PaymentGateways/Save";
    }
    $scope.DeletePaymentGateway = function (PaymentGateway) {
        if (!confirm('Are you sure to delete this?')) { return;} 
        $http({
            method: "post",
            url: "/PaymentGateways/DeletePaymentGateway?nId=" + PaymentGateway.GatewayId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/PaymentGateways/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
   
  
});
