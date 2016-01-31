using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.Security.Cryptography;

namespace Mundasia.Server.Communication
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string Ping();

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string GetPublicKey();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate="CreateAccount?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        bool CreateAccount(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Login?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        int Login(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CreateCharacter?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string CreateCharacter(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListCharacters?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string ListCharacters(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CharacterDetails?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string CharacterDetails(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SelectCharacter?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string SelectCharacter(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Update?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string Update(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "MoveCharacter?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string MoveCharacter(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UpdatePlayScene?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string UpdatePlayScene(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ChangeTiles?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string ChangeTiles(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EquipItem?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string EquipItem(string message);
    }
}
