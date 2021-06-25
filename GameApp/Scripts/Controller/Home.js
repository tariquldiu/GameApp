var app = angular.module("myApp", []);
app.controller("Home", function ($scope, $http, $filter) {
    Clear();
    function Clear() {
        $scope.btnSave = 'Save Notice';
        $scope.Notice = {};
        $scope.NoticeActivityList = [{ IsActive: false, NoticeActivityName: 'No' }, { IsActive: true, NoticeActivityName: 'Yes'}];
        $scope.Notice.NoticeId = 0;
        $scope.NoticeList = [];
        LoadNotice();
        GetAllNotice();
        GetAllGameTopup();
        ManageUserAccount();
    }

    function ManageUserAccount() {
        var loggedInPriod = localStorage.getItem('LoggedInPriod');

        if (loggedInPriod != null && new Date(loggedInPriod) <= new Date()) {
            localStorage.removeItem('UserId');
        }
    }
    function LoadNotice() {
        var notice = sessionStorage.getItem("Notice");
        if (notice != null) {
            $scope.btnSave = 'Update Notice';
            $scope.Notice = JSON.parse(sessionStorage.Notice);
            $scope.ddlNoticeActivity = { IsActive: $scope.Notice.IsActive };
        }
        sessionStorage.removeItem("Notice");
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
    function GetAllNotice() {
        $http({
            method: "get",
            url: "/Notices/GetAllNotice",
        }).then(function (response) {
            $scope.NoticeList = response.data;
            angular.forEach($scope.NoticeList, function (notice) {
                var parsedDate = new Date(parseInt(notice.UpdatedDate.substr(6)));
                if (parsedDate != null && parsedDate != undefined) {
                    var date = ($filter('date')(parsedDate, 'dd/MM/yyyy')).toString();
                    notice.UpdatedDateSt = date;
                }
               
            });
            
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.GoToOrderPage = function (GameTopup) {
        sessionStorage.setItem("GameTopupToOrder", JSON.stringify(GameTopup));
        window.location.href = "/Orders/Save";
    }
    $scope.SaveNotice = function () {
        if ($scope.Notice.NoticeId == 0) {
            $scope.Notice.IsActive = true;
            $scope.Notice.PositiveVote = 0;
            $scope.Notice.NegativeVote = 0;
            $http({
                method: "post",
                url: "/Notices/Save",
                datatype: "json",
                data: JSON.stringify($scope.Notice)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Notice Save Successfull.') {
                    window.location.href = '/Notices/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        else {
            $http({
                method: "post",
                url: "/Notices/Save",
                datatype: "json",
                data: JSON.stringify($scope.Notice)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Notice Save Successfull.') {
                    window.location.href = '/Notices/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
     
    }
    $scope.PositiveVoting = function (notice) {
        var userId = localStorage.getItem('UserId');
        if (userId == null) {
            alert("Please login first.");
            window.location.href = "/Users/Login";
            return;
        }
        var nextDate = localStorage.getItem('NextDate');
       
        if (nextDate != null && new Date(nextDate) > new Date()) {
            alert("You already voted on this poll.");
            return;
        }
        var date = new Date();
        date.setDate(date.getDate() + 2);
        localStorage.setItem('NextDate', date);
        $scope.Notice = notice;
        $scope.Notice.PositiveVote = notice.PositiveVote + 1; 
        $http({
            method: "post",
            url: "/Notices/Save",
            datatype: "json",
            data: JSON.stringify($scope.Notice)
        }).then(function (response) {
            alert("Your Vote is Counted");
            Clear();
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.NegativeVoting = function (notice) {
        var userId = localStorage.getItem('UserId');
        if (userId == null) {
            alert("Please login first.");
            window.location.href = "/Users/Login";
            return;
        }
        var nextDate = localStorage.getItem('NextDate');

        if (nextDate != null && new Date(nextDate) > new Date()) {
            alert("You already voted on this poll.");
            return;
        }
        var date = new Date();
        date.setDate(date.getDate() + 2);
        localStorage.setItem('NextDate', date);

        $scope.Notice = notice;
        $scope.Notice.NegativeVote = notice.NegativeVote + 1;
        $http({
            method: "post",
            url: "/Notices/Save",
            datatype: "json",
            data: JSON.stringify($scope.Notice)
        }).then(function (response) {
            alert("Your Vote is Counted");
            Clear();
        }, function () {
            alert("Error Occur");
        })
    }
  
    $scope.BackToNoticeList = function () {
        window.location.href = "/Notices/Index";
    }
    $scope.AddNotice = function () {
        window.location.href = "/Notices/Save";
    }
    $scope.SignInLink = function () {
        window.location.href = "/Users/Login";
    }
    $scope.EditNotice = function (notice) {
        sessionStorage.setItem("Notice", JSON.stringify(notice));
        window.location.href = "/Notices/Edit";
    }
    $scope.DeleteNotice = function (Notice) {
        if (!confirm('Are you sure to delete this?')) { return;} 
        $http({
            method: "post",
            url: "/Notices/DeleteNotice?nId=" + Notice.NoticeId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/Notices/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
   
  
});
