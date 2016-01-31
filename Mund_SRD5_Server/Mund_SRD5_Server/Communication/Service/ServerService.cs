using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using Mundasia.Communication;
using Mundasia.Objects;


namespace Mundasia.Server.Communication
{
    public class ServerService : IServerService
    {
        public string Ping()
        {
            return DateTime.UtcNow.ToShortTimeString();
        }

        public string GetPublicKey()
        {
            AccountCreation ac = new AccountCreation();
            ac.pubKey = Encryption.GetPubKey();
            ac.message = new byte[0];
            return ac.ToString();
        }

        public bool CreateAccount(string message)
        {
            AccountCreation ac = new AccountCreation(message);
            try
            {
                string decMessage = Encryption.Decrypt(ac.message, ac.pubKey);
                char[] split = new char[] { '\n' };
                string[] credentials = decMessage.Split(split, 2);
                
                // If the account already exists, don't make a new one.
                if(Account.LoadAccount(credentials[0]) != null)
                {
                    return false;
                }

                // otherwise make a new one.
                if (new Account(credentials[0], credentials[1]) != null)
                {
                    return true;
                }
            }
            catch {}
            return false;
        }

        public int Login(string message)
        {
            Login lg = new Login(message);
            Account targetAcct = Account.LoadAccount(lg.userName);
            if(targetAcct == null)
            {
                return -1;
            }
            if (targetAcct.Authenticate(lg.password))
            {
                targetAcct.Address = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
                return Account.GetSessionId(lg.userName);
            }
            return -1;
        }

        public string CreateCharacter(string message)
        {
            CharacterCreation nChar = new CharacterCreation(message);
            Account targetAccount = Account.LoadAccount(nChar.UserId);
            if(targetAccount == null)
            {
                return "Invalid account";
            }
            if (targetAccount.SessionId != nChar.SessionId)
            {
                return "Invalid session Id";
            }
            Creature chr;
            try
            {
                chr = new Creature()
                {
                    // TODO: Update with 5e minimum information
                    AccountName = targetAccount.UserName,
                    CharacterName = nChar.Name,
                    CharacterRace = (uint)nChar.Race,
                    Sex = nChar.Sex,
                    SkinColor = (uint)nChar.SkinColor,
                    HairColor = (uint)nChar.HairColor,
                    HairStyle = (uint)nChar.HairStyle,
                };
            }
            catch
            {
                return "Invalid character data; likely a negative number passed to an unsigned field.";
            }
            if (!chr.ValidateCharacter())
            {
                return "Character violates rules of character creation.";
            }
            if(targetAccount.LoadCharacter(chr.CharacterName) != null)
            {
                return "A character with that name already exists";
            }
            if (!targetAccount.NewCharacter(chr))
            {
                return "Unable to save character";
            }
            try
            {
                // TODO: 5e skills
            }
            catch
            {
                return "Failed to parse aspiration.";
            }
            if (!targetAccount.SaveCharacter(chr))
            {
                return "Could not save character.";
            }
            return "Success: " + chr.CharacterName;
        }

        public string ListCharacters(string message)
        {
            RequestCharacter upd = new RequestCharacter(message);
            Account acct = Account.LoadAccount(upd.UserId);
            if (acct == null)
            {
                return String.Empty;
            }
            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if (acct.SessionId != upd.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }
            acct.KeepAlive();

            StringBuilder str = new StringBuilder();
            foreach(string cha in acct.Characters)
            {
                str.Append(cha);
                str.Append("|");
            }
            return str.ToString();
        }

        public string CharacterDetails(string message)
        {
            RequestCharacter upd = new RequestCharacter(message);
            Account acct = Account.LoadAccount(upd.UserId);
            if(acct == null)
            {
                return String.Empty;
            }
            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if (acct.SessionId != upd.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }
            acct.KeepAlive();

            Creature cha = acct.LoadCharacter(upd.RequestedCharacter);
            if(cha != null)
            {
                return cha.ToString();
            }
            return String.Empty;
        }

        public string SelectCharacter(string message)
        {
            RequestCharacter upd = new RequestCharacter(message);
            Account acct = Account.LoadAccount(upd.UserId);
            if(acct == null)
            {
                return String.Empty;
            }
            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if(acct.SessionId != upd.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }
            acct.KeepAlive();

            Creature cha = acct.LoadCharacter(upd.RequestedCharacter);
            if (cha != null)
            {
                CharacterSelection ret = new CharacterSelection(cha);
                return ret.ToString();
            }
            return String.Empty;
        }

        public string Update(string message)
        {
            SessionUpdate upd = new SessionUpdate(message);
            Account acct = Account.LoadAccount(upd.UserId);
            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if(acct.SessionId != upd.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }
            acct.KeepAlive();
            return String.Empty;
        }

