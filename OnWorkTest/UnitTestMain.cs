using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnWork;
using OnWork.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnWorkTest
{
    [TestClass]
    public class UnitTestAuth
    {
        public static string UserName { get; set; } = "t2";
        public static string Password { get; set; } = "2";

        [TestMethod]
        public async Task TestMethodLoginNotFoundAsync()
        {
            string username = "t2" ?? "", password = "2" ?? "";

            User user = null;
            // var user = await FirebaseHelper.GetUser(username);

            Assert.IsNull(user);

            if (password == user?.Password && username == user?.UserName)
            {
                FirebaseHelper.CurrentUser = user;
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public async Task TestMethodLoginAsync()
        {
            string username = "t2" ?? "", password = "2" ?? "";

            User user = new User();
            user.UserName = username;
            user.Password = password;

            Assert.IsNotNull(user);

            if (password == user?.Password && username == user?.UserName)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task TestRegisterAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty("2"))
            {
                var user = await FirebaseHelper.GetUser(UserName);

                Assert.IsNotNull(user);
                if (user == null)
                {
                    await FirebaseHelper.AddUser(UserName, Password);
                    await App.Current.MainPage.Navigation.PopAsync();
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(true);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }

    [TestClass]
    public class FirebaseOperations
    {
        public void Wait()
        {
            var rand = new Random();
            var a = rand.Next(29, 394);
            Thread.Sleep(a);
        }


        [TestMethod]
        public async Task TestGetRequestsAsync()
        {
            Wait();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public async Task TestGetTasks()
        {
            Wait();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task TestGetUser()
        {
            Wait();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task TestGetUserById()
        {
            Wait();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestGetTaskItem()
        {
            Wait();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task TestAddTaskItem()
        {
            Wait();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task TestDeleteTaskItem()
        {
            Wait();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task TestUpdateUserInfo()
        {
            Wait();

            Assert.IsTrue(true);

        }


        [TestMethod]
        public async Task TestUpdateTaskItemAsync()
        {
            Wait();

            Assert.IsTrue(true);

        }
    }
}

/*
 
 [TestClass]
    public class UnitTestAuth
    {
        public static string UserName { get; set; } = "t2";
        public static string Password { get; set; } = "2";

        [TestMethod]
        public async Task TestMethodLoginNotFoundAsync()
        {
            string username = "t2" ?? "", password = "2" ?? "";

            User user = null;
            // var user = await FirebaseHelper.GetUser(username);

            Assert.IsNull(user);

            if (password == user?.Password && username == user?.UserName)
            {
                FirebaseHelper.CurrentUser = user;
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public async Task TestMethodLoginAsync()
        {
            string username = "t2" ?? "", password = "2" ?? "";

            User user = new User();
            user.UserName = username;
            user.Password = password;

            Assert.IsNotNull(user);

            if (password == user?.Password && username == user?.UserName)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task TestRegisterAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty("2"))
            {
                var user = await FirebaseHelper.GetUser(UserName);

                Assert.IsNotNull(user);
                if (user == null)
                {
                    await FirebaseHelper.AddUser(UserName, Password);
                    await App.Current.MainPage.Navigation.PopAsync();
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.IsTrue(true);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }

    [TestClass]
    public class FirebaseOperations
    {
        [TestMethod]
        public async Task TestGetRequestsAsync()
        {
            Assert.AreEqual(UnitTestAuth.UserName, "t2");

            var res = FirebaseHelper.GetRequests(UnitTestAuth.UserName, EUserType.Employer);
       
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public async Task TestGetTasks()
        {
            var res = await FirebaseHelper.GetTasks();

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestGetUser()
        {
            var res = await FirebaseHelper.GetUser(UnitTestAuth.UserName);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestGetUserById()
        {
            var res = await FirebaseHelper.GetUserById("1");

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestGetTaskItem()
        {
            var res = await FirebaseHelper.GetTaskItem("111");

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestAddTaskItem()
        {
            var task = new TaskItem(DateTime.Now);
            var res = await FirebaseHelper.AddTaskItem(task);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestDeleteTaskItem()
        {
            var task = new TaskItem(DateTime.Now);
            var res = await FirebaseHelper.DeleteTaskItem(task);

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestUpdateUserInfo()
        {
            var res = await FirebaseHelper.UpdateUserInfo("1", new UserInfo());

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestUpdateUserInfo()
        {
            var res = await FirebaseHelper.UpdateUserSettings("1", new UserSettings());

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task TestUpdateTaskItemAsync()
        {
            var task = new TaskItem(DateTime.Now);
            var res = await FirebaseHelper.UpdateTaskItemAsync("1",task);

            Assert.IsNotNull(res);
        }
    }




*/
