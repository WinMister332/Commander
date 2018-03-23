using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.COSMOS
{
    public class TerminalUsers
    {
        private TerminalUserList list = null;

        public TerminalUsers(string rootPassword, string adminPassword = "password")
        {
            list = new TerminalUserList(rootPassword, adminPassword);
            //-----------------------------//
            
        }

        public void Login(string usernameText)
        {
            while (true)
            {
                if (usernameText.EndsWith(" "))
                    Console.Write(usernameText);
                else
                    Console.Write(usernameText + " ");
                var input = Console.ReadLine();
                if (!(input == "" || input == null))
                {
                    if (usernameText.EndsWith(" "))
                        Console.Write(usernameText);
                    else
                        Console.Write($"{usernameText} ");
                    var input2 = new KeyInput();
                    input2.ProcessInput();
                    Console.WriteLine();
                    var usr = GetUserByUsername(input);
                    if (usr == null)
                        Console.WriteLine("The username or password was incorrect!");
                    else
                    {
                        if (usr.Password == input2.ToString())
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("The username or password was incorrect!");
                        }
                    }
                }
            }
        }

        private TerminalUser GetUserByUsername(string username)
        {
            TerminalUser user = null;
            foreach (TerminalUser t in list.GetAllUsers())
            {
                if (t.Username == username)
                {
                    user = t;
                    break;
                }
            }
            return user;
        }

        public class TerminalUser
        {
            private string _fullName = "";
            private string _username = "";
            private string _password = "";
            private UserPermission _permission = UserPermission.STANDARD;
            private bool _enabled = true;
            private bool _loggedin = false;

            public TerminalUser(string username, string password, UserPermission permission, string fullName = "")
            {
                _username = username;
                _password = password;
                _permission = permission;
                _fullName = fullName;
            }

            public TerminalUser(string username, string password, UserPermission permission, bool enabled, string fullName = "")
            {
                _username = username;
                _password = password;
                _permission = permission;
                _fullName = fullName;
                _enabled = enabled;
            }

            public bool Enabled
            {
                get => _enabled;
                set => _enabled = value;
            }

            public bool LoggedIn => _loggedin;

            public string Username
            {
                get => _username;
                set
                {
                    if (!(_permission == UserPermission.ROOT))
                        _username = value;
                }
            }

            internal string Password
            {
                get => _password;
                set => _password = value;
            }

            public string FullName
            {
                get => _fullName;
                set
                {
                    if (!(_permission == UserPermission.ROOT))
                        _fullName = value;
                }
            }

            public UserPermission Permission => _permission;
        }

        internal class TerminalUserList
        {
            private TerminalUser root = null;
            private List<TerminalUser> admins = null;
            private List<TerminalUser> standards = null;
            private List<TerminalUser> limiteds = null;

            /// <summary>
            /// Creates a new instance of the TerminalUserList class and creates a root account and primary admin account.
            /// </summary>
            /// <param name="rootPassword">The password of the root account.</param>
            /// <param name="adminPassword">The admin password of the default administrator account.</param>
            public TerminalUserList(string rootPassword, string adminPassword)
            {
                var root = new TerminalUser("root", rootPassword, UserPermission.ROOT);
                var administrator = new TerminalUser("administrator", adminPassword, UserPermission.ADMINISTRATOR);
                var guest = new TerminalUser("guest", "", UserPermission.LIMITED);
            }

            /// <summary>
            /// Adds a user to the user list.
            /// </summary>
            /// <param name="user">The user to add to the list.</param>
            public void AddUser(TerminalUser user)
            {
                if (user.Permission == UserPermission.ROOT)
                {
                    if (root == null) root = user;
                    else
                        if (CommandUtils.DebugMode) Console.WriteLine("You cannot have more then one root account!");
                }
                else if (user.Permission == UserPermission.ADMINISTRATOR)
                {
                    if (!(HasSameUsername("admin") || HasSameUsername("administrator")) && !(HasSameUsername(user)))
                        admins.Add(user);
                    else
                         if (CommandUtils.DebugMode) Console.WriteLine("You cannot have multiple accounts with the same username!");
                }
                else if (user.Permission == UserPermission.STANDARD)
                {
                    if (!(HasSameUsername(user)))
                        standards.Add(user);
                    else
                        if (CommandUtils.DebugMode) Console.WriteLine("You cannot have multiple accounts with the same username!");
                }
                else if (user.Permission == UserPermission.LIMITED)
                {
                    if (!(HasSameUsername(user)))
                        standards.Add(user);
                    else
                        if (CommandUtils.DebugMode) Console.WriteLine("You cannot have multiple accounts with the same username!");
                }
            }

            /// <summary>
            /// Adds a group of users to the user list.
            /// </summary>
            /// <param name="users">The array of users to add.</param>
            public void AddUsers(TerminalUser[] users)
            {
                foreach (TerminalUser user in users)
                    AddUser(user);
            }

            /// <summary>
            /// Removes a group of users to the user list.
            /// </summary>
            /// <param name="user">The user to add to the list.</param>
            public void RemoveUser(TerminalUser user)
            {
                if (user.Permission == UserPermission.ROOT)
                {
                    if (CommandUtils.DebugMode) Console.WriteLine("You cannot remove the root account.");
                }
                else if (user.Permission == UserPermission.ADMINISTRATOR)
                {
                    if (admins.Count == 1)
                    {
                        if (CommandUtils.DebugMode) Console.WriteLine("You must have at least one administrator account.");
                    }
                    else
                    {
                        admins.Remove(user);
                    }
                }
                else if (user.Permission == UserPermission.STANDARD)
                {
                    if (!(standards.Count < 0) && !(standards.Count == 0))
                    {
                        standards.Remove(user);
                    }
                }
                else
                {
                    limiteds.Remove(user);
                }
            }

            /// <summary>
            /// Removes a group of users from the user list.
            /// </summary>
            /// <param name="users">The group of users to add the the list.</param>
            public void RemoveUsers(TerminalUser[] users)
            {
                foreach (TerminalUser user in users)
                    RemoveUser(user);
            }

            /// <summary>
            /// Gets all non-root users.
            /// </summary>
            /// <returns>All non-root users.</returns>
            public List<TerminalUser> GetAllUsers()
            {
                return Combine();
            }

            private List<TerminalUser> Combine()
            {
                List<TerminalUser> users = new List<TerminalUser>(admins.Count + standards.Count + limiteds.Count);
                foreach (TerminalUser usera in admins)
                {
                    users.Add(usera);
                }
                foreach (TerminalUser userx in standards)
                {
                    users.Add(userx);
                }
                foreach (TerminalUser userl in limiteds)
                {
                    users.Add(userl);
                }
                return users;
            }

            private bool HasSameUsername(string username)
            {
                var users = Combine();
                foreach (TerminalUser user in users)
                {
                    if (user.Username == username) return true;
                }
                return false;
            }
            private bool HasSameUsername(TerminalUser user)
            {
                return HasSameUsername(user.Username);
            }
            private bool HasSameUsername(string username, TerminalUser user)
            {
                if (user.Username == username) return true;
                return false;
            }
        }
    }

    public enum UserPermission
    {
        ROOT = 0, //SYSTEM USE ONLY!
        ADMINISTRATOR = 1, //Administrator usage only.
        STANDARD = 2, //The normal user permission.
        LIMITED = 3 //Guest permissions.
    }
}
