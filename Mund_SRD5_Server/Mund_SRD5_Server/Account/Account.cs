using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Timers;

using Mundasia.Objects;

namespace Mundasia
{
    /// <summary>
    /// The account class is used to manage the saving and checking of credentials of specific players, as well as to find
    /// the characters associated with each account.
    /// </summary>
    [XmlRootAttribute]
    public class Account
    {
        const string delimiter = "|";
        static char[] delim = new char[] { '|' };

        const string listDelimiter = "[";
        static char[] listDelim = new char[] { '[' };

        /// <summary>
        /// Parameterless constructor exists to facilitate serialize/ deserialize operations
        /// </summary>
        internal Account() { }

        /// <summary>
        /// Creates a new account with the stated username and password.
        /// </summary>
        /// <param name="userName">The username to be used.</param>
        /// <param name="password">The hash of the password to be used.</param>
        public Account(string userName, string password)
        {
            UserName = userName;
            _password = password;
            string path = GetPathForId(userName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Characters == null)
            {
                Characters = new List<string>();
            }
            if (LoadedCharacters == null)
            {
                LoadedCharacters = new List<Creature>();
            }
            _lastAccessed = DateTime.UtcNow;
            SaveAccount();
            _cachedAccounts.Add(userName, this);
        }

        public Account(string FileLine)
        {
            string[] fileSplit = FileLine.Split(delim);
            UserName = fileSplit[0];
            _password = fileSplit[1];
            string[] characters = fileSplit[2].Split(listDelim);
            Characters = new List<string>();
            foreach (string character in characters)
            {
                if (String.IsNullOrWhiteSpace(character))
                {
                    continue;
                }
                Characters.Add(character);
            }
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(UserName);
            ret.Append(delimiter);
            ret.Append(_password);
            ret.Append(delimiter);
            foreach(string character in Characters)
            {
                ret.Append(character);
                ret.Append(listDelimiter);
            }
            return ret.ToString();
    }

        public void SaveAccount()
        {
            string path = GetPathForId(UserName);
            using (FileStream stream = new FileStream(path + UserName + ".aco", FileMode.Create))
            {
                using (StreamWriter sr = new StreamWriter(stream))
                {
                    sr.WriteLine(this.ToString());
                    sr.Flush();
                }
            }
        }

        /// <summary>
        /// Transforms the password into a hash, to make the saved string less-accessible than it would be otherwise.
        /// Presumably the machine itself would also be secure, and we're not going to do anything dumb like put these
        /// on a SQL table that enjoys code injections.
        /// 
        /// Portions of the code which intend to authenticate based on user input should still use public key/secure 
        /// key combinations to secure the sending, otherwise this is still vulnerable to a repeater attack.
        /// </summary>
        /// <param name="password">the password to be hashed</param>
        /// <returns>the hashed password</returns>
        private static string GetSha256Hash(string password)
        {
            HashAlgorithm alg = SHA256.Create();
            byte[] hashByte = alg.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder ret = new StringBuilder();
            foreach (byte b in hashByte)
                ret.Append(b.ToString("X2"));

            return ret.ToString();
        }