        public string UpdatePlayScene(string message)
        {
            SessionUpdate upd = new SessionUpdate(message);
            Account acct = Account.LoadAccount(upd.UserId);
            Creature ch = acct.LoadCharacter(upd.CharacterName);
            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if (acct.SessionId != upd.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }
            acct.KeepAlive();
            if (!Map.LoadedMaps.ContainsKey(ch.Map))
            {
                return "False";
            }
            Map currentMap = Map.LoadedMaps[ch.Map];

            if(!currentMap.MapDeltas.ContainsKey(ch))
            {
                return String.Empty;
            }

            string ret = String.Empty;
            lock (currentMap.MapDeltas[ch])
            {
                ret = currentMap.MapDeltas[ch].ToString();
                currentMap.MapDeltas[ch].AddedCharacters.Clear();
                currentMap.MapDeltas[ch].AddedTiles.Clear();
                currentMap.MapDeltas[ch].ChangedCharacters.Clear();
                currentMap.MapDeltas[ch].RemovedCharacters.Clear();
                currentMap.MapDeltas[ch].RemovedTiles.Clear();
            }
            return ret;
        }

        public string MoveCharacter(string message)
        {
            MoveRequest mv = new MoveRequest(message);
            Account acct = Account.LoadAccount(mv.AccountName);
            Creature ch = acct.LoadCharacter(mv.CharacterName);
            if(!Map.LoadedMaps.ContainsKey(ch.Map))
            {
                return "False";
            }
            Map currentMap = Map.LoadedMaps[ch.Map];

            if (currentMap.MoveCharacter(ch, mv.X, mv.Y, mv.Z, mv.Facing))
            {
                acct.SaveCharacter(ch);
                string ret = String.Empty;
                lock(currentMap.MapDeltas[ch])
                {
                    ret = currentMap.MapDeltas[ch].ToString();
                    currentMap.MapDeltas[ch].AddedCharacters.Clear();
                    currentMap.MapDeltas[ch].AddedTiles.Clear();
                    currentMap.MapDeltas[ch].ChangedCharacters.Clear();
                    currentMap.MapDeltas[ch].RemovedCharacters.Clear();
                    currentMap.MapDeltas[ch].RemovedTiles.Clear();
                }
                return ret;
            }
            else 
            { 
                return "False";
            }
        }

        public string ChangeTiles(string message)
        {
            TileChange tc = new TileChange(message);
            Account acct = Account.LoadAccount(tc.AccountName);
            if(acct == null)
            {
                return "Error: invalid account";
            }

            Creature ch = acct.LoadCharacter(tc.CharacterName);
            if(ch == null)
            {
                return "Error: invalid character";
            }

            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if (acct.SessionId != tc.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }

            if(!ch.IsGM)
            {
                return "Error: only DMs may change tiles";
            }

            if (!Map.LoadedMaps.ContainsKey(ch.Map))
            {
                return "Error: invalid map";
            }
            Map currentMap = Map.LoadedMaps[ch.Map];

            foreach(Tile t in tc.RemovedTiles)
            {
                currentMap.Remove(t);
                t.Delete(currentMap.Name);
            }
            foreach(Tile t in tc.AddedTiles)
            {
                currentMap.Add(t);
                t.Save(currentMap.Name);
            }

            string ret = String.Empty;
            if(!currentMap.MapDeltas.ContainsKey(ch))
            {
                return String.Empty;
            }
            lock (currentMap.MapDeltas[ch])
            {
                ret = currentMap.MapDeltas[ch].ToString();
                currentMap.MapDeltas[ch].AddedCharacters.Clear();
                currentMap.MapDeltas[ch].AddedTiles.Clear();
                currentMap.MapDeltas[ch].ChangedCharacters.Clear();
                currentMap.MapDeltas[ch].RemovedCharacters.Clear();
                currentMap.MapDeltas[ch].RemovedTiles.Clear();
            }
            return ret;
        }

        public string EquipItem(string message)
        {
            string ret = String.Empty;
            EquipRequest req = new EquipRequest(message);
            Account acct = Account.LoadAccount(req.RequestingAccount);
            if (acct == null)
            {
                return "Error: invalid account";
            }

            string ip = (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
            if (acct.SessionId != req.SessionId || acct.Address != ip)
            {
                return "Error: incorrect address or session Id";
            }

            acct.KeepAlive();
            Creature ch = acct.LoadCharacter(req.RequestingCharacter);
            if(ch == null)
            {
                return "Error: invalid character";
            }

            if(!ch.IsGM)
            {
                if(ch.EquipItem(req.InventorySlot, req.Identifier))
                {
                    if(req.InventorySlot == (int)InventorySlot.Chest)
                    {
                        if (Map.LoadedMaps.ContainsKey(ch.Map))
                        {
                            Map.LoadedMaps[ch.Map].ChangeCharacterAppearance(ch);
                        }
                    }
                    return ch.ToString();
                }
                else
                {
                    return "Error: cannot equip item";
                }
            }
            else
            {
                Account tgAcct = Account.LoadAccount(req.ChangedAccount);
                if(tgAcct == null)
                {
                    return "Error: invalid account";
                }
                Creature tgCh = tgAcct.LoadCharacter(req.ChangedCharacter);
                if(tgCh == null)
                {
                    return "Error: invalid character";
                }
                if(tgCh.EquipItem(req.InventorySlot, req.Identifier))
                {
                    return ch.ToString();
                }
                else
                {
                    return "Error: cannot equip item";
                }
            }
        }
    }
}
