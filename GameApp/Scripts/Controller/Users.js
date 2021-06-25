var app = angular.module("myApp", []);
app.controller("Users", function ($scope, $http, $filter) {
    Clear();
    function Clear() {
        $scope.User = {};
        $scope.User.UserId = 0;
        $scope.UserList = [];

        GetAllUser();
        LoadUser();
    }

    function LoadUser() {
        var user = sessionStorage.getItem("User");
        if (user != null) {
            $scope.btnSave = 'Update User';
            $scope.User = JSON.parse(sessionStorage.User);
           
        }
        sessionStorage.removeItem("User");
    }

    function GetAllUser() {
        $http({
            method: "get",
            url: "/Users/GetUser",
        }).then(function (response) {
            $scope.UserList = response.data;
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.Login = function () {
        $http({
            method: "post",
            url: "/Users/Login",
            datatype: "json",
            data: JSON.stringify($scope.User)
        }).then(function (response) {
            if (response.data.Message == "1") {
                alert("Login Successfull!");
                localStorage.setItem('UserId', response.data.UserId);
                var date = new Date();
                date.setDate(date.getDate() + 2);
                localStorage.setItem('LoggedInPriod', date);
                window.location.href = '/Home/Index';
            }
            else {
                alert("Invalid Username or Password.");
            }
            Clear();
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.Save = function () {
        if ($scope.User.UserId == 0) {
            $scope.User.RoleName = 'User';
            $scope.User.IsActive = true;

            $http({
                method: "post",
                url: "/Users/Save",
                datatype: "json",
                data: JSON.stringify($scope.User)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Registration Successfull.') {
                    window.location.href = '/Users/Login';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        else {
            $http({
                method: "post",
                url: "/Users/Edit",
                datatype: "json",
                data: JSON.stringify($scope.User)
            }).then(function (response) {
                alert(response.data.Message);
                if (response.data.Message == 'Permitted Successfully.') {
                    window.location.href = '/Users/Index';
                }
                Clear();
            }, function () {
                alert("Error Occur");
            })
        }
        
       
    }
    $scope.Edit = function (user) {
        sessionStorage.setItem("User", JSON.stringify(user));
        window.location.href = "/Users/Edit";
    }
    $scope.BackToList = function () {
        window.location.href = "/Users/Index";
    }
    $scope.Register = function () {
        window.location.href = "/Users/Save";
    }
    $scope.Logout = function () {
        $http({
            method: "get",
            url: "/Users/Logout",
        }).then(function (response) {
            if (response.data == 1) {
                alert("Logout Successfull!");
                localStorage.removeItem('UserId');
                var date = new Date();
                localStorage.setItem('LoggedInPriod', date);
                window.location.href = '/Users/Login';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.Delete = function (user) {
        if (!confirm('Are you sure to delete this?')) { return;} 
        $http({
            method: "post",
            url: "/Users/DeleteUser?uId="+user.UserId,
        }).then(function (response) {
            if (response.data == true) {
                alert("Successfully Deleted");
                window.location.href = '/Users/Index';
                Clear();
            }
        }, function () {
            alert("Error Occur");
        })
    }
   
    $scope.ChangePasswordEvent = function () {
        $http({
            method: "post",
            url: "/Users/ChangePassword",
            datatype: "json",
            data: JSON.stringify($scope.User)
        }).then(function (response) {
            alert(response.data.Message);
            if (response.data.Message == 'Password Updated Successfully.') {
                window.location.href = '/Users/Login';
            }
            Clear();
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.SetPassFocus = function (e) {
        if (e.keyCode === 13) {
            $("#loginPassword").focus();
        }
    }
    $scope.SetloginBtnFocus = function (e) {
        if (e.keyCode === 13) {
            $("#btnLogin").focus();
        }
    }
    $scope.SetSignupPassFocus = function (e) {
        if (e.keyCode === 13) {
            $("#signupPassword").focus();
        }
    }
    $scope.SetSignupRepeatedPassFocus = function (e) {
        if (e.keyCode === 13) {
            $("#signupRepeatedPassword").focus();
        }
    }
    $scope.SetSignupBtnFocus = function (e) {
        if (e.keyCode === 13) {
            $("#btnSignup").focus();
        }
    }

    $scope.SetChangeOldPasswordFocus = function (e) {
        if (e.keyCode === 13) {
            $("#ChangeOldPassword").focus();
        }
    }

    $scope.SetChangePasswordFocus = function (e) {
        if (e.keyCode === 13) {
            $("#ChangePassword").focus();
        }
    }
    $scope.SetChangeConfirmPasswordFocus = function (e) {
        if (e.keyCode === 13) {
            $("#ChangeConfirmPassword").focus();
        }
    }
    $scope.SetBtnChangeFocus = function (e) {
        if (e.keyCode === 13) {
            $("#btnChangePassword").focus();
        }
    }
    
    $scope.SetSignupPhoneFocus = function (e) {
        if (e.keyCode === 13) {
            $("#signupPhone").focus();
        }
    }
    $scope.SetSignupEmailFocus = function (e) {
        if (e.keyCode === 13) {
            $("#signupEmail").focus();
        }
    }
    $scope.SetSignupUsernameFocus = function (e) {
        if (e.keyCode === 13) {
            $("#signupUsername").focus();
        }
    }
  
});
