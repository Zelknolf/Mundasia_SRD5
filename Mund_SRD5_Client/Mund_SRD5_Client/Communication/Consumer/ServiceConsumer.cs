using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using Mundasia.Objects;

namespace Mundasia.Communication
{
    public partial class ServiceConsumer
    {
        public const string StringNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";

        // TODO: Move this to a configuration file
#if DEBUG
        public static string baseServerTarget = "http://localhost:6300/MundasiaServerService/";
#else
        public static string baseServerTarget = "http://75.134.27.112:6300/MundasiaServerService/";
#endif

        public static string Ping()
        {
            try
            {
                string wrURI = baseServerTarget + "ping";
                WebRequest wreq = WebRequest.Create(wrURI);
                WebResponse wresp = wreq.GetResponse();
                using (StreamReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer date = new XmlSerializer(typeof(string), StringNamespace);
                    string timeString = (string)date.Deserialize(sr);
                    return timeString;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool CreateAccount(string userName, string password)
        {
            AccountCreation ac;
            try
            {
                string wrURI = baseServerTarget + "getpublickey";
                WebRequest wreq = WebRequest.Create(wrURI);
                WebResponse wresp = wreq.GetResponse();
                using (StreamReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer rsa = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)rsa.Deserialize(sr);
                    ac = new AccountCreation(resp);
                }
            }
            catch
            {
                return false;
            }

            // Not meant as encryption, but at least makes sure that the text we
            // save on the server isn't an actual password.
            password = Encryption.GetSha256Hash(password);
            ac.message = Encryption.Encrypt(String.Format("{0}\n{1}", userName, password), ac.pubKey);

            try
            {
                string wrURI = baseServerTarget + "createaccount";
                string msg = ac.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(bool), StringNamespace);
                    bool resp = (bool)xml.Deserialize(sr);
                    return resp;
                }

            }
            catch
            {
                return false;
            }
        }

        public static int ClientLogin(string userName, string password)
        {
            password = Encryption.GetSha256Hash(password);
            Login lg = new Login();
            lg.userName = userName;
            lg.password = Encryption.GetSha256Hash(password + userName + DateTime.UtcNow.Day + DateTime.UtcNow.Month + DateTime.UtcNow.Year);

            try
            {
                string wrURI = baseServerTarget + "login";
                string msg = lg.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(int), StringNamespace);
                    int resp = (int)xml.Deserialize(sr);
                    SessionId = resp;
                    UserId = userName;
                    if(Worker != null)
                    {
                        Worker.CancelAsync();
                        Worker.Dispose();
                        Worker = null;
                    }
                    if (SessionId != -1)
                    {
                        Worker = new BackgroundWorker();
                        Worker.DoWork += Worker_DoWork;
                        Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                        Worker.WorkerSupportsCancellation = true;
                        Worker.RunWorkerAsync();
                    }
                    return resp;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static void Update(string userName, int sessionId)
        {
            try
            {
                string wrURI = baseServerTarget + "update";
                string msg = (new SessionUpdate() { SessionId = sessionId, UserId = userName }).ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                }
            }
            catch {  }
        }

        public static string CreateCharacter(string name, int authority, int care, int fairness, int hobby, int loyalty, int profession, int race, int sex, int talent, int tradition, int vice, int virtue, int aspiration, int hairStyle, int hairColor, int skinColor)
        {
            CharacterCreation chr = new CharacterCreation()
            {
                Name = name,
                SessionId = SessionId,
                UserId = UserId,
                HairColor = hairColor,
                HairStyle = hairStyle,
                SkinColor = skinColor,
            };
            try
            {
                string wrURI = baseServerTarget + "createcharacter";
                string msg = chr.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch 
            {
                return String.Empty;
            }
        }

        public static string ListCharacters()
        {
            RequestCharacter req = new RequestCharacter()
            {
                SessionId = SessionId,
                UserId = UserId
            };
            try
            {
                string wrURI = baseServerTarget + "listcharacters";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch 
            {
                return String.Empty;
            }
        }

        public static string CharacterDetails(string character)
        {
            RequestCharacter req = new RequestCharacter()
            {
                RequestedCharacter = character,
                SessionId = SessionId,
                UserId = UserId
            };
            try
            {
                string wrURI = baseServerTarget + "characterdetails";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string SelectCharacter(string character)
        {
            RequestCharacter req = new RequestCharacter()
            {
                RequestedCharacter = character,
                SessionId = SessionId,
                UserId = UserId
            };
            try
            {
                string wrURI = baseServerTarget + "selectcharacter";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using(TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string UpdatePlayScene(string account, string character)
        {
            SessionUpdate req = new SessionUpdate()
            {
                CharacterName = character,
                SessionId = SessionId,
                UserId = account,
            };
            try
            {
                string wrURI = baseServerTarget + "updateplayscene";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch { }
            return String.Empty;
        }

        public static string MoveCharacter(string account, string character, int x, int y, int z, Direction facing)
        {
            MoveRequest req = new MoveRequest()
            {
                AccountName = account,
                CharacterName = character,
                X = x,
                Y = y,
                Z = z,
                Facing = facing,
            };
            try
            {
                string wrURI = baseServerTarget + "movecharacter";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using(TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch { }
            return String.Empty;
        }

        public static string ChangeTiles(TileChange req)
        {
            req.SessionId = SessionId;
            try
            {
                string wrURI = baseServerTarget + "changetiles";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch { }
            return String.Empty;
        }

        public static string EquipItem(string ReqAccount, string ReqChar, string Account, string Character, string Tag, int InventorySlot)
        {
            try
            {
                string wrURI = baseServerTarget + "equipitem";
                string msg = new EquipRequest(ReqChar, ReqAccount, Character, Account, Tag, InventorySlot, SessionId).ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch { }
            return String.Empty;
        }
    }
}
