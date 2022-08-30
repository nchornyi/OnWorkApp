using Firebase.Database;
using Firebase.Database.Query;
using LiteDB;
using OnWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnWork
{
    public static class FirebaseHelper
    {
        public static User CurrentUser { get; set; }
        public static FirebaseClient firebase;
        static FirebaseHelper()
        {
            try
            {
                firebase = new FirebaseClient("https://onwork-f10bb-default-rtdb.firebaseio.com/");
            }
            catch (Exception ex)
            {

            }
        }
        #region TaskItem
        private static async Task<FirebaseObject<TaskItem>> FBGetTaskItem(string id) => 
            (await firebase.Child(ETables.Tasks.ToString())
                           .OnceAsync<TaskItem>())
                           .FirstOrDefault(x => x.Object.id == id);

        public static async Task AddTaskItem(TaskItem task)
        {
            await firebase.Child(ETables.Tasks.ToString()).PostAsync(task);
        }
        
        public static async Task DeleteTaskItem(TaskItem task)
        {
            var taskItem = await FBGetTaskItem(task.id);
            if (taskItem == null) return;
            await firebase.Child(ETables.Tasks.ToString()).Child(taskItem?.Key).DeleteAsync();
        }
        public static async Task<List<TaskItem>> GetTasks()
        {
            return (await firebase.Child(ETables.Tasks.ToString())
                                  .OnceAsync<TaskItem>()).Select(x=>x.Object).ToList();
        }

        public static async Task<TaskItem> GetTaskItem(string taskId)
        {
            var taskItem = await FBGetTaskItem(taskId);
            return taskItem?.Object;
        }

        public static async Task UpdateTaskItemAsync(TaskItem task)
        {
            task.LocationIcon = null;
            var taskItem = await FBGetTaskItem(task.id);
            await firebase.Child(ETables.Tasks.ToString()).Child(taskItem?.Key).PutAsync(task);
        }
        #endregion

        #region Requests
        public static List<TaskRequest> GetRequests(string user, EUserType userType)
        {
            var requests = new List<TaskRequest>();
            var tasksList = new List<TaskItem>();

            switch (userType)
            {
                case EUserType.Employer:
                    tasksList = Task.Run(async () => await GetTasks()).Result.Where(x => x?.OwnerNickName == user).ToList();

                    foreach (var task in tasksList)
                        requests.AddRange(task.Requests);

                    break;
                case EUserType.Employee:
                    tasksList = Task.Run(async () => await GetTasks()).Result.ToList();

                    requests.AddRange(tasksList.SelectMany(taskItem => taskItem.Requests.Where(taskRequest => taskRequest.UserNickName == user)));
                    break;
                default:
                    break;
            }

            return requests;
        }
        #endregion

        #region User
        public static async Task AddUser(string username, string password)
        {
            await firebase.Child(ETables.Users.ToString()).PostAsync(
                           new User() { UserName = username, Password = password });
        }
        public static async Task<User> GetUser(string username)
        {
            return (await FBGetUser(username))?.Object;
        }
        public static async Task<User> GetUserById(string id)
        {
            return (await firebase.Child(ETables.Users.ToString())
                                  .OnceAsync<User>())
                                  .FirstOrDefault(x => x.Object.id == id)?.Object;
        }

        private static async Task<FirebaseObject<User>> FBGetUser(string username)
        {
            return (await firebase.Child(ETables.Users.ToString())
                                  .OnceAsync<User>())
                                  .FirstOrDefault(x => x.Object.UserName == username);
        }

        public static async void UpdateUserInfo(string username, UserInfo info)
        {
            var user = (await FBGetUser(username));
            var userObject = user.Object;
            userObject.Info = info;
            await firebase.Child(ETables.Users.ToString()).Child(user.Key).PutAsync(userObject);
        }

        public static async void UpdateUserSettings(string username, UserSettings settings)
        {
            var user = (await FBGetUser(username));
            var userObject = user.Object;
            userObject.Settings = settings;
            await firebase.Child(ETables.Users.ToString()).Child(user.Key).PutAsync(userObject);
        }
        #endregion
    }
}