        /// <summary>
        /// Loads an account with the userName provided.
        /// </summary>
        /// <param name="userName">The username to load.</param>
        /// <returns>The account, or null on error.</returns>
        public static Account LoadAccount(string userName)
        {
            if (_cachedAccounts.Keys.Contains(userName)) return _cachedAccounts[userName];

            string path = GetPathForId(userName);
            if (!Directory.Exists(path))
            {
                return null;
            }
            try
            {
                using (FileStream stream = new FileStream(path + userName + ".aco", FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        Account ret = new Account(sr.ReadLine());
                        ret._lastAccessed = DateTime.UtcNow;
                        ret.LoadedCharacters = new List<Creature>();
                        if (ret.Characters == null)
                        {
                            ret.Characters = new List<string>();
                        }
                        _cachedAccounts.Add(userName, ret);
                        return ret;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Provides a blob-server-esque path for the account name suggested.
        /// </summary>
        /// <param name="userName">The user name for which to get the path</param>
        /// <returns>The fully-qualified path.</returns>
        public static string GetPathForId(string userName)
        {
            string extraPath = "";
            for(int c = 0; c< userName.Length; c++)
            {
                extraPath = extraPath + "\\" + userName.Substring(c, 1);
            }
            return Directory.GetCurrentDirectory() + extraPath + "\\";
        }

        /// <summary>
        /// Used to tell if given credentials are valid to use to log in.
        /// </summary>
        /// <param name="password">the password as sent by the user (expecting to be a hash of the sessionId and password</param>
        /// <returns>true if the credentials are valid</returns>
        public bool Authenticate(string remotePass)
        {
            _lastAccessed = DateTime.UtcNow;
            string localPass = GetSha256Hash(_password + UserName + DateTime.UtcNow.Day + DateTime.UtcNow.Month + DateTime.UtcNow.Year);
            if (remotePass == localPass)
                return true;
            return false;
        }

        /// <summary>
        /// Create a character for this account with the chr character object.
        /// 
        /// Assumes that the character object has passed validation.
        /// </summary>
        /// <param name="chr">The character to add to this account</param>
        /// <returns>true on success, or false on failure</returns>
        public bool NewCharacter(Creature chr)
        {
            chr.AccountName = this.UserName;
            Characters.Add(chr.CharacterName);
            LoadedCharacters.Add(chr);

            return SaveCharacter(chr);
        }

        /// <summary>
        /// Loads a character by the character's name
        /// </summary>
        /// <param name="characterName">The character to load</param>
        /// <returns>The character object of the loaded object, or null on failure</returns>
        public Creature LoadCharacter(string characterName)
        {
            _lastAccessed = DateTime.UtcNow;
            foreach (Creature ch in LoadedCharacters)
            {
                if (ch.CharacterName == characterName)
                {
                    return ch;
                }
            }

            string path = GetPathForId(UserName);
            if (!Directory.Exists(path))
            {
                return null;
            }
            try
            {
                using (FileStream stream = new FileStream(path + characterName + ".chr", FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        Creature ret = new Creature(sr.ReadLine());
                        LoadedCharacters.Add(ret);
                        return ret;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Save a character object which is already in memory.
        /// </summary>
        /// <param name="characterName">The name of the character to save</param>
        /// <returns>True on success, false on failure</returns>
        public bool SaveCharacter(string characterName)
        {
            Creature ch = LoadCharacter(characterName);
            if(ch != null)
            {
                return SaveCharacter(ch);
            }
            return false;
        }

        /// <summary>
        /// Saves a character object
        /// </summary>
        /// <param name="chr">The character to save</param>
        /// <returns>true on success, false on failure</returns>
        public bool SaveCharacter(Creature chr)
        {
            try
            {
                string path = GetPathForId(UserName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if(!Characters.Contains(chr.CharacterName))
                {
                    Characters.Add(chr.CharacterName);
                }
                using (FileStream stream = new FileStream(path + chr.CharacterName + ".chr", FileMode.Create))
                {
                    using (StreamWriter sr = new StreamWriter(stream))
                    {
                        sr.WriteLine(chr.ToString());
                        sr.Flush();
                    }
                }
                _lastAccessed = DateTime.UtcNow;
                SaveAccount();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The user name
        /// </summary>
        [XmlAttribute]
        public string UserName;

        /// <summary>
        /// A list of characters belonging to this account
        /// </summary>
        [XmlArray]
        public List<String> Characters;

        /// <summary>
        /// A list of loaded characters belonging to this account, plus
        /// all of the details of those characters.
        /// </summary>
        [NonSerialized, XmlIgnore]
        public List<Creature> LoadedCharacters;

        /// <summary>
        /// Defines the amount of time, in minutes, that an account can sit idle before the
        /// cleanup loop will determine it to be lost and unload it.
        /// </summary>
        [NonSerialized, XmlIgnore]
        public const int AccountTimeout = 2;

        /// <summary>
        /// The field we actually hold the password in during regular running. Kept in the private field
        /// as a little buffer against components which play fast and loose with account objects.
        /// </summary>
        [NonSerialized, XmlIgnore]
        private string _password;

        /// <summary>
        /// This field is used to keep track of the last time this account accessed
        /// </summary>
        [NonSerialized, XmlIgnore]
        private DateTime _lastAccessed;

        /// <summary>
        /// This field is used to identify the session by number.
        /// </summary>
        [NonSerialized, XmlIgnore]
        public int SessionId;

        /// <summary>
        /// This field is used to store the client's IP address.
        /// </summary>
        [NonSerialized, XmlIgnore]
        public string Address;

        /// <summary>
        /// A static cache of accounts that we've used recently, indexed by account name for 
        /// quicker response in case of needing reauthentication.
        /// </summary>
        [NonSerialized, XmlIgnore]
        private static Dictionary<string, Account> _cachedAccounts = new Dictionary<string, Account>();

        /// <summary>
        /// A static cache of accounts which are currently logged in, indexed by session id for
        /// quicker response during the use of regular commands.
        /// </summary>
        [NonSerialized, XmlIgnore]
        public static Dictionary<int, Account> ActiveSessions = new Dictionary<int, Account>();

        /// <summary>
        /// Holds the last session Id assigned.
        /// </summary>
        [NonSerialized, XmlIgnore]
        private static int _lastId = 1;

        /// <summary>
        /// Timer that slowly checks on active accounts, and cleans up ones that have been stale too long.
        /// </summary>
        [NonSerialized, XmlIgnore]
        private static Timer _sessionEnder = new Timer();

        /// <summary>
        /// Clears out the indexing of session Ids currently assigned to an account
        /// and provides a new (unique) session Id.
        /// </summary>
        /// <param name="userId">The Id of the account to provide an Id to.</param>
        /// <returns>The Id, or -1 on error</returns>
        public static int GetSessionId(string userId)
        {
            Account loadedAccount = LoadAccount(userId);
            if (loadedAccount != null)
            {
                if (ActiveSessions.ContainsKey(loadedAccount.SessionId))
                {
                    // Player has relogged; clear old session data.
                    ActiveSessions.Remove(loadedAccount.SessionId);
                }
                loadedAccount.SessionId = _lastId;
                _lastId++;
                ActiveSessions.Add(loadedAccount.SessionId, loadedAccount);
                return loadedAccount.SessionId;
            }
            return -1;
        }

        public void KeepAlive()
        {
            this._lastAccessed = DateTime.UtcNow; ;
        }

        
        /// <summary>
        /// Establishes the events needed for the regular check of the event checker, so it can
        /// remove the caching of accounts which haven't been used recently.
        /// </summary>
        private static void StartSessionEnder()
        {
            // Don't restart the session ender if it's already running.
            if (_sessionEnder.Enabled) { return; }

            _sessionEnder.AutoReset = true;
            _sessionEnder.Interval = 60 * 1000; // once a minute

            _sessionEnder.Elapsed += new ElapsedEventHandler(_sessionEnder_Elapsed);

            _sessionEnder.Start();
        }

        /// <summary>
        /// Method which is called when _sessionEnder elapses.
        /// </summary>
        private static void _sessionEnder_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, Account> account in _cachedAccounts)
            {
                if (account.Value._lastAccessed == null)
                {
                    // Must've snuck by somehow. We'll start the count now.
                    account.Value._lastAccessed = DateTime.UtcNow;
                    continue;
                }
                if (DateTime.UtcNow - account.Value._lastAccessed > TimeSpan.FromMinutes(AccountTimeout))
                {
                    toRemove.Add(account.Key);
                    ActiveSessions.Remove(account.Value.SessionId);
                }
            }
            foreach (string rem in toRemove)
            {
                _cachedAccounts.Remove(rem);
            }
        }
    }
}
