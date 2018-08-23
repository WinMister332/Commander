using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMCommandFramework.COSMOS
{
    public class TerminalUsers
    {
        private TerminalUserList userList = null;

        public TerminalUsers(UserCredential rootCredential, UserCredential adminCredential)
        {
            if (rootCredential != null && adminCredential != null)
            {
                TerminalUser rootUser = null;
                if (rootCredential.GetPassword() == null || rootCredential.GetPassword() == "")
                    rootUser = new TerminalUser(rootCredential.GetUsername(), TerminalUser.PermissionGroup.RootPermissions(), true);
                else
                    rootUser = new TerminalUser(rootCredential.GetUsername(), rootCredential.GetPassword(), TerminalUser.PermissionGroup.RootPermissions(), true);
                TerminalUser adminUser = null;
                if (adminCredential.GetPassword() == null || rootCredential.GetPassword() == "")
                    adminUser = new TerminalUser(adminCredential.GetUsername(), TerminalUser.PermissionGroup.AdminPermissions(), true);
                else
                    adminUser = new TerminalUser(adminCredential.GetUsername(), adminCredential.GetPassword(), TerminalUser.PermissionGroup.AdminPermissions(), true);
                userList = new TerminalUserList(rootUser, adminUser);
            }
            else if (rootCredential == null && adminCredential != null)
            {
                TerminalUser rootUser = null;
                TerminalUser adminUser = null;
                if (adminCredential.GetPassword() == null || rootCredential.GetPassword() == "")
                {
                    adminUser = new TerminalUser(adminCredential.GetUsername(), TerminalUser.PermissionGroup.AdminPermissions(), true);
                    rootUser = new TerminalUser("root", TerminalUser.PermissionGroup.AdminPermissions(), true);
                }
                else
                {
                    adminUser = new TerminalUser(adminCredential.GetUsername(), adminCredential.GetPassword(), TerminalUser.PermissionGroup.AdminPermissions(), true);
                    rootUser = new TerminalUser("root", adminCredential.GetPassword(), TerminalUser.PermissionGroup.AdminPermissions(), true);
                }
                userList = new TerminalUserList(rootUser, adminUser);
            }
            else if (rootCredential != null && adminCredential == null)
            {
                TerminalUser rootUser = null;
                if (adminCredential.GetPassword() == null || rootCredential.GetPassword() == "")
                    rootUser = new TerminalUser(rootCredential.GetUsername(), TerminalUser.PermissionGroup.RootPermissions(), true);
                else
                    rootUser = new TerminalUser(rootCredential.GetUsername(), rootCredential.GetPassword(), TerminalUser.PermissionGroup.RootPermissions(), true);
                userList = new TerminalUserList(rootUser);
            }
            else
            {
                TerminalUser rootUser = new TerminalUser("root", TerminalUser.PermissionGroup.RootPermissions(), true);
                userList = new TerminalUserList(rootUser);
            }
        }

        public TerminalUser Login(string username, string password)
        {
            if (username != null && username != "")
            {
                var result = userList.Get(username);
                if (!(result == null))
                {
                    if (result.GetPassword() == "" || result.GetPassword() == null)
                    {
                        result.LoggedIn = true;
                        return result;
                    }
                    else
                    {
                        if (result.GetPassword() == password)
                        {
                            result.LoggedIn = true;
                            return result;
                        }
                        else
                        {
                            result.LoggedIn = false;
                            return result;
                        }
                    }
                }
                else return null;
            }
            return null;
        }

        public void LoginPrompt(string usernameText, CommandProcessor processor)
        {
            while (true)
            {
                if (usernameText == "" || usernameText == null) Console.Write($"Username: ");
                else Console.Write($"{usernameText}: ");
                var username = Console.ReadLine();
                Console.Write("Password: ");
                var password = new KeyInput();
                password.ProcessInput();
                var result = Login(username, password.ToString());
                if (!(result == null))
                {
                    if (result.LoggedIn)
                    {
                        while (result.LoggedIn)
                        {
                            processor.Process();
                        }
                    }
                    else Console.WriteLine("The username or password was incorrect.");
                }
                else
                {
                    Console.WriteLine("The username or password was incorrect.");
                }
            }
        }

        internal IReadOnlyList<TerminalUser> Users()
        {
            return userList.GetAllUsers();
        }

        internal string[] GetUsernames(TerminalUser[] users)
        {
            List<string> s = new List<string>();
            foreach (TerminalUser user in users)
            {
                s.Add(user.GetUsername());
            }
            return s.ToArray();
        }

        internal string[] GetPasswords(TerminalUser[] users)
        {
            List<string> s = new List<string>();
            foreach (TerminalUser user in users)
            {
                s.Add(user.GetPassword());
            }
            return s.ToArray();
        }

        public class UserCredential
        {
            private static string _username;
            private static string _password;

            public UserCredential(string username, string password, bool canPasswordBeNull = false)
            {
                _username = username;
                if (canPasswordBeNull && (password == "" || password == null))
                    _password = "";
                else
                    _password = $@"56ngMvzEN96AGP2eCuxhVxX8qKm5LcOvClp4bSnB1UDZfoQ14r82DKO0hlx9JjQFcaUn6xLD6XfZj4C0gIutmgC3AWwTeLakKX4w0tpUT7Zq7ZnUBfxlaKYUXPnB";
            }

            public string GetUsername()
            {
                return _username;
            }

            public string GetPassword()
            {
                return _password;
            }
        }
    }

    public class TerminalUserList
    {
        private TerminalUser root;
        private List<TerminalUser> admins;
        private List<TerminalUser> users;
        private TerminalUser guest;
        /// <summary>
        /// Contains a list of all users in the system.
        /// </summary>
        /// <param name="rootUser">The system user account</param>
        /// <param name="adminUser">The administrative user account.</param>
        public TerminalUserList(TerminalUser rootUser, TerminalUser adminUser = null)
        {
            if (rootUser == null)
                root = new TerminalUser("root", TerminalUser.PermissionGroup.RootPermissions(), true);
            else
                root = rootUser;
            admins = new List<TerminalUser>(50);
            users = new List<TerminalUser>(250);
            guest = new TerminalUser("guest", TerminalUser.PermissionGroup.GuestPermissions(), false);
            if (!(adminUser == null))
                admins.Add(adminUser);
        }
        /// <summary>
        /// Adds an administrative user to the user list.
        /// </summary>
        /// <param name="user">The user to add to the list.</param>
        public void AddAdmin(TerminalUser user)
        {
            admins.Add(user);
        }
        /// <summary>
        /// Removes an administrative user from the user list.
        /// </summary>
        /// <param name="user">The user to remove from the list.</param>
        public void RemoveAdmin(TerminalUser user)
        {
            admins.Remove(user);
        }
        /// <summary>
        /// Adds a user to the user list.
        /// </summary>
        /// <param name="user">The user to add to the list.</param>
        public void AdminUser(TerminalUser user)
        {
            users.Add(user);
        }
        /// <summary>
        /// Removes a user from the user list.
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(TerminalUser user)
        {
            users.Remove(user);
        }
        /// <summary>
        /// Enables or disables guest mode.
        /// </summary>
        /// <param name="b">The toggle value.</param>
        public void EnableGuest(bool b)
        {
            guest.Enabled = b;
        }
        /// <summary>
        /// Checks to see if guest is enabled.
        /// </summary>
        public bool GuestEnabled
        {
            get => guest.Enabled;
        }
        /// <summary>
        /// Gets the underlying <see cref="TerminalUser"> of the root account.
        /// </summary>
        /// <returns>The <see cref="TerminalUser"> of the Root account.</returns>
        public TerminalUser GetRoot()
        {
            return root;
        }
        /// <summary>
        /// Gets the underlying <see cref="TerminalUser"> of the guest account.
        /// </summary>
        /// <returns>The <see cref="TerminalUser"> of the Guest account.</returns>
        public TerminalUser GetGuest()
        {
            return guest;
        }
        /// <summary>
        /// Checks all users to see if there's any user with the specified name.
        /// </summary>
        /// <param name="name">The name to compare.</param>
        /// <returns>The result. Null if none.</returns>
        public TerminalUser GetByName(string name)
        {
            TerminalUser user = null;
            foreach (TerminalUser u in GetAllUsers())
            {
                if (u.GetName().Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    user = u;
                    break;
                }
            }
            return user;
        }
        /// <summary>
        /// Checks all users to see if there's any user with the specified username.
        /// </summary>
        /// <param name="username">The username to compare.</param>
        /// <returns>The result. Null if none.</returns>
        public TerminalUser GetByUsername(string username)
        {
            TerminalUser user = null;
            foreach (TerminalUser u in GetAllUsers())
            {
                if (u.GetUsername().ToLower() == username.ToLower())
                {
                    user = u;
                    break;
                }
            }
            return user;
        }
        /// <summary>
        /// Checks all users to see if there's any user with the specified value.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns>The result. Null of none.</returns>
        public TerminalUser Get(string value)
        {
            var name = GetByName(value);
            var uname = GetByUsername(value);

            if (name != null && uname == null)
                return name;
            else if (name == null && uname != null)
                return uname;
            else if (name != null && uname != null)
                return uname;
            else return null;
        }
        /// <summary>
        /// Gets a ReadOnly list of all admin accounts.
        /// </summary>
        /// <returns>Readonly list of all admin accounts.</returns>
        public IReadOnlyList<TerminalUser> GetAdmins()
        {
            return admins;
        }
        /// <summary>
        /// Gets a ReadOnly list of all the standard user accounts.
        /// </summary>
        /// <returns>Readonly list of all standard user accounts.</returns>
        public IReadOnlyList<TerminalUser> GetUsers()
        {
            return users;
        }
        /// <summary>
        /// Gets a ReadOnly list of all registered user accounts.
        /// </summary>
        /// <returns>Readonly list of all user accounts.</returns>
        public IReadOnlyList<TerminalUser> GetAllUsers()
        {
            List<TerminalUser> users = new List<TerminalUser>();
            if (!(root == null))
                users.Add(root);
            if (!(GetAdmins() == null))
            {
                foreach (TerminalUser t in GetAdmins())
                {
                    users.Add(t);
                }
            }
            if (!(GetUsers() == null))
            {
                foreach (TerminalUser t in GetUsers())
                {
                    users.Add(t);
                }
            }
            if (!(guest == null))
                users.Add(guest);
            return users;
        }
    }

    public class TerminalUser
    {
        private string username = "", password = "";
        private string name = "";
        private bool enabled = false, loggedIn = false;
        private PermissionGroup permissions = PermissionGroup.UserPermissions();
        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="permissionGroup">The permissions of the user account.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="enabled">Is the account enabled?</param>
        public TerminalUser(string username, string password, PermissionGroup permissionGroup, string name = "", bool enabled = true)
        {
            this.password = password;
            this.username = username;
            this.permissions = permissionGroup;
            this.name = name;
            this.enabled = enabled;
        }
        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="permissionGroup">The permissions of the user account.</param>
        /// <param name="enabled">Is the account enabled?</param>
        public TerminalUser(string username, string password, PermissionGroup permissionGroup, bool enabled = false)
        {
            this.password = password;
            this.username = username;
            this.permissions = permissionGroup;
            this.enabled = enabled;
        }

        internal TerminalUser(string username, PermissionGroup permissionGroup, string name = "", bool enabled = false)
        {
            this.password = "";
            this.username = username;
            this.permissions = permissionGroup;
            this.name = name;
            this.enabled = enabled;
        }

        public TerminalUser(string username, PermissionGroup permissionGroup, bool enabled = false)
        {
            this.password = "";
            this.username = username;
            this.permissions = permissionGroup;
            this.enabled = enabled;
        }
        /// <summary>
        /// Get the username associated with the account.
        /// </summary>
        /// <returns>The username accociated with the account.</returns>
        public string GetUsername() => username;
        /// <summary>
        /// Sets the username associated with the account.
        /// </summary>
        /// <param name="value"></param>
        public void SetUsername(string value) => username = value;
        /// <summary>
        /// Gets the name of the user associated with this account.
        /// </summary>
        /// <returns>Returns the name of the user.</returns>
        public string GetName() => name;
        /// <summary>
        /// Sets the name of the user associated with this account.
        /// </summary>
        /// <param name="value"></param>
        public void SetName(string value) => name = value;
        /// <summary>
        /// Returns the password associated with the account.
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return password;
        }
        /// <summary>
        /// Changes the password associated with this account.
        /// </summary>
        /// <param name="admin">The administrator account with the permission to change a users' password.</param>
        /// <param name="adminPassword">The password of the administrator account to verify.</param>
        /// <param name="newPassword">The new password of this account.</param>
        public void ChangePassword(TerminalUser admin, string adminPassword, string newPassword)
        {
            if (admin.GetPassword() == adminPassword && admin.Permissons.GetPermission().GetPermissionFlags().Contains(UserPermission.PermissionFlags.Users_Modify_Password))
                ChangePassword(password, newPassword);
        }
        /// <summary>
        /// Changes the password associated with this account.
        /// </summary>
        /// <param name="oldPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == password) password = newPassword;
        }
        /// <summary>
        /// Enables or disables access to the account.
        /// </summary>
        public bool Enabled { get => enabled; set { if (value) { enabled = true; } else { enabled = false; } } }
        /// <summary>
        /// Is the user logged in.
        /// </summary>
        public bool LoggedIn { get => loggedIn; internal set { if (value) { loggedIn = true; } else { loggedIn = false; } } }
        /// <summary>
        /// The permissions associated with this account.
        /// </summary>
        public PermissionGroup Permissons { get => permissions; set => permissions = value;}
        /// <summary>
        /// Holds permissions by group.
        /// </summary>
        public struct PermissionGroup
        {
            private string permissionName;
            private UserPermission permission;

            public PermissionGroup(string name, UserPermission permission)
            {
                permissionName = name;
                this.permission = permission;
            }

            public string GetName()
            {
                return permissionName;
            }

            public UserPermission GetPermission()
            {
                return permission;
            }

            public static PermissionGroup RootPermissions()
            {
                var perms = new UserPermission(
                    UserPermission.PermissionFlags.System_Applications_Install,
                    UserPermission.PermissionFlags.System_Applications_Uninstall,
                    UserPermission.PermissionFlags.System_Applications_Enable,
                    UserPermission.PermissionFlags.System_Applications_Use,
                    UserPermission.PermissionFlags.System_Directory_Create,
                    UserPermission.PermissionFlags.System_Directory_Delete,
                    UserPermission.PermissionFlags.System_Directory_Create_Protected,
                    UserPermission.PermissionFlags.System_Files_Append,
                    UserPermission.PermissionFlags.System_Files_Append_Protected,
                    UserPermission.PermissionFlags.System_Files_Create_Protected,
                    UserPermission.PermissionFlags.System_Files_Create,
                    UserPermission.PermissionFlags.System_Files_Delete,
                    UserPermission.PermissionFlags.System_Files_Delete_Protected,
                    UserPermission.PermissionFlags.System_Files_Read,
                    UserPermission.PermissionFlags.System_Files_Read_Protected,
                    UserPermission.PermissionFlags.System_Files_Write,
                    UserPermission.PermissionFlags.System_Flies_Write_Protected,
                    UserPermission.PermissionFlags.System_Modify,
                    UserPermission.PermissionFlags.System_Modify_Protected,
                    UserPermission.PermissionFlags.System_Properties_Modify,
                    UserPermission.PermissionFlags.System_Properties_Modify_Protected,
                    UserPermission.PermissionFlags.System_Update,
                    UserPermission.PermissionFlags.System_Bypass,
                    UserPermission.PermissionFlags.Users_Create,
                    UserPermission.PermissionFlags.Users_Delete,
                    UserPermission.PermissionFlags.Users_Modify,
                    UserPermission.PermissionFlags.User_Name_Modify,
                    UserPermission.PermissionFlags.User_Password_Modify,
                    UserPermission.PermissionFlags.User_Password_Remove,
                    UserPermission.PermissionFlags.User_Username_Modify
                    );
                return new PermissionGroup("root", perms);
            }

            public static PermissionGroup AdminPermissions()
            {
                var perms = new UserPermission(
                    UserPermission.PermissionFlags.System_Applications_Install,
                    UserPermission.PermissionFlags.System_Applications_Uninstall,
                    UserPermission.PermissionFlags.System_Applications_Enable,
                    UserPermission.PermissionFlags.System_Applications_Use,
                    UserPermission.PermissionFlags.System_Directory_Create,
                    UserPermission.PermissionFlags.System_Directory_Delete,
                    UserPermission.PermissionFlags.System_Directory_Create_Protected,
                    UserPermission.PermissionFlags.System_Files_Append,
                    UserPermission.PermissionFlags.System_Files_Append_Protected,
                    UserPermission.PermissionFlags.System_Files_Create_Protected,
                    UserPermission.PermissionFlags.System_Files_Create,
                    UserPermission.PermissionFlags.System_Files_Delete,
                    UserPermission.PermissionFlags.System_Files_Delete_Protected,
                    UserPermission.PermissionFlags.System_Files_Read,
                    UserPermission.PermissionFlags.System_Files_Read_Protected,
                    UserPermission.PermissionFlags.System_Files_Write,
                    UserPermission.PermissionFlags.System_Flies_Write_Protected,
                    UserPermission.PermissionFlags.System_Modify,
                    UserPermission.PermissionFlags.System_Modify_Protected,
                    UserPermission.PermissionFlags.System_Properties_Modify,
                    UserPermission.PermissionFlags.System_Properties_Modify_Protected,
                    UserPermission.PermissionFlags.System_Update,
                    UserPermission.PermissionFlags.Users_Create,
                    UserPermission.PermissionFlags.Users_Delete,
                    UserPermission.PermissionFlags.Users_Modify,
                    UserPermission.PermissionFlags.Users_Modify_Enable,
                    UserPermission.PermissionFlags.Users_Modify_Name,
                    UserPermission.PermissionFlags.Users_Modify_Password,
                    UserPermission.PermissionFlags.Users_Modify_Username,
                    UserPermission.PermissionFlags.User_Name_Modify,
                    UserPermission.PermissionFlags.User_Password_Modify,
                    UserPermission.PermissionFlags.User_Password_Remove,
                    UserPermission.PermissionFlags.User_Username_Modify
                    );
                return new PermissionGroup("administrator", perms);
            }

            public static PermissionGroup UserPermissions()
            {
                var perms = new UserPermission(
                    UserPermission.PermissionFlags.System_Applications_Use,
                    UserPermission.PermissionFlags.System_Directory_Create,
                    UserPermission.PermissionFlags.System_Directory_Delete,
                    UserPermission.PermissionFlags.System_Files_Append,
                    UserPermission.PermissionFlags.System_Files_Create,
                    UserPermission.PermissionFlags.System_Files_Delete,
                    UserPermission.PermissionFlags.System_Files_Read,
                    UserPermission.PermissionFlags.System_Files_Write,
                    UserPermission.PermissionFlags.User_Name_Modify,
                    UserPermission.PermissionFlags.User_Password_Modify,
                    UserPermission.PermissionFlags.User_Password_Remove,
                    UserPermission.PermissionFlags.User_Username_Modify
                    );
                return new PermissionGroup("user", perms);
            }

            public static PermissionGroup GuestPermissions()
            {
                var perms = new UserPermission(
                    UserPermission.PermissionFlags.System_Applications_Use,
                    UserPermission.PermissionFlags.System_Files_Read
                    );
                return new PermissionGroup("guest", perms);
            }
        }
        /// <summary>
        /// Holds various permissions.
        /// </summary>
        public class UserPermission
        {
            private PermissionFlags[] flags;

            public UserPermission(params PermissionFlags[] flags)
            {
                this.flags = flags;
            }

            public IReadOnlyCollection<PermissionFlags> GetPermissionFlags()
            {
                List<PermissionFlags> permissionFlags = new List<PermissionFlags>(flags.Length);
                foreach (PermissionFlags f in flags)
                {
                    permissionFlags.Add(f);
                }
                return permissionFlags;
            }

            public enum PermissionFlags
            {
                Users_Create,
                Users_Delete,
                Users_Modify,
                Users_Modify_Password,
                Users_Modify_Username,
                Users_Modify_Name,
                Users_Modify_Enable,
                User_Username_Modify,
                User_Password_Modify,
                User_Password_Remove,
                User_Name_Modify,
                System_Directory_Create,
                System_Directory_Create_Hidden,
                System_Directory_Create_Protected,
                System_Directory_Delete,
                System_Directory_Delete_Hidden,
                System_Directory_Delete_Protected,
                System_Files_Create,
                System_Files_Create_Hidden,
                System_Files_Create_Protected,
                System_Files_Delete,
                System_Files_Delete_Hidden,
                System_Files_Delete_Protected,
                System_Files_Append,
                System_Files_Append_Hidden,
                System_Files_Append_Protected,
                System_Files_Read,
                System_Files_Read_Hidden,
                System_Files_Read_Protected,
                System_Files_Write,
                System_Files_Write_Hidden,
                System_Flies_Write_Protected,
                System_Properties_Modify,
                System_Properties_Modify_Protected,
                System_Applications_Install,
                System_Applications_Uninstall,
                System_Applications_Use,
                System_Applications_Enable,
                System_Update,
                System_Modify,
                System_Modify_Protected,
                System_Bypass
            }
        }
    }
}
